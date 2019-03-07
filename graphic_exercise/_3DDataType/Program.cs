using _3DDataType.RenderData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3DDataType
{
    class Program
    {
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new RenderDemo());
            Matrix4x4 m1 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7);
            Matrix4x4 m2 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7);
            Matrix4x4 m3 = m1 * m2;
            Vector4 v1 = new Vector4(3, 4, 5, 6);
            Vector4 v2 = m3 * v1;
            float f1 = m3.GetDeterminant();
            Matrix4x4 m4 = m3.Inverse();

            Vector4 v3 = new Vector4(1, 2, 3);
            Matrix4x4 m5 = Camera.BuildViewMatrix(v3, new Vector4(0, 1, 0), new Vector4(0, 0, 1));
            Console.WriteLine(11);
        }
    }
}