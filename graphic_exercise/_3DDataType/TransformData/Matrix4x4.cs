using System;

namespace _3DDataType
{
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public static readonly Matrix4x4 zeroMatrix = new Matrix4x4(new Vector4(0.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 0.0f));

        public static readonly Matrix4x4 identityMatrix = new Matrix4x4(new Vector4(1f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1f));

        private float m00;
        private float m10;
        private float m20;
        private float m30;
        private float m01;
        private float m11;
        private float m21;
        private float m31;
        private float m02;
        private float m12;
        private float m22;
        private float m32;
        private float m03;
        private float m13;
        private float m23;
        private float m33;

        public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m03 = column3.x;
            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
            m13 = column3.y;
            m20 = column0.z;
            m21 = column1.z;
            m22 = column2.z;
            m23 = column3.z;
            m30 = column0.w;
            m31 = column1.w;
            m32 = column2.w;
            m33 = column3.w;
        }

        public Matrix4x4(float a1, float b1, float c1, float d1,
            float a2, float b2, float c2, float d2,
            float a3, float b3, float c3, float d3,
            float a4, float b4, float c4, float d4)
        {
            m00 = a1;
            m01 = b1;
            m02 = c1;
            m03 = d1;
            m10 = a2;
            m11 = b2;
            m12 = c2;
            m13 = d2;
            m20 = a3;
            m21 = b3;
            m22 = c3;
            m23 = d3;
            m30 = a4;
            m31 = b4;
            m32 = c4;
            m33 = d4;
        }

        public void Set(float[,] arr)
        {
            if (arr.GetLength(0) != 4 || arr.GetLength(1) != 4) return;
            Vector4[] vecs = new Vector4[4];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                vecs[i] = new Vector4(arr[i, 0], arr[i, 1], arr[i, 2], arr[i, 3]);
            }

            Set(vecs[0], vecs[1], vecs[2], vecs[3]);
        }



        public void Set(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            this = new Matrix4x4(column0, column1, column2, column3);
        }

        public float this[int row, int column]
        {
            get => this[row + column * 4]; 
            set => this[row + column * 4] = value;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.m00;
                    case 1:
                        return this.m10;
                    case 2:
                        return this.m20;
                    case 3:
                        return this.m30;
                    case 4:
                        return this.m01;
                    case 5:
                        return this.m11;
                    case 6:
                        return this.m21;
                    case 7:
                        return this.m31;
                    case 8:
                        return this.m02;
                    case 9:
                        return this.m12;
                    case 10:
                        return this.m22;
                    case 11:
                        return this.m32;
                    case 12:
                        return this.m03;
                    case 13:
                        return this.m13;
                    case 14:
                        return this.m23;
                    case 15:
                        return this.m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.m00 = value;
                        break;
                    case 1:
                        this.m10 = value;
                        break;
                    case 2:
                        this.m20 = value;
                        break;
                    case 3:
                        this.m30 = value;
                        break;
                    case 4:
                        this.m01 = value;
                        break;
                    case 5:
                        this.m11 = value;
                        break;
                    case 6:
                        this.m21 = value;
                        break;
                    case 7:
                        this.m31 = value;
                        break;
                    case 8:
                        this.m02 = value;
                        break;
                    case 9:
                        this.m12 = value;
                        break;
                    case 10:
                        this.m22 = value;
                        break;
                    case 11:
                        this.m32 = value;
                        break;
                    case 12:
                        this.m03 = value;
                        break;
                    case 13:
                        this.m13 = value;
                        break;
                    case 14:
                        this.m23 = value;
                        break;
                    case 15:
                        this.m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public bool Equals(Matrix4x4 other)
        {
            return GetColumn(0).Equals(other.GetColumn(0)) && this.GetColumn(1).Equals(other.GetColumn(1)) &&
                   this.GetColumn(2).Equals(other.GetColumn(2)) && this.GetColumn(3).Equals(other.GetColumn(3));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Matrix4x4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m00.GetHashCode();
                hashCode = hashCode ^ m10.GetHashCode();
                hashCode = hashCode ^ m20.GetHashCode();
                hashCode = hashCode ^ m30.GetHashCode();
                hashCode = hashCode ^ m01.GetHashCode();
                hashCode = hashCode ^ m11.GetHashCode();
                hashCode = hashCode ^ m21.GetHashCode();
                hashCode = hashCode ^ m31.GetHashCode();
                hashCode = hashCode ^ m02.GetHashCode();
                hashCode = hashCode ^ m12.GetHashCode();
                hashCode = hashCode ^ m22.GetHashCode();
                hashCode = hashCode ^ m32.GetHashCode();
                hashCode = hashCode ^ m03.GetHashCode();
                hashCode = hashCode ^ m13.GetHashCode();
                hashCode = hashCode ^ m23.GetHashCode();
                hashCode = hashCode ^ m33.GetHashCode();
                return hashCode;
            }
        }

        public Vector4 GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(this.m00, this.m10, this.m20, this.m30);
                case 1:
                    return new Vector4(this.m01, this.m11, this.m21, this.m31);
                case 2:
                    return new Vector4(this.m02, this.m12, this.m22, this.m32);
                case 3:
                    return new Vector4(this.m03, this.m13, this.m23, this.m33);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(this.m00, this.m01, this.m02, this.m03);
                case 1:
                    return new Vector4(this.m10, this.m11, this.m12, this.m13);
                case 2:
                    return new Vector4(this.m20, this.m21, this.m22, this.m23);
                case 3:
                    return new Vector4(this.m30, this.m31, this.m32, this.m33);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        public static Matrix4x4 TRS(Vector3 pos, Vector3 eulerAngles, Vector3 scale)
        {
            Matrix4x4 mat = Translate(pos);
            mat = mat * Rotate(eulerAngles) * Scale(scale);
            return mat;
        }

        public static Matrix4x4 Rotate(Vector3 euler)
        {
            return RotateY(euler.y) * RotateX(euler.x) * RotateZ(euler.z);
        }

        public static Matrix4x4 Translate(Vector3 vector)
        {
            Matrix4x4 matrix4x4 = identityMatrix;
            matrix4x4.m00 = 1f;
            matrix4x4.m01 = 0.0f;
            matrix4x4.m02 = 0.0f;
            matrix4x4.m03 = vector.x;
            matrix4x4.m10 = 0.0f;
            matrix4x4.m11 = 1f;
            matrix4x4.m12 = 0.0f;
            matrix4x4.m13 = vector.y;
            matrix4x4.m20 = 0.0f;
            matrix4x4.m21 = 0.0f;
            matrix4x4.m22 = 1f;
            matrix4x4.m23 = vector.z;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m32 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        public static Matrix4x4 RotateX(float rad)
        {
            float sin = Mathf.Sin(rad);
            float cos = (float) Math.Cos(rad);
            Matrix4x4 mat = identityMatrix;
            mat[1, 1] = cos;
            mat[1, 2] = -sin;
            mat[2, 1] = sin;
            mat[2, 2] = cos;
            return mat;
        }

        public static Matrix4x4 RotateY(float rad)
        {
            float sin = Mathf.Sin(rad);
            float cos = (float) Math.Cos(rad);
            Matrix4x4 mat = identityMatrix;
            mat[2, 2] = cos;
            mat[2, 0] = -sin;
            mat[0, 2] = sin;
            mat[0, 0] = cos;

            return mat;
        }
  

        public static Matrix4x4 RotateZ(float rad)
        {
            float sin = (float) Math.Sin(rad);
            float cos = (float) Math.Cos(rad);
            Matrix4x4 mat = identityMatrix;
            mat[0, 0] = cos;
            mat[0, 1] = -sin;
            mat[1, 0] = sin;
            mat[1, 1] = cos;
            return mat;
        }

        /// <summary>
        /// 绕任意轴旋转
        /// </summary>
        /// <param name="axis">轴向量</param>
        /// <param name="angle">弧度</param>
        /// <returns></returns>
        public static Matrix4x4 ArbitraryAxis(Vector4 axis, float angle)
        {
            Matrix4x4 a = new Matrix4x4();
            //第一列
            a[0, 0] = (float)(axis.x * axis.x * (1 - Math.Cos(angle)) + Math.Cos(angle));
            a[1, 0] = (float)(axis.x * axis.y * (1 - Math.Cos(angle)) - axis.z * Math.Sin(angle));
            a[2, 0] = (float)(axis.x * axis.z * (1 - Math.Cos(angle)) + axis.y * Math.Sin(angle));
            a[3, 0] = 0;
            //第二列
            a[0, 1] = (float)(axis.x * axis.y * (1 - Math.Cos(angle)) + axis.z * Math.Sin(angle));
            a[1, 1] = (float)(axis.y * axis.y * (1 - Math.Cos(angle)) + Math.Cos(angle));
            a[2, 1] = (float)(axis.y * axis.z * (1 - Math.Cos(angle)) - axis.x * Math.Sin(angle));
            a[3, 1] = 0;
            //第三列
            a[0, 2] = (float)(axis.x * axis.z * (1 - Math.Cos(angle)) - axis.y * Math.Sin(angle));
            a[1, 2] = (float)(axis.y * axis.z * (1 - Math.Cos(angle)) + axis.x * Math.Sin(angle));
            a[2, 2] = (float)(axis.z * axis.z * (1 - Math.Cos(angle)) + Math.Cos(angle));
            a[3, 2] = 0;
            //第四列
            a[0, 3] = 0;
            a[1, 3] = 0;
            a[2, 3] = 0;
            a[3, 3] = 1;
            return a;
        }

        public static Matrix4x4 Scale(Vector3 scale)
        {
            Matrix4x4 mat = identityMatrix;
            mat[0, 0] = scale.x;
            mat[1, 1] = scale.y;
            mat[2, 2] = scale.z;
            return mat;
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 matrix4X4 = identityMatrix;

            matrix4X4.m00 = (float) (m1.m00 * (double) m2.m00 + m1.m01 * (double) m2.m10 +
                                     m1.m02 * (double) m2.m20 + m1.m03 * (double) m2.m30);
            matrix4X4.m01 = (float) (m1.m00 * (double) m2.m01 + m1.m01 * (double) m2.m11 +
                                     m1.m02 * (double) m2.m21 + m1.m03 * (double) m2.m31);
            matrix4X4.m02 = (float) (m1.m00 * (double) m2.m02 + m1.m01 * (double) m2.m12 +
                                     m1.m02 * (double) m2.m22 + m1.m03 * (double) m2.m32);
            matrix4X4.m03 = (float) (m1.m00 * (double) m2.m03 + m1.m01 * (double) m2.m13 +
                                     m1.m02 * (double) m2.m23 + m1.m03 * (double) m2.m33);
            matrix4X4.m10 = (float) (m1.m10 * (double) m2.m00 + m1.m11 * (double) m2.m10 +
                                     m1.m12 * (double) m2.m20 + m1.m13 * (double) m2.m30);
            matrix4X4.m11 = (float) (m1.m10 * (double) m2.m01 + m1.m11 * (double) m2.m11 +
                                     m1.m12 * (double) m2.m21 + m1.m13 * (double) m2.m31);
            matrix4X4.m12 = (float) (m1.m10 * (double) m2.m02 + m1.m11 * (double) m2.m12 +
                                     m1.m12 * (double) m2.m22 + m1.m13 * (double) m2.m32);
            matrix4X4.m13 = (float) (m1.m10 * (double) m2.m03 + m1.m11 * (double) m2.m13 +
                                     m1.m12 * (double) m2.m23 + m1.m13 * (double) m2.m33);
            matrix4X4.m20 = (float) (m1.m20 * (double) m2.m00 + m1.m21 * (double) m2.m10 +
                                     m1.m22 * (double) m2.m20 + m1.m23 * (double) m2.m30);
            matrix4X4.m21 = (float) (m1.m20 * (double) m2.m01 + m1.m21 * (double) m2.m11 +
                                     m1.m22 * (double) m2.m21 + m1.m23 * (double) m2.m31);
            matrix4X4.m22 = (float) (m1.m20 * (double) m2.m02 + m1.m21 * (double) m2.m12 +
                                     m1.m22 * (double) m2.m22 + m1.m23 * (double) m2.m32);
            matrix4X4.m23 = (float) (m1.m20 * (double) m2.m03 + m1.m21 * (double) m2.m13 +
                                     m1.m22 * (double) m2.m23 + m1.m23 * (double) m2.m33);
            matrix4X4.m30 = (float) (m1.m30 * (double) m2.m00 + m1.m31 * (double) m2.m10 +
                                     m1.m32 * (double) m2.m20 + m1.m33 * (double) m2.m30);
            matrix4X4.m31 = (float) (m1.m30 * (double) m2.m01 + m1.m31 * (double) m2.m11 +
                                     m1.m32 * (double) m2.m21 + m1.m33 * (double) m2.m31);
            matrix4X4.m32 = (float) (m1.m30 * (double) m2.m02 + m1.m31 * (double) m2.m12 +
                                     m1.m32 * (double) m2.m22 + m1.m33 * (double) m2.m32);
            matrix4X4.m33 = (float) (m1.m30 * (double) m2.m03 + m1.m31 * (double) m2.m13 +
                                     m1.m32 * (double) m2.m23 + m1.m33 * (double) m2.m33);
            return matrix4X4;
        }

        public static Vector4 operator *(Matrix4x4 m1, Vector4 vector)
        {
            Vector4 vector4;
            vector4.x = (float) (m1.m00 * (double) vector.x + m1.m01 * (double) vector.y +
                                 m1.m02 * (double) vector.z + m1.m03 * (double) vector.w);
            vector4.y = (float) (m1.m10 * (double) vector.x + m1.m11 * (double) vector.y +
                                 m1.m12 * (double) vector.z + m1.m13 * (double) vector.w);
            vector4.z = (float) (m1.m20 * (double) vector.x + m1.m21 * (double) vector.y +
                                 m1.m22 * (double) vector.z + m1.m23 * (double) vector.w);
            vector4.w = (float) (m1.m30 * (double) vector.x + m1.m31 * (double) vector.y +
                                 m1.m32 * (double) vector.z + m1.m33 * (double) vector.w);
            return vector4;
        }

        public static bool operator ==(Matrix4x4 m1, Matrix4x4 m2)
        {
            return m1.GetColumn(0) == m2.GetColumn(0) && m1.GetColumn(1) == m2.GetColumn(1) &&
                   m1.GetColumn(2) == m2.GetColumn(2) && m1.GetColumn(3) == m2.GetColumn(3);
        }

        public static bool operator !=(Matrix4x4 m1, Matrix4x4 m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// 求逆矩阵
        /// </summary>
        public Matrix4x4 Inverse()
        {
            Set(Inverse(ToArray()));
            return this;
        }

        private float[,] Inverse(float[,] arr)
        {
            float det = GetDeterminant(arr);
            if (det.Equals(0))
            {
                Console.WriteLine("行列式为0，没有逆矩阵");
            }
            else
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = i + 1; j < arr.GetLength(1); j++)
                    {
                        arr[j, i] /= det;
                    }
                }
            }

            return arr;
        }

        private float[,] ToArray()
        {
            float[,] result = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = this[i, j];
                }
            }

            return result;
        }

        private float GetDeterminant()
        {
            float[,] arr = GetArr();
            return GetDeterminant(arr);
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        public Matrix4x4 TransposeMatrix()
        {
            Set(TransposeMatrix(ToArray()));
            return this;
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private float[,] TransposeMatrix(float[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = i + 1; j < arr.GetLength(1); j++)
                {
                    float temp = arr[i, j];
                    arr[i, j] = arr[j, i];
                    arr[j, i] = temp;
                }
            }

            return arr;
        }

        #region 求行列式

        /// <summary>
        /// 求矩阵的行列式
        /// </summary>
        private static float GetDeterminant(float[,] arr)
        {
            if (arr.GetLength(0) <= 2) return GetSubArrDeterminant(arr, 0, 0);
            float num = 0;
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                num += GetSubArrDeterminant(arr, 0, i) * (float) Math.Pow(-1, i);
            }

            return num;
        }

        /// <summary>
        /// 得到余子式的行列式
        /// </summary>
        private static float GetSubArrDeterminant(float[,] arr, int x, int y)
        {
            if (arr.GetLength(0) <= 2)
            {
                return arr[0, 0] * arr[1, 1] - arr[0, 1] * arr[1, 0];
            }

            return arr[x, y] * GetDeterminant(GetSubArr(arr, x, y));
        }
        /// <summary>
        /// 得到余子式
        /// </summary>
        private static float[,] GetSubArr(float[,] arr, int x, int y)
        {
            if (arr.GetLength(0) <= 1) return arr;
            float[,] temp = new float[arr.GetLength(0) - 1, arr.GetLength(1) - 1];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                if (i == x) continue;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j == y) continue;
                    temp[i < x ? i : i - 1, j < y ? j : j - 1] = arr[i, j];
                }
            }

            return temp;
        }

        #endregion

        /// <summary>
        /// 求矩阵的伴随矩阵
        /// 求每个元素的代数余子式，组成一个矩阵
        /// </summary>
        private static float[,] GetAdjointMatrix(float[,] arr)
        {
            if (arr.GetLength(0) <= 2) return arr;
            float[,] result = new float[arr.GetLength(0), arr.GetLength(1)];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    float[,] temp = GetSubArr(arr, i, j);
                    result[i, j] = (float) Math.Pow(-1, i + j) * GetDeterminant(temp);
                }
            }

            return result;
        }


        private float[,] GetArr()
        {
            float[,] arr = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arr[i, j] = this[i, j];
                }
            }

            return arr;
        }
    }
}