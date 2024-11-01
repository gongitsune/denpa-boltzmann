using System;
using System.Net.Sockets;
using System.Text;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Projects.Scripts
{
    public class HandTrackingClient : MonoBehaviour
    {
        [SerializeField] private int port = 5000;

        private UdpClient _client;
        private bool _isRunning;
        public HandData CurrentHandData { get; private set; }
        public bool IsReceiveFirstData { get; private set; }

        private void OnDestroy()
        {
            _isRunning = false;
            _client.Close();
            _client.Dispose();
        }

        public void Initialize()
        {
            _client = new UdpClient(port);

            _isRunning = true;
            ReceiveTask().Forget();
        }

        private async UniTask ReceiveTask()
        {
            IsReceiveFirstData = false;
            while (_isRunning)
            {
                var result = await _client.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                CurrentHandData = JsonUtility.FromJson<HandData>(message);
                IsReceiveFirstData = true;
            }
        }

        [Serializable]
        public struct HandData
        {
            public float2[] positions;
        }
    }
}