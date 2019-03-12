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
            //for (float i = 0; i < 10; i+=0.05f)
            //{
            //    Console.WriteLine(i +"   "+ Math.Cos(i));
            //}
            //Console.ReadKey();
            //ss
        }
    }
}