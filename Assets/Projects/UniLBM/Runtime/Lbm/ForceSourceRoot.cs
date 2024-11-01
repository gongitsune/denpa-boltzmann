using System.Collections.Generic;
using Projects.UniLBM.Runtime.Lbm.Extension;
using Unity.Mathematics;
using UnityEngine;

namespace Projects.UniLBM.Runtime.Lbm
{
    public class ForceSourceRoot : MonoBehaviour
    {
        private readonly List<ILbmForceSource> _sources = new();
        public IReadOnlyList<ILbmForceSource> Sources => _sources;

        private void Start()
        {
            var source1 = AddSource<LbmForceSource>();
            source1.Position = new float3(10, 10, 10);
            source1.SetForce(0.03f);
        }

        public T AddSource<T>() where T : Component, ILbmForceSource
        {
            if (Sources.Count >= LbmForceSourceManager.MaxSourceCount) return null;

            var source = new GameObject().AddComponent<T>();
            source.transform.SetParent(transform);
            _sources.Add(source);
            return source;
        }

        public void RemoveSource(ILbmForceSource source)
        {
            _sources.Remove(source);
        }
    }
}