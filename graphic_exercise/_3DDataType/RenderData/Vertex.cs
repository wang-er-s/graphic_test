using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    public struct Vertex
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
        /// 1/z，用于顶点信息的透视校正，cvv裁切后，各种信息与1/z成线性关系
        /// </summary>
        public float onePerZ;

        public Color pointColor;

        public Color lightingColor;
        
        public float depth;

        public Vertex(Vector4 point, Vector3 normal, float u, float v, float r, float g, float b)
        {
            this.point = point;
            this.point.w = 1;
            this.normal = normal;
            this.u = u;
            this.v = v;
            pointColor = new Color(r, g, b);
            depth = 1;
            lightingColor = new Color(0, 0, 0);
            onePerZ = 1;
        }

        public Vertex(Vertex vertex)
        {
            point = vertex.point;
            normal = vertex.normal;
            pointColor = vertex.pointColor;
            depth = 1;
            u = vertex.u;
            v = vertex.v;
            lightingColor = vertex.lightingColor;
            onePerZ = 1;
        }

        public override string ToString()
        {
            return $"point={point} light={lightingColor} normal={normal} pointColor={pointColor}";
        }
    }
}
