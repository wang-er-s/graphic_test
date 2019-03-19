using _3DDataType.RenderData;
using System;
using System.Drawing;
using System.Windows.Forms;
using Color = _3DDataType.RenderData.Color;

namespace _3DDataType
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RenderDemo());

            //     Matrix4x4 m1 = new Matrix4x4
            //(
            //    1, 1, 2, 3,
            //    3, 1, 2, 3,
            //    2, 3, 1, 3,
            //    5, 5, 5, 1
            //);
            //     m1.Inverse();


        }
    }
}