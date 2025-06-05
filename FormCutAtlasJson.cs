using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace zy_cutPicture
{
    public partial class FormCutAtlasJson : Form
    {
        public static FormCutAtlasJson Instance;
        public FormCutAtlasJson()
        {
            InitializeComponent();
            Instance=this;
        }
               

        private void UpdateFileList()
        {
            fileListBox.Items.Clear();
            foreach (var file in currentFiles)
            {
                fileListBox.Items.Add(Path.GetFileName(file));
            }
        }

     

        private void DisplayFileContent(string filePath)
        {
            // 隐藏所有预览控件
            imagePreview.Visible = false;
            textPreview.Visible = false;

            try
            {
                string extension = Path.GetExtension(filePath).ToLowerInvariant();

                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                        DisplayImage(filePath);
                        break;
                    case ".json":
                        DisplayJson(filePath);
                        break;
                    default:
                        DisplayText(filePath);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法显示文件内容: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayImage(string filePath)
        {
            try
            {
                using (var tempImage = Image.FromFile(filePath))
                {
                    imagePreview.Image = new Bitmap(tempImage);
                }
                imagePreview.Visible = true;
            }
            catch (Exception ex)
            {
                textPreview.Text = $"无法加载图片: {ex.Message}";
                textPreview.Visible = true;
            }
        }

        private void DisplayJson(string filePath)
        {
            try
            {
                string jsonText = File.ReadAllText(filePath, Encoding.UTF8);

                // 美化JSON格式
                try
                {
                    object jsonObj = JsonConvert.DeserializeObject(jsonText);
                    string formattedJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    textPreview.Text = formattedJson;
                }
                catch
                {
                    // 如果JSON格式不正确，显示原始文本
                    textPreview.Text = jsonText;
                }

                textPreview.Visible = true;
            }
            catch (Exception ex)
            {
                textPreview.Text = $"无法读取JSON文件: {ex.Message}";
                textPreview.Visible = true;
            }
        }

        private void DisplayText(string filePath)
        {
            try
            {
                textPreview.Text = File.ReadAllText(filePath, Encoding.UTF8);
                textPreview.Visible = true;
            }
            catch (Exception ex)
            {
                textPreview.Text = $"无法读取文件: {ex.Message}";
                textPreview.Visible = true;
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < currentFiles.Length; i++)
            {
                var filePath = currentFiles[i];
                if (Path.GetExtension(filePath) != ".json")
                    continue;

                string jsonText = File.ReadAllText(filePath, Encoding.UTF8);

                // 美化JSON格式
                try
                {
                    object jsonObj = JsonConvert.DeserializeObject(jsonText);
                    string formattedJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    textPreview.Text = formattedJson;
                }
                catch (Exception ex)
                {
                   
                }
                Console.WriteLine(currentFiles[i], Path.GetExtension(filePath));
            }


        }

        private void openFilesButton_Click_1(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "所有支持的文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.json|图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp|JSON文件|*.json|所有文件|*.*";
                openFileDialog.Title = "选择文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFiles = openFileDialog.FileNames;
                    UpdateFileList();
                }
            }
        }

        private void fileListBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (fileListBox.SelectedIndex >= 0 && fileListBox.SelectedIndex < currentFiles.Length)
            {
                string filePath = currentFiles[fileListBox.SelectedIndex];
                DisplayFileContent(filePath);
            }
        }
        /// <summary>
        /// 转换为XML
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPreview.Text))
                {
                    MessageBox.Show("没有可转换的JSON内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 尝试解析JSON
                JToken jsonToken = JToken.Parse(textPreview.Text);

                // 转换为XML
                var xmlElement = JsonConvert.DeserializeXNode(jsonToken.ToString(), "Root");
                string xmlString = xmlElement.ToString(SaveOptions.None);

                // 显示转换结果
                textPreview.Text = xmlString;
                MessageBox.Show("转换完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class AllManifest_Value
        { 
            public string v { get; set; }// 版本号或哈希值
            public int s { get; set; }// 大小（字节）
        }  
        
        //配置图片资源下载
        static async void DoneRes_AllManifest(string directory)
        {
            try
            {
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/allmanifest1.24591.2.json?v=20250530185809?qufu_version=20";

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    // 反序列化为配置对象
                    Dictionary<string, AllManifest_Value> config = JsonConvert.DeserializeObject<Dictionary<string, AllManifest_Value>>(json);
                    Console.WriteLine($"文件num: {config.Keys.Count}");
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    foreach (var kvp in config)
                    {
                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/resource/{kvp.Value.v.Substring(0, 2)}/{kvp.Value.v}_{kvp.Value.s}{Path.GetExtension(kvp.Key)}";
                        string filePath = Path.Combine(directory, kvp.Key);
                        Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        await DownloadFileAsync(subUrl, filePath);
                        downCount++;
                    }
                    Console.WriteLine($"文件数： {config.Keys.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }
        public class cfgItem_Value
        {


            [JsonProperty("19")]
            public string Prop19 { get; set; }
            

        }
        // 定义与JSON结构对应的类
        class ItemRoot
        {
            [JsonProperty("item")]
            public Dictionary<string, cfgItem_Value> Items { get; set; }
        }
        static async void DoneRes_Item(string directory)
        {
            try
            {
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/1config1.24591.2.json?v=20250530185809";

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    ItemRoot config = JsonConvert.DeserializeObject<ItemRoot>(json);
                    Console.WriteLine($"文件num: {config.Items.Count}");
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    foreach (var v in config.Items)
                    {
                        // //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/tool/334.png?ver=1.0.1
                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/tool/{v.Value.Prop19}.png";
                        string filePath = Path.Combine(directory, $"resource/icon/tool/{v.Value.Prop19}.png");
                        Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        await DownloadFileAsync(subUrl, filePath);
                        downCount++;

                    }
                    Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }


        public class ModelInfoWrapper
        {
            [JsonProperty("modelInfo")]
            public Dictionary<string, Dictionary<string, ModelDetails>> ModelInfo { get; set; }
        }

        public class ModelDetails
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("action")]
            public long Action { get; set; }

            [JsonProperty("intval")]
            public long Intval { get; set; }

            [JsonProperty("totaldir")]
            public long TotalDir { get; set; }

            [JsonProperty("standx")]
            public long StandX { get; set; }

            [JsonProperty("standy")]
            public long StandY { get; set; }
        }
        static async void DoneRes_Model(string directory)
        {
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.24591.2.json?v=20250530185809";//modelinfo

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    ModelInfoWrapper config = JsonConvert.DeserializeObject<ModelInfoWrapper>(json);
                    Console.WriteLine($"文件num: {config.ModelInfo.Count}");
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    foreach (var v in config.ModelInfo)
                    {
                        foreach (var m in v.Value)
                        {
                            ////https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/121014.png?ver=1.0.1
                            // //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/model/2101/2101043.png?ver=1.0.1
                            //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                            //四方向
                            for (int i = 0; i < 5; i++)
                            {
                                string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}";
                                string filePath = Path.Combine(directory, $"resource/model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}");
                                Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                                await DownloadFileAsync(subUrl+".png", filePath + ".png");
                                await DownloadFileAsync(subUrl + ".json", filePath + ".json");
                            }
                            
                            downCount++;

                        }
                    }
                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }
        public class VersionInfo
        {
            [JsonProperty("version")]
            public int Version { get; set; }

            [JsonExtensionData]
            public Dictionary<string, JToken> AdditionalData { get; set; }
        }
        /// <summary>
        /// 版本表资源下载
        /// </summary>
        /// <param name="directory"></param>
        static async void DoneRes_Resversion(string directory)
        {
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809";//modelinfo

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    VersionInfo config = JsonConvert.DeserializeObject<VersionInfo>(json);
                    Console.WriteLine($"文件num: {config.AdditionalData.Count}");
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;

                    foreach (var m in config.AdditionalData)
                    {
                    ////https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/121014.png?ver=1.0.1
                    //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                    https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/124013.png?ver=1.0.1
                        if (!m.Key.Contains(".")) continue;

                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/{m.Key}";
                        string filePath = Path.Combine(directory, $"resource/{m.Key}");
                        Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        //await 
                            DownloadFileAsync(subUrl, filePath);

                        downCount++;
                    }

                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }

        // 递归处理目录及其子目录
        static async void ProcessDirectory(string directoryPath)
        {
            // 检查目录是否存在
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"目录不存在: {directoryPath}");
                return;
            }

            try
            {
                // 获取目录下的所有文件
                string[] files = Directory.GetFiles(directoryPath);

                // 处理每个文件
                foreach (string filePath in files)
                {
                    // 检查文件是否为JSON文件
                    if (Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        // 处理JSON文件
                        ProcessJsonFile(filePath);
                    }
                }

                // 递归处理子目录
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                foreach (string subDirectory in subDirectories)
                {
                    ProcessDirectory(subDirectory);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"访问被拒绝: {directoryPath}");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine($"路径太长: {directoryPath}");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"目录未找到: {directoryPath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"处理目录时发生IO错误: {directoryPath}, 错误: {ex.Message}");
            }
        }

        private static bool ProcessJsonFile(string jsonPath)
        {
            try
            {
                string jsonText = File.ReadAllText(jsonPath, Encoding.UTF8);
                Bitmap bitmap = null;
                using (var tempImage = Image.FromFile(jsonPath.Replace(".json", ".png")))
                {
                    bitmap = new Bitmap(tempImage);
                    if (bitmap == null)
                    {
                        return false;
                    }

                    if (jsonText.Contains("frameRate"))
                    {
                        McConfig data = JsonConvert.DeserializeObject<McConfig>(jsonText);
                        if (data != null && data.Mc != null)
                        {
                            Console.WriteLine($"文件名: {data.Mc.Count} ,{data.Res.Count}");
                            foreach (var mc in data.Mc)
                            {
                               

                                int maxW = 0; int maxH = 0;
                                double minX = 5000; double minY = 5000;
                                double maxX = -5000; double maxY = -5000;

                                for (int i = 0; i < mc.Value.Frames.Count; i++)
                                {
                                    FrameItem fr = mc.Value.Frames[i];
                                    minX = fr.X < minX ? fr.X : minX;
                                    minY = fr.Y < minY ? fr.Y : minY;

                                    maxX = fr.X + data.Res[fr.Res].W > maxX ? fr.X + data.Res[fr.Res].W : maxX;
                                    maxY = fr.Y + data.Res[fr.Res].H > maxY ? fr.Y + data.Res[fr.Res].H : maxY;


                                    maxW = (int)(maxX - minX);
                                    maxH = (int)(maxY - minY);

                                    // maxW = maxW > fr.SourceW? maxW: fr.SourceW;
                                    // maxH = maxH > fr.SourceH? maxH: fr.SourceH;
                                }
                                for (int i = 0; i < mc.Value.Frames.Count; i++)
                                {
                                    FrameItem fr = mc.Value.Frames[i];
                                    var d = Path.GetDirectoryName(jsonPath);
                                    d = jsonPath.Replace(".json", "");
                                    var path = Path.Combine(d, fr.Res + ".png");


                                    if (File.Exists(path))
                                    {
                                        if (minX < 0 || minY < 0)
                                        {
                                            File.Delete(path);
                                        }
                                        else
                                        {
                                            return true;
                                        }

                                    }
                                    // 创建与裁剪区域大小相同的新Bitmap
                                    //Bitmap croppedBitmap = new Bitmap(fr.X + data.Res[fr.Res].W, fr.Y + data.Res[fr.Res].H);
                                    Bitmap croppedBitmap = new Bitmap(maxW, maxH);

                                    // 创建Graphics对象用于绘制裁剪的图像
                                    using (Graphics g = Graphics.FromImage(croppedBitmap))
                                    {
                                        // 将源Bitmap的指定区域绘制到新Bitmap中
                                        g.DrawImage(bitmap, (int)(fr.X - minX), (int)(fr.Y - minY), new Rectangle(data.Res[fr.Res].X, data.Res[fr.Res].Y, data.Res[fr.Res].W, data.Res[fr.Res].H), GraphicsUnit.Pixel);
                                    }
                                   
                                    //Instance.lb_tip.Text = $"mc path: {path}";
                                    Console.WriteLine($"mc path: {path}");
                                    // 创建保存目录（如果不存在）
                                    string directory = Path.GetDirectoryName(path);
                                    if (!Directory.Exists(directory))
                                    {
                                        Directory.CreateDirectory(directory);
                                    }
                                    croppedBitmap.Save(path);
                                    croppedBitmap.Dispose();
                                }
                                return true;
                            }
                        }

                    }
                    else if (jsonText.Contains("count"))
                    {
                        JsonDataModel data2 = JsonConvert.DeserializeObject<JsonDataModel>(jsonText);
                        if (data2 != null && data2.Count > 0)
                        {
                            int maxW = 0; int maxH = 0;
                            double minX = 5000; double minY = 5000;
                            double maxX = -5000; double maxY = -5000;

                            for (int i = 0; i < data2.Frames.Count; i++)
                            {
                                FrameData fr = data2.Frames[i];
                                minX = fr.OffX < minX ? fr.OffX : minX;
                                minY = fr.OffY < minY ? fr.OffY : minY;

                                maxX = fr.OffX + fr.W > maxX ? fr.OffX + fr.W : maxX;
                                maxY = fr.OffY + fr.H > maxY ? fr.OffY + fr.H : maxY;


                                maxW = (int)(maxX - minX);
                                maxH = (int)(maxY - minY);

                               // maxW = maxW > fr.SourceW? maxW: fr.SourceW;
                               // maxH = maxH > fr.SourceH? maxH: fr.SourceH;
                            }
                            for (int i = 0; i < data2.Frames.Count; i++)
                            {
                                FrameData fr = data2.Frames[i];
                                var d = Path.GetDirectoryName(jsonPath);
                                d = jsonPath.Replace(".json", "");
                                var path = Path.Combine(d, i.ToString("D2") + ".png");

                                if (File.Exists(path))
                                {
                                    if (minX < 0 || minY < 0)
                                    {
                                        File.Delete(path);
                                    }
                                    else
                                    {
                                        return true;
                                    }

                                }
                                Console.WriteLine($"model path: {Path.GetFileName(path) } bitmap:{bitmap}, OffX:{(int)(fr.OffX - minX)}, OffY:{(int)(fr.OffY - minY)}, rect:{new Rectangle(fr.X, fr.Y, fr.W, fr.H)},W:{maxW}, H:{maxH}");
                                // 创建与裁剪区域大小相同的新Bitmap
                                Bitmap croppedBitmap = new Bitmap(maxW, maxH);

                                // 创建Graphics对象用于绘制裁剪的图像
                                using (Graphics g = Graphics.FromImage(croppedBitmap))
                                {
                                    // 将源Bitmap的指定区域绘制到新Bitmap中
                                    //g.DrawImage(bitmap, Math.Abs((int)fr.OffX), Math.Abs((int)fr.OffY), new Rectangle(fr.X, fr.Y, fr.W, fr.H), GraphicsUnit.Pixel);
                                    
                                    g.DrawImage(bitmap, (int)(fr.OffX-minX), (int)(fr.OffY - minY), new Rectangle(fr.X, fr.Y, fr.W, fr.H), GraphicsUnit.Pixel);
                                }

                                //Instance.lb_tip.Text = $"model path: {path}";
                                
                                //Console.WriteLine($"model path: {path} bitmap:{bitmap}, fr.OffX:{fr.OffX}, fr.OffY:{fr.OffY}, new Rectangle(fr.X, fr.Y, fr.W, fr.H):{new Rectangle(fr.X, fr.Y, fr.W, fr.H)},fr.SourceW:{fr.SourceW}, fr.SourceH:{fr.SourceH}");
                                // 创建保存目录（如果不存在）
                                string directory = Path.GetDirectoryName(path);
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }
                                croppedBitmap.Save(path);
                                croppedBitmap.Dispose();
                            }
                            return true;
                        }
                    }
                    else
                    {
                        JsonDataUI data3 = JsonConvert.DeserializeObject<JsonDataUI>(jsonText);
                        if (data3 != null && data3.Frames != null && data3.Frames.Count > 0)
                        {
                            foreach (var v in data3.Frames)
                            {
                                var d = Path.GetDirectoryName(jsonPath);
                                d = jsonPath.Replace(".json", "");
                                var path = Path.Combine(d, v.Key + ".png");
                                if (File.Exists(path))
                                {
                                    return true;
                                }
                                // 创建与裁剪区域大小相同的新Bitmap
                                Bitmap croppedBitmap = new Bitmap(v.Value.SourceW, v.Value.SourceH);

                                // 创建Graphics对象用于绘制裁剪的图像
                                using (Graphics g = Graphics.FromImage(croppedBitmap))
                                {
                                    // 将源Bitmap的指定区域绘制到新Bitmap中
                                    g.DrawImage(bitmap, Math.Abs((int)v.Value.OffX), Math.Abs((int)v.Value.OffY), new Rectangle(v.Value.X, v.Value.Y, v.Value.W, v.Value.H), GraphicsUnit.Pixel);
                                }
                              
                                //Instance.lb_tip.Text = $"ui model path: {path}";
                                Console.WriteLine($"ui path: {path}");
                                // 创建保存目录（如果不存在）
                                string directory = Path.GetDirectoryName(path);
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }
                                croppedBitmap.Save(path);
                                croppedBitmap.Dispose();
                            }
                            return true;
                        }
                    }
                   
                  
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"JSON解析错误: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
            return true;
        }
        private void FormCutAtlasJson_Load(object sender, EventArgs e)
        {

        }
        // 下载文件并保存到指定目录
        private async static Task<bool> DownloadFileAsync(string url, string savePath)
        {
            try
            {

                if (File.Exists(savePath)) 
                {
                    return true;
                }
                // 创建保存目录（如果不存在）
                string directory = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var httpClient = new HttpClient())
                {
                    // 设置超时时间（可选）
                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                    // 发送HTTP请求并获取响应流
                    using (HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // 确保请求成功
                        response.EnsureSuccessStatusCode();

                        // 获取文件总大小（用于进度显示）
                        long? contentLength = response.Content.Headers.ContentLength;

                        // 打开文件流准备写入
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            // 如果知道文件大小，显示下载进度
                            if (contentLength.HasValue)
                            {
                                long totalBytesRead = 0;
                                byte[] buffer = new byte[8192];
                                int bytesRead;

                                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalBytesRead += bytesRead;

                                    // 计算并显示进度（范围0-100）
                                    double progress = (double)totalBytesRead / contentLength.Value * 100;
                                }
                            }
                            else
                            {
                                // 不知道文件大小时，直接复制流
                                await contentStream.CopyToAsync(fileStream);
                            }
                        }
                    }
                }

                //MessageBox.Show($"文件已成功保存到: {savePath}", "下载完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (HttpRequestException ex)
            {
                //MessageBox.Show($"下载失败: 网络错误 - {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (TaskCanceledException)
            {
                //MessageBox.Show("下载已取消或超时", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // 选择保存目录的方法
        private string SelectSaveDirectory()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 设置对话框标题
                folderDialog.Description = "请选择保存文件的目录";

                // 可选：设置默认打开的目录
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // 可选：显示"新建文件夹"按钮
                folderDialog.ShowNewFolderButton = true;

                // 显示对话框并获取用户选择
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    // 用户选择了目录，返回选择的路径
                    return folderDialog.SelectedPath;
                }

                // 用户取消了选择或操作失败
                return null;
            }
        }

        private async void btn_allmanifest_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_allmanifest.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => FormCutAtlasJson.DoneRes_AllManifest(selectedPath));
                    MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_allmanifest.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择任何目录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                //saveFileDialog.FileName = fileName;
                saveFileDialog.Title = "选择保存位置";
                // 禁用按钮防止重复点击
              
            }
        }

        private async void btn_itemdown_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_itemdown.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => FormCutAtlasJson.DoneRes_Item(selectedPath));
                    MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_itemdown.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择任何目录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                //saveFileDialog.FileName = fileName;
                saveFileDialog.Title = "选择保存位置";
                // 禁用按钮防止重复点击

            }
        }

        private async void btn_model_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_model.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => FormCutAtlasJson.DoneRes_Model(selectedPath));
                    MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_model.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择任何目录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                //saveFileDialog.FileName = fileName;
                saveFileDialog.Title = "选择保存位置";
                // 禁用按钮防止重复点击

            }
        }

        private async void btn_resv_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_resv.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => FormCutAtlasJson.DoneRes_Resversion(selectedPath));
                    MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_resv.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择任何目录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                //saveFileDialog.FileName = fileName;
                saveFileDialog.Title = "选择保存位置";
                // 禁用按钮防止重复点击

            }
        }

        private  async void btn_cut_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectSaveDirectory();
            //string selectedPath = "F:\\wa7eDoc\\图片\\download\\xxxxx\\resource";//\\model\\125000";// SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_resv.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => ProcessDirectory(selectedPath));
                   // MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_resv.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择任何目录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                //saveFileDialog.FileName = fileName;
                saveFileDialog.Title = "选择保存位置";
                // 禁用按钮防止重复点击

            }
        }
    }




    public class FrameData
    {
        [JsonProperty("x")]
        public int X { get; set; }
       [JsonProperty("y")]
        public int Y { get; set; }
       [JsonProperty("w")]
        public int W { get; set; }
       [JsonProperty("h")]
        public int H { get; set; }
       [JsonProperty("offX")]
        public double OffX { get; set; }
       [JsonProperty("offY")]
        public double OffY { get; set; }
       [JsonProperty("sourceW")]
        public int SourceW { get; set; }
       [JsonProperty("sourceH")]
        public int SourceH { get; set; }
    }
    /// <summary>
    /// ui 解读
    /// </summary>
    public class JsonDataModel
    {
        [JsonProperty("count")]
        public long Count { get; set; }
       [JsonProperty("file")]
        public string File { get; set; }
       [JsonProperty("frames")]
        public List< FrameData> Frames { get; set; }

    }
    /// <summary>
    /// ui 解读
    /// </summary>
    public class JsonDataUI
    {
       [JsonProperty("file")]
        public string File { get; set; }
       [JsonProperty("frames")]
        public Dictionary<string, FrameData> Frames { get; set; }
      
    }

    #region mcdata

    // 对应整个JSON最外层的对象结构
    public class McConfig
    {
        [JsonProperty("mc")]
        public Dictionary<string, McItem> Mc { get; set; }

        [JsonProperty("res")]
        public Dictionary<string, ResourceItem> Res { get; set; }
    }

    // 对应 "mc" 下具体某个元素（如 "啊" 对应的结构）
    public class McItem
    {
        [JsonProperty("frameRate")]
        public int FrameRate { get; set; }

        [JsonProperty("labels")]
        public List<LabelItem> Labels { get; set; }

        [JsonProperty("events")]
        public List<object> Events { get; set; }

        [JsonProperty("frames")]
        public List<FrameItem> Frames { get; set; }
    }

    // 对应 "labels" 数组里的元素结构
    public class LabelItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("frame")]
        public int Frame { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }
    }

    // 对应 "frames" 数组里的元素结构
    public class FrameItem
    {
        [JsonProperty("res")]
        public string Res { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }
    }

    // 对应 "res" 下具体某个元素（如 "sfx_8007_0_0009" 对应的结构）
    public class ResourceItem
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("w")]
        public int W { get; set; }

        [JsonProperty("h")]
        public int H { get; set; }
    }
    
    #endregion
}
