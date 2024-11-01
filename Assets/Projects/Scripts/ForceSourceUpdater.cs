using System;
using Projects.UniLBM.Runtime.Lbm;
using Projects.UniLBM.Runtime.Lbm.Extension;
using Unity.Mathematics;
using UnityEngine;

namespace Projects.Scripts
{
    public class ForceSourceUpdater : MonoBehaviour
    {
        [SerializeField] private float lbmDrawSize = 100;
        [SerializeField] private float3 offset;
        [SerializeField] private float3 dir = new(0, 0, 1);
        [SerializeField] private float forcePower = 0.03f;

        private ForceSourceRoot _forceSourceRoot;

        public void Initialize(ForceSourceRoot forceSourceRoot)
        {
            _forceSourceRoot = forceSourceRoot;
        }

        public void UpdateForceSource(HandTrackingClient.HandData data)
        {
            _forceSourceRoot.ClearSources();

            for (var i = 0; i < data.positions.Length; i++)
            {
                var normHandPos = new float3(data.positions[i].x, data.positions[i].y, 0);
                var forceSource = new PureLbmForceSource
                {
                    Position = normHandPos * lbmDrawSize + offset,
                    Force = math.normalize(dir) * forcePower
                };
                _forceSourceRoot.AddSource(forceSource);
            }
        }
    }
}