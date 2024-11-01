using Projects.UniLBM.Runtime.Lbm;
using UnityEngine;

namespace Projects.UniLBM.Runtime.Common
{
    /// <summary>
    ///     シミュレーションの管理を行うクラス
    /// </summary>
    public class UniSimulator : MonoBehaviour
    {
        private LbmForceSourceManager _forceSourceManager;
        private ILbmSolver _lbmSolver;
        private LbmParticle _particle;

        #region Unity Callback

        private void OnDestroy()
        {
            _lbmSolver?.Dispose();
            _particle?.Dispose();
            _forceSourceManager?.Dispose();
        }

        #endregion

        public void Initialize()
        {
            _lbmSolver = new D3Q19LbmSolver(lbmShader, cellResolution, new D3Q19LbmSolver.Data
            {
                Tau = tau
            });
            _particle = new LbmParticle(particleShader, _lbmSolver, particleMaterial, oneSideParticleNum,
                new LbmParticle.Data
                {
                    ParticleSpeed = particleSpeed,
                    MaxLifetime = maxLifetime,
                    ParticleLayer = particleLayer
                });
            _forceSourceManager =
                new LbmForceSourceManager(forceSourceShader, _lbmSolver, _particle, forceSourceRoot);
        }

        public void Simulate()
        {
            _particle.SetData(new LbmParticle.Data
            {
                ParticleSpeed = particleSpeed,
                MaxLifetime = maxLifetime
            });

            _lbmSolver.ResetField();
            _forceSourceManager.Update();
            _lbmSolver.Update();
            _particle.Update(1 / 60f);
        }

        #region Serialize Field

        [SerializeField] private ComputeShader lbmShader;
        [SerializeField] private uint cellResolution = 128;
        [SerializeField] private float tau = 1.2f;

        [SerializeField] private ComputeShader particleShader;
        [SerializeField] private Material particleMaterial;
        [SerializeField] private uint oneSideParticleNum = 100;
        [SerializeField] private float particleSpeed = 0.1f;
        [SerializeField] private float maxLifetime = 10f;
        [SerializeField] private int particleLayer;

        [SerializeField] private ComputeShader forceSourceShader;
        [SerializeField] private ForceSourceRoot forceSourceRoot;

        #endregion
    }
}