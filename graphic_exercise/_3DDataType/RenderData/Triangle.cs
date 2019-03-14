using _3DDataType.RenderData;
using System;

namespace _3DDataType
{
    public struct Triangle
    {
        public Vertex[] Vertex { get; }
        public int startIndex;//从哪个平面开始裁剪，默认从近平面

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Vertex = new Vertex[3];
            Vertex[0] = v1;
            Vertex[1] = v2;
            Vertex[2] = v3;
            startIndex = 0;
        }

        public Triangle(Vertex v1, Vertex v2, Vertex v3,int startIndex)
        {
            Vertex = new Vertex[3];
            Vertex[0] = v1;
            Vertex[1] = v2;
            Vertex[2] = v3;
            this.startIndex = startIndex;
        }

        public Vertex this[int index]
        {
            get
            {
                if (index < Vertex.Length)
                {
                    return Vertex[index];
                }

                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < Vertex.Length)
                {
                    Vertex[index] = value;
                }

                throw new IndexOutOfRangeException();
            }
        }

    }
}