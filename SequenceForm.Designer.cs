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
        private System.Windows.Forms.FlowLayoutPanel toolboxPanel;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.Button selectToolButton;
        private System.Windows.Forms.Button brushToolButton;
        private System.Windows.Forms.Button eraserToolButton;
        private System.Windows.Forms.Panel customTitleBar;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Button maximizeButton;
        private const int RESIZE_HANDLE_SIZE = 10;

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
            this.panelWorkArea = new System.Windows.Forms.Panel();
            this.toolboxPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolButton = new System.Windows.Forms.Button();
            this.brushToolButton = new System.Windows.Forms.Button();
            this.eraserToolButton = new System.Windows.Forms.Button();
            this.customTitleBar = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.maximizeButton = new System.Windows.Forms.Button();
            this.panelWorkArea.SuspendLayout();
            this.toolboxPanel.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.customTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkArea
            // 
            this.panelWorkArea.Controls.Add(this.toolboxPanel);
            this.panelWorkArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkArea.Location = new System.Drawing.Point(0, 24);
            this.panelWorkArea.Name = "panelWorkArea";
            this.panelWorkArea.Size = new System.Drawing.Size(800, 426);
            this.panelWorkArea.TabIndex = 0;
            // 
            // toolboxPanel
            // 
            this.toolboxPanel.BackColor = System.Drawing.Color.LightGray;
            this.toolboxPanel.Controls.Add(this.selectToolButton);
            this.toolboxPanel.Controls.Add(this.brushToolButton);
            this.toolboxPanel.Controls.Add(this.eraserToolButton);
            this.toolboxPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolboxPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.toolboxPanel.Location = new System.Drawing.Point(0, 0);
            this.toolboxPanel.Name = "toolboxPanel";
            this.toolboxPanel.Size = new System.Drawing.Size(50, 426);
            this.toolboxPanel.TabIndex = 1;
            this.toolboxPanel.WrapContents = false;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.toolsMenuItem,
            this.helpMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 2;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "文件";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.Size = new System.Drawing.Size(124, 22);
            this.newMenuItem.Text = "新建";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openMenuItem.Text = "打开";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveMenuItem.Text = "保存";
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveAsMenuItem.Text = "另存为";
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editMenuItem.Text = "编辑";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.Size = new System.Drawing.Size(124, 22);
            this.undoMenuItem.Text = "撤销";
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.Size = new System.Drawing.Size(124, 22);
            this.redoMenuItem.Text = "重做";
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(124, 22);
            this.copyMenuItem.Text = "复制";
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.Size = new System.Drawing.Size(124, 22);
            this.pasteMenuItem.Text = "粘贴";
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.Add(this.optionsMenuItem);
            this.toolsMenuItem.Name = "toolsMenuItem";
            this.toolsMenuItem.Size = new System.Drawing.Size(37, 20);
            this.toolsMenuItem.Text = "工具";
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Name = "optionsMenuItem";
            this.optionsMenuItem.Size = new System.Drawing.Size(124, 22);
            this.optionsMenuItem.Text = "选项";
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.Add(this.aboutMenuItem);
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(37, 20);
            this.helpMenuItem.Text = "帮助";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(124, 22);
            this.aboutMenuItem.Text = "关于";
            // 
            // selectToolButton
            // 
            this.selectToolButton.FlatAppearance.BorderSize = 0;
            this.selectToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectToolButton.Image = global::zy_cutPicture.Properties.Resources.SelectToolIcon;
            this.selectToolButton.Location = new System.Drawing.Point(5, 5);
            this.selectToolButton.Name = "selectToolButton";
            this.selectToolButton.Size = new System.Drawing.Size(40, 40);
            this.selectToolButton.TabIndex = 0;
            this.selectToolButton.UseVisualStyleBackColor = true;
            this.selectToolButton.Click += new System.EventHandler(this.selectToolButton_Click);
            // 
            // brushToolButton
            // 
            this.brushToolButton.FlatAppearance.BorderSize = 0;
            this.brushToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brushToolButton.Image = global::zy_cutPicture.Properties.Resources.BrushToolIcon;
            this.brushToolButton.Location = new System.Drawing.Point(5, 51);
            this.brushToolButton.Name = "brushToolButton";
            this.brushToolButton.Size = new System.Drawing.Size(40, 40);
            this.brushToolButton.TabIndex = 1;
            this.brushToolButton.UseVisualStyleBackColor = true;
            this.brushToolButton.Click += new System.EventHandler(this.brushToolButton_Click);
            // 
            // eraserToolButton
            // 
            this.eraserToolButton.FlatAppearance.BorderSize = 0;
            this.eraserToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.eraserToolButton.Image = global::zy_cutPicture.Properties.Resources.EraserToolIcon;
            this.eraserToolButton.Location = new System.Drawing.Point(5, 97);
            this.eraserToolButton.Name = "eraserToolButton";
            this.eraserToolButton.Size = new System.Drawing.Size(40, 40);
            this.eraserToolButton.TabIndex = 2;
            this.eraserToolButton.UseVisualStyleBackColor = true;
            this.eraserToolButton.Click += new System.EventHandler(this.eraserToolButton_Click);
            // 
            // customTitleBar
            // 
            this.customTitleBar.BackColor = SystemColors.Control;
            this.customTitleBar.Controls.Add(this.maximizeButton);
            this.customTitleBar.Controls.Add(this.minimizeButton);
            this.customTitleBar.Controls.Add(this.closeButton);
            this.customTitleBar.Controls.Add(this.mainMenuStrip);
            this.customTitleBar.Dock = DockStyle.Top;
            this.customTitleBar.Location = new System.Drawing.Point(0, 0);
            this.customTitleBar.Name = "customTitleBar";
            this.customTitleBar.Size = new System.Drawing.Size(800, 24);
            this.customTitleBar.TabIndex = 3;
            this.customTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            this.mainMenuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            this.closeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            this.minimizeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            this.maximizeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTitleBar_MouseDown);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Location = new System.Drawing.Point(776, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(24, 24);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.FlatAppearance.BorderSize = 0;
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Location = new System.Drawing.Point(752, 0);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(24, 24);
            this.minimizeButton.TabIndex = 1;
            this.minimizeButton.Text = "-";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximizeButton.FlatAppearance.BorderSize = 0;
            this.maximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maximizeButton.Location = new System.Drawing.Point(728, 0);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(24, 24);
            this.maximizeButton.TabIndex = 2;
            this.maximizeButton.Text = "□";
            this.maximizeButton.UseVisualStyleBackColor = true;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // SequenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelWorkArea);
            this.Controls.Add(this.customTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SequenceForm";
            this.Text = "序列图还原工具";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseDown);
            this.panelWorkArea.ResumeLayout(false);
            this.toolboxPanel.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.customTitleBar.ResumeLayout(false);
            this.customTitleBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void selectToolButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("你选择了 选择工具");
        }

        private void brushToolButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("你选择了 画笔工具");
        }

        private void eraserToolButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("你选择了 橡皮擦工具");
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
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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
            if (e.Button == MouseButtons.None)
            {
                var clientSize = this.ClientSize;
                if (e.X <= RESIZE_HANDLE_SIZE && e.Y <= RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE && e.Y <= RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNESW;
                }
                else if (e.X <= RESIZE_HANDLE_SIZE && e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNESW;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE && e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (e.X <= RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeWE;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeWE;
                }
                else if (e.Y <= RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNS;
                }
                else if (e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    this.Cursor = Cursors.SizeNS;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void SequenceForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var clientSize = this.ClientSize;
                int wParam = 0;
                if (e.X <= RESIZE_HANDLE_SIZE && e.Y <= RESIZE_HANDLE_SIZE)
                {
                    wParam = HTTOPLEFT;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE && e.Y <= RESIZE_HANDLE_SIZE)
                {
                    wParam = HTTOPRIGHT;
                }
                else if (e.X <= RESIZE_HANDLE_SIZE && e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    wParam = HTBOTTOMLEFT;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE && e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    wParam = HTBOTTOMRIGHT;
                }
                else if (e.X <= RESIZE_HANDLE_SIZE)
                {
                    wParam = HTLEFT;
                }
                else if (e.X >= clientSize.Width - RESIZE_HANDLE_SIZE)
                {
                    wParam = HTRIGHT;
                }
                else if (e.Y <= RESIZE_HANDLE_SIZE)
                {
                    wParam = HTTOP;
                }
                else if (e.Y >= clientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    wParam = HTBOTTOM;
                }

                if (wParam != 0)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, wParam, 0);
                }
            }
        }
    }
}