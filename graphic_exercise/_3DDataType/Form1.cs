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

namespace _3DDataType
{
    public partial class RenderDemo : Form
    {

        private Bitmap texture;
        private Bitmap frameBuff;
        private Graphics frameG;
        private float[,] zBuff;//z缓冲，用来做深度测试
        private Mesh mesh;
        private Light light;
        private Camera camera;
        private RenderData.Color ambientColor;//全局环境光颜色 
        private RenderMode rendMode;//渲染模式
        private LightMode lightMode;//光照模式
        private System.Drawing.Color[,] textureArray;//纹理颜色值
        private bool canMove = false;
        private int imgWidth = 512;
        private int imgHeight = 512;
        private int width = 800 + 16;
        private int height = 600 + 40;
        public RenderDemo()
        {
            InitializeComponent();
        }

        private void RenderDemo_Load(object sender, EventArgs e)
        {
            Image img = Image.FromFile(@"F:\SVN\graphic_exercise\graphic_exercise\_3DDataType\Texture\texture.jpg");
            texture = new Bitmap(img, imgWidth, imgHeight);
           
            InitTexture();
            rendMode = RenderMode.VertexColor;
            lightMode = LightMode.On;
            frameBuff = new Bitmap(width, height);
            frameG = Graphics.FromImage(frameBuff);
            zBuff = new float[height, width];
            ambientColor = new RenderData.Color(1f, 1f, 1f);
            mesh = new Mesh(CubeTestData.pointList, CubeTestData.indexs, CubeTestData.uvs, CubeTestData.vertColors, CubeTestData.norlmas, QuadTestData.mat);
            //定义光照
            light = new Light(new Vector3(0, 0, -10), new RenderData.Color(1, 1, 1));
            //定义相机
            camera = new Camera(new Vector4(0, 5, 5, 1), new Vector4(0, 1, 0), new Vector4(0, 1, 30, 1), (float)System.Math.PI / 3, this.width / (float)this.height, 3, 30);

            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 60f);

            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;

            mainTimer.Elapsed += new ElapsedEventHandler(Tick);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();


            //Vertex v273 = new Vertex();
            //v273.point = new Vector4(273, 368, -0.4);
            //Vertex v2 = new Vertex();
            //v2.point = new Vector4(200, 200, 0);
            //Vertex v3 = new Vertex();
            //v3.point = new Vector4(0, 300, 0);
            //TriangleRasterization(v1, v2, v3);
            //pictureBox1.Image = Image.FromHbitmap(frameBuff.GetHbitmap());

        }

        private int startXPos = 0;

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(1111111111);
            canMove = true;
            startXPos = e.X;
            Console.WriteLine("e.x = " + e.X);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            canMove = false;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!canMove) return;
            Console.WriteLine("y pos ======== "+e.X);
            rot += (e.X - startXPos) * 0.01f;
        }

        /// <summary>
        /// 保存纹理颜色值
        /// </summary>
        public void InitTexture()
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

        private float rot = 0f;

        private void Tick(object sender, EventArgs e)
        {
            lock (frameBuff)
            {
                ClearBuff();
                
                //*Matrix4x4.RotateX(rot)
                Matrix4x4 worldMatrix = Matrix4x4.Translate(new Vector3(0, 3, 10)) * Matrix4x4.RotateY(rot) * Matrix4x4.RotateX(rot) * Matrix4x4.RotateZ(rot); 
                Matrix4x4 viewMatrix = Camera.BuildViewMatrix(camera.eyePosition, camera.up, camera.lookAt);
                Matrix4x4 projectionMatrix = Camera.BuildProjectionMatrix(camera.fov, camera.aspect, camera.zn, camera.zf);
                //rot += 0.05f;
                Draw(worldMatrix, viewMatrix, projectionMatrix);
                pictureBox1.Image = Image.FromHbitmap(frameBuff.GetHbitmap());
            }
        }
        private uint showTrisCount;//测试数据，记录当前显示的三角形数
        private void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            showTrisCount = 0;
            for (int i = 0; i + 2 < mesh.vertices.Length; i += 3)
            {
                DrawTriangle(mesh.vertices[i], mesh.vertices[i + 1], mesh.vertices[i + 2], m, v, p);
            }
            //Console.WriteLine("显示的三角形数：" + showTrisCount);
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
            if (lightMode == LightMode.On)
            {
                //进行顶点光照
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p1);
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p2);
                Light.BaseLight(m, light, mesh, camera.eyePosition, ambientColor, ref p3);
            }

            //变换到相机空间
            SetMVTransform(m, v, ref p1);
            SetMVTransform(m, v, ref p2);
            SetMVTransform(m, v, ref p3);

            //在相机空间进行背面消隐
            if (Camera.BackFaceCulling(p1, p2, p3) == false)
            {
                return;
            }

            //变换到齐次剪裁空间
            SetProjectionTransform(p, ref p1);
            SetProjectionTransform(p, ref p2);
            SetProjectionTransform(p, ref p3);

            //裁剪 没搞明白 后面再加
            if (Clip(p1) == false && Clip(p2) == false && Clip(p3) == false)
            { 
                return;
            }
            // TODO 上下这两个都需要透视除法 cvv裁切
            TransformToScreen(ref p1);
            TransformToScreen(ref p2);
            TransformToScreen(ref p3);

            //--------------------光栅化阶段---------------------------

            if (rendMode == RenderMode.Wireframe)
            {
                //线框模式
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
        /// 进行mv矩阵变换，从本地模型空间到世界空间，再到相机空间
        /// </summary>
        private void SetMVTransform(Matrix4x4 m, Matrix4x4 v, ref Vertex vertex)
        {
            vertex.point =  m * vertex.point;
            vertex.point = v * vertex.point;
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
                //先进行透视除法，转到cvv
                v.point.x *= 1 / v.point.w;
                v.point.y *= 1 / v.point.w;
                v.point.z *= 1 / v.point.w;
                v.point.w = 1;
                //TODO cvv到屏幕坐标 不理解
                v.point.x = (v.point.x + 1) * 0.5f * this.width;
                v.point.y = (1 - v.point.y) * 0.5f * this.height;
            }
        }

        /// <summary>
        /// 检查是否裁剪这个顶点,简单的cvv裁剪,在透视除法之前
        /// </summary>
        /// <returns>是否通关剪裁</returns>
        private bool Clip(Vertex v)
        {
            //cvv为 x-1,1  y-1,1  z0,1
            if (v.point.x >= -v.point.w && v.point.x <= v.point.w &&
                v.point.y >= -v.point.w && v.point.y <= v.point.w &&
                v.point.z >= 0f && v.point.z <= v.point.w)
            {
                return true;
            }
            return false;
        }

        private void ClearBuff()
        {
            frameG.Clear(System.Drawing.Color.Black);
            Array.Clear(zBuff, 0, zBuff.Length);
        }

        #region 三角形光栅化算法

        void TriangleRasterization(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v1.point.y == v2.point.y)
            {
                if (v1.point.y < v3.point.y)
                {
                    //平顶
                    FillTopFlatTriangle(v3, v2, v1);
                }
                else
                {
                    //平底
                    FillBottomFlatTriangle(v3, v1, v2);
                }
            }
            else if (v1.point.y == v3.point.y)
            {
                if (v1.point.y < v2.point.y)
                {
                    //平顶
                    FillTopFlatTriangle(v2, v3, v1);
                }
                else
                {
                    //平底
                    FillBottomFlatTriangle(v2, v1, v3);
                }
            }
            else if (v2.point.y == v3.point.y)
            {
                if (v2.point.y < v1.point.y)
                {//平顶
                    FillTopFlatTriangle(v1, v3, v2);
                }
                else
                {//平底
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
                FillRightTriangle(top, middle, bottom);
            }
        }

        /// <summary>
        /// V1的下顶点，V2V3是上平行边   点顺序为逆序
        /// </summary>
        void FillTopFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            float invslope1 = (v1.point.x - v2.point.x) / (v1.point.y - v2.point.y);
            float invslope2 = (v1.point.x - v3.point.x) / (v1.point.y - v3.point.y);

            float curx1 = v2.point.x;
            float curx2 = v3.point.x;

            for (int scanlineY = (int)v2.point.y; scanlineY <= v1.point.y; scanlineY++)
            {
                Vertex vl = new Vertex();
                //TODO 设置颜色 uv
                vl.point = new Vector2(curx1, scanlineY);
                Vertex vr = new Vertex();
                vr.point = new Vector2(curx2, scanlineY);
                BresenhamDrawLine(vl,vr);
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        /// <summary>
        /// V1的上顶点，V2V3是上平行边   点顺序为逆序
        /// </summary>
        void FillBottomFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            float invslope1 = (v1.point.x - v2.point.x) / (v2.point.y - v1.point.y);
            float invslope2 = (v1.point.x - v3.point.x) / (v3.point.y - v1.point.y);

            float curx1 = v2.point.x;
            float curx2 = v3.point.x;

            for (int scanlineY = (int)v2.point.y; scanlineY >= v1.point.y; scanlineY--)
            {
                Vertex vl = new Vertex();
                //TODO 设置颜色 uv
                vl.point = new Vector2(curx1, scanlineY);
                Vertex vr = new Vertex();
                vr.point = new Vector2(curx2, scanlineY);
                BresenhamDrawLine(vl, vr);
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        /// <summary>
        /// v1是上面的点，v3是下面的点，v2是中间的点
        /// </summary>
        void FillRightTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v2.point.y == v3.point.y)
            {
                FillBottomFlatTriangle(v1, v2, v3);
            }
            /* check for trivial case of top-flat triangle */
            else if (v1.point.y == v2.point.y)
            {
                FillTopFlatTriangle(v1, v2, v3);
            }
            else
            {
                float v4x = (v2.point.y - v1.point.y) * (v3.point.x - v1.point.x) / (v3.point.y - v1.point.y) + v1.point.x;
                Vector2 v4Point = new Vector2(v4x, v2.point.y);
                Vertex v4 = new Vertex();
                v4.point = v4Point;
                FillBottomFlatTriangle(v1, v2, v4);
                FillTopFlatTriangle(v3, v2, v4);
            }
        }

        #endregion

        #region 2DLine 算法
        private void BresenhamDrawLine(Vertex v1, Vertex v2)
        {
            int startX = (int)(Math.Round(v1.point.x, MidpointRounding.AwayFromZero));
            int startY = (int)(Math.Round(v1.point.y, MidpointRounding.AwayFromZero));
            int endX = (int)(Math.Round(v2.point.x, MidpointRounding.AwayFromZero));
            int endY = (int)(Math.Round(v2.point.y, MidpointRounding.AwayFromZero));
            float disX = endX - startX;
            float disY = endY - startY;
            float k = 0;
            float e = -0.5f;
            int curX = 0, curY = 0;
            if (Math.Abs(disX) > Math.Abs(disY))
            {

                int stepX = 1;
                if (disX < 0) stepX = -stepX;
                else if (disX > 0) k = Math.Abs(disY / disX);
                curY = startY;
                curX = startX;
                while (curX != endX)
                {
                    e += k;
                    if (e > 0)
                    {
                        e--;
                        if (disY > 0)
                            curY++;
                        else
                            curY--;
                    }
                    if (curX > 0 && curY > 0)
                        //TODO 差值计算颜色
                        frameBuff.SetPixel(curX, curY, System.Drawing.Color.Red);
                    curX += stepX;
                }
            }
            else
            {
                int stepY = 1;
                if (disY < 0) stepY = -stepY;
                else if (disY > 0) k = disX / disY;
                curX = startX;
                curY = startY;
                while (curY != endY)
                {
                    e += k;
                    if (e > 0)
                    {
                        e--;
                        e--;
                        if (disX > 0)
                            curX++;
                        else
                            curX--;
                    }
                    //TODO 差值计算颜色
                    frameBuff.SetPixel(curX, curY, System.Drawing.Color.Red);
                    curY += stepY;
                }
            }
        }

        private void BresenhamDrawLine2(Vertex p1, Vertex p2)
        {
            int x = (int)(System.Math.Round(p1.point.x, MidpointRounding.AwayFromZero));
            int y = (int)(System.Math.Round(p1.point.y, MidpointRounding.AwayFromZero));
            int dx = (int)(System.Math.Round(p2.point.x - p1.point.x, MidpointRounding.AwayFromZero));
            int dy = (int)(System.Math.Round(p2.point.y - p1.point.y, MidpointRounding.AwayFromZero));
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
            if (dx >= 0)
            {
                stepx = 1;
            }
            else
            {
                stepx = -1;
                dx = System.Math.Abs(dx);
            }
            if (dy >= 0)
            {
                stepy = 1;
            }
            else
            {
                stepy = -1;
                dy = System.Math.Abs(dy);
            }
            int dx2 = 2 * dx;
            int dy2 = 2 * dy;
            if (dx > dy)
            {
                int max = dx;
                if (max == 0)
                {
                    max = int.MaxValue;
                }
                int error = dy2 - dx;
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    frameBuff.SetPixel(x, y, System.Drawing.Color.Red);
                }
                if (error >= 0)
                {
                    error -= dx2;
                    y += stepy;
                }
                error += dy2;
                x += stepx;
            }
            else
            {
                int max = dy;
                if (max == 0)
                {
                    max = int.MaxValue;
                }
                int error = dx2 - dy;
                for (int i = 0; i < dy; i++)
                {
                    if (x >= 0 && y >= 0 && x < width && y < height)
                    {
                        frameBuff.SetPixel(x, y, System.Drawing.Color.Red);
                    }
                    else
                    {

                    }
                    if (error >= 0)
                    {
                        error -= dy2;
                        x += stepx;
                    }
                    error += dx2;
                    y += stepy;

                }
            }
        }

        void Swap<T>(ref T i1,ref T i2)
        {
            T temp = i1;
            i1 = i2;
            i2 = temp;
        }

        void DDALine(int xa, int ya, int xb, int yb)
        {
            float delta_x, delta_y, x, y;
            int dx, dy, steps;
            dx = xb - xa;
            dy = yb - ya;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                steps = Math.Abs(dx);
            }
            else
            {
                steps = Math.Abs(dy);
            }
            delta_x = (float)dx / (float)steps;
            delta_y = (float)dy / (float)steps;
            x = xa;
            y = ya;
            //  glClear(GL_COLOR_BUFFER_BIT);
            for (int i = 1; i <= steps; i++)
            {
                x += delta_x;
                y += delta_y;
                frameBuff.SetPixel((int)x, (int)y, System.Drawing.Color.Black);
            }
        }

        #endregion
    }
}
