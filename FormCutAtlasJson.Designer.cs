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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.export = new System.Windows.Forms.Button();
            this.btn_allmanifest = new System.Windows.Forms.Button();
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
            this.openFilesButton.Size = new System.Drawing.Size(96, 35);
            this.openFilesButton.TabIndex = 3;
            this.openFilesButton.Text = "打开文件";
            this.openFilesButton.UseVisualStyleBackColor = true;
            this.openFilesButton.Click += new System.EventHandler(this.openFilesButton_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(150, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "导出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(324, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "转换成xml";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(461, 12);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(115, 30);
            this.export.TabIndex = 6;
            this.export.Text = "导出";
            this.export.UseVisualStyleBackColor = true;
            // 
            // btn_allmanifest
            // 
            this.btn_allmanifest.Location = new System.Drawing.Point(595, 12);
            this.btn_allmanifest.Name = "btn_allmanifest";
            this.btn_allmanifest.Size = new System.Drawing.Size(115, 30);
            this.btn_allmanifest.TabIndex = 7;
            this.btn_allmanifest.Text = "allmanifest";
            this.btn_allmanifest.UseVisualStyleBackColor = true;
            this.btn_allmanifest.Click += new System.EventHandler(this.btn_allmanifest_Click);
            // 
            // FormCutAtlasJson
            // 
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.btn_allmanifest);
            this.Controls.Add(this.export);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
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

        }


        #endregion

        private ListBox fileListBox;
        private Panel previewPanel;
        private RichTextBox textPreview;
        private PictureBox imagePreview;
        private SplitContainer splitContainer;
        private Button openFilesButton;
        private Button button2;
        private Button button1;
        private Button export;
        private Button btn_allmanifest;
    }
}