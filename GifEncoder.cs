using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

namespace zy_cutPicture
{
    public class GifEncoder : IDisposable
    {
        private readonly Stream _outputStream;
        private bool _firstFrame = true;
        private bool _disposed = false;
        private Size _frameSize;
        private ColorPalette _globalPalette;

        public GifEncoder(Stream outputStream)
        {
            _outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
        }

        public void AddFrame(Bitmap frame, int delay, bool quantize = true)
        {
            if (_disposed) throw new ObjectDisposedException("GifEncoder");
            if (frame == null) throw new ArgumentNullException(nameof(frame));
            if (delay <= 0) throw new ArgumentOutOfRangeException(nameof(delay));

            // 如果是第一帧，初始化尺寸和调色板
            if (_firstFrame)
            {
                _frameSize = frame.Size;
                _globalPalette = GetOptimizedPalette(frame, quantize);
                WriteHeader(frame);
                _firstFrame = false;
            }

            // 确保所有帧尺寸一致
            if (frame.Size != _frameSize)
            {
                throw new ArgumentException("所有帧的尺寸必须相同");
            }

            // 转换为8位索引图像
            using (var indexedFrame = ConvertToIndexed(frame, quantize))
            {
                WriteGraphicControlExtension(delay);
                WriteImageDescriptor(indexedFrame);
                WriteImageData(indexedFrame);
            }
        }

        private ColorPalette GetOptimizedPalette(Bitmap frame, bool quantize)
        {
            if (!quantize)
            {
                // 使用系统默认的256色调色板
                using (var temp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
                {
                    return temp.Palette;
                }
            }

            // 简单颜色量化（实际项目应该使用更好的量化算法）
            var palette = new List<Color>();
            var colorMap = new Dictionary<Color, bool>();

            for (int y = 0; y < frame.Height; y++)
            {
                for (int x = 0; x < frame.Width; x++)
                {
                    var color = frame.GetPixel(x, y);
                    if (!colorMap.ContainsKey(color))
                    {
                        colorMap[color] = true;
                        palette.Add(color);
                        if (palette.Count >= 256) break;
                    }
                }
                if (palette.Count >= 256) break;
            }

            // 创建调色板
            using (var temp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                var result = temp.Palette;
                for (int i = 0; i < palette.Count && i < 256; i++)
                {
                    result.Entries[i] = palette[i];
                }
                return result;
            }
        }

        private Bitmap ConvertToIndexed(Bitmap original, bool quantize)
        {
            // 如果已经是8位索引图像且不需要重新量化
            if (original.PixelFormat == PixelFormat.Format8bppIndexed && !quantize)
            {
                return new Bitmap(original);
            }

            var indexed = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);

            // 设置调色板
            var palette = indexed.Palette;
            for (int i = 0; i < _globalPalette.Entries.Length; i++)
            {
                palette.Entries[i] = _globalPalette.Entries[i];
            }
            indexed.Palette = palette;

            // 使用锁定位图数据来提高性能
            var sourceData = original.LockBits(
                new Rectangle(0, 0, original.Width, original.Height),
                ImageLockMode.ReadOnly,
                original.PixelFormat);

            var targetData = indexed.LockBits(
                new Rectangle(0, 0, indexed.Width, indexed.Height),
                ImageLockMode.WriteOnly,
                indexed.PixelFormat);

            try
            {
                // 简单的颜色匹配（实际项目应该使用更好的算法）
                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        var color = GetPixel(sourceData, x, y, original.PixelFormat);
                        int index = FindClosestColorIndex(color, palette);
                        SetPixelIndex(targetData, x, y, (byte)index);
                    }
                }
            }
            finally
            {
                original.UnlockBits(sourceData);
                indexed.UnlockBits(targetData);
            }

            return indexed;
        }

        private Color GetPixel(BitmapData data, int x, int y, PixelFormat format)
        {
            IntPtr ptr = data.Scan0 + y * data.Stride + x * (Image.GetPixelFormatSize(format) / 8);

            if (format == PixelFormat.Format32bppArgb || format == PixelFormat.Format32bppRgb)
            {
                byte b = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 0);
                byte g = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 1);
                byte r = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 2);
                byte a = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 3);
                return Color.FromArgb(a, r, g, b);
            }
            else if (format == PixelFormat.Format24bppRgb)
            {
                byte b = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 0);
                byte g = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 1);
                byte r = System.Runtime.InteropServices.Marshal.ReadByte(ptr, 2);
                return Color.FromArgb(r, g, b);
            }

            throw new NotSupportedException("不支持的像素格式");
        }

        private void SetPixelIndex(BitmapData data, int x, int y, byte index)
        {
            IntPtr ptr = data.Scan0 + y * data.Stride + x;
            System.Runtime.InteropServices.Marshal.WriteByte(ptr, index);
        }

        private int FindClosestColorIndex(Color color, ColorPalette palette)
        {
            int minDistance = int.MaxValue;
            int bestIndex = 0;

            for (int i = 0; i < palette.Entries.Length; i++)
            {
                int distance = ColorDistance(color, palette.Entries[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestIndex = i;
                    if (minDistance == 0) break;
                }
            }

            return bestIndex;
        }

        private int ColorDistance(Color a, Color b)
        {
            int dr = a.R - b.R;
            int dg = a.G - b.G;
            int db = a.B - b.B;
            return dr * dr + dg * dg + db * db;
        }

        private void WriteHeader(Bitmap frame)
        {
            // GIF签名 (6 bytes)
            _outputStream.Write(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, 0, 6);

            // 逻辑屏幕描述符 (7 bytes)
            _outputStream.WriteByte((byte)(frame.Width & 0xFF));
            _outputStream.WriteByte((byte)((frame.Width >> 8) & 0xFF));
            _outputStream.WriteByte((byte)(frame.Height & 0xFF));
            _outputStream.WriteByte((byte)((frame.Height >> 8) & 0xFF));

            // 全局颜色表标志 + 颜色分辨率 + 排序标志 + 全局颜色表大小
            _outputStream.WriteByte(0xF7); // 0xF7 = 1111 0111 (全局调色板, 256色)

            _outputStream.WriteByte(0); // 背景色索引
            _outputStream.WriteByte(0); // 像素宽高比（通常为0）

            // 写入全局调色板
            for (int i = 0; i < 256; i++)
            {
                if (i < _globalPalette.Entries.Length)
                {
                    _outputStream.WriteByte(_globalPalette.Entries[i].R);
                    _outputStream.WriteByte(_globalPalette.Entries[i].G);
                    _outputStream.WriteByte(_globalPalette.Entries[i].B);
                }
                else
                {
                    _outputStream.WriteByte(0);
                    _outputStream.WriteByte(0);
                    _outputStream.WriteByte(0);
                }
            }
        }

        private void WriteGraphicControlExtension(int delay)
        {
            // 扩展引入 (2 bytes)
            _outputStream.WriteByte(0x21); // 扩展块标识
            _outputStream.WriteByte(0xF9); // 图形控制扩展

            // 块大小 (1 byte)
            _outputStream.WriteByte(0x04);

            // 处置方法 + 用户输入标志 + 透明色标志 (1 byte)
            _outputStream.WriteByte(0x00); // 无透明色

            // 延迟时间 (2 bytes) - 单位是1/100秒
            _outputStream.WriteByte((byte)(delay & 0xFF));
            _outputStream.WriteByte((byte)((delay >> 8) & 0xFF));

            // 透明色索引 (1 byte)
            _outputStream.WriteByte(0x00);

            // 块终结符 (1 byte)
            _outputStream.WriteByte(0x00);
        }

        private void WriteImageDescriptor(Bitmap frame)
        {
            // 图像描述符标识 (1 byte)
            _outputStream.WriteByte(0x2C);

            // 图像左位置 (2 bytes)
            _outputStream.WriteByte(0x00);
            _outputStream.WriteByte(0x00);

            // 图像上位置 (2 bytes)
            _outputStream.WriteByte(0x00);
            _outputStream.WriteByte(0x00);

            // 图像宽度 (2 bytes)
            _outputStream.WriteByte((byte)(frame.Width & 0xFF));
            _outputStream.WriteByte((byte)((frame.Width >> 8) & 0xFF));

            // 图像高度 (2 bytes)
            _outputStream.WriteByte((byte)(frame.Height & 0xFF));
            _outputStream.WriteByte((byte)((frame.Height >> 8) & 0xFF));

            // 局部颜色表标志 + 交织标志 + 排序标志 + 保留 + 局部颜色表大小 (1 byte)
            _outputStream.WriteByte(0x00); // 不使用局部颜色表
        }

        private void WriteImageData(Bitmap frame)
        {
            // 获取图像数据
            var data = frame.LockBits(
                new Rectangle(0, 0, frame.Width, frame.Height),
                ImageLockMode.ReadOnly,
                frame.PixelFormat);

            try
            {
                // LZW最小代码大小 (1 byte)
                _outputStream.WriteByte(0x08); // 通常为8

                // 使用简单的LZW编码（实际项目应该使用更完整的实现）
                var compressed = SimpleLZWCompress(data);

                // 写入压缩数据
                foreach (var block in SplitIntoBlocks(compressed))
                {
                    _outputStream.WriteByte((byte)block.Length);
                    _outputStream.Write(block, 0, block.Length);
                }

                // 块终结符 (1 byte)
                _outputStream.WriteByte(0x00);
            }
            finally
            {
                frame.UnlockBits(data);
            }
        }

        private byte[] SimpleLZWCompress(BitmapData data)
        {
            // 这是简化的LZW压缩实现，实际项目应该使用更完整的算法
            var result = new List<byte>();

            // 这里只是示例，实际上应该实现真正的LZW算法
            for (int y = 0; y < data.Height; y++)
            {
                IntPtr ptr = data.Scan0 + y * data.Stride;
                for (int x = 0; x < data.Width; x++)
                {
                    byte index = System.Runtime.InteropServices.Marshal.ReadByte(ptr + x);
                    result.Add(index);
                }
            }

            return result.ToArray();
        }

        private IEnumerable<byte[]> SplitIntoBlocks(byte[] data)
        {
            const int blockSize = 255;
            for (int i = 0; i < data.Length; i += blockSize)
            {
                int length = Math.Min(blockSize, data.Length - i);
                byte[] block = new byte[length];
                Array.Copy(data, i, block, 0, length);
                yield return block;
            }
        }

        public void Finish()
        {
            if (!_disposed)
            {
                _outputStream.WriteByte(0x3B); // GIF文件结束符
                _outputStream.Flush();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Finish();
                }
                _disposed = true;
            }
        }
    }
}
