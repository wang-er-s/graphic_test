using _3DDataType.RenderData;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType
{
    class Mathf
    {

        public const float PI = 3.14159274f;

        public const float Deg2Rad = 0.0174532924f;

        public const float Rad2Deg = 57.29578f;

        public static float d2r(float d)
        {
            var fl = Deg2Rad * d; //PI / 180 * d;
            return fl;
        }

        public static float r2d(float r)
        {
            var fl = Rad2Deg * r; //180/PI * r;
            return fl;
        }

        public static float Sin(float f)
        {
            return (float) Math.Sin((double) f);
        }

        public static float Cos(float f)
        {
            return (float) Math.Cos((double) f);
        }

        public static float Asin(float f)
        {
            return (float) Math.Asin((double) f);
        }

        public static float Acos(float f)
        {
            return (float) Math.Acos((double) f);
        }

        public static float Sqrt(float f)
        {
            return (float) Math.Sqrt((double) f);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            return value < min ? min : value;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static float Lerp(float a, float b, float lerp)
        {
            lerp = Clamp(lerp, 0, 1);
            return a + (b - a) * lerp;
        }

        public static void Lerp(ref Color c, Color c1, Color c2, float t)
        {
            if (t < 0)
            {
                t = 0;
            }
            else if (t > 1)
            {
                t = 1;
            }
            c.R = c1.R + (c2.R - c1.R) * t;
            c.G = c1.G + (c2.G - c1.G) * t;
            c.B = c1.B + (c2.B - c1.B) * t;
            //c.R = t * c2.R + (1 - t) * c1.R;
            //c.G = t * c2.G + (1 - t) * c1.G;
            //c.B = t * c2.B + (1 - t) * c1.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        public static void Lerp(ref Vertex v, Vertex v1, Vertex v2, float t)
        {
            //颜色插值
            Lerp(ref v.pointColor, v1.pointColor, v2.pointColor, t);
            //uv插值
            v.u = Lerp(v1.u, v2.u, t);
            v.v = Lerp(v1.v, v2.v, t);
            //光照颜色插值
            Lerp(ref v.lightingColor, v1.lightingColor, v2.lightingColor, t);
            //插值矫正系数
            v.onePerZ = Lerp(v1.onePerZ, v2.onePerZ, t);
            v.depth = Lerp(v1.depth, v2.depth, t);
        }
    }
}
