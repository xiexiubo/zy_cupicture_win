using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public partial class SequenceForm : Form
    {
        // 存储 PictureBoxX 控件的列表
        private List<PictureBoxX> pictureBoxList = new List<PictureBoxX>();

        public SequenceForm()
        {
            InitializeComponent();
            // 开启当前窗体的双缓冲，减少闪烁
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            // 通过反射设置 panelWorkArea 的双缓冲
            SetDoubleBuffered(panelWorkArea);
        }

        // 设置控件的双缓冲
        private void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;
            PropertyInfo doubleBufferProperty = control.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            doubleBufferProperty?.SetValue(control, true, null);
        }

        // 初始化工作区域面板的拖放功能
        public void Initialize()
        {
            panelWorkArea.AllowDrop = true;
            panelWorkArea.DragEnter += PanelWorkArea_DragEnter;
            panelWorkArea.DragDrop += PanelWorkArea_DragDrop;
        }

        // 打开文件对话框选择图片
        private void OpenFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "图片文件 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    AddPictureBoxToPanel(filePath);
                }
            }
        }

        // 向工作区域面板添加 PictureBoxX 控件
        private void AddPictureBoxToPanel(string filePath)
        {
            try
            {
                PictureBoxX pictureBox = new PictureBoxX(filePath);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Location = new Point(panelWorkArea.Width / 2, panelWorkArea.Height / 2);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.MouseDown += PictureBox_MouseDown;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.MouseUp += PictureBox_MouseUp;
                panelWorkArea.Controls.Add(pictureBox);
                panelWorkArea.Paint += PanelWorkArea_Paint;
                pictureBoxList.Add(pictureBox);
                panelWorkArea.Invalidate();
                panelWorkArea.Controls.SetChildIndex(pictureBox, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片时出错: {ex.Message}");
            }
        }

        // 根据索引和总数生成颜色
        public static Color GetColorByIndex(int index, int totalCount)
        {
            if (totalCount <= 0)
            {
                throw new ArgumentException("总数必须是正整数", nameof(totalCount));
            }
            if (index < 0 || index >= totalCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "索引必须在 0 到总数 - 1 的范围内");
            }

            int value = (255 * 3) * index / totalCount;
            int red = 0;
            int green = 0;
            int blue = 0;
            if (value <= 255)
            {
                red = value;
            }
            else if (value > 255 && value <= 255 * 2)
            {
                green = value - 255;
            }
            else if (value > 255 * 2)
            {
                blue = value - 255 * 2;
            }
            return Color.FromArgb(255, red, green, blue);
        }

        // 工作区域面板的绘制事件处理方法
        private void PanelWorkArea_Paint(object sender, PaintEventArgs e)
        {
            for (int i = panelWorkArea.Controls.Count - 1; i >= 0; i--)
            {
                var pictureBox = panelWorkArea.Controls[i] as PictureBoxX;
                if (pictureBox == null) continue;
                Graphics graphics = e.Graphics;
                Image image = pictureBox.ImageX;
                graphics.DrawImage(image, pictureBox.Location.X, pictureBox.Location.Y, image.Width, image.Height);

                // 创建一个 Pen 对象，用于绘制矩形的边框
                using (Pen pen = new Pen(Color.Aqua, 2))
                {
                    // 设置虚线样式
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    // 定义矩形的位置和大小
                    Rectangle rectangle = new Rectangle(pictureBox.Location.X, pictureBox.Location.Y,
                        image.Size.Width, image.Size.Height);
                    // 绘制矩形
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }

        // 设置图层顺序
        public void SetLayerOrder()
        {
            // 可在此处添加设置图层顺序的具体逻辑
        }

        // 记录鼠标上一次的位置
        private Point lastMousePosition;
        // 标记是否正在拖动
        private bool isDragging = false;

        // 鼠标按下事件处理方法
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBoxX)sender;
            panelWorkArea.Controls.SetChildIndex(pictureBox, 1);

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
        }

        // 鼠标移动事件处理方法
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                PictureBox pictureBox = (PictureBox)sender;
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;
                pictureBox.Left += deltaX;
                pictureBox.Top += deltaY;
                panelWorkArea.Invalidate();
            }
        }

        // 鼠标释放事件处理方法
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                panelWorkArea.Invalidate();
            }
        }

        // 工作区域面板的拖放进入事件处理方法
        private void PanelWorkArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // 工作区域面板的拖放完成事件处理方法
        private void PanelWorkArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (Path.GetExtension(filePath).ToLower() == ".jpg" ||
                    Path.GetExtension(filePath).ToLower() == ".jpeg" ||
                    Path.GetExtension(filePath).ToLower() == ".png" ||
                    Path.GetExtension(filePath).ToLower() == ".bmp")
                {
                    AddPictureBoxToPanel(filePath);
                }
            }
        }

        // 新建菜单项点击事件处理方法
        private void newMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加新建操作的具体逻辑
        }

        // 打开菜单项点击事件处理方法
        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFiles();
        }

        // 保存菜单项点击事件处理方法
        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加保存操作的具体逻辑
        }

        // 另存为菜单项点击事件处理方法
        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加另存为操作的具体逻辑
        }

        // 自定义标题栏绘制事件处理方法
        private void customTitleBar_Paint(object sender, PaintEventArgs e)
        {
            // 可在此处添加自定义标题栏绘制的具体逻辑
        }

        // 帮助菜单项点击事件处理方法
        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加帮助操作的具体逻辑
        }

        // 窗口菜单项点击事件处理方法
        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加窗口操作的具体逻辑
        }
    }

    public class PictureBoxX : PictureBox
    {
        public PictureBoxX(string filePath)
        {
            // 开启双缓冲，减少闪烁
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            this.FilePath = filePath;
            this.ImageX = Image.FromFile(filePath);
            this.Size = new Size(ImageX.Width, ImageX.Height);

            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {ImageX.Size} ");
        }

        // 图片对象
        public Image ImageX;
        // 动画序列索引
        public int AnimationSequenceIndex;
        // 是否被选中
        public bool IsSelected = false;
        // 文件路径
        public string FilePath;
    }
}