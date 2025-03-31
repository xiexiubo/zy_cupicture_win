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
            this.panelWorkArea = new System.Windows.Forms.Panel();
            this.toolboxPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.selectToolButton = new System.Windows.Forms.Button();
            this.brushToolButton = new System.Windows.Forms.Button();
            this.eraserToolButton = new System.Windows.Forms.Button();
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
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.动画ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.属性ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customTitleBar = new System.Windows.Forms.Panel();
            this.maximizeButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.panelWorkArea.SuspendLayout();
            this.toolboxPanel.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.customTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkArea
            // 
            this.panelWorkArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWorkArea.BackgroundImage = global::zy_cutPicture.Properties.Resources.方格;
            this.panelWorkArea.Controls.Add(this.toolboxPanel);
            this.panelWorkArea.Location = new System.Drawing.Point(0, 24);
            this.panelWorkArea.Name = "panelWorkArea";
            this.panelWorkArea.Size = new System.Drawing.Size(800, 472);
            this.panelWorkArea.TabIndex = 0;
            // 
            // toolboxPanel
            // 
            this.toolboxPanel.BackColor = System.Drawing.Color.LightGray;
            this.toolboxPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolboxPanel.Controls.Add(this.selectToolButton);
            this.toolboxPanel.Controls.Add(this.brushToolButton);
            this.toolboxPanel.Controls.Add(this.eraserToolButton);
            this.toolboxPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.toolboxPanel.Location = new System.Drawing.Point(12, 21);
            this.toolboxPanel.Name = "toolboxPanel";
            this.toolboxPanel.Size = new System.Drawing.Size(50, 215);
            this.toolboxPanel.TabIndex = 1;
            this.toolboxPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolboxPanel_MouseDown);
            this.toolboxPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolboxPanel_MouseMove);
            this.toolboxPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolboxPanel_MouseUp);
            // 
            // selectToolButton
            // 
            this.selectToolButton.AutoSize = true;
            this.selectToolButton.FlatAppearance.BorderSize = 0;
            this.selectToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectToolButton.Image = global::zy_cutPicture.Properties.Resources.SelectToolIcon;
            this.selectToolButton.Location = new System.Drawing.Point(3, 3);
            this.selectToolButton.Name = "selectToolButton";
            this.selectToolButton.Size = new System.Drawing.Size(39, 37);
            this.selectToolButton.TabIndex = 0;
            this.selectToolButton.Tag = "选择工具";
            this.selectToolButton.UseVisualStyleBackColor = true;
            this.selectToolButton.Click += new System.EventHandler(this.selectToolButton_Click);
            // 
            // brushToolButton
            // 
            this.brushToolButton.FlatAppearance.BorderSize = 0;
            this.brushToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brushToolButton.Image = global::zy_cutPicture.Properties.Resources.BrushToolIcon;
            this.brushToolButton.Location = new System.Drawing.Point(3, 46);
            this.brushToolButton.Name = "brushToolButton";
            this.brushToolButton.Size = new System.Drawing.Size(35, 35);
            this.brushToolButton.TabIndex = 1;
            this.brushToolButton.Tag = "相似工具";
            this.brushToolButton.UseVisualStyleBackColor = true;
            this.brushToolButton.Click += new System.EventHandler(this.brushToolButton_Click);
            // 
            // eraserToolButton
            // 
            this.eraserToolButton.FlatAppearance.BorderSize = 0;
            this.eraserToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.eraserToolButton.Image = global::zy_cutPicture.Properties.Resources.EraserToolIcon;
            this.eraserToolButton.Location = new System.Drawing.Point(3, 87);
            this.eraserToolButton.Name = "eraserToolButton";
            this.eraserToolButton.Size = new System.Drawing.Size(35, 35);
            this.eraserToolButton.TabIndex = 2;
            this.eraserToolButton.UseVisualStyleBackColor = true;
            this.eraserToolButton.Click += new System.EventHandler(this.eraserToolButton_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.toolsMenuItem,
            this.windowToolStripMenuItem,
            this.helpMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(228, 25);
            this.mainMenuStrip.TabIndex = 2;
            this.mainMenuStrip.Text = "menuStrip1";
            this.mainMenuStrip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mainMenuStrip_MouseDoubleClick);
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(44, 21);
            this.fileMenuItem.Text = "文件";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.Size = new System.Drawing.Size(112, 22);
            this.newMenuItem.Text = "新建";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openMenuItem.Text = "打开";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveMenuItem.Text = "保存";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveAsMenuItem.Text = "另存为";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(44, 21);
            this.editMenuItem.Text = "编辑";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.Size = new System.Drawing.Size(100, 22);
            this.undoMenuItem.Text = "撤销";
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.Size = new System.Drawing.Size(100, 22);
            this.redoMenuItem.Text = "重做";
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(100, 22);
            this.copyMenuItem.Text = "复制";
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.Size = new System.Drawing.Size(100, 22);
            this.pasteMenuItem.Text = "粘贴";
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsMenuItem});
            this.toolsMenuItem.Name = "toolsMenuItem";
            this.toolsMenuItem.Size = new System.Drawing.Size(44, 21);
            this.toolsMenuItem.Text = "工具";
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Name = "optionsMenuItem";
            this.optionsMenuItem.Size = new System.Drawing.Size(100, 22);
            this.optionsMenuItem.Text = "选项";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.图层ToolStripMenuItem,
            this.动画ToolStripMenuItem,
            this.属性ToolStripMenuItem});
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
            // 
            // 动画ToolStripMenuItem
            // 
            this.动画ToolStripMenuItem.Name = "动画ToolStripMenuItem";
            this.动画ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.动画ToolStripMenuItem.Text = "动画";
            // 
            // 属性ToolStripMenuItem
            // 
            this.属性ToolStripMenuItem.Name = "属性ToolStripMenuItem";
            this.属性ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.属性ToolStripMenuItem.Text = "属性";
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
            this.customTitleBar.Size = new System.Drawing.Size(800, 24);
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
            this.maximizeButton.Location = new System.Drawing.Point(728, 0);
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
            this.minimizeButton.Location = new System.Drawing.Point(752, 0);
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
            this.closeButton.Location = new System.Drawing.Point(776, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(24, 24);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // SequenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.customTitleBar);
            this.Controls.Add(this.panelWorkArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SequenceForm";
            this.Text = "序列图还原工具";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SequenceForm_MouseMove);
            this.panelWorkArea.ResumeLayout(false);
            this.toolboxPanel.ResumeLayout(false);
            this.toolboxPanel.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.customTitleBar.ResumeLayout(false);
            this.customTitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private void selectToolButton_Click(object sender, EventArgs e)
        {
            SelectTool(sender as Button);
            System.Windows.Forms.MessageBox.Show("你选择了 选择工具");
        }

        private void brushToolButton_Click(object sender, EventArgs e)
        {
            SelectTool(sender as Button);
            System.Windows.Forms.MessageBox.Show("你选择了 画笔工具");
        }

        private void eraserToolButton_Click(object sender, EventArgs e)
        {
            SelectTool(sender as Button);
            System.Windows.Forms.MessageBox.Show("你选择了 橡皮擦工具");
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

        private void toolboxPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingTool)
            {
                int deltaX = e.X - lastMousePositionTool.X;
                int deltaY = e.Y - lastMousePositionTool.Y;
                int newX = toolboxPanel.Left + deltaX;
                int newY = toolboxPanel.Top + deltaY;

                // 限制工具栏在窗口内拖动
                if (newX >= 0 && newX <= panelWorkArea.Width - toolboxPanel.Width)
                {
                    toolboxPanel.Left = newX;
                }
                if (newY >= 0 && newY <= panelWorkArea.Height - toolboxPanel.Height)
                {
                    toolboxPanel.Top = newY;
                }
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
        private ToolStripMenuItem 属性ToolStripMenuItem;
    }
}