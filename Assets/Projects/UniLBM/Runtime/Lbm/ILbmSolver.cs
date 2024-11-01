﻿using System;
using UnityEngine;

namespace Projects.UniLBM.Runtime.Lbm
{
    public interface ILbmSolver : IDisposable
    {
        public GraphicsBuffer VelDensBuffer { get; }
        public GraphicsBuffer FieldBuffer { get; }
        public GraphicsBuffer FieldVelocityBuffer { get; }
        public GraphicsBuffer ExternalForceBuffer { get; }

        /// <summary>
        ///     シミュレーション解像度
        /// </summary>
        public int CellRes { get; }

        /// <summary>
        ///     シミュレーションの更新
        /// </summary>
        public void Update();

        /// <summary>
        ///     フィールド速度をリセット
        /// </summary>
        public void ResetField();
    }
}