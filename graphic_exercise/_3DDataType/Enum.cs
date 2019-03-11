﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DDataType
{
    /// <summary>
    /// 渲染模式
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// 线框模式
        /// </summary>
        Wireframe,
        /// <summary>
        /// 纹理
        /// </summary>
        Textured,
        /// <summary>
        /// 顶点色
        /// </summary>
        VertexColor
    }

    /// <summary>
    /// 光照模式
    /// </summary>
    public enum LightMode
    {
        ON,
        OFF
    }

    public enum TextColor
    {
        ON,
        OFF
    }

}
