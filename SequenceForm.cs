using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static zy_cutPicture.SequenceForm;

namespace zy_cutPicture
{
    public partial class SequenceForm : Form
    {
        private List<PictureBoxX> pictureBoxes = new List<PictureBoxX>();
        public SequenceForm()
        {
            InitializeComponent();
            // 开启当前窗体的双缓冲
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            // 通过反射设置 panelWorkArea 的双缓冲
            SetDoubleBuffered(panelWorkArea);
        }

        private void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;
            PropertyInfo doubleBufferProperty = control.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            doubleBufferProperty?.SetValue(control, true, null);
        }

        public void init()
        {
            panelWorkArea.AllowDrop = true;
            panelWorkArea.DragEnter += panelWorkArea_DragEnter;
            panelWorkArea.DragDrop += panelWorkArea_DragDrop;
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "图片文件 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    AddPictureBox(filePath);
                }
            }
        }

        private void AddPictureBox(string filePath)
        {
            try
            {
                PictureBoxX pictureBox = new PictureBoxX(filePath);               
               // pictureBox.Size = pictureBox.ImageX.Size;
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Location = new Point(panelWorkArea.Width/2, panelWorkArea.Height / 2);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.MouseDown += PictureBox_MouseDown;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.MouseUp += PictureBox_MouseUp;
                panelWorkArea.Controls.Add(pictureBox);
                panelWorkArea.Paint += panelWorkArea_Paint;
                pictureBoxes.Add(pictureBox);
                panelWorkArea.Invalidate();
                panelWorkArea.Controls.SetChildIndex(pictureBox, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片时出错: {ex.Message}");
            }
        }
        public static Color GetColorAtPosition(int i, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("count 必须是正整数", nameof(count));
            }
            if (i < 0 || i >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "i 必须在 0 到 count - 1 的范围内");
            }

            int  value = (255*3)*i / (count);
            int r = 0;
            int g = 0;
            int b = 0;
            if (value <= 255)
            {
                r = value;
            }
            else if (value > 255 && value <= 255 * 2)
            {
                //r = 255;
                g = value-255;
            }
            else if (value > 255 * 2)
            {
                //r = 255;
                //g = 255;
                b = value - 255 * 2;
            }
            return Color.FromArgb(255,r, g, b);
        }
    
        private void panelWorkArea_Paint(object sender, PaintEventArgs e)
        {
            //panelWorkArea.BackgroundImage=null;
            //Graphics bg = e.Graphics;
            //bg.DrawImage(Properties.Resources.方格,0,0, panelWorkArea.Width, panelWorkArea.Height);
           
            for (int i = panelWorkArea.Controls.Count - 1; i >= 0; i--)
            {
                var item = panelWorkArea.Controls[i] as PictureBoxX;
                if (item == null) continue;
                Graphics g = e.Graphics;
                Image img = item.ImageX;
                g.DrawImage(img, new Point(item.Location.X, item.Location.Y));
               
                // 创建一个 Pen 对象，用于绘制矩形的边框
                Pen pen = new Pen(Color.Aqua, 2);
                // 设置虚线样式
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                // 定义矩形的位置和大小
                Rectangle rect = new Rectangle(item.Location.X, item.Location.Y,
                    img.Size.Width, img.Size.Height);
                // 绘制矩形
                g.DrawRectangle(pen, rect);
                // 释放 Pen 对象
                pen.Dispose();
            }

        }
        public void SetLayerOder() 
        {

        }
        private Point lastMousePosition;
        private bool isDragging = false;

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var box = (PictureBoxX)sender;           
            panelWorkArea.Controls.SetChildIndex(box, 1);

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
                
            }
        }

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

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                panelWorkArea.Invalidate();
            }
        }

        private void panelWorkArea_DragEnter(object sender, DragEventArgs e)
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

        private void panelWorkArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (Path.GetExtension(filePath).ToLower() == ".jpg" ||
                    Path.GetExtension(filePath).ToLower() == ".jpeg" ||
                    Path.GetExtension(filePath).ToLower() == ".png" ||
                    Path.GetExtension(filePath).ToLower() == ".bmp")
                {
                    AddPictureBox(filePath);
                }
            }
        }
        private void newMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void openMenuItem_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {

        }        

        private void customTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void helpMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
    public class PictureBoxX : PictureBox
    {
        public PictureBoxX(string path)
        {
            // 开启双缓冲
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            this.path = path;
            this.ImageX = Image.FromFile(path);
            this.Size = new Size(ImageX.Width, ImageX.Height);
        }
        public Image ImageX;

        /// <summary>
        /// 动画序列索引
        /// </summary>
        public int IndexSeq;
        public bool IsSelected = false;
        public string path;
    }
}