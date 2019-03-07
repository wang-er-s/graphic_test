using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    struct Light
    {
        public Vector3 worldPosition;
        public Color lightColor;
        /// <summary>
        /// 如果有衰减还有衰减系数，如果是聚光灯还有内部和外部的距离
        /// </summary>

        public Light(Vector3 worldPosition, Color lightColor)
        {
            this.worldPosition = worldPosition;
            this.lightColor = lightColor;
        }

        public static void BaseLight(Matrix4x4 worldMatrix, Light light,Mesh mesh,Vector3 eyeWorldPos,Color _ambientColor, ref Vertex vertex)
        {
            Vector3 vertexWorldPos = worldMatrix * vertex.point;
            //TODO 法线计算有问题 矩阵* v4 和 矩阵 * v3的问题
            Vector3 normal = worldMatrix.Inverse().TransposeMatrix() * vertex.normal;
            normal = normal.Normalize();
            //自发光
            Color emissiveColor = mesh.material.emissive;
            //环境光反射
            Color ambientColor = _ambientColor * mesh.material.ka;
            //漫反射
            Vector3 dir = (light.worldPosition - vertexWorldPos).Normalize();
            float diffuseLight = Mathf.Max(Vector3.Dot(dir, normal), 0);
            Color diffuseColor = mesh.material.kd * light.lightColor * diffuseLight;
            //镜面反射
            float specularLight = 0;
            if(diffuseLight != 0)
            {
                Vector3 v = (eyeWorldPos - vertexWorldPos).Normalize();
                Vector3 h = (dir + v).Normalize();
                specularLight =(float) Math.Pow(Mathf.Max(Vector3.Dot(normal, h), 0), mesh.material.shininess);
            }
            Color specularColor = mesh.material.ks * specularLight * light.lightColor;

            vertex.lightingColor = emissiveColor + ambientColor + diffuseColor + specularColor;
        }
    }
}
