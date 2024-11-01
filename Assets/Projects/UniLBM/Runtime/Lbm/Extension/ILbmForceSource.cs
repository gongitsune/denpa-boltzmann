﻿using Unity.Mathematics;

namespace Projects.UniLBM.Runtime.Lbm.Extension
{
    /// <summary>
    ///     LBMの力源を表すコンポーネントのインターフェース
    /// </summary>
    public interface ILbmForceSource
    {
        public float3 Force { get; }
        public uint3 CellSize { get; }
        public float3 Position { get; }
    }
}