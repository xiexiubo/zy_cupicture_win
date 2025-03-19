// MainForm.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using zy_cutPicture.Properties;

namespace zy_cutPicture
{
    public partial class MainForm : Form
    {
        // 控件声明
        private PictureBox pictureBox;
        private PictureBox pictureBox_debug;
        private Button btnOpen;
        private Button btnExport;
        private Button btnCancel;
        private NumericUpDown numSpacing;
        private Label lblStatus;
        private Panel controlPanel;

        // 其他成员变量
        private Bitmap sourceImage;
        private Bitmap image_debug;
        private List<Rectangle> subRegions = new List<Rectangle>();
        private BackgroundWorker worker;
        private bool isProcessing = false;
        private bool cancelRequested = false;
        private int spacing = 2;
        private int cutAlpha = 0;
        private float zoomFactor = 1.0f;
        private Point dragStart;
        private Point dragOffset;
        private Label label_pading;
        private TextBox text_output;
        private TextBox text_input;
        private Label label2;
        private Label label1;
        private Button btn_outPath;
        private Panel panel_image;
        private bool isDragging = false;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private Button btn_cut;
        private CheckBox tog_isDebug;
        private Panel panel1_menu;
        private Panel panel2_content;
        private MenuStrip menuStrip1;
        private bool isStartCut = false;



        public MainForm()
        {
            InitializeComponent();
            InitializeWorker();
            InitDefautImage();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBox_debug = new System.Windows.Forms.PictureBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numSpacing = new System.Windows.Forms.NumericUpDown();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.btn_cut = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tog_isDebug = new System.Windows.Forms.CheckBox();
            this.btn_outPath = new System.Windows.Forms.Button();
            this.text_output = new System.Windows.Forms.TextBox();
            this.text_input = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_pading = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel_image = new System.Windows.Forms.Panel();
            this.panel1_menu = new System.Windows.Forms.Panel();
            this.panel2_content = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_debug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel_image.SuspendLayout();
            this.panel1_menu.SuspendLayout();
            this.panel2_content.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(503, 581);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 10;
            this.pictureBox.TabStop = false;
            // 
            // pictureBox_debug
            // 
            this.pictureBox_debug.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_debug.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_debug.Name = "pictureBox_debug";
            this.pictureBox_debug.Size = new System.Drawing.Size(503, 581);
            this.pictureBox_debug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_debug.TabIndex = 10;
            this.pictureBox_debug.TabStop = false;
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(542, 34);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(59, 21);
            this.btnOpen.TabIndex = 9;
            this.btnOpen.Text = "浏览";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnExport.Location = new System.Drawing.Point(12, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(63, 24);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "计算切线";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(198, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numSpacing
            // 
            this.numSpacing.Dock = System.Windows.Forms.DockStyle.Right;
            this.numSpacing.Location = new System.Drawing.Point(523, 5);
            this.numSpacing.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSpacing.Name = "numSpacing";
            this.numSpacing.Size = new System.Drawing.Size(74, 21);
            this.numSpacing.TabIndex = 4;
            this.numSpacing.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numSpacing.ValueChanged += new System.EventHandler(this.NumSpacing_ValueChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.controlPanel.Controls.Add(this.btn_cut);
            this.controlPanel.Controls.Add(this.numericUpDown1);
            this.controlPanel.Controls.Add(this.tog_isDebug);
            this.controlPanel.Controls.Add(this.btn_outPath);
            this.controlPanel.Controls.Add(this.text_output);
            this.controlPanel.Controls.Add(this.text_input);
            this.controlPanel.Controls.Add(this.label2);
            this.controlPanel.Controls.Add(this.label_pading);
            this.controlPanel.Controls.Add(this.label3);
            this.controlPanel.Controls.Add(this.numSpacing);
            this.controlPanel.Controls.Add(this.label1);
            this.controlPanel.Controls.Add(this.btnOpen);
            this.controlPanel.Controls.Add(this.btnExport);
            this.controlPanel.Controls.Add(this.btnCancel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Padding = new System.Windows.Forms.Padding(5);
            this.controlPanel.Size = new System.Drawing.Size(602, 78);
            this.controlPanel.TabIndex = 5;
            // 
            // btn_cut
            // 
            this.btn_cut.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_cut.Location = new System.Drawing.Point(81, 2);
            this.btn_cut.Name = "btn_cut";
            this.btn_cut.Size = new System.Drawing.Size(64, 24);
            this.btn_cut.TabIndex = 20;
            this.btn_cut.Text = "开切";
            this.btn_cut.UseVisualStyleBackColor = false;
            this.btn_cut.Click += new System.EventHandler(this.btn_cut_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(333, 5);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(69, 21);
            this.numericUpDown1.TabIndex = 19;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // tog_isDebug
            // 
            this.tog_isDebug.AutoSize = true;
            this.tog_isDebug.Checked = true;
            this.tog_isDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tog_isDebug.Location = new System.Drawing.Point(260, 8);
            this.tog_isDebug.Name = "tog_isDebug";
            this.tog_isDebug.Size = new System.Drawing.Size(66, 16);
            this.tog_isDebug.TabIndex = 17;
            this.tog_isDebug.Text = "IsDebug";
            this.tog_isDebug.UseVisualStyleBackColor = true;
            // 
            // btn_outPath
            // 
            this.btn_outPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_outPath.Location = new System.Drawing.Point(542, 55);
            this.btn_outPath.Name = "btn_outPath";
            this.btn_outPath.Size = new System.Drawing.Size(59, 21);
            this.btn_outPath.TabIndex = 15;
            this.btn_outPath.Text = "浏览";
            this.btn_outPath.Click += new System.EventHandler(this.btn_outPath_Click);
            // 
            // text_output
            // 
            this.text_output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_output.Location = new System.Drawing.Point(65, 57);
            this.text_output.Name = "text_output";
            this.text_output.Size = new System.Drawing.Size(472, 21);
            this.text_output.TabIndex = 14;
            // 
            // text_input
            // 
            this.text_input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_input.Location = new System.Drawing.Point(65, 36);
            this.text_input.Name = "text_input";
            this.text_input.Size = new System.Drawing.Size(472, 21);
            this.text_input.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "输入图片：";
            // 
            // label_pading
            // 
            this.label_pading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_pading.AutoSize = true;
            this.label_pading.Location = new System.Drawing.Point(434, 9);
            this.label_pading.Name = "label_pading";
            this.label_pading.Size = new System.Drawing.Size(89, 12);
            this.label_pading.TabIndex = 10;
            this.label_pading.Text = "识别间隔(像素)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "切割透明度";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "输出路径：";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(508, 630);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 12);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "lblStatus";
            // 
            // panel_image
            // 
            this.panel_image.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_image.Controls.Add(this.panel1_menu);
            this.panel_image.Controls.Add(this.panel2_content);
            this.panel_image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_image.Location = new System.Drawing.Point(0, 78);
            this.panel_image.Name = "panel_image";
            this.panel_image.Size = new System.Drawing.Size(602, 573);
            this.panel_image.TabIndex = 11;
            // 
            // panel1_menu
            // 
            this.panel1_menu.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1_menu.Controls.Add(this.menuStrip1);
            this.panel1_menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1_menu.Location = new System.Drawing.Point(0, 0);
            this.panel1_menu.Name = "panel1_menu";
            this.panel1_menu.Size = new System.Drawing.Size(100, 573);
            this.panel1_menu.TabIndex = 12;
            // 
            // panel2_content
            // 
            this.panel2_content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2_content.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2_content.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.panel2_content.Controls.Add(this.pictureBox_debug);
            this.panel2_content.Controls.Add(this.pictureBox);
            this.panel2_content.Location = new System.Drawing.Point(99, 0);
            this.panel2_content.Name = "panel2_content";
            this.panel2_content.Size = new System.Drawing.Size(503, 581);
            this.panel2_content.TabIndex = 13;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(100, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(602, 651);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel_image);
            this.Controls.Add(this.controlPanel);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拆分图集器 v1.2";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_debug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel_image.ResumeLayout(false);
            this.panel1_menu.ResumeLayout(false);
            this.panel1_menu.PerformLayout();
            this.panel2_content.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializeWorker()
        {
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void InitDefautImage()
        {
            var path = Settings.Default.LastOpenPath;
            //// 获取配置文件
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //// 设置一个字符串类型的设置项
            //config.AppSettings.Settings["MyAppSetting"].Value = "Hello, App.config!";
            //// 保存配置文件
            //config.Save(ConfigurationSaveMode.Modified);

            if (!string.IsNullOrEmpty(path))
                LoadImage(path);
        }
        private void LoadImage(string path)
        {
            this.sourceImage?.Dispose();
            this.text_input.Text = path;
            this.text_output.Text = path.Replace(Path.GetExtension(path), "\\");
            this.sourceImage = new Bitmap(path);
            this.pictureBox.Image = sourceImage;
            this.pictureBox.BackColor = Color.Transparent;
            //this.image_debug = this.GenerateImage_debug(sourceImage);
            this.pictureBox_debug.Image = sourceImage;
            //this.pictureBox_debug.BackColor = Color.Transparent; 
            zoomFactor = 1.0f;
            UpdateImageDisplay();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"btnOpen_Click: {e}");
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.png;*.jpg;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.LoadImage(dialog.FileName);
                    // 保存设置
                    Properties.Settings.Default.LastOpenPath = dialog.FileName;
                    Properties.Settings.Default.Save(); // 必须调用Save()
                }
            }
        }

        public static Color BlendColors(Color source, Color destination)
        {
            // 将颜色分量转换为0-1范围的浮点数
            float srcA = source.A / 255f;
            float srcR = source.R / 255f;
            float srcG = source.G / 255f;
            float srcB = source.B / 255f;

            float dstA = destination.A / 255f;
            float dstR = destination.R / 255f;
            float dstG = destination.G / 255f;
            float dstB = destination.B / 255f;

            // 计算混合后的Alpha值
            float outA = srcA + dstA * (1 - srcA);

            // 如果完全透明则返回透明黑
            if (outA <= 0) return Color.FromArgb(0, 0, 0, 0);

            // 计算各颜色通道（未预乘Alpha的混合公式）
            float outR = (srcR * srcA + dstR * dstA * (1 - srcA)) / outA;
            float outG = (srcG * srcA + dstG * dstA * (1 - srcA)) / outA;
            float outB = (srcB * srcA + dstB * dstA * (1 - srcA)) / outA;

            // 转换回字节并四舍五入
            int a = (int)(outA * 255 + 0.5f);
            int r = (int)(outR * 255 + 0.5f);
            int g = (int)(outG * 255 + 0.5f);
            int b = (int)(outB * 255 + 0.5f);

            // 确保数值在0-255范围内
            a = Math.Min(255, Math.Max(0, a));
            r = Math.Min(255, Math.Max(0, r));
            g = Math.Min(255, Math.Max(0, g));
            b = Math.Min(255, Math.Max(0, b));

            return Color.FromArgb(a, r, g, b);
        }

        private Bitmap GenerateImage_debug(Bitmap source)
        {

            int width = source.Width;
            int height = source.Height;
            Random random = new Random();
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int a = random.Next(0, 256); // 随机透明度
                    int r = random.Next(0, 256); // 随机红色值
                    int g = random.Next(0, 256); // 随机绿色值
                    int b = random.Next(0, 256); // 随机蓝色值

                    Color c = this.visited[x, y].color;
                    // c = Color.FromArgb(a,r,g,b);
                    bitmap.SetPixel(x, y, BlendColors(c, source.GetPixel(x, y)));
                }
            }
            return bitmap;
        }
        private void ModifyPixelColor(Bitmap bitmap, int x, int y, Color color)
        {
            try
            {
                if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                {
                    bitmap.SetPixel(x, y, color);
                    Console.WriteLine($"已将坐标 ({x}, {y}) 的像素颜色修改为 {color.Name}");
                }
                else
                {
                    Console.WriteLine($"坐标 ({x}, {y}) 超出图片范围。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"修改像素颜色时出错: {ex.Message}");
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (sourceImage == null) return;

            isProcessing = true;
            cancelRequested = false;
            subRegions.Clear();
            worker.RunWorkerAsync();
            lblStatus.Text = "解析中...";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelRequested = true;
            worker.CancelAsync();
        }

        private void NumSpacing_ValueChanged(object sender, EventArgs e)
        {
            spacing = (int)numSpacing.Value;
        }
        VisitItem[,] visited;
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.subRegions.Clear();
            this.visited = null;
            visited = new VisitItem[sourceImage.Width, sourceImage.Height];
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    visited[i, j] = new VisitItem();
                }
            }

            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (visited[x, y].isVisit || IsTransparent(sourceImage.GetPixel(x, y)))
                        continue;

                    bool hasRect = false;
                    for (int i = 0; i < subRegions.Count; i++)
                    {
                        Rectangle r = this.subRegions[i];
                        //if (Contains(r, new Point(x, y)))
                        if (r.Contains(new Point(x, y)))
                        {
                            this.visited[x, y].isVisit = true;
                            this.visited[x, y].color = Color.FromArgb(125, 0, 0, 255);
                            hasRect = true;
                            break;
                        }
                    }
                    if (hasRect)
                    {
                        continue;
                    }

                    var bounds = FindRegionBounds(x, y, visited);
                    worker.ReportProgress(0, bounds);
                }
            }

        }

        // 在MainForm类中添加以下方法
        private bool IsValidPixel(Point point, VisitItem[,] visited)
        {
            // 边界检查（虽然GetNeighbors已经过滤，但保留以确保安全）
            if (point.X < 0 || point.X >= sourceImage.Width) return false;
            if (point.Y < 0 || point.Y >= sourceImage.Height) return false;

            // 检查是否已访问过
            if (visited[point.X, point.Y].isVisit) return false;

            // 检查像素透明度
            Color pixelColor = sourceImage.GetPixel(point.X, point.Y);
            return !this.IsTransparent(pixelColor); // 只要alpha通道>0即视为有效像素
        }

        // 修改后的FindRegionBounds方法保持不变
        private Rectangle FindRegionBounds(int startX, int startY, VisitItem[,] visited)
        {
            var queue = new Queue<Point>();
            var bounds = new Rectangle(startX, startY, 0, 0);
            //Console.WriteLine($"{bounds}    {startX} {startY}");
            queue.Enqueue(new Point(startX, startY));
            visited[startX, startY].isVisit = true;
            int removeIndex = -1;
            while (queue.Count > 0)
            {
                if (worker.CancellationPending) return Rectangle.Empty;

                var point = queue.Dequeue();
                UpdateBounds(ref bounds, point);
                var listNeighbor = GetNeighbors(bounds, point);
                for (int i = listNeighbor.Count - 1; i >= 0; i--)
                {
                    //UpdateBounds(ref bounds, listNeighbor[i]);
                }
                foreach (var neighbor in listNeighbor)
                {
                    if (IsValidPixel(neighbor, visited)) // 这里调用新增的方法
                    {
                        if (!bounds.Contains(neighbor))
                        {
                            visited[neighbor.X, neighbor.Y].isVisit = true;
                            queue.Enqueue(neighbor);
                            visited[neighbor.X, neighbor.Y].color = Color.FromArgb(128, 255, 0, 0);
                        }
                    }

                }
                if (queue.Count == 0)
                {

                    var rectListPos = GetRectangleEdgeCoordinates(bounds);

                    foreach (var neighbor in rectListPos)
                    {
                        if (IsValidPixel(neighbor, visited)) // 这里调用新增的方法
                        {
                            visited[neighbor.X, neighbor.Y].isVisit = true;
                            queue.Enqueue(neighbor);
                        }
                        visited[neighbor.X, neighbor.Y].color = Color.FromArgb(50, 0, 255, 0);
                    }
                    if (queue.Count == 0)
                    {
                        var bd = ProcessRectangleIntersection(bounds, this.subRegions, out removeIndex);
                        if (bd != null)
                        {
                            bounds = bd.Value;
                            rectListPos = GetRectangleEdgeCoordinates(bounds);
                            foreach (var neighbor in rectListPos)
                            {
                                if (IsValidPixel(neighbor, visited)) // 这里调用新增的方法
                                {
                                    visited[neighbor.X, neighbor.Y].isVisit = true;
                                    queue.Enqueue(neighbor);
                                }
                                // visited[neighbor.X, neighbor.Y].color = Color.FromArgb(255, 0, 0, 0);
                            }

                        }
                    }
                    if (queue.Count == 0)
                    {

                        for (int i = 0; i < rectListPos.Count; i += 3)
                        {
                            Point neighbor = rectListPos[i];
                            visited[neighbor.X, neighbor.Y].color = Color.FromArgb(255, 0, 0, 0);
                        }
                    }
                }
            }
            if (removeIndex != -1)
            {
                Console.WriteLine($"{removeIndex} 与{this.subRegions.Count} 有交叉，合并");
                this.subRegions.RemoveAt(removeIndex);
            }
            return bounds;
        }
        public bool Contains(Rectangle rect, Point position)
        {
            return position.X >= rect.X && position.Y >= rect.Y && position.X <= (rect.X + rect.Width) && position.Y <= (rect.Y + rect.Height);
        }

        static List<Point> GetRectangleEdgeCoordinates(Rectangle rect)
        {
            List<Point> points = new List<Point>();

            // 上边界
            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                points.Add(new Point(x, rect.Y));
            }

            // 下边界
            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                points.Add(new Point(x, rect.Y + rect.Height));
            }

            // 左边界（排除上下边界已添加的点）
            for (int y = rect.Y + 1; y < rect.Y + rect.Height - 1; y++)
            {
                points.Add(new Point(rect.X, y));
            }

            // 右边界（排除上下边界已添加的点）
            for (int y = rect.Y + 1; y < rect.Y + rect.Height - 1; y++)
            {
                points.Add(new Point(rect.X + rect.Width, y));
            }

            return points;
        }
        private List<Point> GetNeighbors(Rectangle bounds, Point p)
        {
            var list = new List<Point>();
            for (int dx = spacing; dx >= -spacing; dx--)
            {
                for (int dy = -spacing; dy <= spacing; dy++)
                {
                    int x = p.X + dx;
                    int y = p.Y + dy;
                    if (bounds.Contains(new Point(x, y)))
                        continue;
                    if (x >= 0 && x < sourceImage.Width && y >= 0 && y < sourceImage.Height)
                    {
                        var pos = new Point(x, y);
                        list.Add(pos);
                    }
                }
            }
            return list;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Console.WriteLine($"Worker_ProgressChanged {e.UserState} ,{e.UserState is Rectangle}");
            if (e.UserState is Rectangle rect)
            {
                subRegions.Add(rect);
                //MergeRectangles(subRegions);
                pictureBox.Invalidate();
            }
            lblStatus.Text = e.ToString();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isProcessing = false;
            if (!cancelRequested && this.isStartCut)
            {
                ExportSubImages();
                this.isStartCut = false;
            }
            lblStatus.Text = "Ready";
            if (this.tog_isDebug.Checked)
            {
                this.ShowDebug_texture();
            }
        }

        private void ExportSubImages()
        {
            var path = this.text_output.Text;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine($"目录 {path} 创建成功。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"创建目录 {path} 时出错: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"目录 {path} 已存在。");
            }
            foreach (var rect in subRegions)
            {
                if (rect.Width == 0 || rect.Height == 0)
                {

                    Console.WriteLine($"跳过0宽高{rect}");
                    continue;
                }
                using (var subImage = CropImage(sourceImage, rect))
                {
                    var rectOut = ExpandBounds(rect);
                    subImage.Save(
                        Path.Combine(path,
                            $"{rectOut.X}_{rectOut.Y}_{rectOut.Width}_{rectOut.Height}.png"),
                        ImageFormat.Png);
                }
            }
            try
            {
                Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开文件夹 {path} 时出错: {ex.Message}");
            }
        }

        static Rectangle? ProcessRectangleIntersection(Rectangle rect, List<Rectangle> list, out int index)
        {
            index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (rect.IntersectsWith(list[i]))
                {
                    Rectangle intersectingRect = list[i];
                    //list.RemoveAt(i);
                    index = i;
                    //return Rectangle.Union(rect, intersectingRect);
                    int newX = Math.Min(rect.X, intersectingRect.X);
                    int newY = Math.Min(rect.Y, intersectingRect.Y);
                    int newWidth = Math.Max(rect.Right, intersectingRect.Right) - newX;
                    int newHeight = Math.Max(rect.Bottom, intersectingRect.Bottom) - newY;

                    return new Rectangle(newX, newY, newWidth, newHeight);
                }
            }
            return null;
        }
        static List<Rectangle> MergeRectangles(List<Rectangle> result)
        {
            List<Rectangle> changeList = new List<Rectangle>();
            bool merged;
            do
            {
                merged = false;
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = i + 1; j < result.Count; j++)
                    {
                        if (result[i].IntersectsWith(result[j]))
                        {
                            result[i] = Rectangle.Union(result[i], result[j]);
                            changeList.Add(result[i]);
                            result.RemoveAt(j);
                            merged = true;
                            break;
                        }
                    }
                    if (merged)
                    {
                        break;
                    }
                }
            } while (merged);

            return result;
        }

        private Bitmap CropImage(Bitmap source, Rectangle rect)
        {
            var bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(source,
                    new Rectangle(0, 0, rect.Width, rect.Height),
                    rect,
                    GraphicsUnit.Pixel);
            }
            return bmp;
        }

        private void UpdateImageDisplay()
        {
            //if (sourceImage == null) return;

            //var zoomedImage = new Bitmap(
            //    (int)(sourceImage.Width * zoomFactor),
            //    (int)(sourceImage.Height * zoomFactor));

            //using (var g = Graphics.FromImage(zoomedImage))
            //{
            //    g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //    g.DrawImage(sourceImage,
            //        new Rectangle(0, 0, zoomedImage.Width, zoomedImage.Height),
            //        new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
            //        GraphicsUnit.Pixel);
            //}

            //pictureBox.Image = zoomedImage;
            //pictureBox.Invalidate();

            //if (image_debug == null) return;

            //var zoomedImage_debug = new Bitmap(
            //    (int)(image_debug.Width * zoomFactor),
            //    (int)(image_debug.Height * zoomFactor));

            //using (var g = Graphics.FromImage(zoomedImage_debug))
            //{
            //    g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //    g.DrawImage(image_debug,
            //        new Rectangle(0, 0, zoomedImage_debug.Width, zoomedImage_debug.Height),
            //        new Rectangle(0, 0, image_debug.Width, image_debug.Height),
            //        GraphicsUnit.Pixel);
            //}

            //pictureBox_debug.Image = zoomedImage_debug;
            //pictureBox_debug.Invalidate();
        }



        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            zoomFactor *= e.Delta > 0 ? 1.1f : 0.9f;
            zoomFactor = Math.Max(0.1f, Math.Min(5f, zoomFactor));
            UpdateImageDisplay();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStart = e.Location;
                dragOffset = new Point(0, 0);
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // 实现平移逻辑
                dragOffset = new Point(e.X - dragStart.X, e.Y - dragStart.Y);
                if (pictureBox.Image != null)
                {
                    pictureBox.Invalidate();
                }
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private bool IsTransparent(Color color)
        {
            return color.A <= this.cutAlpha;
        }

        private void UpdateBounds(ref Rectangle rect, Point point)
        {
            if (!rect.Contains(point))
            {
                int newX = Math.Min(rect.X, point.X);
                int newY = Math.Min(rect.Y, point.Y);
                int newWidth = Math.Max(rect.Right, point.X) - newX;
                int newHeight = Math.Max(rect.Bottom, point.Y) - newY;
                rect.X = newX; rect.Y = newY;
                rect.Width = newWidth; rect.Height = newHeight;
            }
        }

        private Rectangle ExpandBounds(Rectangle bounds)
        {
            return new Rectangle(
                Math.Max(0, bounds.X - spacing),
                Math.Max(0, bounds.Y - spacing),
                Math.Min(sourceImage.Width - bounds.X, bounds.Width + 1 + 2 * spacing),
                Math.Min(sourceImage.Height - bounds.Y, bounds.Height + 1 + 2 * spacing));
        }

        private void btn_outPath_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    this.text_output.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btn_debug_Click(object sender, EventArgs e)
        {
            this.ShowDebug_texture();
        }
        private void ShowDebug_texture()
        {
            if (this.visited == null)
            {
                Console.WriteLine("");
                return;
            }
            this.pictureBox_debug.Image = GenerateImage_debug(sourceImage);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.cutAlpha = (int)numericUpDown1.Value;
        }

        private void btn_cut_Click(object sender, EventArgs e)
        {
            if (this.visited != null)
                this.ExportSubImages();
            else
            {
                this.isStartCut = true;
                this.btnExport.PerformClick();
            }
        }
    }
    public class VisitItem
    {
        public bool isVisit = false;
        public Color color = Color.FromArgb(0, 255, 255, 255);
        public VisitItem()
        {
            isVisit = false;
        }
    }
    // Program.cs
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}