using _3DDataType.RenderData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DDataType.Test
{
 static class CubeTestData
 {
  //顶点坐标
  public static Vector4[] PointList =
  {
   new Vector4(-1, 1, -1, 1),
   new Vector4(-1, -1, -1, 1),
   new Vector4(1, -1, -1, 1),
   new Vector4(1, 1, -1, 1),

   new Vector4(-1, 1, 1, 1),
   new Vector4(-1, -1, 1, 1),
   new Vector4(1, -1, 1, 1),
   new Vector4(1, 1, 1, 1)
  };

  //三角形顶点索引 12个面
  public static int[] Indexs =
  {
   0, 3, 2,
   0, 2, 1,
   //
   7, 4, 5,
   7, 5, 6,
   //
   4, 0, 1,
   4, 1, 5,
   //
   4, 7, 3,
   4, 3, 0,
   //
   6, 5, 1,
   6, 1, 2,
   //
   3, 7, 6,
   3, 6, 2
  };

  //uv坐标
  public static Vector2[] UVs =
  {
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),
   //
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),
   //
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),
   //
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),
   //
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),
   //
   new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
   new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0),

  };

  public static Vector3[] VertColors =
  {
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
   //
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
   //
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
   //
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
   //
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
   //
   new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
   new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1)
  };

  //法线
  public static  readonly  Vector4[] Normals =
  {
   //前032 021
   new Vector3(-0.5773503f, 0.5773503f, -0.5773503f), new Vector3(0.5773503f, 0.5773503f, -0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, 0.5773503f, -0.5773503f), new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
   //后 745 756
   new Vector3(0.5773503f, 0.5773503f, 0.5773503f), new Vector3(-0.5773503f, 0.5773503f, 0.5773503f),
   new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, 0.5773503f, 0.5773503f), new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, 0.5773503f),
   //左401 415
   new Vector3(-0.5773503f, 0.5773503f, 0.5773503f), new Vector3(-0.5773503f, 0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, 0.5773503f, 0.5773503f), new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
   //上 473 430
   new Vector3(-0.5773503f, 0.5773503f, 0.5773503f), new Vector3(0.5773503f, 0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, 0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, 0.5773503f, 0.5773503f), new Vector3(0.5773503f, 0.5773503f, -0.5773503f),
   new Vector3(-0.5773503f, 0.5773503f, -0.5773503f),
   //下 651 612
   new Vector3(0.5773503f, -0.5773503f, 0.5773503f), new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
   new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, 0.5773503f), new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
   //右 376 362
   new Vector3(0.5773503f, 0.5773503f, -0.5773503f), new Vector3(0.5773503f, 0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, 0.5773503f, -0.5773503f), new Vector3(0.5773503f, -0.5773503f, 0.5773503f),
   new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
  };

  //材质
  public static Material mat = new Material(new Color(0, 0, 0.1f), 0.1f, new Color(0.3f, 0.3f, 0.3f),
   new Color(1, 1, 1), 99);

 }
}
