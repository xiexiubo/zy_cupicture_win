using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace zy_cutPicture
{
    partial class SequenceForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelWorkArea;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.Button selectToolButton;
        private System.Windows.Forms.Button brushToolButton;
        private System.Windows.Forms.Panel customTitleBar;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Button maximizeButton;
        private const int RESIZE_HANDLE_SIZE = 10;

        private bool isDraggingTool = false;
        private Point lastMousePositionTool;
        private Button selectedToolButton; // 新增：用于记录当前选中的工具按钮

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.brushToolButton = new System.Windows.Forms.Button();
            this.selectToolButton = new System.Windows.Forms.Button();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.排列图组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.对齐图组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.动画ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customTitleBar = new System.Windows.Forms.Panel();
            this.maximizeButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.panelWorkArea = new System.Windows.Forms.Panel();
            this.panel_xuanxiang = new System.Windows.Forms.Panel();
            this.btn_resize_pic = new System.Windows.Forms.Button();
            this.num_rongcha = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_Area = new System.Windows.Forms.Panel();
            this.panel_anim = new System.Windows.Forms.Panel();
            this.num_anim_interval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_play = new System.Windows.Forms.Button();
            this.pic_anim = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel_tuceng = new System.Windows.Forms.Panel();
            this.btn_layer2anim = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel_layer = new System.Windows.Forms.Panel();
            this.textBox_layer_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.icon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            this.customTitleBar.SuspendLayout();
            this.panelWorkArea.SuspendLayout();
            this.panel_xuanxiang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_rongcha)).BeginInit();
            this.panel_Area.SuspendLayout();
            this.panel_anim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_anim_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_anim)).BeginInit();
            this.panel_tuceng.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel_layer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // brushToolButton
            // 
            this.brushToolButton.BackgroundImage = global::zy_cutPicture.Properties.Resources.选框;
            this.brushToolButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.brushToolButton.FlatAppearance.BorderSize = 0;
            this.brushToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brushToolButton.Location = new System.Drawing.Point(29, 2);
            this.brushToolButton.Name = "brushToolButton";
            this.brushToolButton.Size = new System.Drawing.Size(20, 20);
            this.brushToolButton.TabIndex = 1;
            this.brushToolButton.Tag = "相似工具";
            this.toolTip.SetToolTip(this.brushToolButton, "识别区工具(M)");
            this.brushToolButton.UseVisualStyleBackColor = true;
            this.brushToolButton.Click += new System.EventHandler(this.brushToolButton_Click);
            // 
            // selectToolButton
            // 
            this.selectToolButton.AutoSize = true;
            this.selectToolButton.BackgroundImage = global::zy_cutPicture.Properties.Resources.选择箭头;
            this.selectToolButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.selectToolButton.FlatAppearance.BorderSize = 0;
            this.selectToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectToolButton.Location = new System.Drawing.Point(3, 1);
            this.selectToolButton.Name = "selectToolButton";
            this.selectToolButton.Size = new System.Drawing.Size(20, 20);
            this.selectToolButton.TabIndex = 0;
            this.selectToolButton.Tag = "选择工具";
            this.toolTip.SetToolTip(this.selectToolButton, "选择工具(V)");
            this.selectToolButton.UseVisualStyleBackColor = true;
            this.selectToolButton.Click += new System.EventHandler(this.selectToolButton_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.windowToolStripMenuItem,
            this.helpMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(184, 25);
            this.mainMenuStrip.TabIndex = 2;
            this.mainMenuStrip.Text = "menuStrip1";
            this.mainMenuStrip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mainMenuStrip_MouseDoubleClick);
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.重新打开ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.另ToolStripMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(44, 21);
            this.fileMenuItem.Text = "文件";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMenuItem.Size = new System.Drawing.Size(190, 22);
            this.newMenuItem.Text = "新建";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openMenuItem.Text = "打开";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // 重新打开ToolStripMenuItem
            // 
            this.重新打开ToolStripMenuItem.Name = "重新打开ToolStripMenuItem";
            this.重新打开ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.重新打开ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.重新打开ToolStripMenuItem.Text = "重新打开";
            this.重新打开ToolStripMenuItem.Click += new System.EventHandler(this.重新打开ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 另ToolStripMenuItem
            // 
            this.另ToolStripMenuItem.Name = "另ToolStripMenuItem";
            this.另ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.另ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.另ToolStripMenuItem.Text = "另存为";
            this.另ToolStripMenuItem.Click += new System.EventHandler(this.另ToolStripMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.排列图组ToolStripMenuItem,
            this.对齐图组ToolStripMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(44, 21);
            this.editMenuItem.Text = "编辑";
            this.editMenuItem.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // 排列图组ToolStripMenuItem
            // 
            this.排列图组ToolStripMenuItem.Name = "排列图组ToolStripMenuItem";
            this.排列图组ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.排列图组ToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.排列图组ToolStripMenuItem.Text = "排列图组";
            this.排列图组ToolStripMenuItem.Click += new System.EventHandler(this.排列图组ToolStripMenuItem_Click);
            // 
            // 对齐图组ToolStripMenuItem
            // 
            this.对齐图组ToolStripMenuItem.Name = "对齐图组ToolStripMenuItem";
            this.对齐图组ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.B)));
            this.对齐图组ToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.对齐图组ToolStripMenuItem.Text = "对齐图组";
            this.对齐图组ToolStripMenuItem.Click += new System.EventHandler(this.对齐图组ToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.图层ToolStripMenuItem,
            this.动画ToolStripMenuItem,
            this.选项ToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.windowToolStripMenuItem.Text = "窗口";
            this.windowToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // 图层ToolStripMenuItem
            // 
            this.图层ToolStripMenuItem.Name = "图层ToolStripMenuItem";
            this.图层ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.图层ToolStripMenuItem.Text = "图层";
            this.图层ToolStripMenuItem.Click += new System.EventHandler(this.图层ToolStripMenuItem_Click);
            // 
            // 动画ToolStripMenuItem
            // 
            this.动画ToolStripMenuItem.Name = "动画ToolStripMenuItem";
            this.动画ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.动画ToolStripMenuItem.Text = "动画";
            this.动画ToolStripMenuItem.Click += new System.EventHandler(this.动画ToolStripMenuItem_Click);
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.选项ToolStripMenuItem.Text = "选项";
            this.选项ToolStripMenuItem.Click += new System.EventHandler(this.选项ToolStripMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 21);
            this.helpMenuItem.Text = "帮助";
            this.helpMenuItem.Click += new System.EventHandler(this.helpMenuItem_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(100, 22);
            this.aboutMenuItem.Text = "关于";
            // 
            // customTitleBar
            // 
            this.customTitleBar.BackColor = System.Drawing.SystemColors.Control;
            this.customTitleBar.Controls.Add(this.maximizeButton);
            this.customTitleBar.Controls.Add(this.minimizeButton);
            this.customTitleBar.Controls.Add(this.closeButton);
            this.customTitleBar.Controls.Add(this.mainMenuStrip);
            this.customTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.customTitleBar.Location = new System.Drawing.Point(0, 0);
            this.customTitleBar.Name = "customTitleBar";
            this.customTitleBar.Size = new System.Drawing.Size(598, 24);
            this.customTitleBar.TabIndex = 3;
            this.customTitleBar.Paint += new System.Windows.Forms.PaintEventHandler(this.customTitleBar_Paint);
            this.customTitleBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDoubleClick);
            this.customTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            // 
            // maximizeButton
            // 
            this.maximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximizeButton.FlatAppearance.BorderSize = 0;
            this.maximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maximizeButton.Location = new System.Drawing.Point(526, 0);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(24, 24);
            this.maximizeButton.TabIndex = 2;
            this.maximizeButton.Text = "□";
            this.maximizeButton.UseVisualStyleBackColor = true;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.FlatAppearance.BorderSize = 0;
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Location = new System.Drawing.Point(550, 0);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(24, 24);
            this.minimizeButton.TabIndex = 1;
            this.minimizeButton.Text = "-";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Location = new System.Drawing.Point(574, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(24, 24);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // panelWorkArea
            // 
            this.panelWorkArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWorkArea.Controls.Add(this.panel_xuanxiang);
            this.panelWorkArea.Controls.Add(this.panel_Area);
            this.panelWorkArea.Location = new System.Drawing.Point(0, 24);
            this.panelWorkArea.Name = "panelWorkArea";
            this.panelWorkArea.Size = new System.Drawing.Size(598, 479);
            this.panelWorkArea.TabIndex = 0;
            // 
            // panel_xuanxiang
            // 
            this.panel_xuanxiang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_xuanxiang.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel_xuanxiang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_xuanxiang.Controls.Add(this.selectToolButton);
            this.panel_xuanxiang.Controls.Add(this.brushToolButton);
            this.panel_xuanxiang.Controls.Add(this.btn_resize_pic);
            this.panel_xuanxiang.Controls.Add(this.num_rongcha);
            this.panel_xuanxiang.Controls.Add(this.label6);
            this.panel_xuanxiang.Location = new System.Drawing.Point(3, 3);
            this.panel_xuanxiang.Name = "panel_xuanxiang";
            this.panel_xuanxiang.Size = new System.Drawing.Size(595, 25);
            this.panel_xuanxiang.TabIndex = 4;
            // 
            // btn_resize_pic
            // 
            this.btn_resize_pic.Location = new System.Drawing.Point(241, 1);
            this.btn_resize_pic.Name = "btn_resize_pic";
            this.btn_resize_pic.Size = new System.Drawing.Size(111, 23);
            this.btn_resize_pic.TabIndex = 5;
            this.btn_resize_pic.Text = "跟据识别重新生成";
            this.btn_resize_pic.UseVisualStyleBackColor = true;
            this.btn_resize_pic.Click += new System.EventHandler(this.btn_resize_pic_Click);
            // 
            // num_rongcha
            // 
            this.num_rongcha.Location = new System.Drawing.Point(172, 2);
            this.num_rongcha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.num_rongcha.Name = "num_rongcha";
            this.num_rongcha.Size = new System.Drawing.Size(54, 21);
            this.num_rongcha.TabIndex = 4;
            this.num_rongcha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_rongcha.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "容差";
            // 
            // panel_Area
            // 
            this.panel_Area.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Area.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.panel_Area.Controls.Add(this.panel_anim);
            this.panel_Area.Controls.Add(this.panel_tuceng);
            this.panel_Area.Location = new System.Drawing.Point(3, 31);
            this.panel_Area.Name = "panel_Area";
            this.panel_Area.Size = new System.Drawing.Size(593, 445);
            this.panel_Area.TabIndex = 5;
            // 
            // panel_anim
            // 
            this.panel_anim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_anim.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_anim.Controls.Add(this.num_anim_interval);
            this.panel_anim.Controls.Add(this.label3);
            this.panel_anim.Controls.Add(this.btn_play);
            this.panel_anim.Controls.Add(this.pic_anim);
            this.panel_anim.Controls.Add(this.label4);
            this.panel_anim.Location = new System.Drawing.Point(329, 0);
            this.panel_anim.Name = "panel_anim";
            this.panel_anim.Size = new System.Drawing.Size(266, 142);
            this.panel_anim.TabIndex = 3;
            // 
            // num_anim_interval
            // 
            this.num_anim_interval.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.num_anim_interval.Location = new System.Drawing.Point(80, 117);
            this.num_anim_interval.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.num_anim_interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_anim_interval.Name = "num_anim_interval";
            this.num_anim_interval.Size = new System.Drawing.Size(73, 21);
            this.num_anim_interval.TabIndex = 3;
            this.num_anim_interval.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "间隔(毫秒)";
            // 
            // btn_play
            // 
            this.btn_play.Location = new System.Drawing.Point(180, 113);
            this.btn_play.Name = "btn_play";
            this.btn_play.Size = new System.Drawing.Size(45, 24);
            this.btn_play.TabIndex = 1;
            this.btn_play.Text = "Play";
            this.btn_play.UseVisualStyleBackColor = true;
            this.btn_play.Click += new System.EventHandler(this.btn_play_Click);
            // 
            // pic_anim
            // 
            this.pic_anim.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pic_anim.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.pic_anim.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_anim.Image = global::zy_cutPicture.Properties.Resources.生成播放按钮2;
            this.pic_anim.Location = new System.Drawing.Point(3, 24);
            this.pic_anim.Name = "pic_anim";
            this.pic_anim.Size = new System.Drawing.Size(256, 86);
            this.pic_anim.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_anim.TabIndex = 0;
            this.pic_anim.TabStop = false;
            this.pic_anim.Click += new System.EventHandler(this.pic_anim_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "序列动画";
            // 
            // panel_tuceng
            // 
            this.panel_tuceng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_tuceng.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_tuceng.Controls.Add(this.btn_layer2anim);
            this.panel_tuceng.Controls.Add(this.panel2);
            this.panel_tuceng.Controls.Add(this.label1);
            this.panel_tuceng.Location = new System.Drawing.Point(453, 238);
            this.panel_tuceng.Name = "panel_tuceng";
            this.panel_tuceng.Size = new System.Drawing.Size(135, 207);
            this.panel_tuceng.TabIndex = 2;
            // 
            // btn_layer2anim
            // 
            this.btn_layer2anim.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_layer2anim.Location = new System.Drawing.Point(3, 183);
            this.btn_layer2anim.Name = "btn_layer2anim";
            this.btn_layer2anim.Size = new System.Drawing.Size(126, 19);
            this.btn_layer2anim.TabIndex = 2;
            this.btn_layer2anim.Text = "层到序列动画";
            this.btn_layer2anim.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.panel_layer);
            this.panel2.Location = new System.Drawing.Point(3, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(126, 165);
            this.panel2.TabIndex = 1;
            // 
            // panel_layer
            // 
            this.panel_layer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_layer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_layer.Controls.Add(this.textBox_layer_name);
            this.panel_layer.Controls.Add(this.label2);
            this.panel_layer.Controls.Add(this.icon);
            this.panel_layer.Location = new System.Drawing.Point(0, 10);
            this.panel_layer.Name = "panel_layer";
            this.panel_layer.Size = new System.Drawing.Size(126, 25);
            this.panel_layer.TabIndex = 0;
            // 
            // textBox_layer_name
            // 
            this.textBox_layer_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_layer_name.Location = new System.Drawing.Point(29, 1);
            this.textBox_layer_name.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_layer_name.Name = "textBox_layer_name";
            this.textBox_layer_name.Size = new System.Drawing.Size(93, 21);
            this.textBox_layer_name.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "名字";
            this.label2.Visible = false;
            // 
            // icon
            // 
            this.icon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.icon.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.icon.Image = global::zy_cutPicture.Properties.Resources.EraserToolIcon;
            this.icon.Location = new System.Drawing.Point(3, 0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(23, 22);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层";
            // 
            // SequenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 505);
            this.Controls.Add(this.customTitleBar);
            this.Controls.Add(this.panelWorkArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SequenceForm";
            this.Text = "序列图还原工具";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseMove);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.customTitleBar.ResumeLayout(false);
            this.customTitleBar.PerformLayout();
            this.panelWorkArea.ResumeLayout(false);
            this.panel_xuanxiang.ResumeLayout(false);
            this.panel_xuanxiang.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_rongcha)).EndInit();
            this.panel_Area.ResumeLayout(false);
            this.panel_anim.ResumeLayout(false);
            this.panel_anim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_anim_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_anim)).EndInit();
            this.panel_tuceng.ResumeLayout(false);
            this.panel_tuceng.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel_layer.ResumeLayout(false);
            this.panel_layer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void selectToolButton_Click(object sender, EventArgs e)
        {
            SelectTool(sender as Button);
        }

        private void brushToolButton_Click(object sender, EventArgs e)
        {
            SelectTool(sender as Button);
        }
       

        private void SelectTool(Button button)
        {
            if (selectedToolButton != null)
            {
                selectedToolButton.FlatAppearance.BorderSize = 0;
                selectedToolButton.BackColor = SystemColors.Control;
            }

            string s = (string)button.Tag;
            this.ToolType = (eToolType)Enum.Parse(typeof(eToolType), s);//eToolType.选择;
            if (this.ToolType == eToolType.相似工具)
            {
                Cursor.Current = Cursors.Cross;
            }
            else 
            {
                Cursor.Current = Cursors.Default;
            }
            button.FlatAppearance.BorderSize = 1;
            button.BackColor = SystemColors.Highlight;
            selectedToolButton = button;
        }

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        private void customTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender != closeButton && sender != minimizeButton && sender != maximizeButton)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }
        private void customTitleBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mainMenuStrip_MouseDoubleClick(sender, e);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.maximizeButton.Text = "🗗";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.maximizeButton.Text = "□";
            }
        }

        private void SequenceForm_MouseMove(object sender, MouseEventArgs e)
        {
            var formSize = this.Size;
            var cursorPos = this.PointToClient(Cursor.Position);

            if (cursorPos.X <= RESIZE_HANDLE_SIZE && cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                this.Cursor = Cursors.SizeNESW;
            }
            else if (cursorPos.X >= formSize.Width - RESIZE_HANDLE_SIZE && cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (cursorPos.X <= RESIZE_HANDLE_SIZE)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (cursorPos.X >= formSize.Width - RESIZE_HANDLE_SIZE)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SequenceForm_MouseDown(object sender, MouseEventArgs e)
        {
            var formSize = this.Size;
            var cursorPos = this.PointToClient(Cursor.Position);
            int wParam = 0;

            if (cursorPos.X <= RESIZE_HANDLE_SIZE && cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                wParam = HTBOTTOMLEFT;
            }
            else if (cursorPos.X >= formSize.Width - RESIZE_HANDLE_SIZE && cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                wParam = HTBOTTOMRIGHT;
            }
            else if (cursorPos.X <= RESIZE_HANDLE_SIZE)
            {
                wParam = HTLEFT;
            }
            else if (cursorPos.X >= formSize.Width - RESIZE_HANDLE_SIZE)
            {
                wParam = HTRIGHT;
            }
            else if (cursorPos.Y >= formSize.Height - RESIZE_HANDLE_SIZE)
            {
                wParam = HTBOTTOM;
            }

            if (wParam != 0)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, wParam, 0);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x00040000; // 添加WS_THICKFRAME样式
                return cp;
            }
        }

        private void toolboxPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingTool = true;
                lastMousePositionTool = e.Location;
            }
        }

     

        private void toolboxPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingTool = false;
        }

        // 双击菜单栏事件处理方法
        private void mainMenuStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 判断双击位置是否在菜单空白区域
            bool isInMenuItem = false;
            foreach (ToolStripMenuItem item in mainMenuStrip.Items)
            {
                if (item.Bounds.Contains(e.Location))
                {
                    isInMenuItem = true;
                    break;
                }
            }

            if (!isInMenuItem)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                    this.maximizeButton.Text = "🗗";
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    this.maximizeButton.Text = "□";
                }
            }
        }

        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem 图层ToolStripMenuItem;
        private ToolStripMenuItem 动画ToolStripMenuItem;
        private Panel panel_tuceng;
        private Panel panel2;
        private Label label1;
        private Panel panel_layer;
        private PictureBox icon;
        private TextBox textBox_layer_name;
        private Label label2;
        private Button btn_layer2anim;
        private Panel panel_anim;
        private PictureBox pic_anim;
        private Label label4;
        private ToolStripMenuItem 选项ToolStripMenuItem;
        private Panel panel_xuanxiang;
        private NumericUpDown num_rongcha;
        private Label label6;
        private Button btn_resize_pic;
        private ToolStripMenuItem 排列图组ToolStripMenuItem;
        private ToolStripMenuItem 对齐图组ToolStripMenuItem;
        private Panel panel_Area;
        private ToolStripMenuItem 重新打开ToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem;
        private ToolStripMenuItem 另ToolStripMenuItem;
        private ToolTip toolTip;
        private Label label3;
        private Button btn_play;
        private NumericUpDown num_anim_interval;
    }
}