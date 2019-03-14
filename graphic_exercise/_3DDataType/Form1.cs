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
        private float[,] zBuff; //z缓冲
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
            Image img = Image.FromFile(@"F:\SVN\graphic_exercise\graphic_exercise\_3DDataType\Texture\texture.jpg");
            texture = new Bitmap(img, imgWidth, imgHeight);
            InitTexture();
            rendMode = RenderMode.Textured;
            isOpenLight = true;
            isOpenTexture = true;
            frameBuff = new Bitmap(width, height);
            frameG = Graphics.FromImage(frameBuff);
            zBuff = new float[width, height];
            ambientColor = new Color(0.1f, 0.1f, 0.1f);
            mesh = new Mesh(CubeTestData.PointList, CubeTestData.Indexs, CubeTestData.UVs, CubeTestData.VertColors,
                CubeTestData.Normals, QuadTestData.Mat);
            //定义光照
            light = new Light(new Vector3(0, 0, -10), new Color(1, 1, 1));
            //定义相机
            camera = new Camera(new Vector4(0, 4, 5, 1), new Vector4(0, 1, 0,0), new Vector4(0, 4, 6, 1),
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
        private Vector4 tempPos;
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                canRotate = true;
            startXPos = e.X;
            startYPos = e.Y;
            Console.WriteLine("e.x = " + e.X);
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
            Vector4 pos = camera.lookAt - camera.eyePosition;
            pos = Matrix4x4.ArbitraryAxis(right, rotY) * Matrix4x4.RotateY(rotX) * pos;
            camera.lookAt = pos + camera.eyePosition;

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
            //camera.lookAt = camera.eyePosition + new Vector4(0, 0, 1, 0);
            Console.WriteLine(camera.eyePosition);
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
        private void Tick(object sender, EventArgs e)
        {
            lock (frameBuff)
            {
                ClearBuff();

                //*Matrix4x4.RotateX(rot)
                Matrix4x4 worldMatrix = Matrix4x4.Translate(new Vector3(0, 4, 15)) * Matrix4x4.RotateY(0) *
                                        Matrix4x4.RotateX(-0.3f) * Matrix4x4.RotateZ(0);
                Matrix4x4 viewMatrix = Camera.BuildViewMatrix(camera.eyePosition, camera.up, camera.lookAt);
                Matrix4x4 projectionMatrix =
                    Camera.BuildProjectionMatrix(camera.fov, camera.aspect, camera.zn, camera.zf);
                Draw(worldMatrix, viewMatrix, projectionMatrix);
                pictureBox1.Image = Image.FromHbitmap(frameBuff.GetHbitmap());
            }
        }

        private uint showTrisCount; //测试数据，记录当前显示的三角形数

        private void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            showTrisCount = 0;
            for (int i = 0; i + 2 < mesh.vertices.Length; i += 3)
            {
                DrawTriangle(mesh.vertices[i], mesh.vertices[i + 1], mesh.vertices[i + 2], m, v, p);
            }
        }

        /// <summary>
        /// 绘制三角形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="mvp"></param>
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

            //简单剔除
            if (Clip(p1) == false && Clip(p2) == false && Clip(p3) == false)
            {
                return;
            }

            // TODO 上下这两个都需要透视除法 cvv裁切
            TransformToScreen(ref p1);
            TransformToScreen(ref p2);
            TransformToScreen(ref p3);

            //--------------------光栅化阶段---------------------------

            if (isCull)
            {
                ValueTuple<Triangle, Triangle, int> outValue = new ValueTuple<Triangle, Triangle, int>(
                    new Triangle(p1, p2, p3),new Triangle(new Vertex(),new Vertex(),new Vertex()) ,1);
                clip(new Triangle(p1, p2, p3), ref outValue);
                Rasterization(outValue.Item1[0], outValue.Item1[1], outValue.Item1[2]);
                if (outValue.Item3 == 2)
                {
                    Rasterization(outValue.Item2[0], outValue.Item2[1], outValue.Item2[2]);
                }
            }
            else
            {
                Rasterization(p1, p2, p3);
            }
        }

        void Rasterization(Vertex p1, Vertex p2, Vertex p3)
        {
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
        private bool Clip(Vertex v)
        {
            //cvv为 xyz 需要都需要在-1,1内
            if (v.point.x >= -v.point.w && v.point.x <= v.point.w &&
                v.point.y >= -v.point.w && v.point.y <= v.point.w &&
                v.point.z >= -v.point.w && v.point.z <= v.point.w)
            {
                return true;
            }

            return false;
        }

        private void ClearBuff()
        {
            frameG.Clear(System.Drawing.Color.AliceBlue);
            for (int i = 0; i < zBuff.GetLength(0); i++)
            {
                for (int j = 0; j < zBuff.GetLength(1); j++)
                {
                    zBuff[i, j] = 1;
                }
            }
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
            int startX = (int)(Math.Round(p1.point.x, MidpointRounding.AwayFromZero));
            int startY = (int)(Math.Round(p1.point.y, MidpointRounding.AwayFromZero));
            int endX = (int)(Math.Round(p2.point.x, MidpointRounding.AwayFromZero));
            int endY = (int)(Math.Round(p2.point.y, MidpointRounding.AwayFromZero));
            int curX = startX, curY = startY;
            float disX = endX - startX;
            float disY = endY - startY;
            int stepx = 1;
            int stepy = 1;
            //求w缓冲系数
            float w = 0;
            //插值因子
            float t = 0;
            //uv坐标
            int u = 0;
            int v = 0;
            //最终颜色
            Color finalColor = new Color(1, 1, 1);
            stepx = disX >= 0 ? 1 : -1;
            stepy = disY >= 0 ? 1 : -1;
            float e = -0.5f;
            float k = 0;

            if (Math.Abs(disX) > Math.Abs(disY))
            {
                if (disX == 0)
                {
                    disX = int.MaxValue;
                }
                k = Math.Abs(disY / disX);
                while (true)
                {
                    t = (curX - startX) / disX;
                    w = Mathf.Lerp(p1.depth, p2.depth, t);
                    w = 1 / w;
                    //初始化颜色值
                    finalColor.Reset();
                    //光照颜色
                    if (isOpenLight)
                    {
                        Mathf.Lerp(ref finalColor, p1.lightingColor, p2.lightingColor, t);
                        finalColor *= w;
                    }
                    if (!isOpenTexture)
                    {
                        //颜色和光照混合
                        Color temp = new Color();
                        Mathf.Lerp(ref temp, p1.pointColor, p2.pointColor, t);
                        finalColor = temp * w * finalColor;
                    }
                    else
                    {
                        ////uv坐标，乘以图片的宽高来对应图片的像素点
                        u = (int)(Mathf.Lerp(p1.u, p2.u, t) * w * (imgWidth - 1));
                        v = (int)(Mathf.Lerp(p1.v, p2.v, t) * w * (imgHeight - 1));
                        //纹理颜色
                        finalColor = new Color(Tex(u, v)) * finalColor;
                    }
                    //
                    if (curX >= 0 && curY >= 0 && curX < width && curY < height)
                    {
                        frameBuff.SetPixel(curX, curY, finalColor.TransToSystemColor());
                    }
                    e += k;
                    if (e > 0)
                    {
                        e--;
                        if (disY > 0)
                            curY++;
                        else
                            curY--;
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

                while (true)
                {
                    //w缓冲
                    t = (curY - startY) / disY;
                    w = Mathf.Lerp(p1.depth, p2.depth, t);
                    w = 1 / w;
                    //初始化颜色值
                    finalColor.Reset();
                    if (isOpenLight)
                    {
                        Mathf.Lerp(ref finalColor, p1.lightingColor, p2.lightingColor, t);
                        finalColor *= w;
                    }
                    if (!isOpenTexture)
                    {
                        //颜色和光照混合
                        Color temp = new Color();
                        Mathf.Lerp(ref temp, p1.pointColor, p2.pointColor, t);
                        finalColor = temp * w * finalColor;
                    }
                    else
                    {
                        //uv坐标
                        u = (int)(Mathf.Lerp(p1.u, p2.u, t) * w * (imgWidth - 1));
                        v = (int)(Mathf.Lerp(p1.v, p2.v, t) * w * (imgHeight - 1));

                        //纹理颜色
                        finalColor = new Color(Tex(u, v)) * finalColor;
                    }

                    if (curX >= 0 && curY >= 0 && curX < width && curY < height)
                    {
                        frameBuff.SetPixel(curX, curY, finalColor.TransToSystemColor());
                    }

                    e += k;
                    if (e > 0)
                    {
                        e--;
                        if (disX > 0)
                            curX++;
                        else
                            curX--;
                    }
                    if (curY == endY) break;
                    curY += stepy;

                }
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

        #region 裁剪-- 一个方法，可以用于裁剪六个面

        Queue<Triangle> clipQueue = new Queue<Triangle>();//裁剪列表

        Vector4[] dotVectors =//顶点和该向量插值，判断顶点到平面的直线距离
          {
                new Vector4(0,0,1),//前
                new Vector4(0,0,-1),//后
                new Vector4(1,0,0),//左
                new Vector4(-1,0,0),//右
                new Vector4(0,1,0),//上
                new Vector4(0,-1,0)//下
        };
        float[] distance = new float[] { -1, -1, 0f, -799, 0f, -599 };//各个平面到原点“距离”
        //bool[] isfront = { true, false, false, false, false, false };//是否是近平面剪裁
        //裁剪方法
        private bool clip(Triangle triangle, ref ValueTuple<Triangle, Triangle, int> outValue)
        {
            bool isClip = false;
            //i代表将裁剪那个平面，默认都是从近平面开始，若该三角形被前面的面（近平面）裁剪，
            //其产生的子三角形将从后面的面（原平面）裁剪
            for (int i = triangle.startIndex; i < distance.Length; i++)
            {
                if (isClip == false)
                {
                    isClip = clip_Test(triangle[0], triangle[1], triangle[2], dotVectors[i], distance[i], i,ref outValue);
                }
                else
                {
                    break;
                }
            }
            return isClip;
        }

        /// <summary>
        /// 裁剪主方法
        /// </summary>
        /// <param name="v1">顶点</param>
        /// <param name="v2">顶点</param>
        /// <param name="v3">顶点</param>
        /// <param name="dotVector">点积向量，求顶点到平面的距离</param>
        /// <param name="distance">平面“位置”</param>
        /// <param name="startIndex">当前裁剪的是那个平面，其裁剪产生的子三角形将从下一个平面开始裁剪</param>
        /// <returns>是否被裁剪</returns>
        private bool clip_Test(Vertex v1, Vertex v2, Vertex v3, Vector4 dotVector, float distance, int startIndex,ref ValueTuple<Triangle, Triangle,int> outValue)
        {
            outValue.Item3 = 1;
            //插值因子
            float t = 0;
            //点在法线上的投影
            float projectV1 = Vector4.Dot(dotVector, v1.point);
            float projectV2 = Vector4.Dot(dotVector, v2.point);
            float projectV3 = Vector4.Dot(dotVector, v3.point);
            //点与点之间的距离
            float dv1v2 = Math.Abs(projectV1 - projectV2);
            float dv1v3 = Math.Abs(projectV1 - projectV3);
            float dv2v3 = Math.Abs(projectV2 - projectV3);
            //点倒平面的距离
            float pv1 = Math.Abs(projectV1 - distance);
            float pv2 = Math.Abs(projectV2 - distance);
            float pv3 = Math.Abs(projectV3 - distance);

            //v1,v2,v3都在立方体内
            if (projectV1 > distance && projectV2 > distance && projectV3 > distance)
            {
                //不做任何处理
                outValue.Item1 = new Triangle(v1, v2, v3);
                return false;
            }
            else if (projectV1 < distance && projectV2 > distance && projectV3 > distance)//只有v1在外
            {
                Vertex temp2 = new Vertex();
                t = pv2 / dv1v2;
                temp2.point.x = Mathf.Lerp(v2.point.x, v1.point.x, t);
                temp2.point.y = Mathf.Lerp(v2.point.y, v1.point.y, t);
                temp2.point.z = Mathf.Lerp(v2.point.z, v1.point.z, t);

                Mathf.Lerp( ref temp2, v2, v1, t);
                Vertex temp1 = new Vertex();
                t = pv3 / dv1v3;
                temp1.point.x = Mathf.Lerp(v3.point.x, v1.point.x, t);
                temp1.point.y = Mathf.Lerp(v3.point.y, v1.point.y, t);
                temp1.point.z = Mathf.Lerp(v3.point.z, v1.point.z, t);
                Mathf.Lerp(ref temp1, v3, v1, t);
                //画线或光栅化
                outValue.Item1 = new Triangle(temp1, temp2, v2, startIndex + 1);
                outValue.Item2 = new Triangle(temp1, v2, v3, startIndex + 1);
                outValue.Item3 = 2;
                return true;
            }
            else if (projectV1 > distance && projectV2 < distance && projectV3 > distance)//只有v2在外
            {
                Vertex temp1 = new Vertex();
                t = pv1 / dv1v2;
                temp1.point.x = Mathf.Lerp(v1.point.x, v2.point.x, t);
                temp1.point.y = Mathf.Lerp(v1.point.y, v2.point.y, t);
                temp1.point.z = Mathf.Lerp(v1.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp1, v1, v2, t);


                Vertex temp2 = new Vertex();
                t = pv3 / dv2v3;
                temp2.point.x = Mathf.Lerp(v3.point.x, v2.point.x, t);
                temp2.point.y = Mathf.Lerp(v3.point.y, v2.point.y, t);
                temp2.point.z = Mathf.Lerp(v3.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp2, v3, v2, t);
                //画线或光栅化
                outValue.Item1 = new Triangle(temp1, temp2, v3, startIndex + 1);
                outValue.Item2 = new Triangle(temp1, v3, v1, startIndex + 1);
                outValue.Item3 = 2;
                return true;
            }
            else if (projectV1 > distance && projectV2 > distance && projectV3 < distance)//只有v3在外
            {
                Vertex temp1 = new Vertex();
                t = pv2 / dv2v3;
                temp1.point.x = Mathf.Lerp(v2.point.x, v3.point.x, t);
                temp1.point.y = Mathf.Lerp(v2.point.y, v3.point.y, t);
                temp1.point.z = Mathf.Lerp(v2.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp1, v2, v3, t);

                Vertex temp2 = new Vertex();
                t = pv1 / dv1v3;
                temp2.point.x = Mathf.Lerp(v1.point.x, v3.point.x, t);
                temp2.point.y = Mathf.Lerp(v1.point.y, v3.point.y, t);
                temp2.point.z = Mathf.Lerp(v1.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp2, v1, v3, t);
                //画线或光栅化
                outValue.Item1 = new Triangle(temp1, temp2, v1, startIndex + 1);
                outValue.Item2 = new Triangle(temp1, v1, v2, startIndex + 1);
                outValue.Item3 = 2;
                return true;
            }

            else if (projectV1 > distance && projectV2 < distance && projectV3 < distance)//只有v1在内
            {
                Vertex temp1 = new Vertex();
                t = pv1 / dv1v2;
                temp1.point.x = Mathf.Lerp(v1.point.x, v2.point.x, t);
                temp1.point.y = Mathf.Lerp(v1.point.y, v2.point.y, t);
                temp1.point.z = Mathf.Lerp(v1.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp1, v1, v2, t);

                Vertex temp2 = new Vertex();
                t = pv1 / dv1v3;
                temp2.point.x = Mathf.Lerp(v1.point.x, v3.point.x, t);
                temp2.point.y = Mathf.Lerp(v1.point.y, v3.point.y, t);
                temp2.point.z = Mathf.Lerp(v1.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp2, v1, v3, t);
                //画线或光栅化
                outValue.Item1 = new Triangle(temp1, temp2, v1, startIndex + 1);
                return true;
            }
            else if (projectV1 < distance && projectV2 > distance && projectV3 < distance)//只有v2在内
            {
                Vertex temp1 = new Vertex();
                t = pv2 / dv2v3;
                temp1.point.x = Mathf.Lerp(v2.point.x, v3.point.x, t);
                temp1.point.y = Mathf.Lerp(v2.point.y, v3.point.y, t);
                temp1.point.z = Mathf.Lerp(v2.point.z, v3.point.z, t);
                Mathf.Lerp(ref temp1, v2, v3, t);

                Vertex temp2 = new Vertex();
                t = pv2 / dv1v2;
                temp2.point.x = Mathf.Lerp(v2.point.x, v1.point.x, t);
                temp2.point.y = Mathf.Lerp(v2.point.y, v1.point.y, t);
                temp2.point.z = Mathf.Lerp(v2.point.z, v1.point.z, t);
                Mathf.Lerp(ref temp2, v2, v1, t);
                //画线或光栅化
                clipQueue.Enqueue(new Triangle(temp1, temp2, v2, startIndex + 1));
                return true;
            }
            else if (projectV1 < distance && projectV2 < distance && projectV3 > distance)//只有v3在内
            {
                Vertex temp1 = new Vertex();
                t = pv3 / dv1v3;
                temp1.point.x = Mathf.Lerp(v3.point.x, v1.point.x, t);
                temp1.point.y = Mathf.Lerp(v3.point.y, v1.point.y, t);
                temp1.point.z = Mathf.Lerp(v3.point.z, v1.point.z, t);
                Mathf.Lerp(ref temp1, v3, v1, t);

                Vertex temp2 = new Vertex();
                t = pv3 / dv2v3;
                temp2.point.x = Mathf.Lerp(v3.point.x, v2.point.x, t);
                temp2.point.y = Mathf.Lerp(v3.point.y, v2.point.y, t);
                temp2.point.z = Mathf.Lerp(v3.point.z, v2.point.z, t);
                Mathf.Lerp(ref temp2, v3, v2, t);
                //画线或光栅化
                clipQueue.Enqueue(new Triangle(temp1, temp2, v3, startIndex + 1));
                return true;
            }
            return false;
        }

        #endregion


    }
}
