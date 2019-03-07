using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    struct Color : IEquatable<Color>
    {

        public Color (float r,float g,float b)
        {
            this.r = Mathf.Clamp(r, 0, 1);
            this.g = Mathf.Clamp(g, 0, 1);
            this.b = Mathf.Clamp(b, 0, 1);
        }

        private float r;
        private float g;
        private float b;

        public float R
        {
            get { return r; }
            set { r = Mathf.Clamp(value, 0, 1); }
        }

        public float G
        {
            get { return g; }
            set { r = Mathf.Clamp(value, 0, 1); }
        }

        public float B
        {
            get { return b; }
            set { r = Mathf.Clamp(value, 0, 1); }
        }

        public Color(System.Drawing.Color c)
        {
            r = Mathf.Clamp((float)c.R / 255, 0, 1);
            g = Mathf.Clamp((float)c.G / 255, 0, 1);
            b = Mathf.Clamp((float)c.B / 255, 0, 1);
        }

        public static Color operator*(Color c1, Color c2)
        {
            Color c = new Color
            {
                r = c1.r * c2.r,
                g = c1.g * c2.g,
                b = c1.b * c2.b
            };
            return c;
        }

        public static Color operator *(Color c, float num)
        {
            Color temp = new Color
            {
                r = c.r * num,
                g = c.g * num,
                b = c.b * num
            };
            return temp;
        }

        public static Color operator +(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r + b.r;
            c.g = a.g + b.g;
            c.b = a.b + b.b;
            return c;
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.g;
        }

        public static bool operator !=(Color a, Color b)
        {
            return !(a == b);
        }

        public bool Equals(Color other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = r.GetHashCode();
                hashCode = (hashCode * 397) ^ g.GetHashCode();
                hashCode = (hashCode * 397) ^ b.GetHashCode();
                return hashCode;
            }
        }
    }
}
