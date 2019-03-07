using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DDataType.RenderData
{
    class Mesh
    {
        public Vertex[] vertices { get; private set; }
        public Material material { get; private set; }
        public Mesh(Vector3[] pointList, int[] indexs, Vector2[] Uvs, Vector3[] vertColors, Vector3[] normals, Material mat)
        {
            vertices = new Vertex[indexs.Length];
            //生成顶点列表
            for (int i = 0; i < indexs.Length; i++)
            {
                int pointIndex = indexs[i];
                Vector3 point = pointList[pointIndex];
                vertices[i] = new Vertex(point, normals[i], Uvs[i].x, Uvs[i].y, vertColors[i].x, vertColors[i].y, vertColors[i].z);
            }
            material = mat;
        }
    }
}
