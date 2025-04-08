using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace zy_cutPicture
{ 
    public class ColorDifference
    {
        // RGB 转 XYZ 颜色空间
        private static double[] RGBToXYZ(double[] rgb)
        {
            double r = rgb[0] / 255.0;
            double g = rgb[1] / 255.0;
            double b = rgb[2] / 255.0;

            r = r > 0.04045 ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = g > 0.04045 ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = b > 0.04045 ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            double X = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double Z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new double[] { X, Y, Z };
        }

        // XYZ 转 CIELAB 颜色空间
        private static double[] XYZToLab(double[] xyz)
        {
            double Xn = 0.95047;
            double Yn = 1.00000;
            double Zn = 1.08883;

            double x = xyz[0] / Xn;
            double y = xyz[1] / Yn;
            double z = xyz[2] / Zn;

            x = x > 0.008856 ? Math.Pow(x, 1.0 / 3.0) : 7.787 * x + 16.0 / 116.0;
            y = y > 0.008856 ? Math.Pow(y, 1.0 / 3.0) : 7.787 * y + 16.0 / 116.0;
            z = z > 0.008856 ? Math.Pow(z, 1.0 / 3.0) : 7.787 * z + 16.0 / 116.0;

            double L = 116 * y - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new double[] { L, a, b };
        }

        // RGB 转 CIELAB 颜色空间
        private static double[] RGBToLab(double[] rgb)
        {
            double[] xyz = RGBToXYZ(rgb);
            return XYZToLab(xyz);
        }

        // CIE76 色差公式
        public static double CIE76(double[] rgb1, double[] rgb2)
        {
            double[] lab1 = RGBToLab(rgb1);
            double[] lab2 = RGBToLab(rgb2);

            double deltaL = lab1[0] - lab2[0];
            double deltaA = lab1[1] - lab2[1];
            double deltaB = lab1[2] - lab2[2];

            return Math.Sqrt(deltaL * deltaL + deltaA * deltaA + deltaB * deltaB);
        }

        // CIEDE2000 色差公式
        public static double CIEDE2000(double[] rgb1, double[] rgb2)
        {
            double[] lab1 = RGBToLab(rgb1);
            double[] lab2 = RGBToLab(rgb2);

            double L1 = lab1[0];
            double a1 = lab1[1];
            double b1 = lab1[2];
            double L2 = lab2[0];
            double a2 = lab2[1];
            double b2 = lab2[2];

            double C1 = Math.Sqrt(a1 * a1 + b1 * b1);
            double C2 = Math.Sqrt(a2 * a2 + b2 * b2);
            double CabBar = (C1 + C2) / 2;

            double G = 0.5 * (1 - Math.Sqrt(Math.Pow(CabBar, 7) / (Math.Pow(CabBar, 7) + Math.Pow(25, 7))));

            double a1Prime = (1 + G) * a1;
            double a2Prime = (1 + G) * a2;

            double C1Prime = Math.Sqrt(a1Prime * a1Prime + b1 * b1);
            double C2Prime = Math.Sqrt(a2Prime * a2Prime + b2 * b2);

            double h1Prime = Math.Atan2(b1, a1Prime);
            if (h1Prime < 0) h1Prime += 2 * Math.PI;
            double h2Prime = Math.Atan2(b2, a2Prime);
            if (h2Prime < 0) h2Prime += 2 * Math.PI;

            double deltaLPrime = L2 - L1;
            double deltaCPrime = C2Prime - C1Prime;

            double deltahPrime;
            if (C1Prime * C2Prime == 0)
            {
                deltahPrime = 0;
            }
            else if (Math.Abs(h2Prime - h1Prime) <= Math.PI)
            {
                deltahPrime = h2Prime - h1Prime;
            }
            else if (h2Prime - h1Prime > Math.PI)
            {
                deltahPrime = h2Prime - h1Prime - 2 * Math.PI;
            }
            else
            {
                deltahPrime = h2Prime - h1Prime + 2 * Math.PI;
            }

            double deltaHPrime = 2 * Math.Sqrt(C1Prime * C2Prime) * Math.Sin(deltahPrime / 2);

            double LPrimeBar = (L1 + L2) / 2;
            double CPrimeBar = (C1Prime + C2Prime) / 2;

            double hPrimeBar;
            if (C1Prime * C2Prime == 0)
            {
                hPrimeBar = h1Prime + h2Prime;
            }
            else if (Math.Abs(h1Prime - h2Prime) <= Math.PI)
            {
                hPrimeBar = (h1Prime + h2Prime) / 2;
            }
            else if (h1Prime + h2Prime < 2 * Math.PI)
            {
                hPrimeBar = (h1Prime + h2Prime + 2 * Math.PI) / 2;
            }
            else
            {
                hPrimeBar = (h1Prime + h2Prime - 2 * Math.PI) / 2;
            }

            double T = 1 - 0.17 * Math.Cos(hPrimeBar - Math.PI / 6) + 0.24 * Math.Cos(2 * hPrimeBar) + 0.32 * Math.Cos(3 * hPrimeBar + Math.PI / 30) - 0.20 * Math.Cos(4 * hPrimeBar - 63 * Math.PI / 180);

            double deltaTheta = 30 * Math.Exp(-Math.Pow((hPrimeBar * 180 / Math.PI - 275) / 25, 2));

            double RC = 2 * Math.Sqrt(Math.Pow(CPrimeBar, 7) / (Math.Pow(CPrimeBar, 7) + Math.Pow(25, 7)));

            double SL = 1 + (0.015 * Math.Pow(LPrimeBar - 50, 2)) / Math.Sqrt(20 + Math.Pow(LPrimeBar - 50, 2));
            double SC = 1 + 0.045 * CPrimeBar;
            double SH = 1 + 0.015 * CPrimeBar * T;

            double RT = -Math.Sin(2 * deltaTheta * Math.PI / 180) * RC;

            return Math.Sqrt(
                Math.Pow(deltaLPrime / SL, 2) +
                Math.Pow(deltaCPrime / SC, 2) +
                Math.Pow(deltaHPrime / SH, 2) +
                RT * (deltaCPrime / SC) * (deltaHPrime / SH)
            );
        }
    }

    public class BitmapHelper
    {
        public static Bitmap RotateBitmap90DegreesClockwise(Bitmap source)
        {
            Bitmap rotatedBitmap = new Bitmap(source.Height, source.Width);
            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.TranslateTransform(rotatedBitmap.Width / 2, rotatedBitmap.Height / 2);
                g.RotateTransform(-90);
                g.TranslateTransform(-source.Width / 2, -source.Height / 2);
                g.DrawImage(source, 0, 0);
            }
            return rotatedBitmap;
        }
        // 水平翻转
        public static Bitmap FlipBitmapHorizontally(Bitmap source)
        {
            Bitmap flippedBitmap = new Bitmap(source);
            flippedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return flippedBitmap;
        }

        // 垂直翻转
        public static Bitmap FlipBitmapVertically(Bitmap source)
        {
            Bitmap flippedBitmap = new Bitmap(source);
            flippedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return flippedBitmap;
        }

        public static Bitmap ModifyBitmap(Bitmap source,Func<byte, byte, byte, byte, byte[]> action)
        {
            // Use LockBits for much faster pixel manipulation
            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);

            var sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            var resultData = result.LockBits(
                new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                int bytesPerPixel = 4; // For 32bppArgb
                int byteCount = sourceData.Stride * sourceData.Height;
                byte[] pixels = new byte[byteCount];

                // Copy source data
                Marshal.Copy(sourceData.Scan0, pixels, 0, byteCount);

                // Process pixels
                for (int i = 0; i < byteCount; i += bytesPerPixel)
                {
                    // Pixel format is BGRA
                    byte b = pixels[i];
                    byte g = pixels[i + 1];
                    byte r = pixels[i + 2];
                    byte a = pixels[i + 3];


                    if (action != null)
                    {
                        var bytes = action(a, r, g, b);
                        pixels[i] = bytes[3];     // B
                        pixels[i + 1] = bytes[2]; // G
                        pixels[i + 2] = bytes[1]; // R
                        pixels[i + 3] = bytes[0]; //A
                    }
                    else if (a > 0) // If not transparent
                    {
                        // Set to red while preserving original alpha
                        pixels[i] = 0;     // B
                        pixels[i + 1] = 0; // G
                        pixels[i + 2] = 255; // R
                        // Alpha remains the same
                    }
                }

                // Copy back to result
                Marshal.Copy(pixels, 0, resultData.Scan0, byteCount);
            }
            finally
            {
                source.UnlockBits(sourceData);
                result.UnlockBits(resultData);
            }

            return result;
        }

        public static Rectangle CalculateImageRectangle(Size sizePanel, Size sizeContent,float space = 0.1f)
        {
            // 计算可用区域，离边缘至少10%空间
            int marginX = (int)(sizePanel.Width * space);
            int marginY = (int)(sizePanel.Height * space);
            int availableWidth = sizePanel.Width - 2 * marginX;
            int availableHeight = sizePanel.Height - 2 * marginY;

            // 计算保持高宽比的图像大小
            float aspectRatio = (float)sizeContent.Width / sizeContent.Height;
            int displayWidth, displayHeight;
            if (availableWidth / aspectRatio <= availableHeight)
            {
                displayWidth = availableWidth;
                displayHeight = (int)(availableWidth / aspectRatio);
            }
            else
            {
                displayHeight = availableHeight;
                displayWidth = (int)(availableHeight * aspectRatio);
            }

            // 计算图像的绘制位置，使其居中
            int x = marginX + (availableWidth - displayWidth) / 2;
            int y = marginY + (availableHeight - displayHeight) / 2;

            return new Rectangle(x, y, displayWidth, displayHeight);
        }

        public static Rectangle Union(List<PictureBox> list)
        {
            if (list == null || list.Count == 0)
            {
                return Rectangle.Empty;
            }

            Rectangle result = list[0].Bounds;
            for (int i = 1; i < list.Count; i++)
            {
                result = Rectangle.Union(result, list[i].Bounds);
            }
            return result;
        }
        public static Size MaxSize(List<PictureBoxX> list)
        {
            if (list == null || list.Count == 0)
            {
                return Size.Empty;
            }

            Size result = list[0].Size;
            for (int i = 1; i < list.Count; i++)
            {
                result.Width =Math.Max(result.Width, list[i].Width);
                result.Height =Math.Max(result.Height, list[i].Height);
            }
            return result;
        }

        public static Rectangle CalculateImagePicturesOutRect(List<PictureBoxX> pictureBoxList)
        {          

            // 用于存储每个图片框的实际矩形
            List<Rectangle> rectangles = new List<Rectangle>();
            
            // 遍历每个图片框
            for (int i = 0; i < pictureBoxList.Count; i++)
            {
                PictureBox pictureBox = pictureBoxList[i];
                Point p = pictureBoxList[i].simmilarPos;

                // 计算图片框在重叠点对齐后的实际位置
                Point actualLocation = new Point(0 - p.X, 0 - p.Y);
                // 创建图片框的实际矩形
                Rectangle rect = new Rectangle(actualLocation, pictureBox.Size);
                rectangles.Add(rect);                
            }

            // 初始化合并矩形
            Rectangle unionRect = Rectangle.Empty;
            // 合并所有矩形
            foreach (Rectangle rect in rectangles)
            {
                unionRect = Rectangle.Union(unionRect, rect);
            }
            for (int i = 0; i < pictureBoxList.Count; i++)
            {
                PictureBoxX p = pictureBoxList[i];
                Point acPos =new Point( - p.simmilarPos.X,  - p.simmilarPos.Y);
                // 存储图片框相对于合并矩形的位置
                pictureBoxList[i].pos_in_outrect = new Point(acPos.X - unionRect.X, acPos.Y - unionRect.Y);
            }

            return unionRect;
        }

        /// <summary>
        /// 查找连续相似点
        /// </summary>       
        public static Rectangle FindConnectedRegion(PictureBoxX pictureBox, Point clickPoint, int tolerance)
        {
            // 获取 PictureBox 中的图像
            Bitmap bitmap = pictureBox.bitmap;
            // 获取点击点的颜色
            Color targetColor = bitmap.GetPixel(clickPoint.X, clickPoint.Y);
            // 用于标记哪些点已被访问
            bool[,] visited = new bool[bitmap.Width, bitmap.Height];
            // 用于存储待处理的点
            System.Collections.Generic.Queue<Point> queue = new System.Collections.Generic.Queue<Point>();
            // 初始化矩形的边界
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

            // 将点击点加入队列并标记为已访问
            queue.Enqueue(clickPoint);
            visited[clickPoint.X, clickPoint.Y] = true;

            // 开始广度优先搜索
            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                // 更新矩形的边界
                minX = Math.Min(minX, current.X);
                maxX = Math.Max(maxX, current.X);
                minY = Math.Min(minY, current.Y);
                maxY = Math.Max(maxY, current.Y);

                // 定义周围点的偏移量
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };

                // 遍历周围的点
                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    // 检查新点是否在图像范围内且未被访问
                    if (newX >= 0 && newX < bitmap.Width && newY >= 0 && newY < bitmap.Height && !visited[newX, newY])
                    {
                        // 获取新点的颜色
                        Color newColor = bitmap.GetPixel(newX, newY);
                        // 计算颜色差异
                        int diff = Math.Abs(newColor.R - targetColor.R) + Math.Abs(newColor.G - targetColor.G) + Math.Abs(newColor.B - targetColor.B);

                        // 如果颜色差异小于容差，则将该点加入队列并标记为已访问
                        if (diff <= tolerance)
                        {
                            queue.Enqueue(new Point(newX, newY));
                            visited[newX, newY] = true;
                        }
                    }
                }
            }

            // 创建并返回矩形
            return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
    }
}
