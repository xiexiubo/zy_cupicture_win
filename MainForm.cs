﻿// MainForm.cs
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
using zy_cutPicture.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

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
        private Bitmap image_combina;
        private Bitmap image_selected;
        private Bitmap image_selected_fast;
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
        private bool isStartCut = false;



        public MainForm()
        {
            InitializeComponent();
            InitializeWorker();
            InitDefautImage();

            InitializeListView();
            InitPictureEvent();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_debug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel_image.SuspendLayout();
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
            this.pictureBox.Size = new System.Drawing.Size(500, 581);
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
            this.pictureBox_debug.Size = new System.Drawing.Size(500, 581);
            this.pictureBox_debug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
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
            this.panel2_content.Location = new System.Drawing.Point(100, 0);
            this.panel2_content.Name = "panel2_content";
            this.panel2_content.Size = new System.Drawing.Size(500, 581);
            this.panel2_content.TabIndex = 13;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(602, 651);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel_image);
            this.Controls.Add(this.controlPanel);
            this.DoubleBuffered = true;
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
            this.sourceImage = null ;
            this.image_debug?.Dispose();
            this.image_debug = null;
            this.visited = null;
            this.subRegions.Clear();
            this.removeRects.Clear();
            this.newRects.Clear();
            //this.

            this.text_input.Text = path;
            this.text_output.Text = path.Replace(Path.GetExtension(path), "\\");
            this.sourceImage = new Bitmap(path);
            this.pictureBox.Image = sourceImage;
            this.pictureBox.BackColor = Color.Transparent;
            //this.image_debug = this.GenerateImage_debug(sourceImage);
            this.pictureBox_debug.Image = sourceImage;
            //this.pictureBox_debug.BackColor = Color.Transparent; 
            zoomFactor = 1.0f;
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

        public static Color BlendColors(Color destination, params Color[] sources)
        {

            for (int i = 0; i < sources.Length; i++)
            {
                Color source = sources[i];
                // 将颜色分量转换为0 - 1范围的浮点数
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
                //if (outA <= 0) return Color.FromArgb(0, 0, 0, 0);

                // 计算各颜色通道（未预乘Alpha的混合公式）
                float outR = (srcR * srcA + dstR * dstA * (1 - srcA)) / outA;
                float outG = (srcG * srcA + dstG * dstA * (1 - srcA)) / outA;
                float outB = (srcB * srcA + dstB * dstA * (1 - srcA)) / outA;

                // 转换回字节并四舍五入
                int a = (int)(outA * 255 + 0.5f);
                int r = (int)(outR * 255 + 0.5f);
                int g = (int)(outG * 255 + 0.5f);
                int b = (int)(outB * 255 + 0.5f);

                // 确保数值在0 - 255范围内
                a = Math.Min(255, Math.Max(0, a));
                r = Math.Min(255, Math.Max(0, r));
                g = Math.Min(255, Math.Max(0, g));
                b = Math.Min(255, Math.Max(0, b));

                destination = Color.FromArgb(a, r, g, b);
            }

            return destination;
        }
        private void SetPixel(Point pos, Bitmap img)
        {
            if (this.sourceImage == null || img == null) return;

            Color c_debug = Color.FromArgb(0, 0, 0, 0);
            Color c_combina = Color.FromArgb(0, 0, 0, 0);
            Color c_selected = Color.FromArgb(0, 0, 0, 0);
            Color c_selected_pre = Color.FromArgb(0, 0, 0, 0);
            if (this.visited != null)
            {
                c_debug = this.visited[pos.X, pos.Y].color;
                if (this.visited[pos.X, pos.Y].isCombine)
                    c_combina = Color.FromArgb(255, 0, 255, 0);
                if (!this.IsTransparent(this.sourceImage.GetPixel(pos.X, pos.Y)) && this.visited[pos.X, pos.Y].isCheck)
                    c_selected = Color.FromArgb(200, 0, 255, 255);
                if (this.visited[pos.X, pos.Y].isSelected)
                    c_selected_pre = Color.FromArgb(200, 0, 0, 255);
                img.SetPixel(pos.X, pos.Y, BlendColors(this.sourceImage.GetPixel(pos.X, pos.Y), c_debug, c_combina, c_selected, c_selected_pre));
               // img.SetPixel(pos.X, pos.Y, BlendColors(this.sourceImage.GetPixel(pos.X, pos.Y), c_debug, c_combina));
            }
        }
        private void GenerateImage_debug(Bitmap source,List<Point> listPos=null)
        {
            int width = source.Width;
            int height = source.Height;
            if (this.image_debug == null|| image_debug.Width==0) 
            {
                this.image_debug = new Bitmap(width, height);
            }
            if (listPos != null)
            {
                for (int i = 0; i < listPos.Count; i++) 
                {
                    SetPixel(listPos[i], this.image_debug);
                }
            }
            else 
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        SetPixel(new Point(x, y), this.image_debug);
                    }
                }
            }

            this.pictureBox_debug.Image = this.image_debug;

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
        /// <summary>
        /// 矩形坐标点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        static List<Point> GetRectangleEdgeCoordinates(Rectangle rect)
        {
            List<Point> points = new List<Point>();

            // 上边界
            for (int x = rect.X; x <= rect.X + rect.Width; x++)
            {
                points.Add(new Point(x, rect.Y));
            }

            // 下边界
            for (int x = rect.X; x <= rect.X + rect.Width; x++)
            {
                points.Add(new Point(x, rect.Y + rect.Height));
            }

            // 左边界（排除上下边界已添加的点）
            for (int y = rect.Y + 1; y <= rect.Y + rect.Height; y++)
            {
                points.Add(new Point(rect.X, y));
            }

            // 右边界（排除上下边界已添加的点）
            for (int y = rect.Y + 1; y <= rect.Y + rect.Height; y++)
            {
                points.Add(new Point(rect.X + rect.Width, y));
            }

            return points;
        }

        static List<Point> GetRectangleEdgePoints(Rectangle rect, int d, Rectangle rect2)
        {
            List<Point> points = new List<Point>();

            // 顶部边
            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Top; y < rect.Top + d; y++)
                {
                    Point p = new Point(x, y);
                    if (rect2.Contains(p))
                    {
                        points.Add(p);
                    }
                }
            }

            // 底部边
            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Bottom - d; y < rect.Bottom; y++)
                {
                    Point p = new Point(x, y);
                    if (rect2.Contains(p))
                    {
                        points.Add(p);
                    }
                }
            }

            // 左边边
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.Left; x < rect.Left + d; x++)
                {
                    Point p = new Point(x, y);
                    if (rect2.Contains(p))
                    {
                        points.Add(p);
                    }
                }
            }

            // 右边边
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.Right - d; x < rect.Right; x++)
                {
                    Point p = new Point(x, y);
                    if (rect2.Contains(p))
                    {
                        points.Add(p);
                    }
                }
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
            GenerateMenuItems();
        }

        private void ExportSubImages(List<Rectangle> rects=null)
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


            var listOut = new List<Rectangle>();
            if (rects != null) { listOut.AddRange(rects); }
            else
            {
                listOut.AddRange(this.subRegions);
                listOut.AddRange(this.newRects);
                foreach (var r in this.removeRects)
                {
                    listOut.Remove(r);
                }
            }



            foreach (var rect in listOut)
            {
                if (rect.Width == 0 || rect.Height == 0)
                {

                    Console.WriteLine($"跳过0宽高{rect}");
                    continue;
                }

                var rectOut = ExpandBounds(rect);
                using (var subImage = CropImage(sourceImage, rectOut))
                {                   
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

        /// <summary>
        /// 多个合成一个
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Rectangle MergeRectanglesOne(List<Rectangle> list)
        {
            if (list == null || list.Count == 0)
            {
                return Rectangle.Empty;
            }

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxRight = int.MinValue;
            int maxBottom = int.MinValue;

            foreach (Rectangle rect in list)
            {
                minX = Math.Min(minX, rect.X);
                minY = Math.Min(minY, rect.Y);
                maxRight = Math.Max(maxRight, rect.Right);
                maxBottom = Math.Max(maxBottom, rect.Bottom);
            }

            return new Rectangle(minX, minY, maxRight - minX, maxBottom - minY);
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
        /// <summary>
        /// 有相交的部分合成 返回变化的部分 并移除
        /// </summary>
        /// <param name="result"> </param>
        /// <returns></returns>
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
            int p = Math.Max(1, spacing / 2);
            return new Rectangle(
                Math.Max(0, bounds.X - p),
                Math.Max(0, bounds.Y - p),
                Math.Min(sourceImage.Width - bounds.X, bounds.Width + 1 + 2 * p),
                Math.Min(sourceImage.Height - bounds.Y, bounds.Height + 1 + 2 * p));
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
            if (this.visited == null || sourceImage==null)
            {
                Console.WriteLine("");
                return;
            }

            this.GenerateImage_debug(sourceImage);
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

        #region 左菜单
        private MenuListView MenuItemPanel;
       
        private void InitializeListView()
        {
            MenuItemPanel = new MenuListView(this);
            MenuItemPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            MenuItemPanel.Height = this.panel1_menu.Height;
            this.panel1_menu.Controls.Add(MenuItemPanel);
            this.InitResizing();
            //MenuItemPanel.ItemChecked += ListView1_ItemChecked;
            //MenuItemPanel.ItemSelectionChanged += ListView1_ItemSelected;
        }

        private void GenerateMenuItems()
        {
            if (this.subRegions == null || this.subRegions.Count == 0) 
                return;
            MenuItemPanel.Clear();
            MenuItemPanel.Dispose();
            MenuItemPanel = null;
            InitializeListView();
            MenuItemPanel.ItemChecked -= ListView1_ItemChecked;
            MenuItemPanel.ItemSelectionChanged -= ListView1_ItemSelected;
            for (int i = 0; i < this.subRegions.Count; i++)
            {
                var r= this.subRegions[i];
                MenuItemPanel.AddMenuItem($"{i}|{r.X}|{r.Y}|{r.Width}|{r.Height}");
            }
            MenuItemPanel.ItemChecked += ListView1_ItemChecked;
            MenuItemPanel.ItemSelectionChanged += ListView1_ItemSelected;
            MenuItemPanel.Refresh();
            this.removeRects.Clear();
            this.newRects.Clear();
        }
        #endregion

        #region ResizingMenuPanel
        private bool isResizing = false;
        private Point lastMousePositionResizing;
        private const int ResizeMargin = 5; // 可拖动区域的边距

        private void InitResizing() 
        {
            // 订阅鼠标事件
            this.MenuItemPanel.MouseDown += Panel_MouseDown;
            this.MenuItemPanel.MouseMove += Panel_MouseMove;
            this.MenuItemPanel.MouseUp += Panel_MouseUp;
            //this.MenuItemPanel.Width = this.panel1_menu.Width+50;
            this.MenuItemPanel.SetWidth(this.panel1_menu.Width);
        }
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            // 检查是否点击了右边线
            if (e.Location.X >= ((MenuListView)sender).Width - ResizeMargin)
            {
                isResizing = true;
                lastMousePositionResizing = e.Location;
                Cursor.Current = Cursors.SizeWE; // 设置光标为双向箭头
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            var panel = (MenuListView)sender;

            // 如果鼠标在右边线附近，改变光标为双向箭头
            if (e.Location.X >= panel.Width - ResizeMargin)
            {
                Cursor.Current = Cursors.SizeWE; // 设置光标为双向箭头
            }
            else
            {
                Cursor.Current = Cursors.Default; // 恢复默认光标
            }

            // 如果正在调整大小
            if (isResizing)
            {
                // 计算宽度变化
                int widthChange = e.Location.X - lastMousePositionResizing.X;
                panel.Width += widthChange;
                 this.panel1_menu.Width= this.MenuItemPanel.Width;

                // 更新鼠标位置
                lastMousePositionResizing = e.Location;
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            Cursor.Current = Cursors.Default; // 恢复默认光标
        }
        #endregion

        #region 绘制选中 右键功能
        List<Rectangle> removeRects= new List<Rectangle>();
        List<Rectangle> newRects= new List<Rectangle>();
        
        public void MergeRectanglesOneName(List<string> rects)
        {
            List<Rectangle> list = new List<Rectangle>();
            foreach (var v in rects)
            {
                var aary = v.Split('|');
                var r = new Rectangle();
                r.X = int.Parse(aary[1]);
                r.Y = int.Parse(aary[2]);
                r.Width = int.Parse(aary[3]);
                r.Height = int.Parse(aary[4]);
                list.Add(r);
                removeRects.Add(r);
            }
            var r2 = MergeRectanglesOne(list);
            this.newRects.Add(r2);
            if (this.visited == null) return;
            var listPos = GetRectangleEdgePoints(r2,4,new Rectangle(0,0,this.sourceImage.Width,this.sourceImage.Height));
            for (int i = 0; i < listPos.Count; i++) 
            {
                this.visited[listPos[i].X, listPos[i].Y].isCombine = true;
            }
            this.GenerateImage_debug(this.sourceImage, listPos);
        }
        public void ExportSelectedItems(List<string> rects,bool isComb)
        {
            List<Rectangle> list = new List<Rectangle>();
            foreach (var v in rects)
            {
                var aary = v.Split('|');
                var r = new Rectangle();
                r.X = int.Parse(aary[1]);
                r.Y = int.Parse(aary[2]);
                r.Width = int.Parse(aary[3]);
                r.Height = int.Parse(aary[4]);
                list.Add(r);
                removeRects.Add(r);
            }
            if (isComb)
            {
                var r = MergeRectanglesOne(list);
                list.Clear();
                list.Add(r);
                this.ExportSubImages(list);
            }
            else 
            {
                this.ExportSubImages(list);
            }            
           // this.newRects.Add(MergeRectanglesOne(list));
        }
        private void ListView1_ItemSelected(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // 当 CheckBox 状态改变时，此方法会被调用
            ListViewItem item = e.Item;
            if (item.Selected)
            {
                //MessageBox.Show($"Item '{item.Text}' 已被选中。");
                SetVisitedSelectedName(true,null, item.Text);
            }
            else
            {
                //MessageBox.Show($"Item '{item.Text}' 已被取消选中。");
                SetVisitedSelectedName(false,null, item.Text);
            }
        }
        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 当 CheckBox 状态改变时，此方法会被调用
            ListViewItem item = e.Item;
            if (item.Checked)
            {
                //MessageBox.Show($"Item '{item.Text}' 已被选中。");
                SetVisitedSelectedName(null, true, item.Text);
            }
            else
            {
                //MessageBox.Show($"Item '{item.Text}' 已被取消选中。");
                SetVisitedSelectedName(null,false, item.Text);
            }
        }

      
        public void SetVisitedSelectedName(bool? isSelectd,bool? isCheck, params string[] rects) 
        {
            List<Rectangle> list = new List<Rectangle>();
            foreach (var v in rects)
            {
                var aary = v.Split('|');
                var r = new Rectangle();
                r.X = int.Parse(aary[1]);
                r.Y = int.Parse(aary[2]);
                r.Width = int.Parse(aary[3]);
                r.Height = int.Parse(aary[4]);
                list.Add(r);
            }
            SetVisitedChecked(isSelectd, isCheck, list.ToArray());
        }
        private void SetVisitedChecked(bool? isSelectd, bool? isCheck, params Rectangle[] rects) 
        {
            if (this.visited == null || rects == null || rects.Length == 0) return;

            //foreach (var v in this.visited)
            //{
            //    v.isSelected = false;
            //}
            var listPos=new List<Point>();
            foreach (var rect in rects) 
            {
                var list = GetPointsInRectangle(this.ExpandBounds(rect));
                listPos.AddRange(list);
                foreach (var p in list)
                {
                    if (isSelectd != null)
                        this.visited[p.X, p.Y].isSelected = isSelectd.Value;
                    if (isCheck != null)
                        this.visited[p.X, p.Y].isCheck = isCheck.Value;
                }
            }
            if(this.sourceImage != null)
                this.GenerateImage_debug(this.sourceImage, listPos);
        }
        public static List<Point> GetPointsInRectangle(Rectangle rect)
        {
            List<Point> points = new List<Point>();
            int px = rect.Width < 10 ? 1 : 1;
            int py = rect.Height < 10 ? 1 : 1;
            for (int x = rect.Left; x < rect.Right; x+= px)
            {
                for (int y = rect.Top; y < rect.Bottom; y+= py)
                {
                    points.Add(new Point(x, y));
                }
            }
            return points;
        }
        #endregion

        #region 对Picture点击选中操作

        private Point startPoint_pictureBox_debug;
        private bool isDragging_pictureBox_debug = false;
        private Rectangle currentDragRect_pictureBox_debug = Rectangle.Empty;
        private ContextMenuStrip contextMenuStrip_pictureBox_debug;
        private void InitPictureEvent() 
        {
            this.pictureBox_debug.MouseClick += pictureBox_debug_MouseClick;
            this.pictureBox_debug.MouseDown += pictureBox_debug_MouseDown;
            this.pictureBox_debug.MouseMove += pictureBox_debug_MouseMove;
            this.pictureBox_debug.MouseUp += pictureBox_debug_MouseUp;
            this.pictureBox_debug.Paint += pictureBox_debug_Paint;


            // 创建上下文菜单
            contextMenuStrip_pictureBox_debug = new ContextMenuStrip();
            contextMenuStrip_pictureBox_debug.Items.AddRange(new ToolStripItem[]
          {
                new ToolStripMenuItem("复制", null, (s, e) => this.MenuItemPanel.CopyMenuItem_Click()),
                new ToolStripMenuItem("勾选", null, (s, e) => this.MenuItemPanel.SelectCurr()),
                new ToolStripMenuItem("全勾选", null, (s, e) => this.MenuItemPanel.SelectAllItems()),
                new ToolStripMenuItem("清空勾选", null, (s, e) => this.MenuItemPanel.ClearSelection()),
                new ToolStripSeparator(),

                new ToolStripMenuItem("合并选中", null, (s, e) => this.MenuItemPanel.MergeSelectedItems()),
                  new ToolStripMenuItem("导出选中", null, (s, e) => this.MenuItemPanel.ExportSelectedItems(false)),
                  new ToolStripMenuItem("导出选中(合并的)", null, (s, e) => this.MenuItemPanel.ExportSelectedItems(true)),
                new ToolStripMenuItem("手动排列选中", null, (s, e) => this.MenuItemPanel.ArrangeSelected()),
                new ToolStripMenuItem("手动排列全部", null, (s, e) => this.MenuItemPanel.ArrangeAll())
          });

            this.pictureBox_debug.ContextMenuStrip = contextMenuStrip_pictureBox_debug;
        }
        private void CheckedMenuItem_Click()
        {
            // 这里添加合并操作的代码
            //MessageBox.Show("你点击了打勾");

            foreach (ListViewItem item in this.MenuItemPanel.Items)
            {
                if (item.Selected)
                {
                    item.Checked = true;
                    this.MenuItemPanel.EnsureVisible(item.Index);
                }
            }
            this.MenuItemPanel.Focus();
        }   
        private void MergeMenuItem_Click()
        {
            // 这里添加合并操作的代码
            //MessageBox.Show("你点击了合并");
            this.MenuItemPanel.MergeSelectedItems();
            this.MenuItemPanel.Focus();
        }

        private void pictureBox_debug_MouseClick(object sender, MouseEventArgs e)
        {
            Point pixelPoint = ConvertToBitmapCoordinates(this.pictureBox_debug, e.Location);
            for (int i = 0; i < this.subRegions.Count; i++) 
            {
                if (this.subRegions[i].Contains(pixelPoint))
                {
                    var r = this.subRegions[i];
                    var text = $"|{r.X}|{r.Y}|{r.Width}|{r.Height}";
                    List<string> lines = new List<string>();
                    lines.Add(text);
                    this.SelectListViewItem(this.MenuItemPanel, lines);
                    break;
                }
            }
            // 可以在这里使用转换后的坐标
            Console.WriteLine($"Bitmap Pixel Coordinates: {pixelPoint.X}, {pixelPoint.Y}");
        }
        private void pictureBox_debug_Paint(object sender, PaintEventArgs e)
        {
            if (isDragging_pictureBox_debug && !currentDragRect_pictureBox_debug.IsEmpty)
            {
                // 计算在 PictureBox 上显示的矩形位置
                Rectangle displayRect = GetDisplayRectangle(this.pictureBox_debug, currentDragRect_pictureBox_debug);
                e.Graphics.DrawRectangle(Pens.Red, displayRect);
            }
        }
        private void pictureBox_debug_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint_pictureBox_debug = e.Location;
                isDragging_pictureBox_debug = true;
            }
        }

        private void pictureBox_debug_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging_pictureBox_debug)
            {
                Rectangle dragRect = GetBitmapRectangle(pictureBox_debug, startPoint_pictureBox_debug, e.Location);
                // 这里可以进行绘制矩形等操作，例如在 PictureBox 上绘制拖动的矩形
                // 你可以根据需要更新 UI 显示
                currentDragRect_pictureBox_debug = GetBitmapRectangle(this.pictureBox_debug, startPoint_pictureBox_debug, e.Location);
                this.pictureBox_debug.Invalidate(); // 触发 Paint 事件
            }
        }

        private void pictureBox_debug_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging_pictureBox_debug)
            {
               
                Rectangle finalRect = GetBitmapRectangle(pictureBox_debug, startPoint_pictureBox_debug, e.Location);
                List<string> lines = new List<string>();
                if (e.Button == MouseButtons.Left) 
                {
                    for (int i = 0; i < this.subRegions.Count; i++)
                    {
                        if (this.subRegions[i].IntersectsWith(finalRect))
                        {
                            var r = this.subRegions[i];
                            var text = $"|{r.X}|{r.Y}|{r.Width}|{r.Height}";
                            lines.Add(text);
                        }
                    }
                }
                this.SelectListViewItem(this.MenuItemPanel, lines);

                if (Control.ModifierKeys == Keys.Alt)
                {

                    this.MenuItemPanel.MergeSelectedItems();
                    // 这里可以添加 Ctrl 键抬起后要执行的逻辑代码，也就是判断用户此时没按 Ctrl 键了
                    Console.WriteLine("Ctrl 键已经抬起，当前没按 Ctrl 键");
                }
                // 可以在这里使用最终的矩形坐标，例如输出到控制台
                Console.WriteLine($"Bitmap Rectangle: {finalRect.X}, {finalRect.Y}, {finalRect.Width}, {finalRect.Height}");
                this.pictureBox_debug.Invalidate(); // 触发 Paint 事件
                isDragging_pictureBox_debug = false;
            }
        }

        private Rectangle GetBitmapRectangle(PictureBox pictureBox, Point start, Point end)
        {
            if (pictureBox.Image == null)
            {
                return Rectangle.Empty;
            }

            // 获取 PictureBox 的显示区域
            Rectangle displayRectangle = pictureBox.ClientRectangle;

            // 获取图像的原始尺寸
            Size imageSize = pictureBox.Image.Size;

            // 计算图像在 PictureBox 中的缩放比例
            float scaleX = (float)imageSize.Width / displayRectangle.Width;
            float scaleY = (float)imageSize.Height / displayRectangle.Height;

            // 根据缩放比例计算起始点和结束点在位图中的像素坐标
            int startX = (int)(start.X * scaleX);
            int startY = (int)(start.Y * scaleY);
            int endX = (int)(end.X * scaleX);
            int endY = (int)(end.Y * scaleY);

            // 确保矩形的坐标和尺寸为正值
            int x = Math.Min(startX, endX);
            int y = Math.Min(startY, endY);
            int width = Math.Abs(endX - startX);
            int height = Math.Abs(endY - startY);

            return new Rectangle(x, y, width, height);
        }
        private Rectangle GetDisplayRectangle(PictureBox pictureBox, Rectangle bitmapRect)
        {
            if (pictureBox.Image == null)
            {
                return Rectangle.Empty;
            }

            // 获取 PictureBox 的显示区域
            Rectangle displayRectangle = pictureBox.ClientRectangle;

            // 获取图像的原始尺寸
            Size imageSize = pictureBox.Image.Size;

            // 计算图像在 PictureBox 中的缩放比例
            float scaleX = (float)displayRectangle.Width / imageSize.Width;
            float scaleY = (float)displayRectangle.Height / imageSize.Height;

            // 根据缩放比例计算在 PictureBox 上显示的矩形位置
            int x = (int)(bitmapRect.X * scaleX);
            int y = (int)(bitmapRect.Y * scaleY);
            int width = (int)(bitmapRect.Width * scaleX);
            int height = (int)(bitmapRect.Height * scaleY);

            return new Rectangle(x, y, width, height);
        }

        private Point ConvertToBitmapCoordinates(PictureBox pictureBox, Point clickPoint)
        {
            if (pictureBox.Image == null)
            {
                return Point.Empty;
            }

            // 获取 PictureBox 的显示区域
            Rectangle displayRectangle = pictureBox.ClientRectangle;

            // 获取图像的原始尺寸
            Size imageSize = pictureBox.Image.Size;

            // 计算图像在 PictureBox 中的缩放比例
            float scaleX = (float)imageSize.Width / displayRectangle.Width;
            float scaleY = (float)imageSize.Height / displayRectangle.Height;

            // 根据缩放比例计算位图中的像素坐标
            int pixelX = (int)(clickPoint.X * scaleX);
            int pixelY = (int)(clickPoint.Y * scaleY);

            return new Point(pixelX, pixelY);
        }

        private void SelectListViewItem(MenuListView listView, List<string> itemTexts)
        {
            if (Control.ModifierKeys != Keys.Control&& Control.ModifierKeys != Keys.Shift)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    item.Selected = false;
                }
                // 这里可以添加 Ctrl 键抬起后要执行的逻辑代码，也就是判断用户此时没按 Ctrl 键了
                Console.WriteLine("Ctrl 键已经抬起，当前没按 Ctrl 键");
            }
          
            foreach (string text in itemTexts)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.Text.Contains(text))
                    {
                        item.Selected = true;
                        listView.EnsureVisible(item.Index);
                        break;
                    }
                }

            }
            listView.Focus();
        }
        #endregion


    }
    public class VisitItem
    {
        public bool isVisit = false;

        //用于显示
        public bool isCheck = false;
        public bool isSelected =false;
        public bool isCombine =false;
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