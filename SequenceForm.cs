using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public partial class SequenceForm : Form
    {
        // 存储 PictureBoxX 控件的列表
        private List<PictureBoxX> pictureBoxList = new List<PictureBoxX>();
        private enum eToolType
        {
            选择工具,
            相似工具,
            橡皮擦工具
        }
        eToolType ToolType;
        public SequenceForm()
        {
            InitializeComponent();
            // 开启当前窗体的双缓冲，减少闪烁
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            // 通过反射设置 panel_Area 的双缓冲
            SetDoubleBuffered(panel_Area);
            brushToolButton.PerformClick();
            panel_anim.MouseDown += Panel_anim_MouseDown;
            panel_anim.MouseMove += Panel_anim_MouseMove;
            panel_anim.MouseUp += Panel_anim_MouseUp;
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
            panel_Area.AllowDrop = true;
            panel_Area.DragEnter += panel_Area_DragEnter;
            panel_Area.DragDrop += panel_Area_DragDrop;
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
        private Bitmap CropImage(Bitmap source, Rectangle rect)
        {

            var bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(source,
                    new Rectangle(0, 0, rect.Width, rect.Height),
                    rect,
                    GraphicsUnit.Pixel);
            }
            return bmp;
        }
        // 向工作区域面板添加 PictureBoxX 控件
        private void AddPictureBoxToPanel(string filePath)
        {
            try
            {
                PictureBoxX pictureBox = new PictureBoxX(filePath);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Location = new Point(panel_Area.Width / 2, panel_Area.Height / 2);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.MouseDown += PictureBox_MouseDown;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.MouseUp += PictureBox_MouseUp;
                panel_Area.Controls.Add(pictureBox);
                panel_Area.Paint += panel_Area_Paint;
                pictureBoxList.Add(pictureBox);
                panel_Area.Invalidate();
                panel_Area.Controls.SetChildIndex(pictureBox, 1);


               //var panelL= new Panel(panel_layer)
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
        private void panel_Area_Paint(object sender, PaintEventArgs e)
        {

            for (int i = panel_Area.Controls.Count - 1; i >= 0; i--)
            {
                var pictureBox = panel_Area.Controls[i] as PictureBoxX;
                if (pictureBox == null) continue;
                Graphics graphics = e.Graphics;
                Image image = pictureBox.bitmap;
                graphics.DrawImage(image, pictureBox.Location.X, pictureBox.Location.Y, image.Width, image.Height);

                // 创建一个 Pen 对象，用于绘制矩形的边框
                using (Pen pen = new Pen(Color.Aqua, 2))
                {
                    if (i == 0)
                    {
                        pen.Color = Color.FromArgb(255, 20, 255, 255);
                    }
                    else
                    {
                        pen.Color = Color.FromArgb(50, 0, 255, 255);
                    }
                    // 设置虚线样式
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    // 定义矩形的位置和大小
                    Rectangle rectangle = new Rectangle(pictureBox.Location.X, pictureBox.Location.Y,
                        image.Size.Width, image.Size.Height);
                    // 绘制矩形
                    graphics.DrawRectangle(pen, rectangle);
                }
                if (this.similarMap != null)
                {
                    var p = pictureBox.Location;
                    p.X = pictureBox.simmilarPos.X + p.X;
                    p.Y = pictureBox.simmilarPos.Y + p.Y;
                    graphics.DrawRectangle(Pens.Red, new Rectangle(p.X, p.Y, this.similarMap.Width, this.similarMap.Height));
                }
            }
            if (!currentDragRect_pictureBox_debug.IsEmpty && panel_Area.Controls.Count > 1 && this.ToolType == eToolType.相似工具)
            {
                var p = panel_Area.Controls[0] as PictureBoxX;
                // 计算在 PictureBox 上显示的矩形位置
                Rectangle displayRect = GetDisplayRectangle(p, currentDragRect_pictureBox_debug);
                displayRect.X = displayRect.X + p.Location.X;
                displayRect.Y = displayRect.Y + p.Location.Y;
                e.Graphics.DrawRectangle(Pens.Red, displayRect);
            }
            if (this.pic_anim_Timer == null&& this.pictureBoxList.Count>0)
                this.pic_anim.BackgroundImage = this.pictureBoxList[0].bitmap;
        }



        // 记录鼠标上一次的位置
        private Point lastMousePosition;
        // 标记是否正在拖动
        private bool isDragging = false;
        private Rectangle currentDragRect_pictureBox_debug = Rectangle.Empty;
        // 鼠标按下事件处理方法
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.similarMap = null;
            var pictureBox = (PictureBoxX)sender;
            panel_Area.Controls.SetChildIndex(pictureBox, 0);
            if (this.ToolType == eToolType.相似工具)
            {
                currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, lastMousePosition, e.Location);
            }

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
            if (this.ToolType == eToolType.相似工具)
            {
                Cursor.Current = Cursors.Cross;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // 鼠标移动事件处理方法
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                PictureBoxX pictureBox = (PictureBoxX)sender;
                if (this.ToolType == eToolType.选择工具)
                {

                    int deltaX = e.X - lastMousePosition.X;
                    int deltaY = e.Y - lastMousePosition.Y;
                    pictureBox.Left += deltaX;
                    pictureBox.Top += deltaY;
                }
                else if (this.ToolType == eToolType.相似工具)
                {
                    currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                }

                panel_Area.Invalidate();

                if (this.ToolType == eToolType.相似工具)
                {
                    Cursor.Current = Cursors.Cross;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        // 鼠标释放事件处理方法
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBoxX pictureBox = (PictureBoxX)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (isDragging)
                {

                    if (Control.ModifierKeys == Keys.Alt)
                    {
                        if (this.ToolType == eToolType.相似工具)
                        {
                            if (currentDragRect_pictureBox_debug.Width > 0 && currentDragRect_pictureBox_debug.Height > 1)
                            {
                                this.similarMap = CropImage(pictureBox.bitmap, currentDragRect_pictureBox_debug);
                                //currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                                this.SetPointSimmilar(this.similarMap, this.pictureBoxList);
                            }
                        }
                        //this.MenuItemPanel.MergeSelectedItems();
                        // 这里可以添加 Ctrl 键抬起后要执行的逻辑代码，也就是判断用户此时没按 Ctrl 键了
                        Console.WriteLine("Keys.Alt 键已经按");
                    }
                }
                this.setPosTemp(false);
                isDragging = false;
                panel_Area.Invalidate();
            }

        }

        // 工作区域面板的拖放进入事件处理方法
        private void panel_Area_DragEnter(object sender, DragEventArgs e)
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
        private void panel_Area_DragDrop(object sender, DragEventArgs e)
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
        private Rectangle GetBitmapRectangle(PictureBoxX pictureBox, Point start, Point end)
        {
            if (pictureBox.bitmap == null)
            {
                return Rectangle.Empty;
            }

            // 获取 PictureBox 的显示区域
            Rectangle displayRectangle = pictureBox.ClientRectangle;

            // 获取图像的原始尺寸
            Size imageSize = pictureBox.bitmap.Size;

            // 计算图像在 PictureBox 中的缩放比例
            float scaleX = (float)imageSize.Width / displayRectangle.Width;
            float scaleY = (float)imageSize.Height / displayRectangle.Height;

            // 根据缩放比例计算起始点和结束点在位图中的像素坐标
            int startX = (int)(start.X * scaleX);
            int startY = (int)(start.Y * scaleY);
            int endX = (int)(end.X * scaleX);
            int endY = (int)(end.Y * scaleY);

            // 确保矩形的坐标和尺寸为正值
            int x = Math.Min(startX, endX);
            int y = Math.Min(startY, endY);
            int width = Math.Abs(endX - startX);
            int height = Math.Abs(endY - startY);

            return new Rectangle(x, y, width, height);
        }
        private Rectangle GetDisplayRectangle(PictureBoxX pictureBox, Rectangle bitmapRect)
        {
            if (pictureBox.bitmap == null)
            {
                return Rectangle.Empty;
            }

            // 获取 PictureBox 的显示区域
            Rectangle displayRectangle = pictureBox.ClientRectangle;

            // 获取图像的原始尺寸
            Size imageSize = pictureBox.bitmap.Size;

            // 计算图像在 PictureBox 中的缩放比例
            float scaleX = (float)displayRectangle.Width / imageSize.Width;
            float scaleY = (float)displayRectangle.Height / imageSize.Height;

            // 根据缩放比例计算在 PictureBox 上显示的矩形位置
            int x = (int)(bitmapRect.X * scaleX);
            int y = (int)(bitmapRect.Y * scaleY);
            int width = (int)(bitmapRect.Width * scaleX);
            int height = (int)(bitmapRect.Height * scaleY);

            return new Rectangle(x, y, width, height);
        }
        // 新建菜单项点击事件处理方法
        private void newMenuItem_Click(object sender, EventArgs e)
        {
            // 可在此处添加新建操作的具体逻辑
            similarMap = null;
            bitmapCache.Clear();
            pictureBoxList.Clear();
            this.anim_icon_info.Text = "";
            this.panel_Area.Controls.Clear();
            this.OpenFiles();
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

        #region 查找相似的像素

        //// 此方法用于计算两个矩形区域的像素相似度
        //private static double CalculateSimilarity(Bitmap source, Bitmap target, int x, int y)
        //{
        //    int width = Math.Min(source.Width, target.Width);
        //    int height = Math.Min(source.Height, target.Height);
        //    int totalPixels = width * height;
        //    int matchingPixels = 0;

        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            if (source.GetPixel(x + i, y + j) == target.GetPixel(i , j))
        //            {
        //                matchingPixels++;
        //            }
        //        }
        //    }

        //    return (double)matchingPixels / totalPixels;
        //}

        //// 该方法会找出目标图像中与源图像相似度最高的位置
        //private static Point FindMostSimilarPosition(Bitmap source, Bitmap target)
        //{
        //    double maxSimilarity = 0;
        //    Point mostSimilarPoint = new Point(0, 0);

        //    for (int x = 0; x <= source.Width - target.Width ; x++)
        //    {
        //        for (int y = 0; y <= source.Height - target.Height; y++)
        //        {
        //            double similarity = CalculateSimilarity(source, target, x, y);
        //            if (similarity > maxSimilarity)
        //            {
        //                maxSimilarity = similarity;
        //                mostSimilarPoint = new Point(x, y);
        //            }
        //        }
        //    }

        //    return mostSimilarPoint;
        //}

        //// 此方法会将 PictureBox 列表中每个元素与 similarMap 对比，找到相似度最高的位置并设置其 Location
        //private void SetPointSimmilar(Bitmap similarMap, List<PictureBoxX> list)
        //{
        //    foreach (PictureBoxX pictureBox in list)
        //    {
        //        if (pictureBox.bitmap != null)
        //        {
        //            Bitmap sourceBitmap = new Bitmap(pictureBox.bitmap);
        //            Point mostSimilarPosition = FindMostSimilarPosition(sourceBitmap, similarMap);
        //            pictureBox.simmilarPos = mostSimilarPosition;
        //            Console.WriteLine("mostSimilarPosition:" + mostSimilarPosition);
        //        }
        //    }
        //}

        private int tolerance = 1; // 颜色容差值
        private Bitmap similarMap = null;
        private Dictionary<Bitmap, BitmapDataCache> bitmapCache = new Dictionary<Bitmap, BitmapDataCache>();
        // 优化后的相似度计算方法
        private unsafe double CalculateSimilarityOptimized(BitmapDataCache sourceCache, BitmapDataCache targetCache,
            int sourceX, int sourceY, int width, int height)
        {
            int matchingPixels = 0;


            fixed (byte* sourcePtr = sourceCache.Pixels)
            fixed (byte* targetPtr = targetCache.Pixels)
            {
                for (int y = 0; y < height; y++)
                {
                    byte* sourceRow = sourcePtr + ((sourceY + y) * sourceCache.Stride) + (sourceX * sourceCache.BytesPerPixel);
                    byte* targetRow = targetPtr + (y * targetCache.Stride);

                    for (int x = 0; x < width; x++)
                    {
                        if (true)
                        {
                            // 快速比较颜色差异（使用曼哈顿距离）
                            int diff = //Math.Abs(sourceRow[3] - targetRow[3]) +  // A
                                Math.Abs(sourceRow[2] - targetRow[2]) +  // R
                                       Math.Abs(sourceRow[1] - targetRow[1]) +  // G
                                       Math.Abs(sourceRow[0] - targetRow[0]);   // B

                            if (diff <= tolerance * 3&& 0.01f >= Math.Abs(sourceRow[3] - targetRow[3]) ) // 三个通道的总容差
                                matchingPixels++;
                        }
                        else
                        {                    

                            if (0.01f >= Math.Abs(sourceRow[3] - targetRow[3]) &&   //A
                                tolerance >= Math.Abs(sourceRow[2] - targetRow[2]) &&//R
                                tolerance >= Math.Abs(sourceRow[1] - targetRow[1]) &&//G
                                tolerance >= Math.Abs(sourceRow[0] - targetRow[0])   //B
                                ) // 三个通道的都容差
                                matchingPixels++;
                        }





                        sourceRow += sourceCache.BytesPerPixel;
                        targetRow += targetCache.BytesPerPixel;
                    }
                }
            }

            return (double)matchingPixels / (width * height);
        }

        
        // 优化后的查找方法
        private Point FindMostSimilarPositionOptimized(Bitmap source, Bitmap target,int sourceW,int sourceH,int targetW,int targetH)
        {
            var sourceCache = GetBitmapDataCache(source);
            var targetCache = GetBitmapDataCache(target);

            int searchStep = 2; // 搜索步长（平衡速度与精度）
            int width = targetW;
            int height = targetH;

            int s_width = sourceW;
            int s_height = sourceH;

            double maxSimilarity = 0;
            Point mostSimilarPoint = Point.Empty;

            // 并行搜索
            Parallel.For(0, (s_height - height) / searchStep + 1, y =>
            {
                int currentY = y * searchStep;
                for (int x = 0; x <= s_width - width; x += searchStep)
                {
                    double similarity = CalculateSimilarityOptimized(
                        sourceCache, targetCache,
                        x, currentY,
                        width, height);

                    if (similarity > maxSimilarity)
                    {
                        lock (sourceCache)
                        {
                            if (similarity > maxSimilarity)
                            {
                                maxSimilarity = similarity;
                                mostSimilarPoint = new Point(x, currentY);
                            }
                        }
                    }
                }
            });

            // 局部精确搜索（在找到的最佳点附近进行精细搜索）
            int refineRange = searchStep * 2;
            for (int y = Math.Max(0, mostSimilarPoint.Y - refineRange);
                 y < Math.Min(s_height - height, mostSimilarPoint.Y + refineRange);
                 y++)
            {
                for (int x = Math.Max(0, mostSimilarPoint.X - refineRange);
                     x < Math.Min(s_width - width, mostSimilarPoint.X + refineRange);
                     x++)
                {
                    double similarity = CalculateSimilarityOptimized(
                        sourceCache, targetCache,
                        x, y,
                        width, height);

                    if (similarity > maxSimilarity)
                    {
                        maxSimilarity = similarity;
                        mostSimilarPoint = new Point(x, y);
                    }
                }
            }

            return mostSimilarPoint;
        }

        // 获取位图数据缓存（带内存缓存）
        private BitmapDataCache GetBitmapDataCache(Bitmap bmp)
        {
            if (bitmapCache.TryGetValue(bmp, out var cache))
                return cache;

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] pixels = new byte[bmpData.Stride * bmp.Height];

            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bmpData);

            var newCache = new BitmapDataCache
            {
                Pixels = pixels,
                Stride = bmpData.Stride,
                BytesPerPixel = bytesPerPixel
            };

            bitmapCache[bmp] = newCache;
            return newCache;
        }

        // 更新调用方法
        private void SetPointSimmilar(Bitmap similarMap, List<PictureBoxX> list)
        {
            // 预缓存目标图像数据
            var targetCache = GetBitmapDataCache(similarMap);

            Parallel.ForEach(list, pictureBox =>
            {
                if (pictureBox.bitmap != null)
                {
                    var sourceCache = GetBitmapDataCache(pictureBox.bitmap);
                    Point mostSimilarPosition = FindMostSimilarPositionOptimized(pictureBox.bitmap, similarMap, pictureBox.Width, pictureBox.Height, similarMap.Width, similarMap.Height);
                    pictureBox.simmilarPos = mostSimilarPosition;
                    Console.WriteLine(pictureBox.FilePath + " " + mostSimilarPosition);
                    var p = pictureBox;
                    Console.WriteLine("---p: " + p.Location + "  " + p.Size + " " + p.FilePath);
                }
            });
        }
        #endregion

        private void 图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel_tuceng.Visible = !this.panel_tuceng.Visible;
        }

        private void 动画ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel_anim.Visible = !this.panel_anim.Visible;
        }

      

        private void 选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             this.panel_xuanxiang.Visible = !this.panel_xuanxiang.Visible;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.tolerance = (int)num_rongcha.Value;
        }

        private void btn_resize_pic_Click(object sender, EventArgs e)
        {
            this.ReGeneratePic(this.pictureBoxList);
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {

        }

        int typeArrange = 2;
        bool isDuiqi = false;
        Rectangle outRect = Rectangle.Empty;

        private void ArrangePicType(int type)
        {
            int spacing = 10; // 图片之间的间距
            int currentX = 10;
            int currentY = 10;

            foreach (PictureBoxX pictureBox in pictureBoxList)
            {
                if (type == 0)
                {
                    pictureBox.Location = new Point(currentX, currentY);
                    currentX += pictureBox.Width + spacing;

                    // 如果超出面板宽度，换行
                    if (currentX + pictureBox.Width > panel_Area.Width)
                    {
                        currentX = 10;
                        currentY += pictureBox.Height + spacing;
                    }
                }
                else if (type == 1)
                {
                    int x = (this.panel_Area.Width - pictureBoxList[0].Width) / 2;
                    int y = (this.panel_Area.Height - pictureBoxList[0].Height) / 2;
                    pictureBox.Location = new Point(x, y);
                }
                else if (type == 2)
                {
                    int x = (this.panel_Area.Width - pictureBox.Width) / 2;
                    int y = (this.panel_Area.Height - pictureBox.Height) / 2;
                    pictureBox.Location = new Point(x, y);
                }
                
            }
            if (type == 3)
            {
                setPosTemp(true);
                //Console.WriteLine("outRect: " + outRect + "x:" + x + "  y:" + y);
            }
            panel_Area.Invalidate();
        }

        void setPosTemp(bool setLocation) 
        {
            // 以第一个图片的位置为基准进行左对齐                
            int x = (this.panel_Area.Width - pictureBoxList[0].Width) / 2;
            int y = (this.panel_Area.Height - pictureBoxList[0].Height) / 2;
            Point pos_fixed = pictureBoxList[0].simmilarPos;
            outRect = Rectangle.Empty;

            foreach (PictureBoxX p in pictureBoxList)
            {

                var pos = new Point(x - (p.simmilarPos.X - pos_fixed.X), y - (p.simmilarPos.Y - pos_fixed.Y));

                if (setLocation)
                    p.Location = pos;
                //Console.WriteLine("p: " + p.Location + "  " + p.Size + "   outRect" + outRect + "   " + p.FilePath);
                p.simmilarPosTemp = pos;
                if (outRect == Rectangle.Empty)
                    outRect = new Rectangle(pos, p.Size);
                outRect = Rectangle.Union(outRect, new Rectangle(pos, p.Size));
            }
            Console.WriteLine("outRect: " + outRect + "x:" + x + "  y:" + y);
        }
        void ReGeneratePic(List<PictureBoxX> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }
            if (outRect.Width == 0) 
            {
                MessageBox.Show("还没有识别特征点！(用矩形工具)","");
                return;
            }

            // 找出最大的宽度和高度
            int maxWidth = outRect.Width;
            int maxHeight = outRect.Height;
            // 调整每个图片的大小
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                //var p = new Point(x - (p.simmilarPos.X - pos_fixed.X), y - (p.simmilarPos.Y - pos_fixed.Y));

                Bitmap newBitmap = new Bitmap(maxWidth, maxHeight);
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(item.bitmap, new Rectangle(item.simmilarPosTemp.X- outRect.X, item.simmilarPosTemp.Y - outRect.Y, item.bitmap.Width, item.bitmap.Height));
                }
                list[i].bitmap = newBitmap;
            }
        }
        // 排列图组
        private void 排列图组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            typeArrange++;
            if (typeArrange == 3) typeArrange = 0;
            ArrangePicType(typeArrange);
        }

        // 对齐图组
        private void 对齐图组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxList.Count > 0)
            {
                this.isDuiqi = !this.isDuiqi;
                if (this.isDuiqi)
                {
                    ArrangePicType(3);
                }
                else
                {
                    ArrangePicType(typeArrange);
                }
            }
        }
        private int elapsedTime = 0;
        Timer pic_anim_Timer = null;
        int PlayOder = 0;
        private void pic_anim_Timer_Tick(object sender, EventArgs e)
        {
            if (pictureBoxList.Count > 0)
            {
                PlayOder = (PlayOder + 1) % pictureBoxList.Count;
                this.pic_anim.Image = pictureBoxList[PlayOder].bitmap;
                this.anim_icon_info.Text = $"({pictureBoxList[PlayOder].bitmap.Width},{pictureBoxList[PlayOder].bitmap.Height})";
                this.pic_anim.Invalidate();
            }
        }

        private void pic_anim_Click(object sender, EventArgs e)
        {
            if (pic_anim_Timer != null)
            {
                pic_anim_Timer.Stop();
                pic_anim_Timer = null;
                this.pic_anim.BackgroundImage =this.pictureBoxList[0].bitmap;
                this.pic_anim.BackgroundImageLayout = ImageLayout.Stretch;
                this.pic_anim.Image = zy_cutPicture.Properties.Resources.生成播放按钮2;
            }
            else
            {
                if (pictureBoxList.Count > 0)
                {
                    pic_anim_Timer = new Timer();
                    pic_anim_Timer.Interval = 1000/5;
                    pic_anim_Timer.Tick += pic_anim_Timer_Tick;
                    pic_anim_Timer.Start();
                    PlayOder = 0;
                    this.pic_anim.BackgroundImage = zy_cutPicture.Properties.Resources.方格;
                    this.pic_anim.BackgroundImageLayout = ImageLayout.Tile;
                    this.pic_anim.Image = pictureBoxList[PlayOder].bitmap;
                }
            }
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            if (pic_anim_Timer != null)
            {
                pic_anim_Timer.Stop();
                pic_anim_Timer = null;
                this.pic_anim.BackgroundImage = this.pictureBoxList[0].bitmap;
                this.pic_anim.BackgroundImageLayout = ImageLayout.Stretch;
                this.pic_anim.Image = zy_cutPicture.Properties.Resources.生成播放按钮2;
                this.btn_play.Text = "Play";
            }
            else
            {
                if (pictureBoxList.Count > 0)
                {
                    pic_anim_Timer = new Timer();
                    pic_anim_Timer.Interval = (int)num_anim_interval.Value;
                    pic_anim_Timer.Tick += pic_anim_Timer_Tick;
                    pic_anim_Timer.Start();
                    PlayOder = 0;
                    this.pic_anim.BackgroundImage = zy_cutPicture.Properties.Resources.方格;
                    this.pic_anim.BackgroundImageLayout = ImageLayout.Tile;
                    this.pic_anim.Image = pictureBoxList[PlayOder].bitmap;
                    this.btn_play.Text = "Stop";
                }
            }
        }

       

        private void 重新打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           similarMap = null;
            bitmapCache.Clear();
            this.anim_icon_info.Text = "";
            for (int i = 0; i < pictureBoxList.Count; i++) 
            {
                var p = pictureBoxList[i];
               p.setpicX(p.FilePath);
            }           
        }

      

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var p in pictureBoxList)
                {
                    if (File.Exists(p.FilePath))
                    {
                        // 确保文件没有被其他程序占用
                        using (FileStream fs = new FileStream(p.FilePath, FileMode.Open, FileAccess.Write))
                        {
                            p.bitmap.Save(fs, ImageFormat.Png);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"文件路径 {p.FilePath} 不存在，无法保存。");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件时出错: {ex.Message}");
            }
        }

        private void 另ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG 文件|*.png";
            saveFileDialog.Title = "另存为";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    string directory = Path.GetDirectoryName(filePath);
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    for (int i = 0; i < pictureBoxList.Count; i++)
                    {
                        PictureBoxX p = pictureBoxList[i];
                        string newFilePath = Path.Combine(directory, $"{name}_{i}.png");
                        using (FileStream fs = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
                        {
                            p.bitmap.Save(fs, ImageFormat.Png);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存文件时出错: {ex.Message}");
                }
            }


        }

        static bool HasCreateFolderPermission(string path)
        {
            try
            {
                // 获取当前用户的身份
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                // 获取目录的访问控制列表
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
                AuthorizationRuleCollection rules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                // 检查是否有创建文件夹的权限
                foreach (FileSystemAccessRule rule in rules)
                {
                    NTAccount ntAccount = rule.IdentityReference as NTAccount;
                    if (ntAccount != null && principal.IsInRole(ntAccount.Value) &&
                        (rule.FileSystemRights & FileSystemRights.CreateDirectories) == FileSystemRights.CreateDirectories &&
                        rule.AccessControlType == AccessControlType.Allow)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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
            this.bitmap = new Bitmap(filePath);
            this.Size = new Size(bitmap.Width, bitmap.Height);

            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {bitmap.Size} ");
        }
        public  void setpicX(string filePath)
        {           
            this.FilePath = filePath;
            this.bitmap = new Bitmap(filePath);
            this.Size = new Size(bitmap.Width, bitmap.Height);
            this.simmilarPos = Point.Empty;
            this.simmilarPosTemp = Point.Empty;
            IsSelected = false;
            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {bitmap.Size} ");
        }

        // 图片对象
        public Bitmap bitmap;
        // 动画序列索引
        public int AnimationSequenceIndex;
        // 是否被选中
        public bool IsSelected = false;
        // 文件路径
        public string FilePath;
        public Point simmilarPos = Point.Empty;
        public Point simmilarPosTemp = Point.Empty;
    }
    // 添加新的类成员用于缓存图像数据
    class BitmapDataCache
    {
        public byte[] Pixels { get; set; }
        public int Stride { get; set; }
        public int BytesPerPixel { get; set; }
    }
}