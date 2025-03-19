using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public class MenuListView : ListView
    {
        private bool isDragging;
        private int dragStartY;
        private int scrollStartValue;
        private readonly ContextMenuStrip contextMenu;

        public MenuListView()
        {
            // 初始化 ListView 设置
            this.View = View.Details;
            this.FullRowSelect = true;
            this.MultiSelect = true;
            this.HeaderStyle = ColumnHeaderStyle.None; // 隐藏表头
            this.Scrollable = true;
            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.White;
            this.DoubleBuffered = true; // 减少闪烁
            this.CheckBoxes = true; // 启用复选框

            // 添加列
            this.Columns.Add("MenuColumn", this.Width - SystemInformation.VerticalScrollBarWidth);

            // 初始化右键菜单
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
            new ToolStripMenuItem("合并选中", null, (s, e) => MergeSelectedItems()),
            new ToolStripMenuItem("手动排列选中", null, (s, e) => ArrangeSelected()),
            new ToolStripMenuItem("手动排列全部", null, (s, e) => ArrangeAll())
            });

            // 绑定右键菜单
            this.ContextMenuStrip = contextMenu;

            // 启用自定义滚动
            this.MouseWheel += MenuListView_MouseWheel;
            this.MouseDown += MenuListView_MouseDown;
            this.MouseMove += MenuListView_MouseMove;
            this.MouseUp += MenuListView_MouseUp;
        }

        // 添加菜单项
        public void AddMenuItem(string text)
        {
            var item = new ListViewItem(text)
            {
                Checked = false // 默认未选中
            };
            this.Items.Add(item);
        }

        // 合并选中项
        private void MergeSelectedItems()
        {
            var selectedItems = this.CheckedItems;
            if (selectedItems.Count > 0)
            {
                string mergedText = string.Join(", ", selectedItems.Cast<ListViewItem>().Select(x => x.Text));
                MessageBox.Show($"合并的项: {mergedText}", "合并选中", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("没有选中任何项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 手动排列选中项
        private void ArrangeSelected()
        {
            var selectedItems = this.CheckedItems;
            if (selectedItems.Count > 0)
            {
                // 示例：将选中的项移动到顶部
                foreach (ListViewItem item in selectedItems)
                {
                    item.Remove();
                    this.Items.Insert(0, item);
                }
            }
            else
            {
                MessageBox.Show("没有选中任何项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 手动排列全部项
        private void ArrangeAll()
        {
            // 示例：按文本排序
            var items = this.Items.Cast<ListViewItem>().OrderBy(x => x.Text).ToList();
            this.Items.Clear();
            this.Items.AddRange(items.ToArray());
        }

        // 鼠标滚轮滚动
        private void MenuListView_MouseWheel(object sender, MouseEventArgs e)
        {
            int newScrollValue = this.GetScrollOffset() - (e.Delta / 2);
            newScrollValue = Math.Max(0, Math.Min(this.GetTotalHeight() - this.Height, newScrollValue));
            this.ScrollTo(newScrollValue);
        }

        // 鼠标拖动滚动
        private void MenuListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.Items.Count > 0)
            {
                isDragging = true;
                dragStartY = e.Y;
                scrollStartValue = this.GetScrollOffset();
                Cursor = Cursors.Hand;
            }
        }

        private void MenuListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int delta = e.Y - dragStartY;
                int newValue = scrollStartValue - delta;

                newValue = Math.Max(0, Math.Min(this.GetTotalHeight() - this.Height, newValue));
                this.ScrollTo(newValue);
            }
        }

        private void MenuListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                Cursor = Cursors.Default;
            }
        }

        // 获取当前滚动偏移量
        private int GetScrollOffset()
        {
            if (this.Items.Count<1)
            {
                return 0;
            }
            const int LVM_GETTOPINDEX = 0x1027;
            int topIndex = NativeMethods.SendMessage(this.Handle, LVM_GETTOPINDEX, 0, 0);
            return topIndex * this.Items[0].Bounds.Height;
        }

        // 获取总高度
        private int GetTotalHeight()
        {
            return this.Items.Count * this.Items[0].Bounds.Height;
        }

        // 滚动到指定位置
        private void ScrollTo(int offset)
        {
            const int LVM_SCROLL = 0x1014;
            int delta = offset - this.GetScrollOffset();
            NativeMethods.SendMessage(this.Handle, LVM_SCROLL, 0, delta);
        }

        // 用于调用 Windows API 的辅助类
        private static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        }
    }
}
