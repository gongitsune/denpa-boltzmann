using Unity.Mathematics;
using UnityEngine;

namespace Projects.UniLBM.Runtime.Lbm.Extension
{
    /// <summary>
    ///     LBMの力源を表すコンポーネントの簡易実装
    /// </summary>
    public class LbmForceSource : MonoBehaviour, ILbmForceSource
    {
        private float _forcePower;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }

        public float3 Force => transform.forward * _forcePower;
        public uint3 CellSize => 1;

        public float3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetForce(float force)
        {
            _forcePower = force;
        }
    }
}