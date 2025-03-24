using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public partial class SequenceForm : Form
    {
       
        public SequenceForm()
        {
            InitializeComponent();
        }
        private void InitializeToolbox()
        {
            // 创建工具框面板
            toolboxPanel = new FlowLayoutPanel();
            toolboxPanel.BackColor = Color.LightGray;
            toolboxPanel.Dock = DockStyle.Left;
            toolboxPanel.Width = 50;
            toolboxPanel.FlowDirection = FlowDirection.TopDown;
            toolboxPanel.WrapContents = false;

            // 添加工具按钮
            AddToolButton("选择工具", Properties.Resources.方格);
            AddToolButton("画笔工具", Properties.Resources.shezhi);
            AddToolButton("橡皮擦工具", Properties.Resources.方格);

            this.Controls.Add(toolboxPanel);
        }

        private void AddToolButton(string toolName, Image toolIcon)
        {
            Button toolButton = new Button();
            toolButton.Image = toolIcon;
            toolButton.Size = new Size(40, 40);
            toolButton.Margin = new Padding(5);
            toolButton.FlatStyle = FlatStyle.Flat;
            toolButton.FlatAppearance.BorderSize = 0;
            toolButton.Click += (sender, e) =>
            {
                MessageBox.Show($"你选择了 {toolName}");
            };

            toolboxPanel.Controls.Add(toolButton);
        }
    }
}
