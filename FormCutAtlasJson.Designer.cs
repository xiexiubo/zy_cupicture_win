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
            this.fileListBox = new System.Windows.Forms.ListBox();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.textPreview = new System.Windows.Forms.RichTextBox();
            this.imagePreview = new System.Windows.Forms.PictureBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.openFilesButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.export = new System.Windows.Forms.Button();
            this.btn_allmanifest = new System.Windows.Forms.Button();
            this.btn_itemdown = new System.Windows.Forms.Button();
            this.btn_model = new System.Windows.Forms.Button();
            this.btn_resv = new System.Windows.Forms.Button();
            this.btn_cut = new System.Windows.Forms.Button();
            this.lb_tip = new System.Windows.Forms.Label();
            this.btn_cut1 = new System.Windows.Forms.Button();
            this.btn_icon1 = new System.Windows.Forms.Button();
            this.previewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileListBox
            // 
            this.fileListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.ItemHeight = 12;
            this.fileListBox.Location = new System.Drawing.Point(0, 0);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new System.Drawing.Size(288, 412);
            this.fileListBox.TabIndex = 0;
            this.fileListBox.SelectedIndexChanged += new System.EventHandler(this.fileListBox_SelectedIndexChanged_1);
            // 
            // previewPanel
            // 
            this.previewPanel.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.previewPanel.Controls.Add(this.textPreview);
            this.previewPanel.Controls.Add(this.imagePreview);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel.Location = new System.Drawing.Point(0, 0);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(573, 412);
            this.previewPanel.TabIndex = 1;
            // 
            // textPreview
            // 
            this.textPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPreview.Location = new System.Drawing.Point(0, 0);
            this.textPreview.Name = "textPreview";
            this.textPreview.Size = new System.Drawing.Size(573, 412);
            this.textPreview.TabIndex = 1;
            this.textPreview.Text = "";
            // 
            // imagePreview
            // 
            this.imagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePreview.Location = new System.Drawing.Point(0, 0);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(573, 412);
            this.imagePreview.TabIndex = 0;
            this.imagePreview.TabStop = false;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(12, 67);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.fileListBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.splitContainer.Panel2.Controls.Add(this.previewPanel);
            this.splitContainer.Size = new System.Drawing.Size(865, 412);
            this.splitContainer.SplitterDistance = 288;
            this.splitContainer.TabIndex = 2;
            // 
            // openFilesButton
            // 
            this.openFilesButton.Location = new System.Drawing.Point(21, 14);
            this.openFilesButton.Name = "openFilesButton";
            this.openFilesButton.Size = new System.Drawing.Size(68, 35);
            this.openFilesButton.TabIndex = 3;
            this.openFilesButton.Text = "打开文件";
            this.openFilesButton.UseVisualStyleBackColor = true;
            this.openFilesButton.Click += new System.EventHandler(this.openFilesButton_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "转换成xml";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(176, 16);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(67, 30);
            this.export.TabIndex = 6;
            this.export.Text = "导出";
            this.export.UseVisualStyleBackColor = true;
            // 
            // btn_allmanifest
            // 
            this.btn_allmanifest.Location = new System.Drawing.Point(354, 12);
            this.btn_allmanifest.Name = "btn_allmanifest";
            this.btn_allmanifest.Size = new System.Drawing.Size(84, 30);
            this.btn_allmanifest.TabIndex = 7;
            this.btn_allmanifest.Text = "allmanifest";
            this.btn_allmanifest.UseVisualStyleBackColor = true;
            this.btn_allmanifest.Click += new System.EventHandler(this.btn_allmanifest_Click);
            // 
            // btn_itemdown
            // 
            this.btn_itemdown.Location = new System.Drawing.Point(444, 12);
            this.btn_itemdown.Name = "btn_itemdown";
            this.btn_itemdown.Size = new System.Drawing.Size(61, 30);
            this.btn_itemdown.TabIndex = 8;
            this.btn_itemdown.Text = "items";
            this.btn_itemdown.UseVisualStyleBackColor = true;
            this.btn_itemdown.Click += new System.EventHandler(this.btn_itemdown_Click);
            // 
            // btn_model
            // 
            this.btn_model.Location = new System.Drawing.Point(511, 12);
            this.btn_model.Name = "btn_model";
            this.btn_model.Size = new System.Drawing.Size(57, 30);
            this.btn_model.TabIndex = 9;
            this.btn_model.Text = "models";
            this.btn_model.UseVisualStyleBackColor = true;
            this.btn_model.Click += new System.EventHandler(this.btn_model_Click);
            // 
            // btn_resv
            // 
            this.btn_resv.Location = new System.Drawing.Point(574, 12);
            this.btn_resv.Name = "btn_resv";
            this.btn_resv.Size = new System.Drawing.Size(79, 30);
            this.btn_resv.TabIndex = 10;
            this.btn_resv.Text = "res版本资源";
            this.btn_resv.UseVisualStyleBackColor = true;
            this.btn_resv.Click += new System.EventHandler(this.btn_resv_Click);
            // 
            // btn_cut
            // 
            this.btn_cut.Location = new System.Drawing.Point(673, 12);
            this.btn_cut.Name = "btn_cut";
            this.btn_cut.Size = new System.Drawing.Size(79, 30);
            this.btn_cut.TabIndex = 11;
            this.btn_cut.Text = "切图(目录)";
            this.btn_cut.UseVisualStyleBackColor = true;
            this.btn_cut.Click += new System.EventHandler(this.btn_cut_Click);
            // 
            // lb_tip
            // 
            this.lb_tip.AutoSize = true;
            this.lb_tip.Location = new System.Drawing.Point(21, 506);
            this.lb_tip.Name = "lb_tip";
            this.lb_tip.Size = new System.Drawing.Size(41, 12);
            this.lb_tip.TabIndex = 12;
            this.lb_tip.Text = "label1";
            // 
            // btn_cut1
            // 
            this.btn_cut1.Location = new System.Drawing.Point(758, 12);
            this.btn_cut1.Name = "btn_cut1";
            this.btn_cut1.Size = new System.Drawing.Size(79, 30);
            this.btn_cut1.TabIndex = 13;
            this.btn_cut1.Text = "切图(文件)";
            this.btn_cut1.UseVisualStyleBackColor = true;
            this.btn_cut1.Click += new System.EventHandler(this.btn_cut1_Click);
            // 
            // btn_icon1
            // 
            this.btn_icon1.Location = new System.Drawing.Point(314, 488);
            this.btn_icon1.Name = "btn_icon1";
            this.btn_icon1.Size = new System.Drawing.Size(79, 30);
            this.btn_icon1.TabIndex = 14;
            this.btn_icon1.Text = "btn_icon1";
            this.btn_icon1.UseVisualStyleBackColor = true;
            this.btn_icon1.Click += new System.EventHandler(this.btn_icon1_Click);
            // 
            // FormCutAtlasJson
            // 
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.btn_icon1);
            this.Controls.Add(this.btn_cut1);
            this.Controls.Add(this.lb_tip);
            this.Controls.Add(this.btn_cut);
            this.Controls.Add(this.btn_resv);
            this.Controls.Add(this.btn_model);
            this.Controls.Add(this.btn_itemdown);
            this.Controls.Add(this.btn_allmanifest);
            this.Controls.Add(this.export);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.openFilesButton);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormCutAtlasJson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件浏览器";
            this.Load += new System.EventHandler(this.FormCutAtlasJson_Load);
            this.previewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private ListBox fileListBox;
        private Panel previewPanel;
        private RichTextBox textPreview;
        private PictureBox imagePreview;
        private SplitContainer splitContainer;
        private Button openFilesButton;
        private Button button1;
        private Button export;
        private Button btn_allmanifest;
        private Button btn_itemdown;
        private Button btn_model;
        private Button btn_resv;
        private Button btn_cut;
        private Label lb_tip;
        private Button btn_cut1;
        private Button btn_icon1;
    }
}