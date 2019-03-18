using _3DDataType.RenderData;
using _3DDataType.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Color = _3DDataType.RenderData.Color;

namespace _3DDataType
{
    public partial class RenderDemo : Form
    {
        private Bitmap texture;
        private Bitmap frameBuff;
        private Graphics frameG;
        private Mesh mesh;
        private Light light;
        private Camera camera;
        private Color ambientColor; //全局环境光颜色 
        private RenderMode rendMode; //渲染模式
        private bool isOpenLight; //光照模式
        private bool isOpenTexture; //纹理采样
        private System.Drawing.Color[,] textureArray; //纹理颜色值
        private bool isCull = true;

        private int imgWidth = 512;
        private int imgHeight = 512;
        private int width = 800;
        private int height = 600;

        public RenderDemo()
        {
            InitializeComponent();
        }

        private void RenderDemo_Load(object sender, EventArgs e)
        {
            Image img = Image.FromFile(Environment.CurrentDirectory + @"\..\..\..\Texture\texture.jpg");
            texture = new Bitmap(img, imgWidth, imgHeight);
            InitTexture();
            rendMode = RenderMode.Textured;
            isOpenLight = true;
            isOpenTexture = true;
            frameBuff = new Bitmap(width, height);
            frameG = Graphics.FromImage(frameBuff);
            ambientColor = new Color(0.1f, 0.1f, 0.1f);
            mesh = new Mesh(CubeTestData.PointList, CubeTestData.Indexs, CubeTestData.UVs, CubeTestData.VertColors,
                CubeTestData.Normals, QuadTestData.Mat);
            light = new Light(new Vector3(0, 10, 0), new Color(1, 1, 1));
            camera = new Camera(new Vector4(0, 4, 2, 1), new Vector4(0, 1, 0,0), new Vector4(0, 4, 6, 1),
                (float) Math.PI /3, width / (float) height, 5, 30);

            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;

            KeyPress += OnLeftKeyDown;


            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 10f);
            mainTimer.Elapsed += new ElapsedEventHandler(Tick);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();

        }

        #region 按钮按键和鼠标事件

        private void RenderBtn_Click(object sender, EventArgs e)
        {
            switch (rendMode)
            {
                case RenderMode.Wireframe:
                    rendMode = RenderMode.Textured;
                    RenderBtn.Text = "贴图";
                    break;
                case RenderMode.Textured:
                    rendMode = RenderMode.Wireframe;
                    RenderBtn.Text = "线框";
                    break;
            }
        }

        private void Texture_Click(object sender, EventArgs e)
        {
            isOpenTexture = !isOpenTexture;
        }

        private void CullingBtn_Click(object sender, EventArgs e)
        {
            isCull = !isCull;
        }

        private void LightBtn_Click(object sender, EventArgs e)
        {
            isOpenLight = !isOpenLight;
        }

        private int startXPos = 0;
        private float startYPos = 0;
        private bool canRotate = false;
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                canRotate = true;
            startXPos = e.X;
            startYPos = e.Y;
        }
         
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            canRotate = false;
        }
        float angle = (float)(Math.PI / 90);
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!canRotate) return;
            //Console.WriteLine($"e.y = {e.Location.Y}  startY = {startYPos}");
            rotY = e.Y - startYPos;
            if (rotY < 0)
                rotY = angle;
            else if (rotY > 0)
                rotY = -angle;
            
            rotX = e.X - startXPos;
            if (rotX < 0)
                rotX = -angle;
            else if (rotX > 0)
                rotX = angle;
            startXPos = e.X;
            startYPos = e.Y;
            Vector4 forward = camera.lookAt - camera.eyePosition;
            forward.Normalize();
            Vector4 right = Vector4.Cross(camera.up, forward);
            Vector4 lookPos = camera.lookAt - camera.eyePosition;
            lookPos = Matrix4x4.ArbitraryAxis(right, rotY) * Matrix4x4.RotateY(rotX) * lookPos;
            camera.lookAt = lookPos + camera.eyePosition;
            camera.up = Vector4.Cross( camera.lookAt,right).Normalize();
        }

        private void OnLeftKeyDown(object sender,KeyPressEventArgs e)
        {
            Vector4 forward = camera.lookAt - camera.eyePosition;
            forward.Normalize();
            Vector4 right = Vector4.Cross(camera.up, forward);
            right.Normalize();
            Vector4 up = Vector4.Cross(forward, right);
            up.Normalize();
            if (e.KeyChar == 'a')
            {
                camera.eyePosition -= right * 0.1f;
                camera.lookAt -= right * 0.1f;
            }
            if (e.KeyChar == 'd')
            {
                camera.eyePosition += right * 0.1f;
                camera.lookAt += right * 0.1f;
            }
            if (e.KeyChar == 'w')
            {
                camera.eyePosition += forward* 0.1f;
                camera.lookAt += forward* 0.1f;
            }
            if (e.KeyChar == 's')
            {
                camera.eyePosition -= forward * 0.1f;
            }
            camera.up = up;
        }
        #endregion

        /// <summary>
        /// 保存纹理颜色值
        /// </summary>
        private void InitTexture()
        {
            textureArray = new System.Drawing.Color[imgWidth, imgHeight];
            for (int i = 0; i < imgWidth; i++)
            {
                for (int j = 0; j < imgHeight; j++)
                {
                    textureArray[i, j] = texture.GetPixel(i, j);
                }
            }
        }

        private float rotY = 0f;
        private float rotX = 0f;

        private float rot = 0;
        private void Tick(object sender, EventArgs e)
        {
            lock (frameBuff)
            {
                ClearBuff();

                //*Matrix4x4.RotateX(rot)
                Matrix4x4 worldMatrix = Matrix4x4.Translate(new Vector3(0, 3, 12)) * Matrix4x4.RotateY(0) *
                                        Matrix4x4.RotateX(-0.3f) * Matrix4x4.RotateZ(0);
                Matrix4x4 viewMatrix = Camera.BuildViewMatrix(camera.eyePosition, camera.up, camera.lookAt);
                Matrix4x4 projectionMatrix =
                    Camera.BuildProjectionMatrix(camera.fov, camera.aspect, camera.zn, camera.zf);
                Draw(worldMatrix, viewMatrix, projectionMatrix);
                pictureBox1.Image = Image.FromHbitmap(frameBuff.GetHbitmap());
            }
        }


        private void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            for (int i = 0; i + 2 < mesh.vertices.Length; i += 3)
            {
                DrawTriangle(mesh.vertices[i], mesh.vertices[i + 1], mesh.vertices[i + 2], m, v, p);
            }
        }

        /// <summary>
        /// 绘制三角形
        /// </summary>
        private void DrawTriangle(Vertex p1, Vertex p2, Vertex p3, Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            //--------------------几何阶段---------------------------

            SetModelToWorld(m, ref p1);
            SetModelToWorld(m, ref p2);
            SetModelToWorld(m, ref p3);

            if (isOpenLight)
            {
                //进行顶点光照
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p1);
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p2);
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p3);
            }

            //模型空间变到世界空间变换到相机空间
            SetWorldToCamera(v, ref p1);
            SetWorldToCamera(v, ref p2);
            SetWorldToCamera(v, ref p3);

            //在相机空间进行背面消隐
            if (Camera.BackFaceCulling(p1, p2, p3) == false)
            {
                return;
            }

            //变换到齐次剪裁空间
            SetProjectionTransform(p, ref p1);
            SetProjectionTransform(p, ref p2);
            SetProjectionTransform(p, ref p3);

            // TODO 上下这两个都需要透视除法 cvv裁切
            TransformToScreen(ref p1);
            TransformToScreen(ref p2);
            TransformToScreen(ref p3);

            //--------------------光栅化阶段---------------------------

            if (isCull)
            {
                List<Triangle> outValue;
                CubeClip(new Triangle(p1, p2, p3), out outValue);
                for (int i = 0; i < outValue.Count; i++)
                {
                    Rasterization(outValue[i][0], outValue[i][1], outValue[i][2]);
                }
            }
            else
            {
                Rasterization(p1, p2, p3);
            }
        }

        void Rasterization(Vertex p1, Vertex p2, Vertex p3)
        {
            if (Clip(p1) && Clip(p2) && Clip(p3)) return;
            if (rendMode == RenderMode.Wireframe)
            {
                BresenhamDrawLine(p1, p2);
                BresenhamDrawLine(p2, p3);
                BresenhamDrawLine(p3, p1);
            }
            else
            {
                TriangleRasterization(p1, p2, p3);
            }
        }

        bool Clip(Vertex v)
        {
            if (v.point.z < -1 || v.point.z > 1)
                return true;
            return false;
        }

        /// <summary>
        /// 世界空间到相机空间
        /// </summary>
        private void SetWorldToCamera( Matrix4x4 v, ref Vertex vertex)
        {
            vertex.point = v * vertex.point;
        }

        /// <summary>
        /// 模型空间到世界空间
        /// </summary>
        private void SetModelToWorld(Matrix4x4 m, ref Vertex vertex)
        {
            vertex.point = m * vertex.point;
        }

        /// <summary>
        /// 投影变换，从相机空间到齐次剪裁空间
        /// </summary>
        /// <param name="p"></param>
        /// <param name="vertex"></param>
        private void SetProjectionTransform(Matrix4x4 p, ref Vertex vertex)
        {
            vertex.point = p * vertex.point;
        }

        /// <summary>
        /// 从齐次剪裁坐标系转到屏幕坐标
        /// </summary>
        private void TransformToScreen(ref Vertex v)
        {
            if (v.point.w != 0)
            {
                //插值矫正系数
                v.depth = 1 / v.point.w;
                //先进行透视除法，转到cvv
                v.point.x *= v.depth;
                v.point.y *= v.depth;
                v.point.z *= v.depth;
                v.point.w = 1;
                v.onePerZ = (v.point.z + 1) / 2;
                //cvv到屏幕坐标
                v.point.x = (v.point.x + 1) * 0.5f * width;
                v.point.y = (1 - v.point.y) * 0.5f * height;
                v.u *= v.depth;
                v.v *= v.depth;
                v.pointColor *= v.depth;
                v.lightingColor *= v.depth;
            }
        }

        /// <summary>
        /// 检查是否这个顶点是否在视锥体内
        /// </summary>
        /// <returns>是否通过剪裁</returns>
        //private bool Clip(Vertex v)
        //{
        //    //cvv为 xyz 需要都需要在-1,1内
        //    if (v.point.x < width && v.point.x > 0 &&
        //        v.point.y < height && v.point.y > 0
        //        && v.point.z < 1 && v.point.z > -1)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        private void ClearBuff()
        {
            frameG.Clear(System.Drawing.Color.AliceBlue);
        }

        #region 三角形光栅化算法

        /// <summary>
        /// 光栅化三角形，根据传入的顶点区分调用
        /// </summary>
        private void TriangleRasterization(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v1.point.y == v2.point.y)
            {
                if (v1.point.y < v3.point.y)
                {
                    FillTopFlatTriangle(v3, v2, v1);
                }
                else
                {
                    FillBottomFlatTriangle(v3, v2, v1);
                }
            }
            else if (v1.point.y == v3.point.y)
            {
                if (v1.point.y < v2.point.y)
                {
                    FillTopFlatTriangle(v2, v1, v3);
                }
                else
                {
                    FillBottomFlatTriangle(v2, v1, v3);
                }
            }
            else if (v2.point.y == v3.point.y)
            {
                if (v2.point.y < v1.point.y)
                {
                    FillTopFlatTriangle(v1, v3, v2);
                }
                else
                {
                    FillBottomFlatTriangle(v1, v2, v3);
                }
            }
            else
            {
                Vertex top;
                Vertex bottom;
                Vertex middle;
                if (v1.point.y > v2.point.y && v2.point.y > v3.point.y)
                {
                    top = v3;
                    middle = v2;
                    bottom = v1;
                }
                else if (v3.point.y > v2.point.y && v2.point.y > v1.point.y)
                {
                    top = v1;
                    middle = v2;
                    bottom = v3;
                }
                else if (v2.point.y > v1.point.y && v1.point.y > v3.point.y)
                {
                    top = v3;
                    middle = v1;
                    bottom = v2;
                }
                else if (v3.point.y > v1.point.y && v1.point.y > v2.point.y)
                {
                    top = v2;
                    middle = v1;
                    bottom = v3;
                }
                else if (v1.point.y > v3.point.y && v3.point.y > v2.point.y)
                {
                    top = v2;
                    middle = v3;
                    bottom = v1;
                }
                else if (v2.point.y > v3.point.y && v3.point.y > v1.point.y)
                {
                    top = v1;
                    middle = v3;
                    bottom = v2;
                }
                else
                {
                    //三点共线
                    return;
                }

                FillTriangle(top, middle, bottom);
            }
        }



        /// <summary>
        /// V1的上顶点，V2V3是下平行边   v2v3点顺序为无所谓
        /// </summary>
        private void FillBottomFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            //因为设置像素点不能为float，所以需要下面的都是使用int
            int x1 = (int) (Math.Ceiling(v1.point.x));
            int x2 = (int) (Math.Ceiling(v2.point.x));
            int x3 = (int) (Math.Ceiling(v3.point.x));
            int y1 = (int) (Math.Ceiling(v1.point.y));
            int y2 = (int) (Math.Ceiling(v2.point.y));
            int y3 = (int) (Math.Ceiling(v3.point.y));
            float invslopeL = (x2 - x1) * 1.0f / (y2 - y1);
            float invslopeR = (x3 - x1) * 1.0f / (y3 - y1);
            int curxL = 0;
            int curxR = 0;

            for (int scanlineY = y1; scanlineY < v2.point.y; scanlineY++)
            {
                curxL = (int) Math.Ceiling((scanlineY - y1) * invslopeL + x1);
                curxR = (int) Math.Ceiling((scanlineY - y1) * invslopeR + x1);
                Vertex vl = new Vertex
                {
                    point = new Vector4(curxL, scanlineY, 0, 0)
                };
                Vertex vr = new Vertex
                {
                    point = new Vector4(curxR, scanlineY, 0, 0)
                };
                //插值求uv 颜色等
                float t = (scanlineY - y1) / (v2.point.y - y1);
                Mathf.Lerp(ref vl, v1, v2, t);
                Mathf.Lerp(ref vr, v1, v3, t);
                BresenhamDrawLine(vl, vr);
            }
        }

        /// <summary>
        /// V1的下顶点，V2V3是上平行边   
        /// </summary>
        private void FillTopFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            int x1 = (int) (Math.Ceiling(v1.point.x));
            int x2 = (int) (Math.Ceiling(v2.point.x));
            int x3 = (int) (Math.Ceiling(v3.point.x));
            int y1 = (int) (Math.Ceiling(v1.point.y));
            int y2 = (int) (Math.Ceiling(v2.point.y));
            int y3 = (int) (Math.Ceiling(v3.point.y));
            float invslopeL = (x3 - x1) * 1.0f / (y1 - y3);
            float invslopeR = (x2 - x1) * 1.0f / (y1 - y2);
            int curxL = 0;
            int curxR = 0;

            for (int scanlineY = y1; scanlineY >= y2; scanlineY--)
            {
                curxL = (int) Math.Ceiling(x1 + (y1 - scanlineY) * invslopeL);
                curxR = (int) Math.Ceiling(x1 + (y1 - scanlineY) * invslopeR);
                Vertex vl = new Vertex
                {
                    point = new Vector4(curxL, scanlineY, 0, 0)
                };
                Vertex vr = new Vertex
                {
                    point = new Vector4(curxR, scanlineY, 0, 0)
                };
                float t = (y1 - scanlineY) * 1.0f / (y1 - y3);
                Mathf.Lerp(ref vl, v1, v3, t);
                Mathf.Lerp(ref vr, v1, v2, t);
                BresenhamDrawLine(vl, vr);
            }
        }

        /// <summary>
        /// v1是上面的点，v3是下面的点，v2是中间的点
        /// </summary>
        private void FillTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v2.point.y == v3.point.y)
            {
                FillBottomFlatTriangle(v1, v2, v3);
            }
            else if (v1.point.y == v2.point.y)
            {
                FillTopFlatTriangle(v3, v2, v1);
            }
            else
            {
                float v4x = (v2.point.y - v1.point.y) * (v3.point.x - v1.point.x) / (v3.point.y - v1.point.y) +
                            v1.point.x;
                float t = (v2.point.y - v1.point.y) / (v3.point.y - v1.point.y);
                Vector2 v4Point = new Vector2(v4x, v2.point.y);
                Vertex v4 = new Vertex {point = v4Point};
                //插值计算uv 颜色等
                Mathf.Lerp(ref v4, v1, v3, t);
                FillBottomFlatTriangle(v1, v2, v4);
                FillTopFlatTriangle(v3, v2, v4);
            }
        }


        #endregion

        #region 2DLine 算法
        private void BresenhamDrawLine(Vertex p1, Vertex p2)
        {
            int startX = (int)Math.Ceiling(p1.point.x);
            int startY = (int)Math.Ceiling(p1.point.y);
            int endX = (int)Math.Ceiling(p2.point.x);
            int endY = (int)Math.Ceiling(p2.point.y);
            int curX = startX, curY = startY;
            float disX = endX - startX;
            int disY = endY - startY;
            int stepx = Math.Sign(disX);
            int stepy = Math.Sign(disY);
            float t = 0;
            float e = -0.5f;
            float k = 0;

            if (Math.Abs(disX) > Math.Abs(disY))
            {
                if (disX == 0)
                {
                    disX = int.MaxValue;
                }
                k = Math.Abs(disY / disX);
                disX = 1 / disX;
                while (true)
                {
                    t = (curX - startX) * disX;
                    //混合颜色和贴图
                    MixColor(p1, p2, t, curX, curY);
                    e += k;
                    if (e > 0)
                    {
                        e--;
                        curY += stepy;
                    }
                    if (curX == endX) break;
                    curX += stepx;
                }
            }
            else
            {
                if (disY == 0)
                {
                    disY = int.MaxValue;
                }
                k = Math.Abs(disX / disY);
                disY = 1 / disY;
                while (true)
                {
                    t = (curY - startY) * disY;
                    //混合颜色和贴图
                    MixColor(p1, p2, t, curX, curY);
                    e += k;
                    if (e > 0)
                    {
                        e--;
                        curX += stepx;
                    }
                    if (curY == endY) break;
                    curY += stepy;
                }
            }
        }

        private void MixColor(Vertex v1,Vertex v2,float t,int curX,int curY)
        {
            Color finalColor = new Color(1, 1, 1);
            float w = Mathf.Lerp(v1.depth, v2.depth, t);
            w = w == 0 ? 0 : 1 / w;
            if (isOpenLight)
            {
                Mathf.Lerp(ref finalColor, v1.lightingColor, v2.lightingColor, t);
                finalColor *= w;
            }
            if (!isOpenTexture)
            {
                //颜色和光照混合
                Color temp = new Color();
                Mathf.Lerp(ref temp, v1.pointColor, v2.pointColor, t);
                finalColor = temp * w * finalColor;
            }
            else
            {
                //uv坐标
                int u = (int)(Mathf.Lerp(v1.u, v2.u, t) * w * (imgWidth - 1));
                int v = (int)(Mathf.Lerp(v1.v, v2.v, t) * w * (imgHeight - 1));

                //纹理颜色
                finalColor = new Color(Tex(u, v)) * finalColor;
            }
            if (curX >= 0 && curY >= 0 && curX < width && curY < height)
            {
                frameBuff.SetPixel(curX, curY, finalColor.TransToSystemColor());
            }
        }

        private System.Drawing.Color Tex(int i, int j)
        {
            if (i < 0 || i > imgWidth - 1 || j < 0 || j > imgHeight - 1)
            {
                return System.Drawing.Color.Black;
            }

            return textureArray[i, imgHeight - 1 - j];
        }


        private void Swap<T>(ref T i1, ref T i2)
        {
            T temp = i1;
            i1 = i2;
            i2 = temp;
        }

        private void DDALine(int xa, int ya, int xb, int yb)
        {
            float delta_x, delta_y, x, y;
            int dx, dy, steps;
            dx = xb - xa;
            dy = yb - ya;
            steps = Math.Abs(Math.Abs(dx) > Math.Abs(dy) ? dx : dy);

            delta_x =  dx / (float) steps;
            delta_y =  dy / (float) steps;
            x = xa;
            y = ya;
            //  glClear(GL_COLOR_BUFFER_BIT);
            for (int i = 1; i <= steps; i++)
            {
                x += delta_x;
                y += delta_y;
                frameBuff.SetPixel((int) x, (int) y, System.Drawing.Color.Black);
            }
        }

        #endregion



        #region 裁剪

        List<ValueTuple<Vector3, int, Func<float, int, bool>>> normalAndDisList = new List<(Vector3, int, Func<float, int, bool>)>()
        {
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(0,0,1),-1,(view,dis)=>view>dis),//前
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(0,0,-1),-1,(view,dis)=>view>dis),//后
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(1,0,0),1,(view,dis)=>view>dis),//左
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(-1,0,0),-799,(view,dis)=>view>dis),//右
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(0,-1,0),0,(view,dis)=>view<dis),//上
            new ValueTuple<Vector3,int,Func<float,int,bool>>(new Vector3(0,1,0),599,(view,dis)=>view<dis),//下
        };

        private Queue<Triangle> clipQueue = new Queue<Triangle>();

        private void CubeClip(Triangle triangle,out List<Triangle> outValue)
        {
            outValue = new List<Triangle>();
            clipQueue.Enqueue(triangle);
            bool isClip = false;
            Triangle temp;
            while (clipQueue.Count > 0)
            {
                temp = clipQueue.Dequeue();
                for (int i = temp.startIndex; i < normalAndDisList.Count; i++)
                {
                    if (!isClip)
                    {
                        isClip = CubeClip(temp[0], temp[1], temp[2], normalAndDisList[i], i);
                    }
                    else
                    {
                        break;
                    }
                }
                if (!isClip)
                {
                    outValue.Add(temp);
                }
                isClip = false;
            }
            
        }

        bool CubeClip(Vertex v1, Vertex v2, Vertex v3, ValueTuple<Vector3, int, Func<float, int, bool>> norAndDis,int startIndex)
        {
            Vector3 normal = norAndDis.Item1;
            int dis = norAndDis.Item2;
            Func<float, int, bool> checkIsIn = norAndDis.Item3;
            //点在法线上的投影
            float projectV1 = Vector3.Dot(normal, v1.point);
            float projectV2 = Vector3.Dot(normal, v2.point);
            float projectV3 = Vector3.Dot(normal, v3.point);
            //点与点之间的距离
            float dv1v2 = Math.Abs(projectV1 - projectV2);
            float dv1v3 = Math.Abs(projectV1 - projectV3);
            float dv2v3 = Math.Abs(projectV2 - projectV3);
            //颠倒平面的距离
            float pv1 = Math.Abs(projectV1 - dis);
            float pv2 = Math.Abs(projectV2 - dis);
            float pv3 = Math.Abs(projectV3 - dis);
            //插值
            float t = 0;

            if (checkIsIn(projectV1, dis) && checkIsIn(projectV2, dis) && checkIsIn(projectV3, dis))
            {
                //都在里面 
                return false;
            }
            if (!checkIsIn(projectV1, dis) && checkIsIn(projectV2, dis) && checkIsIn(projectV3, dis))//v1在外面
            {
                Vertex temp12 = new Vertex();
                t = pv2 / dv1v2;
                temp12.point.x = Mathf.Lerp(v2.point.x, v1.point.x, t);
                temp12.point.y = Mathf.Lerp(v2.point.y, v1.point.y, t);
                temp12.point.z = dis;
                temp12.point.w = 1;
                Mathf.Lerp(ref temp12, v2, v1, t);

                Vertex temp13 = new Vertex();
                t = pv3 / dv1v3;
                temp13.point.x = Mathf.Lerp(v3.point.x, v1.point.x, t);
                temp13.point.y = Mathf.Lerp(v3.point.y, v1.point.y, t);
                temp13.point.z = dis;
                temp13.point.w = 1;
                Mathf.Lerp(ref temp13, v3, v1, t);

                clipQueue.Enqueue(new Triangle(temp13, temp12, v2, startIndex + 1));
                clipQueue.Enqueue(new Triangle(temp13, v2, v3, startIndex + 1));
                return true;
            }
            if (checkIsIn(projectV1, dis) && !checkIsIn(projectV2, dis) && checkIsIn(projectV3, dis))//v2在外面
            {
                Vertex temp12 = new Vertex();
                t = pv1 / dv1v2;
                temp12.point.x = Mathf.Lerp(v1.point.x, v2.point.x, t);
                temp12.point.y = Mathf.Lerp(v1.point.y, v2.point.y, t);
                temp12.point.z = Mathf.Lerp(v1.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp12, v1, v2, t);


                Vertex temp23 = new Vertex();
                t = pv3 / dv2v3;
                temp23.point.x = Mathf.Lerp(v3.point.x, v2.point.x, t);
                temp23.point.y = Mathf.Lerp(v3.point.y, v2.point.y, t);
                temp23.point.z = Mathf.Lerp(v3.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp23, v3, v2, t);

                clipQueue.Enqueue(new Triangle(temp12, temp23, v3, startIndex + 1));
                clipQueue.Enqueue(new Triangle(temp12, v3, v1, startIndex + 1));
                return true;
            }
            if (checkIsIn(projectV1, dis) && checkIsIn(projectV2, dis) && !checkIsIn(projectV3, dis))//v3在外面
            {
                Vertex temp23 = new Vertex();
                t = pv2 / dv2v3;
                temp23.point.x = Mathf.Lerp(v2.point.x, v3.point.x, t);
                temp23.point.y = Mathf.Lerp(v2.point.y, v3.point.y, t);
                temp23.point.z = Mathf.Lerp(v2.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp23, v2, v3, t);

                Vertex temp13 = new Vertex();
                t = pv1 / dv1v3;
                temp13.point.x = Mathf.Lerp(v1.point.x, v3.point.x, t);
                temp13.point.y = Mathf.Lerp(v1.point.y, v3.point.y, t);
                temp13.point.z = Mathf.Lerp(v1.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp13, v1, v3, t);

                clipQueue.Enqueue(new Triangle(temp23, temp13, v1, startIndex + 1));
                clipQueue.Enqueue(new Triangle(temp23, v1, v2, startIndex + 1));
                return true;
            }
            if (!checkIsIn(projectV1, dis) && !checkIsIn(projectV2, dis) && checkIsIn(projectV3, dis))//v1 v2在外面
            {
                Vertex temp13 = new Vertex();
                t = pv3 / dv1v3;
                temp13.point.x = Mathf.Lerp(v3.point.x, v1.point.x, t);
                temp13.point.y = Mathf.Lerp(v3.point.y, v1.point.y, t);
                temp13.point.z = Mathf.Lerp(v3.point.z, v1.point.z, t);
                Mathf.Lerp(ref temp13, v3, v1, t);

                Vertex temp23 = new Vertex();
                t = pv3 / dv2v3;
                temp23.point.x = Mathf.Lerp(v3.point.x, v2.point.x, t);
                temp23.point.y = Mathf.Lerp(v3.point.y, v2.point.y, t);
                temp23.point.z = Mathf.Lerp(v3.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp23, v3, v2, t);

                clipQueue.Enqueue(new Triangle(temp13, temp23, v3, startIndex + 1));
                return true;
            }
            if (!checkIsIn(projectV1, dis) && checkIsIn(projectV2, dis) && !checkIsIn(projectV3, dis))//v1 v3在外面
            {
                Vertex temp23 = new Vertex();
                t = pv2 / dv2v3;
                temp23.point.x = Mathf.Lerp(v2.point.x, v3.point.x, t);
                temp23.point.y = Mathf.Lerp(v2.point.y, v3.point.y, t);
                temp23.point.z = Mathf.Lerp(v2.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp23, v2, v3, t);

                Vertex temp12 = new Vertex();
                t = pv2 / dv1v2;
                temp12.point.x = Mathf.Lerp(v2.point.x, v1.point.x, t);
                temp12.point.y = Mathf.Lerp(v2.point.y, v1.point.y, t);
                temp12.point.z = Mathf.Lerp(v2.point.z, v1.point.z, t);
                Mathf.Lerp(ref temp12, v2, v1, t);

                clipQueue.Enqueue( new Triangle(temp23, temp12, v2, startIndex + 1));
                return true;
            }
            if (checkIsIn(projectV1, dis) && !checkIsIn(projectV2, dis) && !checkIsIn(projectV3, dis))//v2 v3在外面
            {
                Vertex temp12 = new Vertex();
                t = pv1 / dv1v2;
                temp12.point.x = Mathf.Lerp(v1.point.x, v2.point.x, t);
                temp12.point.y = Mathf.Lerp(v1.point.y, v2.point.y, t);
                temp12.point.z = Mathf.Lerp(v1.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp12, v1, v2, t);

                Vertex temp13 = new Vertex();
                t = pv1 / dv1v3;
                temp13.point.x = Mathf.Lerp(v1.point.x, v3.point.x, t);
                temp13.point.y = Mathf.Lerp(v1.point.y, v3.point.y, t);
                temp13.point.z = Mathf.Lerp(v1.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp13, v1, v3, t);

                clipQueue.Enqueue( new Triangle(temp12, temp13, v1, startIndex + 1));
                return true;
            }
            return false;
        }
        #endregion
    }
}
