using System;
using System.Collections.Generic;
using System.Text;

namespace _3DDataType.RenderData
{
    struct Material
    {
        /// <summary>
        /// 自发光颜色 （反射光）
        /// </summary>
        public Color emissive;

        /// <summary>
        /// 环境光反射系数
        /// </summary>
        public float ka;

        /// <summary>
        /// 漫反射系数
        /// </summary>
        public Color kd;

        /// <summary>
        /// 镜面反射系数
        /// </summary>
        public Color ks;

        /// <summary>
        /// 光泽度
        /// </summary>
        public float shininess;

        public Material(Color emissive, float ka, Color kd, Color ks, float shininess)
        {
            this.emissive = emissive;
            this.ka = ka;
            this.kd = kd;
            this.ks = ks;
            this.shininess = shininess;
        }
    }
}
