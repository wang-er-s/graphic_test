using System;

namespace _3DDataType
{
    public struct Vector2 : IEquatable<Vector2>
    {
        private static readonly Vector2 zeroVector2 = new Vector2(0.0f, 0.0f);
        private static readonly Vector2 oneVector2 = new Vector2(1, 1);

        public static Vector2 Zero => zeroVector2;
        public static Vector2 One => oneVector2;


        public float x;
        public float y;

        public float magnitude => (float) Math.Sqrt(x * x + y * y );

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (float) ((double) v1.x * (double) v2.x + (double) v1.y * (double) v2.y);
        }

        public Vector2(float x = 0.0f, float y = 0.0f)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }
        
        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }
        
        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }
        
        public static Vector2 operator *(Vector2 v1, float num)
        {
            return new Vector2(v1.x * num, v1.y * num);
        }
        
        public static Vector2 operator /(Vector2 v1, float num)
        {
            return new Vector2(v1.x / num, v1.y / num);
        }

        public static Vector2 operator -(Vector2 v1)
        {
            return new Vector2(-v1.x, -v1.y);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).magnitude < 1e-7;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public static implicit operator Vector2(Vector3 v3)
        {
            Vector2 s = new Vector2(v3.x, v3.y);
            return s;
        }

        public static implicit operator Vector2(Vector4 v3)
        {
            Vector2 s = new Vector2(v3.x, v3.y);
            return s;
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                return hashCode;
            }
        }

        public void Normalize()
        {
            if (magnitude > 0)
                this /= this / magnitude;
            else
                this = Zero;
        } 
    }
}