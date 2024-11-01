using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts
{
    public class PyCam : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;

        private WebCamTexture _webCamTexture;

        public async UniTask Initialize()
        {
            await UniTask.WhenAny(
                UniTask.WaitUntil(() => WebCamTexture.devices.Any(d => d.name == "OBS Virtual Camera")),
                UniTask.Delay(60000)
            );
            if (WebCamTexture.devices.All(d => d.name != "OBS Virtual Camera"))
            {
                Debug.LogError("OBS Virtual Camera not found");
                return;
            }

            var device = WebCamTexture.devices.First(d => d.name == "OBS Virtual Camera");
            _webCamTexture = new WebCamTexture(device.name, 640, 480, 30);
            rawImage.texture = _webCamTexture;

            var retryCount = 0;
            while (true)
            {
                _webCamTexture.Play();
                await UniTask.Delay(5000);
                if (_webCamTexture.isPlaying)
                    break;
                    
                retryCount++;
                if (retryCount > 50)
                {
                    Debug.LogError("Failed to play OBS Virtual Camera");
                    return;
                }
                await UniTask.Delay(5000);
            }
        }
    }
}