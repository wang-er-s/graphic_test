using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    struct Camera
    {
        public Vector4 eyePosition;
        public Vector4 up;
        public Vector4 lookAt;

        /// <summary>
        /// 观察角，弧度
        /// </summary>
        public float fov;

        /// <summary>
        /// 纵横比 长宽比
        /// </summary>
        public float aspect;

        /// <summary>
        /// 近裁平面到原点的距离
        /// </summary>
        public float zn;

        /// <summary>
        /// 远裁平面到原点的距离
        /// </summary>
        public float zf;

        public Camera(Vector4 eyePosition, Vector4 up, Vector4 lookAt, float fov, float aspect, float zn, float zf)
        {
            this.eyePosition = eyePosition;
            this.up = up;
            this.lookAt = lookAt;
            this.fov = fov;
            this.aspect = aspect;
            this.zn = zn;
            this.zf = zf;
        }

        /// <summary>
        /// 求视图矩阵
        /// 视图矩阵为右手坐标系，我们之前使用的都是左手坐标系
        /// 所以需要先转成右手坐标系
        /// </summary>
        public static Matrix4x4 BuildViewMatrix(Vector4 cameraPosition, Vector4 up, Vector4 lookAt)
        {
            Vector4 d = lookAt - cameraPosition;
            d.Normalize();
            Vector4 r = Vector4.Cross(up, d);
            r.Normalize();
            up = Vector4.Cross(d, r);
            up.Normalize();
            Matrix4x4 m1 = new Matrix4x4
            (
                1, 0, 0, -cameraPosition.x,
                0, 1, 0, -cameraPosition.y,
                0, 0, 1, -cameraPosition.z,
                0, 0, 0, 1
            );

            Matrix4x4 m2 = new Matrix4x4
            (
                r.x, r.y, r.z, 0,
                up.x, up.y, up.z, 0,
                d.x, d.y, d.z, 0,
                0, 0, 0, 1

            );

            Matrix4x4 m3 = Matrix4x4.identityMatrix;
            m3[2, 2] = -1;
            return m3 * m2 * m1;

        }


        /// <summary>
        ///  求投影矩阵
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect">纵横比</param>
        /// <param name="zn">近裁切面到原点的距离</param>
        /// <param name="zf">远裁切面到原点的距离</param>
        /// <returns></returns>
        public static Matrix4x4 BuildProjectionMatrix(float fov, float aspect, float zn, float zf)
        {
            Matrix4x4 proj = new Matrix4x4
            {
                [0, 0] = (float) (1 / (Math.Tan(fov * 0.5f) * aspect)),
                [1, 1] = (float) (1 / Math.Tan(fov * 0.5f)),
                [2, 2] = -(zf + zn) / (zf - zn),
                [3, 2] = -1.0f,
                [2, 3] = (2 * zn * zf) / (zn - zf)
            };

            return proj;
        }

        /// <summary>
        /// 背面消隐  逆序
        /// </summary>
        public static bool BackFaceCulling(Vertex p1, Vertex p2, Vertex p3)
        {
            //其中p1 P2 p3必定严格按照逆时针或者顺时针的顺序存储
            //而且p1 p2 p3的point必须是视图空间的坐标
            Vector3 v1 = p2.point - p1.point;
            Vector3 v2 = p3.point - p2.point;
            Vector3 normal = Vector3.Cross(v1, v2); //计算法线
            //由于在视空间中，所以相机点就是（0,0,0）
            Vector3 viewDir = p1.point - Vector4.Zero;
            return Vector3.Dot(normal, viewDir) > 0;
        }
    }
}
