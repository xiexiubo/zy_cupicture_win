using System.Drawing;
using System.Windows.Forms;

namespace zy_cutPicture
{
    partial class FormCutAtlasJson
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


       
        private string[] currentFiles = new string[0];
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_onekey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.img_1 = new System.Windows.Forms.PictureBox();
            this.img_2 = new System.Windows.Forms.PictureBox();
            this.img_3 = new System.Windows.Forms.PictureBox();
            this.img_4 = new System.Windows.Forms.PictureBox();
            this.img_5 = new System.Windows.Forms.PictureBox();
            this.img_6 = new System.Windows.Forms.PictureBox();
            this.ck_6 = new System.Windows.Forms.CheckBox();
            this.ck_5 = new System.Windows.Forms.CheckBox();
            this.ck_4 = new System.Windows.Forms.CheckBox();
            this.ck_3 = new System.Windows.Forms.CheckBox();
            this.ck_2 = new System.Windows.Forms.CheckBox();
            this.ck_1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_allmanifest = new System.Windows.Forms.Button();
            this.btn_itemdown = new System.Windows.Forms.Button();
            this.btn_model = new System.Windows.Forms.Button();
            this.btn_resv = new System.Windows.Forms.Button();
            this.btn_icon1 = new System.Windows.Forms.Button();
            this.btn_cut1 = new System.Windows.Forms.Button();
            this.btn_cut = new System.Windows.Forms.Button();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.imagePreview = new System.Windows.Forms.PictureBox();
            this.txt_resVer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_dir = new System.Windows.Forms.Label();
            this.txt_dir = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ck_00 = new System.Windows.Forms.CheckBox();
            this.ck_01 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_6)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.previewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(12, 127);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.splitContainer.Panel2.Controls.Add(this.previewPanel);
            this.splitContainer.Size = new System.Drawing.Size(865, 442);
            this.splitContainer.SplitterDistance = 213;
            this.splitContainer.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.btn_onekey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.img_1);
            this.groupBox1.Controls.Add(this.img_2);
            this.groupBox1.Controls.Add(this.img_3);
            this.groupBox1.Controls.Add(this.img_4);
            this.groupBox1.Controls.Add(this.img_5);
            this.groupBox1.Controls.Add(this.img_6);
            this.groupBox1.Controls.Add(this.ck_6);
            this.groupBox1.Controls.Add(this.ck_5);
            this.groupBox1.Controls.Add(this.ck_4);
            this.groupBox1.Controls.Add(this.ck_3);
            this.groupBox1.Controls.Add(this.ck_2);
            this.groupBox1.Controls.Add(this.ck_1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(213, 442);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // btn_onekey
            // 
            this.btn_onekey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_onekey.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_onekey.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_onekey.Location = new System.Drawing.Point(13, 259);
            this.btn_onekey.Name = "btn_onekey";
            this.btn_onekey.Size = new System.Drawing.Size(182, 62);
            this.btn_onekey.TabIndex = 19;
            this.btn_onekey.Text = "开 始";
            this.btn_onekey.UseVisualStyleBackColor = false;
            this.btn_onekey.Click += new System.EventHandler(this.btn_onekey_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "拆图集";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "下载";
            // 
            // img_1
            // 
            this.img_1.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_1.Location = new System.Drawing.Point(173, 31);
            this.img_1.Name = "img_1";
            this.img_1.Size = new System.Drawing.Size(20, 20);
            this.img_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_1.TabIndex = 20;
            this.img_1.TabStop = false;
            // 
            // img_2
            // 
            this.img_2.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_2.Location = new System.Drawing.Point(126, 63);
            this.img_2.Name = "img_2";
            this.img_2.Size = new System.Drawing.Size(20, 20);
            this.img_2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_2.TabIndex = 21;
            this.img_2.TabStop = false;
            // 
            // img_3
            // 
            this.img_3.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_3.Location = new System.Drawing.Point(167, 94);
            this.img_3.Name = "img_3";
            this.img_3.Size = new System.Drawing.Size(20, 20);
            this.img_3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_3.TabIndex = 22;
            this.img_3.TabStop = false;
            // 
            // img_4
            // 
            this.img_4.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_4.Location = new System.Drawing.Point(150, 127);
            this.img_4.Name = "img_4";
            this.img_4.Size = new System.Drawing.Size(20, 20);
            this.img_4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_4.TabIndex = 23;
            this.img_4.TabStop = false;
            // 
            // img_5
            // 
            this.img_5.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_5.Location = new System.Drawing.Point(132, 161);
            this.img_5.Name = "img_5";
            this.img_5.Size = new System.Drawing.Size(20, 20);
            this.img_5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_5.TabIndex = 24;
            this.img_5.TabStop = false;
            // 
            // img_6
            // 
            this.img_6.Image = global::zy_cutPicture.Properties.Resources.勾选;
            this.img_6.Location = new System.Drawing.Point(124, 216);
            this.img_6.Name = "img_6";
            this.img_6.Size = new System.Drawing.Size(20, 20);
            this.img_6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_6.TabIndex = 25;
            this.img_6.TabStop = false;
            // 
            // ck_6
            // 
            this.ck_6.AutoSize = true;
            this.ck_6.Checked = true;
            this.ck_6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_6.Location = new System.Drawing.Point(24, 216);
            this.ck_6.Name = "ck_6";
            this.ck_6.Size = new System.Drawing.Size(120, 16);
            this.ck_6.TabIndex = 6;
            this.ck_6.Text = "图集|序列图 拆切";
            this.ck_6.UseVisualStyleBackColor = true;
            // 
            // ck_5
            // 
            this.ck_5.AutoSize = true;
            this.ck_5.Checked = true;
            this.ck_5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_5.Location = new System.Drawing.Point(25, 162);
            this.ck_5.Name = "ck_5";
            this.ck_5.Size = new System.Drawing.Size(108, 16);
            this.ck_5.TabIndex = 4;
            this.ck_5.Text = "下载怪头像图标";
            this.ck_5.UseVisualStyleBackColor = true;
            // 
            // ck_4
            // 
            this.ck_4.AutoSize = true;
            this.ck_4.Checked = true;
            this.ck_4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_4.Location = new System.Drawing.Point(25, 129);
            this.ck_4.Name = "ck_4";
            this.ck_4.Size = new System.Drawing.Size(126, 16);
            this.ck_4.TabIndex = 3;
            this.ck_4.Text = "下载Res版本资源图";
            this.ck_4.UseVisualStyleBackColor = true;
            // 
            // ck_3
            // 
            this.ck_3.AutoSize = true;
            this.ck_3.Checked = true;
            this.ck_3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_3.Location = new System.Drawing.Point(25, 97);
            this.ck_3.Name = "ck_3";
            this.ck_3.Size = new System.Drawing.Size(144, 16);
            this.ck_3.TabIndex = 2;
            this.ck_3.Text = "下载Models序列图资源";
            this.ck_3.UseVisualStyleBackColor = true;
            // 
            // ck_2
            // 
            this.ck_2.AutoSize = true;
            this.ck_2.Checked = true;
            this.ck_2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_2.Location = new System.Drawing.Point(25, 65);
            this.ck_2.Name = "ck_2";
            this.ck_2.Size = new System.Drawing.Size(102, 16);
            this.ck_2.TabIndex = 1;
            this.ck_2.Text = "下载Items图标";
            this.ck_2.UseVisualStyleBackColor = true;
            // 
            // ck_1
            // 
            this.ck_1.AutoSize = true;
            this.ck_1.Checked = true;
            this.ck_1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_1.Location = new System.Drawing.Point(25, 35);
            this.ck_1.Name = "ck_1";
            this.ck_1.Size = new System.Drawing.Size(150, 16);
            this.ck_1.TabIndex = 0;
            this.ck_1.Text = "下载Allmanifest配置图";
            this.ck_1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_allmanifest);
            this.groupBox2.Controls.Add(this.btn_itemdown);
            this.groupBox2.Controls.Add(this.btn_model);
            this.groupBox2.Controls.Add(this.btn_resv);
            this.groupBox2.Controls.Add(this.btn_icon1);
            this.groupBox2.Controls.Add(this.btn_cut1);
            this.groupBox2.Controls.Add(this.btn_cut);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 442);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Visible = false;
            // 
            // btn_allmanifest
            // 
            this.btn_allmanifest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_allmanifest.Location = new System.Drawing.Point(8, 20);
            this.btn_allmanifest.Name = "btn_allmanifest";
            this.btn_allmanifest.Size = new System.Drawing.Size(187, 30);
            this.btn_allmanifest.TabIndex = 7;
            this.btn_allmanifest.Text = "allmanifest";
            this.btn_allmanifest.UseVisualStyleBackColor = true;
            this.btn_allmanifest.Click += new System.EventHandler(this.btn_allmanifest_Click);
            // 
            // btn_itemdown
            // 
            this.btn_itemdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_itemdown.Location = new System.Drawing.Point(8, 62);
            this.btn_itemdown.Name = "btn_itemdown";
            this.btn_itemdown.Size = new System.Drawing.Size(187, 30);
            this.btn_itemdown.TabIndex = 8;
            this.btn_itemdown.Text = "items";
            this.btn_itemdown.UseVisualStyleBackColor = true;
            this.btn_itemdown.Click += new System.EventHandler(this.btn_itemdown_Click);
            // 
            // btn_model
            // 
            this.btn_model.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_model.Location = new System.Drawing.Point(8, 98);
            this.btn_model.Name = "btn_model";
            this.btn_model.Size = new System.Drawing.Size(187, 30);
            this.btn_model.TabIndex = 9;
            this.btn_model.Text = "models";
            this.btn_model.UseVisualStyleBackColor = true;
            this.btn_model.Click += new System.EventHandler(this.btn_model_Click);
            // 
            // btn_resv
            // 
            this.btn_resv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_resv.Location = new System.Drawing.Point(8, 137);
            this.btn_resv.Name = "btn_resv";
            this.btn_resv.Size = new System.Drawing.Size(187, 30);
            this.btn_resv.TabIndex = 10;
            this.btn_resv.Text = "res版本资源";
            this.btn_resv.UseVisualStyleBackColor = true;
            this.btn_resv.Click += new System.EventHandler(this.btn_resv_Click);
            // 
            // btn_icon1
            // 
            this.btn_icon1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_icon1.Location = new System.Drawing.Point(8, 175);
            this.btn_icon1.Name = "btn_icon1";
            this.btn_icon1.Size = new System.Drawing.Size(187, 30);
            this.btn_icon1.TabIndex = 14;
            this.btn_icon1.Text = "怪头像";
            this.btn_icon1.UseVisualStyleBackColor = true;
            this.btn_icon1.Click += new System.EventHandler(this.btn_icon1_Click);
            // 
            // btn_cut1
            // 
            this.btn_cut1.Location = new System.Drawing.Point(8, 257);
            this.btn_cut1.Name = "btn_cut1";
            this.btn_cut1.Size = new System.Drawing.Size(73, 30);
            this.btn_cut1.TabIndex = 13;
            this.btn_cut1.Text = "切图(文件)";
            this.btn_cut1.UseVisualStyleBackColor = true;
            this.btn_cut1.Click += new System.EventHandler(this.btn_cut1_Click);
            // 
            // btn_cut
            // 
            this.btn_cut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cut.Location = new System.Drawing.Point(118, 257);
            this.btn_cut.Name = "btn_cut";
            this.btn_cut.Size = new System.Drawing.Size(77, 30);
            this.btn_cut.TabIndex = 11;
            this.btn_cut.Text = "切图(目录)";
            this.btn_cut.UseVisualStyleBackColor = true;
            this.btn_cut.Click += new System.EventHandler(this.btn_cut_Click);
            // 
            // previewPanel
            // 
            this.previewPanel.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.previewPanel.Controls.Add(this.logTextBox);
            this.previewPanel.Controls.Add(this.imagePreview);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel.Location = new System.Drawing.Point(0, 0);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(648, 442);
            this.previewPanel.TabIndex = 1;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(648, 442);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            // 
            // imagePreview
            // 
            this.imagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePreview.Location = new System.Drawing.Point(0, 0);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(648, 442);
            this.imagePreview.TabIndex = 0;
            this.imagePreview.TabStop = false;
            // 
            // txt_resVer
            // 
            this.txt_resVer.Location = new System.Drawing.Point(76, 35);
            this.txt_resVer.Name = "txt_resVer";
            this.txt_resVer.Size = new System.Drawing.Size(100, 21);
            this.txt_resVer.TabIndex = 2;
            this.txt_resVer.Text = "1.28929.3";
            this.txt_resVer.TextChanged += new System.EventHandler(this.txt_resVer_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "资源版本";
            // 
            // lb_dir
            // 
            this.lb_dir.AutoSize = true;
            this.lb_dir.Location = new System.Drawing.Point(16, 73);
            this.lb_dir.Name = "lb_dir";
            this.lb_dir.Size = new System.Drawing.Size(53, 12);
            this.lb_dir.TabIndex = 16;
            this.lb_dir.Text = "保存目录";
            // 
            // txt_dir
            // 
            this.txt_dir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_dir.Location = new System.Drawing.Point(74, 70);
            this.txt_dir.Name = "txt_dir";
            this.txt_dir.Size = new System.Drawing.Size(727, 21);
            this.txt_dir.TabIndex = 17;
            this.txt_dir.TextChanged += new System.EventHandler(this.txt_dir_TextChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(811, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 25);
            this.button1.TabIndex = 18;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ck_00
            // 
            this.ck_00.AutoSize = true;
            this.ck_00.Checked = true;
            this.ck_00.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_00.Location = new System.Drawing.Point(13, 107);
            this.ck_00.Name = "ck_00";
            this.ck_00.Size = new System.Drawing.Size(60, 16);
            this.ck_00.TabIndex = 19;
            this.ck_00.Text = "自动区";
            this.ck_00.UseVisualStyleBackColor = true;
            this.ck_00.CheckedChanged += new System.EventHandler(this.ck_00_CheckedChanged);
            // 
            // ck_01
            // 
            this.ck_01.AutoSize = true;
            this.ck_01.Location = new System.Drawing.Point(73, 106);
            this.ck_01.Name = "ck_01";
            this.ck_01.Size = new System.Drawing.Size(60, 16);
            this.ck_01.TabIndex = 20;
            this.ck_01.Text = "手动区";
            this.ck_01.UseVisualStyleBackColor = true;
            this.ck_01.CheckedChanged += new System.EventHandler(this.ck_01_CheckedChanged);
            // 
            // FormCutAtlasJson
            // 
            this.ClientSize = new System.Drawing.Size(884, 581);
            this.Controls.Add(this.ck_01);
            this.Controls.Add(this.ck_00);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_dir);
            this.Controls.Add(this.lb_dir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_resVer);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormCutAtlasJson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "霸业图片资源";
            this.Load += new System.EventHandler(this.FormCutAtlasJson_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_6)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.previewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private Panel previewPanel;
        private RichTextBox logTextBox;
        private PictureBox imagePreview;
        private SplitContainer splitContainer;
        private Button btn_allmanifest;
        private Button btn_itemdown;
        private Button btn_model;
        private Button btn_resv;
        private Button btn_cut;
        private Button btn_cut1;
        private Button btn_icon1;
        private TextBox txt_resVer;
        private CheckBox ck_6;
        private CheckBox ck_5;
        private CheckBox ck_4;
        private CheckBox ck_3;
        private CheckBox ck_2;
        private CheckBox ck_1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lb_dir;
        private TextBox txt_dir;
        private Button button1;
        private GroupBox groupBox1;
        private CheckBox ck_00;
        private CheckBox ck_01;
        private GroupBox groupBox2;
        private Button btn_onekey;
        private PictureBox img_1;
        private PictureBox img_6;
        private PictureBox img_5;
        private PictureBox img_4;
        private PictureBox img_3;
        private PictureBox img_2;
    }
}