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
            return (float)Math.Sin((double)f);
        }

        public static float Cos(float f)
        {
            return (float)Math.Cos((double)f);
        }

        public static float Asin(float f)
        {
            return (float)Math.Asin((double)f);
        }

        public static float Acos(float f)
        {
            return (float)Math.Acos((double)f);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt((double)f);
        }

        public static float Clamp(float value,float min,float max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        public static float Max(float a,float b)
        {
            return a > b ? a : b;
        }
    }
}
