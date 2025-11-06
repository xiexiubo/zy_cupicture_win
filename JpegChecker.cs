using System;
using System.IO;
namespace zy_cutPicture
{
    public static class JpegChecker
    {
        /// <summary>
        /// ture 是有效果正常的文件  false 不存在或者是有损文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsLossyJpeg(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) { return false; }
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // 检查JPEG文件头 (FF D8 FF)
                    byte[] header = br.ReadBytes(3);
                    if (header[0] != 0xFF || header[1] != 0xD8 || header[2] != 0xFF)
                    {
                        return false; // 不是有效的JPEG文件
                    }

                    // JPEG本身就是有损格式，但可以检查是否包含无损标记
                    fs.Seek(0, SeekOrigin.Begin);
                    return !IsLosslessJpeg(br);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查文件时出错: {ex.Message}");
                return false;
            }
        }

        private static bool IsLosslessJpeg(BinaryReader br)
        {
            // 检查是否包含无损JPEG标记 (SOF3 - 起始帧，无损)
            try
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);

                while (br.BaseStream.Position < br.BaseStream.Length - 1)
                {
                    byte marker = br.ReadByte();
                    if (marker == 0xFF)
                    {
                        byte nextByte = br.ReadByte();

                        // SOF3 (0xFFC3) 表示无损JPEG
                        if (nextByte == 0xC3)
                        {
                            return true;
                        }

                        // SOF0 (0xFFC0) - 基线JPEG (有损)
                        // SOF1 (0xFFC1) - 扩展顺序JPEG (有损)
                        // SOF2 (0xFFC2) - 渐进JPEG (有损)
                        if (nextByte >= 0xC0 && nextByte <= 0xC2)
                        {
                            return false;
                        }

                        // 跳过标记段长度
                        if (nextByte != 0xD8 && nextByte != 0xD9 && nextByte != 0x01)
                        {
                            ushort length = ReadBigEndianUInt16(br);
                            br.BaseStream.Seek(length - 2, SeekOrigin.Current);
                        }
                    }
                }
            }
            catch
            {
                // 读取错误，默认为有损
            }

            return false;
        }

        private static ushort ReadBigEndianUInt16(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(2);
            return (ushort)((bytes[0] << 8) | bytes[1]);
        }
    }
}
