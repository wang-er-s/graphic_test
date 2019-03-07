using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _3DDataType.RenderData
{
    static class Tools
    {

        private static Matrix4x4 worldMatrix;
        public static Matrix4x4 WorldMatrix
        {
            get
            {
                if (worldMatrix == new Matrix4x4())
                {
                    worldMatrix = Matrix4x4.Translate(new Vector3(0, 0, 10)) * Matrix4x4.Rotate(new Vector3(0, 0, 0));
                }
                return worldMatrix;
            }
        }

        public static void BresenhamDrawLine(Vertex p1, Vertex p2,ref Bitmap bitmap)
        {
            int x = (int)(System.Math.Round(p1.point.x, MidpointRounding.AwayFromZero));
            int y = (int)(System.Math.Round(p1.point.y, MidpointRounding.AwayFromZero));
            int dx = (int)(System.Math.Round(p2.point.x - p1.point.x, MidpointRounding.AwayFromZero));
            int dy = (int)(System.Math.Round(p2.point.y - p1.point.y, MidpointRounding.AwayFromZero));
            int stepx = 1;
            int stepy = 1;

            if (dx >= 0)
            {
                stepx = 1;
            }
            else
            {
                stepx = -1;
                dx = System.Math.Abs(dx);
            }

            if (dy >= 0)
            {
                stepy = 1;
            }
            else
            {
                stepy = -1;
                dy = System.Math.Abs(dy);
            }

            int dx2 = 2 * dx;
            int dy2 = 2 * dy;

            if (dx > dy)
            {
                int error = dy2 - dx;
                for (int i = 0; i <= dx; i++)
                {
                    bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                    if (error >= 0)
                    {
                        error -= dx2;
                        y += stepy;
                    }
                    error += dy2;
                    x += stepx;

                }
            }
            else
            {
                int error = dx2 - dy;
                for (int i = 0; i <= dy; i++)
                {
                    //_frameBuff.SetPixel(x, y, System.Drawing.Color.White);
                    if (error >= 0)
                    {
                        error -= dy2;
                        x += stepx;
                    }
                    error += dx2;
                    y += stepy;

                }
            }

        }

        //private static void DrawTriangle(Vertex p1, Vertex p2, Vertex p3, Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        //{
        //    //--------------------几何阶段---------------------------

        //    //将三个顶点先变换到世界空间再变换到相机空间
        //    //SetMVTransform(m, v, ref p1);
        //    //SetMVTransform(m, v, ref p2);
        //    //SetMVTransform(m, v, ref p3);

        //    //在相机空间进行背面消隐
        //    if (Camera.BackFaceCulling(p1, p2, p3) == false)
        //    {
        //        return;
        //    }

        //    //将三个顶点变换到齐次剪裁空间
        //    SetProjectionTransform(p, ref p1);
        //    SetProjectionTransform(p, ref p2);
        //    SetProjectionTransform(p, ref p3);

        //    //裁剪
        //    if (Clip(p1) == false || Clip(p2) == false || Clip(p3) == false)
        //    {
        //        return;
        //    }

        //    //变换到屏幕坐标
        //    TransformToScreen(ref p1);
        //    TransformToScreen(ref p2);
        //    TransformToScreen(ref p3);

        //    //--------------------光栅化阶段---------------------------

        //    if (_currentMode == RenderMode.Wireframe)
        //    {//线框模式
        //        BresenhamDrawLine(p1, p2);
        //        BresenhamDrawLine(p2, p3);
        //        BresenhamDrawLine(p3, p1);
        //    }
        //    else
        //    {
        //        TriangleRasterization(p1, p2, p3);
        //    }
        //}

        static void DrawDDALine(Vertex start,Vertex end)
        {
            int x0 =(int) start.point.x;
            int y0 =(int) start.point.y;
            int x1 =(int) end.point.x;
            int y1 =(int) end.point.y;
            Color color = new Color(255, 0, 0);
            int x;
            float dx, dy, y, k;
            dx = x1 - x0;
            dy = y1 - y0;
            k = dy / dx;
            y = y0;
            float x2 = start.point.x;
            float y2 = end.point.x;
            for (x = x0; x <= x1; x++)
            {   //(20,360)是坐标轴的起点
                x2 = 20 + x * 30; //30是坐标轴的单位刻度
                //pDC->Ellipse(x2 - 5, y2 - 5, x2 + 5, y2 + 5);
                y = k * x + k;
                y = (int)(y + 0.5);
                y2 = 360 - y * 30;
            }
        }

    }
}
