//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace zy_cutPicture
//{
//    internal class Class1
//    {

//        //this.SuspendLayout();

//        //// 配置主窗体
//        //this.Text = "Image Exporter";
//        //this.Size = new Size(800, 600);
//        //this.StartPosition = FormStartPosition.CenterScreen;
//        //this.DoubleBuffered = true;

//        //// 配置PictureBox
//        //// pictureBox.Dock = DockStyle.Fill;
//        ////pictureBox.BackColor = Color.DarkGray;
//        ////pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
//        ////pictureBox.MouseWheel += PictureBox_MouseWheel;
//        ////pictureBox.MouseDown += PictureBox_MouseDown;
//        ////pictureBox.MouseMove += PictureBox_MouseMove;
//        ////pictureBox.MouseUp += PictureBox_MouseUp;
//        ////pictureBox.Paint += PictureBox_Paint;

//        //// 配置控制面板
//        ////controlPanel.Dock = DockStyle.Right;
//        ////controlPanel.Width = 200;
//        ////controlPanel.BackColor = SystemColors.Control;
//        ////controlPanel.Padding = new Padding(5);

//        ////// 配置按钮
//        ////btnOpen.Text = "Open Image";
//        ////btnOpen.Dock = DockStyle.Top;
//        ////btnOpen.Height = 40;
//        ////btnOpen.Click += BtnOpen_Click;

//        ////btnExport.Text = "Export";
//        ////btnExport.Dock = DockStyle.Top;
//        ////btnExport.Height = 40;
//        ////btnExport.Click += BtnExport_Click;

//        ////btnCancel.Text = "Cancel";
//        ////btnCancel.Dock = DockStyle.Top;
//        ////btnCancel.Height = 40;
//        ////btnCancel.Click += BtnCancel_Click;

//        ////// 配置间距调节控件
//        ////var spacingLabel = new Label
//        ////{
//        ////    Text = "Spacing:",
//        ////    Dock = DockStyle.Top,
//        ////    Height = 20
//        ////};

//        ////numSpacing.Minimum = 0;
//        ////numSpacing.Maximum = 10;
//        ////numSpacing.Value = spacing;
//        ////numSpacing.Dock = DockStyle.Top;
//        ////numSpacing.Height = 40;
//        ////numSpacing.ValueChanged += NumSpacing_ValueChanged;

//        ////// 配置状态标签
//        ////lblStatus.Text = "Ready";
//        ////lblStatus.Dock = DockStyle.Bottom;
//        ////lblStatus.Height = 20;

//        ////// 将控件添加到控制面板
//        ////controlPanel.Controls.AddRange(new Control[]
//        ////{
//        ////    btnCancel,
//        ////    btnExport,
//        ////    numSpacing,
//        ////    spacingLabel,
//        ////    btnOpen,
//        ////    lblStatus
//        ////});

//        ////// 将控件添加到主窗体
//        ////this.Controls.Add(pictureBox);
//        ////this.Controls.Add(controlPanel);

//        //this.ResumeLayout(false);

//        //private void InitializeComponent()
//        {
//            this.pictureBox = new System.Windows.Forms.PictureBox();
//            this.btnOpen = new System.Windows.Forms.Button();
//            this.btnExport = new System.Windows.Forms.Button();
//            this.btnCancel = new System.Windows.Forms.Button();
//            this.numSpacing = new System.Windows.Forms.NumericUpDown();
//            this.controlPanel = new System.Windows.Forms.Panel();
//            this.lblStatus = new System.Windows.Forms.Label();
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
//            //this.SuspendLayout();

//            // pictureBox

//            //this.pictureBox.Location = new System.Drawing.Point(14, 27);
//            //this.pictureBox.Name = "pictureBox";
//            //this.pictureBox.Size = new System.Drawing.Size(224, 350);
//            //this.pictureBox.TabIndex = 0;
//            //this.pictureBox.TabStop = false;

//            // btnOpen

//            //this.btnOpen.Location = new System.Drawing.Point(244, 29);
//            //this.btnOpen.Name = "btnOpen";
//            //this.btnOpen.Size = new System.Drawing.Size(74, 32);
//            //this.btnOpen.TabIndex = 1;
//            //this.btnOpen.Text = "btnOpen打开";
//            //this.btnOpen.UseVisualStyleBackColor = true;
//            //this.btnOpen.Click += new System.EventHandler(this.button1_Click);

//            // btnExport

//            //this.btnExport.Location = new System.Drawing.Point(244, 67);
//            //this.btnExport.Name = "btnExport";
//            //this.btnExport.Size = new System.Drawing.Size(74, 32);
//            //this.btnExport.TabIndex = 2;
//            //this.btnExport.Text = "btnExport";
//            //this.btnExport.UseVisualStyleBackColor = true;

//            // btnCancel

//            //this.btnCancel.Location = new System.Drawing.Point(244, 105);
//            //this.btnCancel.Name = "btnCancel";
//            //this.btnCancel.Size = new System.Drawing.Size(74, 32);
//            //this.btnCancel.TabIndex = 3;
//            //this.btnCancel.Text = "btnCancel";
//            //this.btnCancel.UseVisualStyleBackColor = true;

//            // numSpacing

//            //this.numSpacing.Location = new System.Drawing.Point(244, 209);
//            //this.numSpacing.Name = "numSpacing";
//            //this.numSpacing.Size = new System.Drawing.Size(74, 21);
//            //this.numSpacing.TabIndex = 4;

//            // controlPanel

//            //this.controlPanel.Location = new System.Drawing.Point(244, 236);
//            //this.controlPanel.Name = "controlPanel";
//            //this.controlPanel.Size = new System.Drawing.Size(200, 100);
//            //this.controlPanel.TabIndex = 5;

//            // lblStatus

//            //this.lblStatus.AutoSize = true;
//            //this.lblStatus.Location = new System.Drawing.Point(249, 350);
//            //this.lblStatus.Name = "lblStatus";
//            //this.lblStatus.Size = new System.Drawing.Size(59, 12);
//            //this.lblStatus.TabIndex = 6;
//            //this.lblStatus.Text = "lblStatus";

//            // MainForm

//            //this.ClientSize = new System.Drawing.Size(325, 383);
//            //this.Controls.Add(this.lblStatus);
//            //this.Controls.Add(this.controlPanel);
//            //this.Controls.Add(this.numSpacing);
//            //this.Controls.Add(this.btnCancel);
//            //this.Controls.Add(this.btnExport);
//            //this.Controls.Add(this.btnOpen);
//            //this.Controls.Add(this.pictureBox);
//            //this.Name = "MainForm";
//            //((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
//            //((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
//            //this.ResumeLayout(false);
//            //this.PerformLayout();

//            this.SuspendLayout();

//            //配置主窗体
//            this.Text = "Image Exporter";
//            this.Size = new Size(800, 600);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.DoubleBuffered = true;

//            //配置PictureBox
//            pictureBox.Dock = DockStyle.Fill;
//            pictureBox.BackColor = Color.DarkGray;
//            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
//            pictureBox.MouseWheel += PictureBox_MouseWheel;
//            pictureBox.MouseDown += PictureBox_MouseDown;
//            pictureBox.MouseMove += PictureBox_MouseMove;
//            pictureBox.MouseUp += PictureBox_MouseUp;
//            pictureBox.Paint += PictureBox_Paint;

//            //配置控制面板
//            controlPanel.Dock = DockStyle.Right;
//            controlPanel.Width = 200;
//            controlPanel.BackColor = SystemColors.Control;
//            controlPanel.Padding = new Padding(5);

//            // 配置按钮
//            btnOpen.Text = "Open Image";
//            btnOpen.Dock = DockStyle.Top;
//            btnOpen.Height = 40;
//            btnOpen.Click += BtnOpen_Click;

//            btnExport.Text = "Export";
//            btnExport.Dock = DockStyle.Top;
//            btnExport.Height = 40;
//            btnExport.Click += BtnExport_Click;

//            btnCancel.Text = "Cancel";
//            btnCancel.Dock = DockStyle.Top;
//            btnCancel.Height = 40;
//            btnCancel.Click += BtnCancel_Click;

//            // 配置间距调节控件
//            var spacingLabel = new Label
//            {
//                Text = "Spacing:",
//                Dock = DockStyle.Top,
//                Height = 20
//            };

//            numSpacing.Minimum = 0;
//            numSpacing.Maximum = 10;
//            numSpacing.Value = spacing;
//            numSpacing.Dock = DockStyle.Top;
//            numSpacing.Height = 40;
//            numSpacing.ValueChanged += NumSpacing_ValueChanged;

//            // 配置状态标签
//            lblStatus.Text = "Ready";
//            lblStatus.Dock = DockStyle.Bottom;
//            lblStatus.Height = 20;

//            // 将控件添加到控制面板
//            controlPanel.Controls.AddRange(new Control[]
//            {
//            btnCancel,
//            btnExport,
//            numSpacing,
//            spacingLabel,
//            btnOpen,
//            lblStatus
//            });

//            // 将控件添加到主窗体
//            this.Controls.Add(pictureBox);
//            this.Controls.Add(controlPanel);

//            this.Name = "MainForm";
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }
//    }
//}
