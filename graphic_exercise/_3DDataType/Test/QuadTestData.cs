using _3DDataType.RenderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DDataType.Test
{
    class QuadTestData
    {
        //顶点坐标
        public static Vector3[] PointList =
        {
            new Vector3(-1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
        };

        //三角形顶点索引 12个面
        public static int[] Indexs =
        {
            0, 1, 2,
            0, 2, 3,
        };

        //uv坐标
        public static Vector2[] UVs =
        {
            new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0),
        };

        //顶点色
        public static Vector3[] VertColors =
        {
            new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
            new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
        };

        //法线
        public static Vector3[] Normals =
        {
            new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1),
            new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1),
        };

        //材质
        public static Material Mat = new Material(new Color(0, 0, 0.1f), 0.1f, new Color(0.3f, 0.3f, 0.3f),
            new Color(1, 1, 1), 99);

    }
}
