using Cysharp.Threading.Tasks;
using Projects.UniLBM.Runtime.Common;
using Projects.UniLBM.Runtime.Lbm;
using UnityEngine;

namespace Projects.Scripts
{
    public class AppLifetime : MonoBehaviour
    {
        private ForceSourceRoot _forceSourceRoot;
        private ForceSourceUpdater _forceSourceUpdater;
        private HandTrackingClient _handTrackingClient;
        private bool _isInitialized;
        private PyCam _pyCam;
        private PythonInitializer _pythonInitializer;
        private UniSimulator _uniSimulator;

        private void Start()
        {
            _pythonInitializer = new PythonInitializer();
            _handTrackingClient = FindAnyObjectByType<HandTrackingClient>();
            _uniSimulator = FindAnyObjectByType<UniSimulator>();
            _forceSourceRoot = FindAnyObjectByType<ForceSourceRoot>();
            _forceSourceUpdater = FindAnyObjectByType<ForceSourceUpdater>();
            _pyCam = FindAnyObjectByType<PyCam>();

            _handTrackingClient.Initialize();
            _pyCam.Initialize().Forget();
            UniTask.Create(async () =>
            {
                Debug.Log("Wait until receive first data");
                await UniTask.WhenAny(
                    UniTask.WaitUntil(() => _handTrackingClient.IsReceiveFirstData),
                    UniTask.Delay(2000)
                );
                if (!_handTrackingClient.IsReceiveFirstData)
                {
                    Debug.Log("Failed to receive first data. Launch PythonInitializer");
                    _pythonInitializer.Initialize();
                }

                Debug.Log("Wait until receive first data");
                await UniTask.WaitUntil(() => _handTrackingClient.IsReceiveFirstData);

                _uniSimulator.Initialize();
                Debug.Log("UniSimulator initialized");
                _forceSourceUpdater.Initialize(_forceSourceRoot);
                Debug.Log("ForceSourceUpdater initialized");

                _isInitialized = true;
            });
        }

        private void Update()
        {
            if (!_isInitialized) return;

            _forceSourceUpdater.UpdateForceSource(_handTrackingClient.CurrentHandData);
            _uniSimulator.Simulate();
        }

        private void OnDestroy()
        {
            _pythonInitializer?.Dispose();
        }
    }
}