using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    struct Vertex
    {
        public Vector4 point;

        /// <summary>
        /// uv坐标
        /// </summary>
        public float u, v;

        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 normal;

        /// <summary>
        /// 顶点深度值，用1/z表示
        /// </summary>
        public float depth;

        public Color pointColor;

        public Color lightingColor;

        /// <summary>
        /// 1/z，用于顶点信息的透视校正
        /// </summary>
        public float onePerZ;

        public Vertex(Vector4 point, Vector3 normal, float u, float v, float r, float g, float b)
        {
            this.point = point;
            this.point.w = 1;
            this.normal = normal;
            pointColor = new Color(r,g,b);
            onePerZ = 1;
            this.u = u;
            this.v = v;
            lightingColor = new Color(r,g,b);
            depth = 1;
        }

        public Vertex(Vertex v)
        {
            point = v.point;
            normal = v.normal;
            this.pointColor = v.pointColor;
            onePerZ = 1;
            this.u = v.u;
            this.v = v.v;
            this.lightingColor = v.lightingColor;
            depth = 1;
        }

    }
}
