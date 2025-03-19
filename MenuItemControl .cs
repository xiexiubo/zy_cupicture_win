using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public class MenuItemPanel : UserControl
    {
        private readonly FlowLayoutPanel flowPanel;
        private readonly ContextMenuStrip contextMenu;

        public MenuItemPanel()
        {
            // 主布局设置
            this.AutoScroll = true;
            this.VerticalScroll.Visible = false;
            this.VerticalScroll.Enabled = true;
            this.HorizontalScroll.Enabled = false;

            // 创建流式布局面板
            flowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top
            };

            // 右键菜单
            contextMenu = new ContextMenuStrip();
            var mergeItem = contextMenu.Items.Add("合并选中");
            var arrangeSelectedItem = contextMenu.Items.Add("手动排列选中");
            var arrangeAllItem = contextMenu.Items.Add("手动排列全部");

            mergeItem.Click += (s, e) => MergeSelectedItems();
            arrangeSelectedItem.Click += (s, e) => ArrangeSelected();
            arrangeAllItem.Click += (s, e) => ArrangeAll();

            this.Controls.Add(flowPanel);
            this.MouseWheel += MenuPanel_MouseWheel;
        }

        // 添加示例项目
        public void AddItem(string text)
        {
            var item = new MenuItemControl(text);
            flowPanel.Controls.Add(item);
        }

        // 合并选中项逻辑
        private void MergeSelectedItems()
        {
            var selected = flowPanel.Controls.OfType<MenuItemControl>().Where(c => c.Checked);
            // 实现合并逻辑
        }

        // 滚动处理
        private void MenuPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            this.VerticalScroll.Value = Math.Max(
                this.VerticalScroll.Minimum,
                Math.Min(
                    this.VerticalScroll.Maximum,
                    this.VerticalScroll.Value - e.Delta
                ));
            this.Refresh();
        }

        // 自定义菜单项控件
        private class MenuItemControl : UserControl
        {
            public bool Checked => checkBox.Checked;

            private readonly CheckBox checkBox;

            public MenuItemControl(string text)
            {
                this.Size = new Size(200, 30);
                this.Margin = new Padding(0);

                checkBox = new CheckBox
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    CheckAlign = ContentAlignment.MiddleLeft
                };

                this.Controls.Add(checkBox);
                this.ContextMenuStrip = new ContextMenuStrip();
            }
        }

        // 其他方法实现...
        private void ArrangeSelected() 
        {
        }
        private void ArrangeAll()
        {
        }
    }
}
