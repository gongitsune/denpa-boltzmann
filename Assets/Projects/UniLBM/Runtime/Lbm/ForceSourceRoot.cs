using System.Collections.Generic;
using Projects.UniLBM.Runtime.Lbm.Extension;
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
            // source1.Position
        }

        public T AddSource<T>() where T : Component, ILbmForceSource
        {
            if (Sources.Count >= LbmForceSourceManager.MaxSourceCount) return null;

            var source = new GameObject().AddComponent<T>();
            _sources.Add(source);
            return source;
        }

        public void RemoveSource(ILbmForceSource source)
        {
            _sources.Remove(source);
        }
    }
}