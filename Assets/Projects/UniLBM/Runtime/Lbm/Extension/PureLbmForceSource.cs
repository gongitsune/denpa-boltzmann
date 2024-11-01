using Unity.Mathematics;

namespace Projects.UniLBM.Runtime.Lbm.Extension
{
    public class PureLbmForceSource : ILbmForceSource
    {
        public float3 Force { get; set; }
        public uint3 CellSize { get; set; }
        public float3 Position { get; set; }
    }
}