namespace _3DDataType
{
    partial class RenderDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.RenderBtn = new System.Windows.Forms.Button();
            this.TextureBtn = new System.Windows.Forms.Button();
            this.CullingBtn = new System.Windows.Forms.Button();
            this.LightBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(133, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // RenderBtn
            // 
            this.RenderBtn.Location = new System.Drawing.Point(30, 25);
            this.RenderBtn.Name = "RenderBtn";
            this.RenderBtn.Size = new System.Drawing.Size(75, 23);
            this.RenderBtn.TabIndex = 1;
            this.RenderBtn.Text = "线框";
            this.RenderBtn.UseVisualStyleBackColor = true;
            this.RenderBtn.Click += new System.EventHandler(this.RenderBtn_Click);
            // 
            // TextureBtn
            // 
            this.TextureBtn.Location = new System.Drawing.Point(30, 83);
            this.TextureBtn.Name = "TextureBtn";
            this.TextureBtn.Size = new System.Drawing.Size(75, 23);
            this.TextureBtn.TabIndex = 2;
            this.TextureBtn.Text = "贴图";
            this.TextureBtn.UseVisualStyleBackColor = true;
            this.TextureBtn.Click += new System.EventHandler(this.Texture_Click);
            // 
            // CullingBtn
            // 
            this.CullingBtn.Location = new System.Drawing.Point(30, 147);
            this.CullingBtn.Name = "CullingBtn";
            this.CullingBtn.Size = new System.Drawing.Size(75, 23);
            this.CullingBtn.TabIndex = 3;
            this.CullingBtn.Text = "裁剪";
            this.CullingBtn.UseVisualStyleBackColor = true;
            this.CullingBtn.Click += new System.EventHandler(this.CullingBtn_Click);
            // 
            // LightBtn
            // 
            this.LightBtn.Location = new System.Drawing.Point(30, 219);
            this.LightBtn.Name = "LightBtn";
            this.LightBtn.Size = new System.Drawing.Size(75, 23);
            this.LightBtn.TabIndex = 4;
            this.LightBtn.Text = "灯光";
            this.LightBtn.UseVisualStyleBackColor = true;
            this.LightBtn.Click += new System.EventHandler(this.LightBtn_Click);
            // 
            // RenderDemo
            // 
            this.ClientSize = new System.Drawing.Size(1002, 623);
            this.Controls.Add(this.LightBtn);
            this.Controls.Add(this.CullingBtn);
            this.Controls.Add(this.TextureBtn);
            this.Controls.Add(this.RenderBtn);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.Name = "RenderDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Random";
            this.Load += new System.EventHandler(this.RenderDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button RenderBtn;
        private System.Windows.Forms.Button TextureBtn;
        private System.Windows.Forms.Button CullingBtn;
        private System.Windows.Forms.Button LightBtn;
    }
}