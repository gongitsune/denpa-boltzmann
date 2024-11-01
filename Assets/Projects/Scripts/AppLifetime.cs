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
        private PythonInitializer _pythonInitializer;
        private UniSimulator _uniSimulator;

        private void Start()
        {
            _pythonInitializer = new PythonInitializer();
            _handTrackingClient = FindAnyObjectByType<HandTrackingClient>();
            _uniSimulator = FindAnyObjectByType<UniSimulator>();
            _forceSourceRoot = FindAnyObjectByType<ForceSourceRoot>();
            _forceSourceUpdater = FindAnyObjectByType<ForceSourceUpdater>();

            _pythonInitializer.Initialize();
            _handTrackingClient.Initialize();
            UniTask.Create(async () =>
            {
                await UniTask.WaitUntil(() => _handTrackingClient.IsReceiveFirstData);

                _uniSimulator.Initialize();
                _forceSourceUpdater.Initialize(_forceSourceRoot);

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