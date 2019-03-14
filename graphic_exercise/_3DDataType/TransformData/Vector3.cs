using System;
using System.Collections;

namespace _3DDataType
{
    public struct Vector3 : IEquatable<Vector3>
    {

        private static readonly Vector3 zeroVector3 = new Vector3(0.0f, 0.0f, 0.0f);
        private static readonly Vector3 oneVector3 = new Vector3(1, 1, 1);
        private static readonly Vector3 upVector = new Vector3(0.0f, 1f, 0.0f);
        private static readonly Vector3 downVector = new Vector3(0.0f, -1f, 0.0f);
        private static readonly Vector3 leftVector = new Vector3(-1f, 0.0f, 0.0f);
        private static readonly Vector3 rightVector = new Vector3(1f, 0.0f, 0.0f);
        private static readonly Vector3 forwardVector = new Vector3(0.0f, 0.0f, 1f);
        private static readonly Vector3 backVector = new Vector3(0.0f, 0.0f, -1f);

        public static Vector3 Zero => zeroVector3;
        public static Vector3 One => oneVector3;
        public static Vector3 Forward => forwardVector;
        public static Vector3 Back => backVector;
        public static Vector3 Up => upVector;
        public static Vector3 Down => downVector;
        public static Vector3 Left => leftVector;
        public static Vector3 Right => rightVector;


        public float x;
        public float y;
        public float z;

        public float magnitude => (float)Math.Sqrt(Dot(this, this));

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return (float)(v1.x * (double)v2.x + v1.y * (double)v2.y + v1.z * (double)v2.z);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 r2)
        {
            return new Vector3((float)(v1.y * (double)r2.z - v1.z * (double)r2.y),
                (float)(v1.z * (double)r2.x - v1.x * (double)r2.z),
                (float)(v1.x * (double)r2.y - v1.y * (double)r2.x));
        }

        public Vector3(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vector3 operator *(Vector3 v1, float num)
        {
            return new Vector3(v1.x * num, v1.y * num, v1.z * num);
        }

        public static Vector3 operator /(Vector3 v1, float num)
        {
            return new Vector3(v1.x / num, v1.y / num, v1.z / num);
        }

        public static Vector3 operator -(Vector3 v1)
        {
            return new Vector3(-v1.x, -v1.y, -v1.z);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return (v1 - v2).magnitude < 1e-7;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static implicit operator Vector3(Vector2 v2)
        {
            Vector3 s = new Vector3(v2.x, v2.y, 0);
            return s;
        }

        public static implicit operator Vector3(Vector4 v2)
        {
            Vector3 s = new Vector3(v2.x, v2.y, v2.z);
            return s;
        }

        public bool Equals(Vector3 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }

        public Vector3 Normalize()
        {
            if (magnitude > 0)
                this /= magnitude;
            else
                this = Zero;
            return this;
        }

        public override string ToString()
        {
            return $"x={x} y={y} z={z}";
        }
    }
}