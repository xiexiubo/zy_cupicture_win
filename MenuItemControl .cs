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
        private MainForm mainForm;
        public MenuListView(MainForm mainForm)
        {
            this.mainForm = mainForm;
            // 初始化 ListView 设置
            this.View = View.Details;
            this.FullRowSelect = true;
            this.MultiSelect = true;
            this.HeaderStyle = ColumnHeaderStyle.None; // 隐藏表头
            this.Scrollable = true; // 禁用默认滚动条
            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.White;
            this.DoubleBuffered = true; // 减少闪烁
            this.CheckBoxes = true; // 启用复选框
            this.OwnerDraw = true; // 启用自定义绘制

            // 添加列
            this.Columns.Add("MenuColumn", this.Width - SystemInformation.VerticalScrollBarWidth);
            
            // 初始化右键菜单
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("复制", null, (s, e) => CopyMenuItem_Click(s,e)),
                new ToolStripMenuItem("选择", null, (s, e) => SelectCurr(s,e)),
                new ToolStripMenuItem("全选", null, (s, e) => SelectAllItems()),
                new ToolStripMenuItem("清空选择", null, (s, e) => ClearSelection()),
                new ToolStripSeparator(),
              
                new ToolStripMenuItem("合并选中", null, (s, e) => MergeSelectedItems()),
                  new ToolStripMenuItem("导出选中", null, (s, e) => ExportSelectedItems(false)),
                  new ToolStripMenuItem("导出选中(合并的)", null, (s, e) => ExportSelectedItems(true)),
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

            // 启用自定义绘制事件
            this.DrawItem += MenuListView_DrawItem;
            this.DrawColumnHeader += MenuListView_DrawColumnHeader;

            // 隐藏滚动条
            NativeMethods.ShowScrollBar(this.Handle, NativeMethods.SB_VERT, false);
        }

        public void SetWidth(int width) 
        {
            this.Width = width;
            this.Columns.Add("MenuColumn", this.Width - SystemInformation.VerticalScrollBarWidth);
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

        // copyName
        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedItems.Count > 0)
            {
                string selectedText = this.SelectedItems[0].Text;
                Clipboard.SetText(selectedText);
                MessageBox.Show($"已复制: {selectedText}");
            }
            else
            {
                MessageBox.Show("请先选择一个项目。");
            }
        } 
        // 全选当前
        private void SelectCurr(object sender, EventArgs e)
        {   bool selected = true;
            foreach (ListViewItem item in this.SelectedItems) 
            {
                if (!item.Checked) 
                {
                    selected = false;
                    break;
                }
                
            }
            foreach (ListViewItem item in this.SelectedItems)
            {
                if (!selected)
                    item.Checked = true;
                else
                    item.Checked = !item.Checked;
            }
        }  
        // 全选
        private void SelectAllItems()
        {
            foreach (ListViewItem item in this.Items)
            {
                item.Checked = true;
            }
        }

        // 清空选择
        private void ClearSelection()
        {
            foreach (ListViewItem item in this.Items)
            {
                item.Checked = false;
            }
        }

        // 导出选中项
        private void ExportSelectedItems(bool isComb)
        {
            var selectedItems = this.CheckedItems;
            if (selectedItems.Count > 0)
            {
                string mergedText = string.Join(", ", selectedItems.Cast<ListViewItem>().Select(x => x.Text));

                this.mainForm.ExportSelectedItems(selectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList(), isComb);

                //MessageBox.Show($"合并的项: {mergedText}", "合并选中", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (this.SelectedItems.Count > 0) 
            {
                this.mainForm.ExportSelectedItems(this.SelectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList(), isComb);
            }
            else
            {
                MessageBox.Show("没有选中任何项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } 
        // 合并选中项
        private void MergeSelectedItems()
        {
            var selectedItems = this.CheckedItems;
            if (selectedItems.Count > 0)
            {
                string mergedText = string.Join(", ", selectedItems.Cast<ListViewItem>().Select(x => x.Text));
                this.mainForm.MergeRectanglesOneName(selectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList());
                //MessageBox.Show($"合并的项: {mergedText}", "合并选中", MessageBoxButtons.OK, MessageBoxIcon.Information);
                for (int i = 0; i < selectedItems.Count; i++) 
                {
                    var item = selectedItems[i] as ListViewItem;
                    if (item != null) item.ForeColor = Color.White;
                }
            }
            else if (this.SelectedItems.Count > 0)
            {                
                this.mainForm.MergeRectanglesOneName(this.SelectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList());
                for (int i = 0; i < this.SelectedItems.Count; i++)
                {
                    var item = this.SelectedItems[i];
                    if (item != null) item.ForeColor = Color.White;
                }
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

        // 自定义绘制菜单项
        private void MenuListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true; // 使用默认绘制

            // 高亮选中项
            if (e.Item.Checked)
            {
                e.Item.BackColor = Color.LightBlue; // 设置选中项背景色
            }
            else
            {
                e.Item.BackColor = Color.White; // 恢复未选中项背景色
            }
        }

        // 自定义绘制列头（隐藏列头）
        private void MenuListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = false; // 不绘制列头
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
            public const int SB_VERT = 1;

            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
        }
    }
}
