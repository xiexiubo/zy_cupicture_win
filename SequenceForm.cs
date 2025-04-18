using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using Image = System.Drawing.Image;
using Application = System.Windows.Forms.Application;
using AnimatedGif;

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
            魔术棒工具
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
            panel_anim.MouseDown += Panel_anim_MouseDown;
            panel_anim.MouseMove += Panel_anim_MouseMove;
            panel_anim.MouseUp += Panel_anim_MouseUp;
            this.KeyDown += SequenceForm_KeyDown;
            this.pic_anim.PreviewKeyDown += Focused_KeyDown;

            SelectTool(this.brushToolButton);
            //panel_anim.Parent = this.panelWorkArea;
            panel_anim.Location = new Point(this.Width- panel_anim.Width-30,32);
           
            this.pic_anim.Paint+= pic_anim_Paint;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            // 获取程序集名称
            string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            // 获取当前程序集
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            // 获取资源名称
            string resourceName = currentAssembly.GetName().Name + "." + assemblyName;

            // 从资源中读取程序集数据
            using (Stream stream = currentAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    // 加载程序集
                    return Assembly.Load(assemblyData);
                }
            }

            return null;
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
                    try
                    {
                        AddPictureBoxToPanel(filePath);
                    }
                    catch (Exception ex)
                    {
                        // 处理异常，例如记录日志或显示错误消息
                        MessageBox.Show($"处理文件 {filePath} 时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
        public void AddPictureBoxToPanel(string filePath)
        {
            AddPictureBoxToPanel(filePath,null);
        }

        public void AddPictureBoxToPanel(string path,Bitmap bitmap)
        {
            try
            {
                PictureBoxX pictureBox = new PictureBoxX(path);
                if(bitmap!=null)
                pictureBox.setpicX(bitmap);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Location = new Point(panel_Area.Width / 2, panel_Area.Height / 2);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.MouseDown += PictureBox_MouseDown;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.MouseUp += PictureBox_MouseUp;
                pictureBox.PreviewKeyDown += Focused_KeyDown;
                panel_Area.Controls.Add(pictureBox);
                panel_Area.Paint += panel_Area_Paint;
                this.pictureBoxList.Add(pictureBox);
                panel_Area.Controls.SetChildIndex(pictureBox, 1);
                pictureBox.index_Anim = panel_Area.Controls.Count - 1;


                panel_Area.Invalidate();
                //var panelL= new Panel(panel_layer)
                if (outRect == Rectangle.Empty)
                    outRect = new Rectangle(0, 0, pictureBox.Width, pictureBox.Height);
                outRect = Rectangle.Union(outRect, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片时出错: {ex.Message}");
            }
            ArrangePicType(0);
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
        private readonly object _lock_similarMap = new object();
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
                    pen.Color = Color.FromArgb(50, 0, 0, 255);
                    if (pictureBox == this.pictureBoxList[PlayOder])
                    {
                        pen.Color = Color.FromArgb(255, 0, 255, 0);
                    }
                    if (pictureBox.Focused)
                    {
                        pen.Color = Color.FromArgb(255, 255, 0, 0);
                    }
                   
                    // 设置虚线样式
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    // 定义矩形的位置和大小
                    Rectangle rectangle = new Rectangle(pictureBox.Location.X, pictureBox.Location.Y,
                        image.Size.Width, image.Size.Height);
                    // 绘制矩形
                    graphics.DrawRectangle(pen, rectangle);
                }

                lock (_lock_similarMap)
                {
                    if (this.similarMap != null)
                    {
                        var p = pictureBox.Location;
                        p.X = pictureBox.simmilarPos.X + p.X;
                        p.Y = pictureBox.simmilarPos.Y + p.Y;
                        graphics.DrawRectangle(Pens.Red, new Rectangle(p.X, p.Y, this.similarMap.Width, this.similarMap.Height));
                    }
                }               
            }
            {
            if ((this.ToolType == eToolType.相似工具|| this.ToolType == eToolType.魔术棒工具) && this.currentDragRect_area_debug!=Rectangle.Empty)
                //var p = panel_Area.Controls[0] as PictureBoxX;
                // 计算在 PictureBox 上显示的矩形位置                
                e.Graphics.DrawRectangle(Pens.Red, currentDragRect_area_debug);
            }


        }
        private void pic_anim_Paint(object sender, PaintEventArgs e)
        {
          
            //Console.WriteLine("=SelectedIndex=" + this.type_pre_pic.SelectedIndex);
            if (pictureBoxList.Count < 1)
                return;
            PictureBoxX p;
            Rectangle rect =outRect!=Rectangle.Empty?outRect:new Rectangle(0,0, this.pic_anim.Width, this.pic_anim.Height);
            var maxSize = outRect.Size;//BitmapHelper.MaxSize(this.pictureBoxList);
            var r_max = BitmapHelper.CalculateImageRectangle(this.pic_anim.Size, maxSize,0.01f);
           
            float rate_w_out = 1;
            float rate_h_out = 1;
            Rectangle re = Rectangle.Empty;
            if (this.ck_duibi.Checked)
            {   //对比帧-1
                //None 0
                //正常 1
                //红色 2
                //绿色 3
                //蓝色 4
                //反色 5
                if (this.mode_pre_pic.SelectedIndex == 1)//上一帧
                {
                    if (PlayOder - 1 >= 0)
                        p = this.pictureBoxList[PlayOder - 1];
                    else
                        p = this.pictureBoxList[this.pictureBoxList.Count - 1];
                }
                else if (this.mode_pre_pic.SelectedIndex == 2)//第一帧
                {

                    p = this.pictureBoxList[0];
                }
                else if (this.mode_pre_pic.SelectedIndex == 3)//最后一帧
                {
                    p = this.pictureBoxList[this.pictureBoxList.Count - 1];
                }
                else 
                {
                    p = this.pictureBoxList[0];
                }


                rate_w_out = (float)maxSize.Width / r_max.Width;
                rate_h_out = (float)maxSize.Height / r_max.Height;

                re.X = (int)(r_max.X + p.pos_in_outrect.X/ rate_w_out);
                re.Y = (int)(r_max.Y + p.pos_in_outrect.Y/ rate_h_out);
                re.Width = (int)(p.Width/ rate_w_out);
                re.Height = (int)(p.Height / rate_h_out);


                Bitmap bitmap = p.bitmap;


                if (this.type_pre_pic.SelectedIndex == 2)
                {
                    bitmap = BitmapHelper.ModifyBitmap(bitmap, (a, r, g, b) =>
                    {
                        var bs = new byte[4];
                        bs[0] = a;
                        bs[1] = 255;
                        bs[2] = 0;
                        bs[3] = 0;
                        return bs;
                    });
                }
                else if (this.type_pre_pic.SelectedIndex == 3)
                {
                    bitmap = BitmapHelper.ModifyBitmap(bitmap, (a, r, g, b) =>
                    {
                        var bs = new byte[4];
                        bs[0] = a;
                        bs[1] = 0;
                        bs[2] = 255;
                        bs[3] = 0;
                        return bs;
                    });
                }
                else if (this.type_pre_pic.SelectedIndex == 4)
                {
                    bitmap = BitmapHelper.ModifyBitmap(bitmap, (a, r, g, b) =>
                    {
                        var bs = new byte[4];
                        bs[0] = a;
                        bs[1] = 0;
                        bs[2] = 0;
                        bs[3] = 255;
                        return bs;
                    });
                }
                else if (this.type_pre_pic.SelectedIndex == 5)
                {
                    bitmap = BitmapHelper.ModifyBitmap(bitmap, (a, r, g, b) =>
                    {
                        var bs = new byte[4];
                        bs[0] = a;
                        bs[1] = (byte)(255 - r);
                        bs[2] = (byte)(255 - g);
                        bs[3] = (byte)(255 - b);
                        return bs;
                    });
                }
                else 
                {
                    bitmap = BitmapHelper.ModifyBitmap(bitmap, (a, r, g, b) =>
                    {
                        var bs = new byte[4];
                        bs[0] = a;
                        bs[1] = 255;
                        bs[2] = 0;
                        bs[3] = 0;
                        return bs;
                    });
                }
                e.Graphics.DrawImage(bitmap, re);
            }

            //return;

            p = this.pictureBoxList[PlayOder];
            rect = new Rectangle(p.pos_in_outrect, p.Size);

            rate_w_out = (float)maxSize.Width / r_max.Width;
            rate_h_out = (float)maxSize.Height / r_max.Height;

            re.X = (int)(r_max.X + p.pos_in_outrect.X / rate_w_out);
            re.Y = (int)(r_max.Y + p.pos_in_outrect.Y / rate_h_out);

            re.Width = (int)(p.Width / rate_w_out);
            re.Height = (int)(p.Height / rate_h_out);
            //Console.WriteLine($"re--- {re}   p.temp {p.simmilarPosTemp_anim}");
            e.Graphics.DrawRectangle(Pens.Black , r_max);

            e.Graphics.DrawRectangle(Pens.Brown, new Rectangle(r_max.X, r_max.Y, re.Width, re.Height));
            e.Graphics.DrawImage(p.bitmap, re);

            // 创建一个 Pen 对象，用于绘制矩形的边框
            using (Pen pen = new Pen(Color.Aqua, 5))
            {
                if (this.pic_anim.Focused)
                {
                    pen.Color = Color.FromArgb(255, 255, 0, 0);
                }
                else
                {
                    pen.Color = Color.FromArgb(255, 0, 0, 255);
                }
                // 设置虚线样式
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // 绘制矩形
                e.Graphics.DrawRectangle(pen, re);
                
            }
            if (this.ck_duijiao.Checked)
            {
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    // 设置虚线样式
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(pen, r_max.Left, r_max.Top, r_max.Right, r_max.Bottom);
                    e.Graphics.DrawLine(pen, r_max.Left, r_max.Bottom, r_max.Right, r_max.Top);
                }

            }


        }


        // 记录鼠标上一次的位置
        private Point lastMousePosition;
        // 标记是否正在拖动
        private bool isDragging = false;
        private Rectangle currentDragRect_pictureBox_debug = Rectangle.Empty;
        private Rectangle currentDragRect_area_debug = Rectangle.Empty;
        // 鼠标按下事件处理方法
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //this.similarMap = null;
            var pictureBox = (PictureBoxX)sender;
            panel_Area.Controls.SetChildIndex(pictureBox, 0);
            //if (this.ToolType == eToolType.相似工具)
            //{
            //    currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, lastMousePosition, e.Location);
            //}

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;

                if (pic_anim_Timer == null)
                {
                    PlayOder = this.pictureBoxList.IndexOf(pictureBox);
                    this.anim_icon_info.Text = $"{PlayOder} ({pictureBoxList[PlayOder].bitmap.Width},{pictureBoxList[PlayOder].bitmap.Height})";
                }
               
            }
            if (this.ToolType == eToolType.相似工具)
            {
                Cursor.Current = Cursors.Cross;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            if (this.ToolType == eToolType.魔术棒工具) 
            {
                var r = BitmapHelper.FindConnectedRegion(pictureBox, e.Location, this.tolerance);
                currentDragRect_pictureBox_debug = r;
                currentDragRect_area_debug = new Rectangle(r.X + pictureBox.Location.X, r.Y + pictureBox.Location.Y, r.Width, r.Height);
                if (currentDragRect_pictureBox_debug.Width > 0 && currentDragRect_pictureBox_debug.Height > 1)
                {
                    this.similarMap = CropImage(pictureBox.bitmap, currentDragRect_pictureBox_debug);
                    //currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                    this.SetPointSimmilar(this.similarMap, this.pictureBoxList);
                }
            }
            this.pic_anim.Invalidate();
            this.panel_Area.Invalidate();   
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
                    var r = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                    if (r.Width > 3 && r.Height > 3)
                        currentDragRect_pictureBox_debug = r;
                    currentDragRect_area_debug = new Rectangle(r.X+pictureBox.Location.X,r.Y+pictureBox.Location.Y,r.Width,r.Height);
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
            panel_Area.Invalidate();
        }

        // 鼠标释放事件处理方法
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            currentDragRect_area_debug = Rectangle.Empty;
            PictureBoxX pictureBox = (PictureBoxX)sender;
            pictureBox.Focus();
            if (e.Button == MouseButtons.Left)
            {
                if (isDragging)
                {
                    if (this.ToolType == eToolType.选择工具)
                    {
                        int targetIndex = 0;
                        int currentIndex = 0;
                        double distance = int.MaxValue;
                        Point center = Point.Empty;
                        for (int i = 0; i < pictureBoxList.Count; i++)
                        {
                            if (pictureBox == pictureBoxList[i]) 
                            {
                                currentIndex= i;
                                continue; 
                            }
                            Point p = new Point(pictureBoxList[i].Location.X + (int)(pictureBoxList[i].Width / 2), pictureBoxList[i].Location.Y + (int)(pictureBoxList[i].Height / 2));

                           // Console.WriteLine($"{p}  {e.Location} {pictureBox.Location}");
                            double dx = p.X - (pictureBox.Location.X+e.X);
                            double dy = p.Y - (pictureBox.Location.Y+e.Y);
                            double d = Math.Sqrt(dx * dx + dy * dy);
                            if (distance > d)
                            {
                                targetIndex = i;
                                distance = d;
                                center=p;
                            }
                        }

                        
                        if (Control.ModifierKeys == Keys.Alt||Math.Abs(currentIndex-targetIndex)==1)
                        {
                            PictureBoxX temp = pictureBoxList[targetIndex];
                            pictureBoxList[targetIndex] = pictureBoxList[currentIndex];
                            pictureBoxList[currentIndex] = temp;
                        }
                        else
                        {
                            targetIndex = center.X < pictureBox.Location.X+e.X ? targetIndex + 1 : targetIndex;

                            if (targetIndex < currentIndex)
                                this.pictureBoxList.RemoveAt(currentIndex);
                            this.pictureBoxList.Insert(targetIndex, pictureBox);
                            if (targetIndex > currentIndex)
                                this.pictureBoxList.RemoveAt(currentIndex);

                        }

                        this.ArrangePicType(0);
                        this.panel_Area.Invalidate();
                        this.pic_anim.Invalidate();
                       // this.panelWorkArea.Invalidate();
                    }
                    if (Control.ModifierKeys == Keys.Alt)
                    {
                        
                         if (this.ToolType == eToolType.相似工具)
                        {
                            var r = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                            if (r.Width > 3 && r.Height > 3)
                            {
                                if (currentDragRect_pictureBox_debug.Width > 0 && currentDragRect_pictureBox_debug.Height > 1)
                                {
                                    this.similarMap = CropImage(pictureBox.bitmap, currentDragRect_pictureBox_debug);
                                    //currentDragRect_pictureBox_debug = GetBitmapRectangle(pictureBox, this.lastMousePosition, e.Location);
                                    this.SetPointSimmilar(this.similarMap, this.pictureBoxList);
                                }
                            }

                        }
                        //this.MenuItemPanel.MergeSelectedItems();
                        // 这里可以添加 Ctrl 键抬起后要执行的逻辑代码，也就是判断用户此时没按 Ctrl 键了
                        Console.WriteLine("Keys.Alt 键已经按");
                    }
                }
                this.setPosTemp(false);
                isDragging = false;
                this.pic_anim.Invalidate();
                this.panel_Area.Invalidate();
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
            outRect = Rectangle.Empty;
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
            this.ReGeneratePic(this.pictureBoxList,true);
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {

        }

        int typeArrange = 2;
        bool isDuiqi = false;
        Rectangle outRect = Rectangle.Empty;

        private void ArrangePicType(int type)
        {
            this.typeArrange = type;
            int spacing = 10; // 图片之间的间距
            int currentX = 10;
            int currentY = 10;

            var maxsize = BitmapHelper.MaxSize(pictureBoxList);
            for (int i = 0; i < pictureBoxList.Count; i++)
            {
                PictureBoxX pictureBox = pictureBoxList[i];
                if (type == 0)
                {
                    pictureBox.Location = new Point(currentX, currentY);
                    currentX += maxsize.Width + spacing;

                    // 如果超出面板宽度，换行
                    if (currentX + maxsize.Width > panel_Area.Width - this.panel_anim.Width)
                    {
                        currentX = 10;
                        currentY += maxsize.Height + spacing;
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
            }
            outRect = BitmapHelper.CalculateImagePicturesOutRect(this.pictureBoxList);
        }
        void ReGeneratePicOne(PictureBoxX p,bool show)
        {
            if (p == null || this.pictureBoxList == null || this.pictureBoxList.Count == 0)
            {
                return;
            }
            if (outRect.Width == 0)
            {
                MessageBox.Show("还没有识别特征点！(用矩形工具)", "");
                return;
            }
            // 找出最大的宽度和高度
            int maxWidth = outRect.Width;
            int maxHeight = outRect.Height;
            Bitmap newBitmap = new Bitmap(maxWidth, maxHeight);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(p.bitmap, new Rectangle(p.pos_in_outrect.X, p.pos_in_outrect.Y, p.bitmap.Width, p.bitmap.Height));
            }
            if (show)
            {
                p.bitmap = newBitmap;
                p.Width = (newBitmap.Width);
                p.Height = (newBitmap.Height);
                p.simmilarPos = Point.Empty;
                p.pos_in_outrect =Point.Empty;
            }
            else
            {
                p.bitmapGenerate = newBitmap;
            }
                   

        }

        void ReGeneratePic(List<PictureBoxX> list, bool show)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }
            if (outRect.Width == 0)
            {
                MessageBox.Show("还没有识别特征点！(用矩形工具)", "");
                return;
            }
            // 调整每个图片的大小
            for (int i = 0; i < list.Count; i++)
            {
                ReGeneratePicOne(list[i],show);
            }
        }
        // 排列图组
        private void 排列图组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.typeArrange++;
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
                this.pic_anim.BackgroundImage = Properties.Resources.方格;
                this.pic_anim.BackgroundImageLayout = ImageLayout.Tile;
                this.pic_anim.Image = null;//zy_cutPicture.Properties.Resources.生成播放按钮2;
                this.anim_icon_info.Text = $"{PlayOder} ({pictureBoxList[PlayOder].bitmap.Width},{pictureBoxList[PlayOder].bitmap.Height})";

                this.panel_Area.Invalidate();
                this.pic_anim.Invalidate();
            }
        }

        private void pic_anim_Click(object sender, EventArgs e)
        {      
            this.pic_anim.Focus();
            this.pic_anim.Invalidate();
            this.panel_Area.Invalidate();

        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            if (pic_anim_Timer != null)
            {
                pic_anim_Timer.Stop();
                pic_anim_Timer = null;
                //this.pic_anim.BackgroundImage = this.pictureBoxList[0].bitmap;
                //this.pic_anim.BackgroundImageLayout = ImageLayout.Stretch;
                //this.pic_anim.Image = zy_cutPicture.Properties.Resources.生成播放按钮2;
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
                    //this.pic_anim.Image = pictureBoxList[PlayOder].bitmap;
                    this.btn_play.Text = "Stop";
                }
            }
            this.pic_anim.Focus();
        }
        private void set_pic_anim() 
        {
            if (pic_anim_Timer != null)
            {
                pic_anim_Timer.Stop();
                pic_anim_Timer = null;
                this.btn_play.Text = "Play";
            }
            this.anim_icon_info.Text = $"{PlayOder} ({pictureBoxList[PlayOder].bitmap.Width},{pictureBoxList[PlayOder].bitmap.Height})";
        }
        private void btn_pre_Click(object sender, EventArgs e)
        {
            PlayOder = (PlayOder - 1);
            if (PlayOder < 0)
                PlayOder = pictureBoxList.Count - 1;
            set_pic_anim();
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
            this.pic_anim.Focus();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {           
            PlayOder = (PlayOder + 1);
            if (PlayOder >= pictureBoxList.Count)
                PlayOder = 0;           
            set_pic_anim();
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
            this.pic_anim.Focus();
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
            this.panel_Area.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                // 当按下左箭头键时，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            if (keyData == Keys.Right)
            {
                // 当按下右箭头键时，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            if (keyData == Keys.Up)
            {
                // 当按下向上箭头键时，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            if (keyData == Keys.Down)
            {
                // 当按下向下箭头键时，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            if (keyData == Keys.Tab)
            {
                // 当按下Tab键时，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            if ((keyData & Keys.Alt) == Keys.Alt && (keyData & Keys.Tab) == Keys.Tab)
            {
                // 检测到 Alt + Tab 组合键，返回 true 表示已处理该按键，不执行默认操作
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void SequenceForm_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Ctrl + C 组合键
            if ( e.KeyCode == Keys.W)
            {
                this.btn_magic.PerformClick();               
            }
            if (e.KeyCode == Keys.M)
            {
                this.brushToolButton.PerformClick();
            }
            if (e.KeyCode == Keys.V)
            {
                this.selectToolButton.PerformClick();
            }
            if (e.KeyCode == Keys.R&&!e.Alt) 
            {
                旋转90ToolStripMenuItem_Click(null, null);
            }
            

        }
        private void Focused_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (pictureBoxList.Count < 1) 
                return;
            if (e.KeyCode == Keys.Tab)
            {
                if (!this.pic_anim.Focused)
                {
                    pic_anim_Timer_Tick(null, null);
                    panel_Area.Controls.SetChildIndex(this.pictureBoxList[PlayOder], 0);
                    this.pictureBoxList[PlayOder].Focus();
                }
                else 
                {
                    pic_anim_Timer_Tick(null, null);
                    panel_Area.Controls.SetChildIndex(this.pictureBoxList[PlayOder], 0);
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                var p = sender as PictureBoxX;
                if (p != null)
                {

                    if (p.Focused&& this.similarMap!=null)
                    {
                        p.simmilarPos = new Point(p.simmilarPos.X - 1, p.simmilarPos.Y);

                        this.setPosTemp(false);
                    }
                    //else 
                    //{
                    //    p.Location = new Point(p.Location.X - 1, p.Location.Y);
                    //}
                }
                var p2 = sender as PictureBox;
                if (p2 != null && p2.Tag != null && p2.Tag.ToString() == "pic_anim") 
                {
                    p =this.pictureBoxList[PlayOder];
                    p.simmilarPos = new Point(p.simmilarPos.X + 1, p.simmilarPos.Y);

                    this.setPosTemp(false);
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                var p = sender as PictureBoxX;
                if (p != null)
                {

                    if (p.Focused && this.similarMap != null)
                    {
                        p.simmilarPos = new Point(p.simmilarPos.X + 1, p.simmilarPos.Y);

                        this.setPosTemp(false);
                    }
                    //else 
                    //{
                    //    p.Location = new Point(p.Location.X + 1, p.Location.Y);
                    //}
                }
                var p2 = sender as PictureBox;
                if (p2 != null && p2.Tag!=null&& p2.Tag.ToString() == "pic_anim")
                {
                    p = this.pictureBoxList[PlayOder];
                    p.simmilarPos = new Point(p.simmilarPos.X - 1, p.simmilarPos.Y);

                    this.setPosTemp(false);
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                var p = sender as PictureBoxX;
                if (p != null)
                {
                    
                    if (p.Focused && this.similarMap != null)
                    {
                        p.simmilarPos = new Point(p.simmilarPos.X, p.simmilarPos.Y - 1);

                        this.setPosTemp(false);
                    }
                    //else 
                    //{
                    //    p.Location = new Point(p.Location.X, p.Location.Y - 1);
                    //}
                }
                var p2 = sender as PictureBox;
                if (p2 != null && p2.Tag != null && p2.Tag.ToString() == "pic_anim")
                {
                    p = this.pictureBoxList[PlayOder];
                    p.simmilarPos = new Point(p.simmilarPos.X, p.simmilarPos.Y + 1);

                    this.setPosTemp(false);
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                var p = sender as PictureBoxX;
                if (p != null)
                {

                    if (p.Focused && this.similarMap != null)
                    {
                        p.simmilarPos = new Point(p.simmilarPos.X, p.simmilarPos.Y + 1);

                        this.setPosTemp(false);
                    }
                    //else 
                    //{
                    //    p.Location = new Point(p.Location.X, p.Location.Y + 1);
                    //}
                }
                var p2 = sender as PictureBox;
                if (p2 != null && p2.Tag != null && p2.Tag.ToString() == "pic_anim")
                {
                    p = this.pictureBoxList[PlayOder];
               
                    p.simmilarPos = new Point(p.simmilarPos.X , p.simmilarPos.Y-1);

                    this.setPosTemp(false);
                }
            }
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate(); 

        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxList.Count < 1)
                return;

            try
            {
                this.ReGeneratePic(this.pictureBoxList,false);
                foreach (var p in pictureBoxList)
                {
                    if (File.Exists(p.FilePath))
                    {
                        // 确保文件没有被其他程序占用
                        using (FileStream fs = new FileStream(p.FilePath, FileMode.Open, FileAccess.Write))
                        {
                            if (p.bitmapGenerate != null)
                                p.bitmapGenerate.Save(fs, ImageFormat.Png);
                            else
                                p.bitmap.Save(fs, ImageFormat.Png);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"文件路径 {p.FilePath} 不存在，无法保存。");
                    }
                }
                string directory = Path.GetDirectoryName(pictureBoxList[0].FilePath);
                Process.Start("explorer.exe", directory);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件时出错: {ex.Message}");
            }
        }

        private void 另ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files|*.png|GIF file (*.gif) | *.gif";
            saveFileDialog.Title = "另存为";
            var r = saveFileDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                try
                {
                    this.ReGeneratePic(this.pictureBoxList,false);
                    string filePath = saveFileDialog.FileName;
                    string directory = Path.GetDirectoryName(filePath);
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    string fileExtension = Path.GetExtension(filePath).ToLower();
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    for (int i = 0; i < pictureBoxList.Count; i++)
                    {
                        PictureBoxX p = pictureBoxList[i];
                        string newFilePath = Path.Combine(directory, $"{name}_{i}.png");
                        if (fileExtension == ".png")
                        {
                            using (FileStream fs = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
                            {
                                if (p.bitmapGenerate != null)
                                    p.bitmapGenerate.Save(fs, ImageFormat.Png);
                                else
                                    p.bitmap.Save(fs, ImageFormat.Png);
                            }
                        }
                        else if (fileExtension == ".gif")
                        {
                            using (AnimatedGifCreator gifCreator = AnimatedGif.AnimatedGif.Create(saveFileDialog.FileName, (int)this.num_anim_interval.Value))
                            {
                                foreach (var pp in pictureBoxList)
                                {
                                    Bitmap bitmap = pp.bitmapGenerate ?? pp.bitmap;
                                    gifCreator.AddFrame(pp.bitmapGenerate, delay: (int)this.num_anim_interval.Value, quality: GifQuality.Bit8);
                                }
                            }
                        }



                    }
                    Process.Start("explorer.exe", directory);
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

        private void 旋转90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panel_Area.Controls.Count < 1)
                return;
            var p = this.panel_Area.Controls[0] as PictureBoxX;
            var bitmap = BitmapHelper.RotateBitmap90DegreesClockwise(p.bitmap);
            p.setpicX(bitmap);
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
        }

        private void 全体旋转90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panel_Area.Controls.Count < 1)
                return;
            for (int i = 0; this.panel_Area.Controls.Count > i; i++)
            {
                var p = this.panel_Area.Controls[i] as PictureBoxX;
               var bitmap = BitmapHelper.RotateBitmap90DegreesClockwise(p.bitmap);
                p.setpicX(bitmap);
            }
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item0 = this.panel_Area.Controls[0];
            this.pictureBoxList.Remove(item0 as PictureBoxX);
            this.panel_Area.Controls.RemoveAt(0);
        }

        private void 智能删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int  widthLimit = 10;
            for (int i = pictureBoxList.Count - 1; i >= 0; i--) 
            {
                PictureBoxX item = this.pictureBoxList[i];
                if (item.bitmap.Width < widthLimit) 
                {
                    pictureBoxList.RemoveAt(i);
                }
            }
            for (int i = this.panel_Area.Controls.Count - 1; i >= 0; i--)
            {
                PictureBoxX item = this.panel_Area.Controls[i] as PictureBoxX;
                if (item.bitmap.Width < widthLimit)
                {
                    this.panel_Area.Controls.RemoveAt(i);
                }
            }
        }

        private void 切图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread messageThread = new Thread(() =>
            {
                var win = new MainForm();                
                Application.Run(win);
            });

            // 配置线程
            messageThread.SetApartmentState(ApartmentState.STA);
            messageThread.IsBackground = false;
            messageThread.Start();
        }

        private void num_anim_interval_ValueChanged(object sender, EventArgs e)
        {
            if (this.pic_anim_Timer != null)
                this.pic_anim_Timer.Interval = (int)num_anim_interval.Value;
        }

        private void panel_anim_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ck_duibi_CheckedChanged(object sender, EventArgs e)
        {
            this.pic_anim.Invalidate();
        }

        private void 水平翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panel_Area.Controls.Count < 1)
                return;
            var p = this.panel_Area.Controls[0] as PictureBoxX;
            var bitmap = BitmapHelper.FlipBitmapHorizontally(p.bitmap);
            p.setpicX(bitmap);
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
        }

        private void 垂直翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panel_Area.Controls.Count < 1)
                return;
            var p = this.panel_Area.Controls[0] as PictureBoxX;
            var bitmap = BitmapHelper.FlipBitmapVertically(p.bitmap);
            p.setpicX(bitmap);
            this.panel_Area.Invalidate();
            this.pic_anim.Invalidate();
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

            try
            {
                using (Bitmap tempBitmap = new Bitmap(filePath))
                {                 
                    int width = tempBitmap.Width;
                    int height = tempBitmap.Height;                 

                    this.bitmap = new Bitmap(tempBitmap, width, height);
                    this.Size = new Size(this.bitmap.Width, this.bitmap.Height);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载图片时出错: {ex.Message}");
                // 可以根据需要进行其他处理，如显示默认图片
            }

            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {this.bitmap?.Size} ");
        }
        public  void setpicX(string filePath)
        {           
            this.FilePath = filePath;
            this.bitmap = new Bitmap(filePath);
            this.Size = new Size(bitmap.Width, bitmap.Height);
            this.simmilarPos = Point.Empty;
            this.pos_in_outrect = Point.Empty;
            IsSelected = false;
            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {bitmap.Size} ");
        }
        public void setpicX(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.Size = new Size(bitmap.Width, bitmap.Height);
            this.simmilarPos = Point.Empty;
            this.pos_in_outrect = Point.Empty;
            IsSelected = false;
            Console.WriteLine($"文件路径: {this.FilePath}   控件大小: {this.Size}    图片大小: {bitmap.Size} ");
        }

        // 图片对象
        public Bitmap bitmap;
        public Bitmap bitmapGenerate;
        // 动画序列索引
        public int AnimationSequenceIndex;
        // 是否被选中
        public bool IsSelected = false;
        // 文件路径
        public string FilePath;
        public Point simmilarPos = Point.Empty;
        public Point pos_in_outrect =Point.Empty; 
        public int index_Anim = 0;
    }
    // 添加新的类成员用于缓存图像数据
    class BitmapDataCache
    {
        public byte[] Pixels { get; set; }
        public int Stride { get; set; }
        public int BytesPerPixel { get; set; }
    }
}