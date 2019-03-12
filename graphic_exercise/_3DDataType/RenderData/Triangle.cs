using System;

namespace _3DDataType
{
    public struct Triangle
    {
        public Vector3[] Vertex { get; }

        public Triangle(Vector3[] vertexs)
        {
            Vertex = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                if (i < vertexs.Length)
                    Vertex[i] = vertexs[i];
                else
                    Vertex[i] = Vector3.Zero;
            }
        }

        public Vector3 this[int index]
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