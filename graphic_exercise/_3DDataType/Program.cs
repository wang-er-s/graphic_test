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
        }
    }
}