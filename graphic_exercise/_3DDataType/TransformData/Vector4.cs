using System;

namespace _3DDataType
{
    public struct Vector4 : IEquatable<Vector4>
    {
        private static readonly Vector4 zeroVector4 = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        private static readonly Vector4 oneVector4 = new Vector4(1, 1, 1, 1);

        public static Vector4 Zero => zeroVector4;
        public static Vector4 One => oneVector4;


        public float x;
        public float y;
        public float z;
        public float w;

        private float magnitude => (float) Math.Sqrt(Dot(this, this));

        public static Vector4 Cross(Vector4 lhs, Vector4 rhs)
        {
            float x = lhs.y * rhs.z - lhs.z * rhs.y;
            float y = lhs.z * rhs.x - lhs.x * rhs.z;
            float z = lhs.x * rhs.y - lhs.y * rhs.x;
            return new Vector4(x, y, z);
        }

        public static float Dot(Vector4 v1, Vector4 v2)
        {
            return (float) (v1.x *  v2.x + v1.y *  v2.y + v1.z *  v2.z + v1.w * v2.w);
        }

        public Vector4(float x = 0.0f, float y = 0.0f, float z = 0.0f, float w = 0.0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        public static Vector4 operator /(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }

        public static Vector4 operator *(Vector4 v1, float num)
        {
            return new Vector4(v1.x * num, v1.y * num, v1.z * num, v1.w * num);
        }

        public static Vector4 operator /(Vector4 v1, float num)
        {
            return new Vector4(v1.x / num, v1.y / num, v1.z / num, v1.w / num);
        }

        public static Vector4 operator -(Vector4 v1)
        {
            return new Vector4(-v1.x, -v1.y, -v1.z, -v1.w);
        }

        public static bool operator ==(Vector4 v1, Vector4 v2)
        {
            return (v1 - v2).magnitude < 1e-7;
        }

        public static bool operator !=(Vector4 v1, Vector4 v2)
        {
            return !(v1 == v2);
        }

        public static implicit operator Vector4(Vector2 v2)
        {
            Vector4 s = new Vector4(v2.x, v2.y, 0, 0);
            return s;
        }

        public static implicit operator Vector4(Vector3 v2)
        {
            Vector4 s = new Vector4(v2.x, v2.y, v2.z, 0);
            return s;
        }

        public bool Equals(Vector4 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }

        public Vector4 Normalize()
        {
            if (magnitude > 0)
                this /= this / magnitude;
            else
                this = Zero;
            return this;
        }
    }
}