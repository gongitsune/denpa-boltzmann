using System.Collections.Generic;
using Projects.UniLBM.Runtime.Lbm.Extension;
using UnityEngine;

namespace Projects.UniLBM.Runtime.Lbm
{
    public class ForceSourceRoot : MonoBehaviour
    {
        private readonly List<ILbmForceSource> _sources = new();
        public IReadOnlyList<ILbmForceSource> Sources => _sources;

        public bool AddSource(ILbmForceSource source)
        {
            if (Sources.Count >= LbmForceSourceManager.MaxSourceCount) return false;

            _sources.Add(source);
            return true;
        }

        public void ClearSources()
        {
            _sources.Clear();
        }

        public void RemoveSource(ILbmForceSource source)
        {
            _sources.Remove(source);
        }
    }
}