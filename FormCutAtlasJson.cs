using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using zy_cutPicture.Properties;
namespace zy_cutPicture
{
    public partial class FormCutAtlasJson : Form
    {
        public static FormCutAtlasJson Instance;
        public static bool IsDebug = false;
        private static int DebugLimitCount = 100;
        public FormCutAtlasJson()
        {
            InitializeComponent();
            Instance = this;
            Reset();
        }

        public void Reset()
        {
            this.img_1.Visible = false;
            this.img_2.Visible = false;
            this.img_3.Visible = false;
            this.img_4.Visible = false;
            this.img_5.Visible = false;
            this.img_6.Visible = false;
            this.txt_dir.Text = Settings.Default.saveDir_diwang;
            this.txt_resVer.Text = Settings.Default.resVersion_diwang;
            this.listStr.Clear();
            this.logTextBox.Clear();
        }
        List<string> listStr = new List<string>();
        public void AddLog(string message)
        {
            AddLog(message, Color.Black);
        }
        public void AddLog(string message, Color color)
        {
            listStr.Add(message);
            logTextBox.Invoke(new Action(() =>
            {
                logTextBox.SelectionStart = logTextBox.TextLength;
                logTextBox.SelectionColor = color;
                logTextBox.AppendText(message + Environment.NewLine);
                logTextBox.ScrollToCaret();
            }));

        }
        public void listClear()
        {
            listStr.Clear();
            logTextBox.Clear();
        }

        public class AllManifest_Value
        {
            public string v { get; set; }// 版本号或哈希值
            public int s { get; set; }// 大小（字节）
        }

        //配置图片资源下载
        static async Task DoneRes_AllManifest(string directory)
        {
            try
            {
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/allmanifest1.24591.2.json?v=20250530185809?qufu_version=20";
                url = "https://cdn.ascq.zlm4.com/aoshi_20240419/allmanifest1.28929.3.json?v=20251017185057?qufu_version=30";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/allmanifest{Instance.txt_resVer.Text}.json?v=20251017185057?qufu_version=30";

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
                    Instance.AddLog($"DoneRes_AllManifest下载 num: {config.Keys.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    foreach (var kvp in config)
                    {
                        // 检查v的长度是否足够
                        if (string.IsNullOrEmpty(kvp.Value.v) || kvp.Value.v.Length < 2)
                        {
                            Instance.AddLog($"跳过无效条目 {kvp.Key}：v的长度不足2", Color.Yellow);
                            continue;
                        }

                        // 安全截取子串
                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/resource/{kvp.Value.v.Substring(0, 2)}/{kvp.Value.v}_{kvp.Value.s}{Path.GetExtension(kvp.Key)}";
                        string filePath = Path.Combine(directory, kvp.Key);
                        //Console.WriteLine($"{downCount}/{config.Count} subUrl: {subUrl}  filePath:{filePath}");
                        Instance.AddLog($"{downCount}/{config.Count} ALLMan subUrl: {subUrl}  filePath:{kvp.Key}", Color.Black);

                        DownloadFileAsync(subUrl, filePath);
                        downCount++;
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;
                    }

                    //Console.WriteLine($"文件数： {config.Keys.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误1: {ex.Message}", Color.Red);
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
        static async Task DoneRes_Item(string directory)
        {
            try
            {
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/1config1.24591.2.json?v=20250530185809";
                url = "https://cdn.ascq.zlm4.com/aoshi_20240419/1config1.28929.3.json?v=20251017185057";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/1config{Instance.txt_resVer.Text}.json?v=20251017185057";

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
                    Instance.AddLog($"-----物品图标下载:{config.Items.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    string strCurr = "";
                    foreach (var v in config.Items)
                    {
                        if (v.Value.Prop19 == strCurr)
                        {
                            downCount++;
                            continue;
                        }

                        // //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/tool/334.png?ver=1.0.1
                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/tool/{v.Value.Prop19}.png";
                        string filePath = Path.Combine(directory, $"resource/icon/tool/{v.Value.Prop19}.png");
                        //Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        Instance.AddLog($"{downCount}/{config.Items.Count} Item subUrl: {subUrl}  filePath:icon/tool/{v.Value.Prop19}.png", Color.Black);
                        DownloadFileAsync(subUrl, filePath);
                        downCount++;
                        strCurr = v.Value.Prop19;
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;

                    }

                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误2: {ex.Message}", Color.Red);
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
        static async Task DoneRes_Model(string directory)
        {
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.24591.2.json?v=20250530185809";//modelinfo
                url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.28929.3.json?v=20251017185057";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/0config{Instance.txt_resVer.Text}.json?v=20251017185057";

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();
                    //if (!response.IsSuccessStatusCode) 
                    //{
                    //    return;
                    //}

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    ModelInfoWrapper config = JsonConvert.DeserializeObject<ModelInfoWrapper>(json);
                    Console.WriteLine($"文件num: {config.ModelInfo.Count}");
                    Instance.AddLog($"-----模型资源下载:{config.ModelInfo.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int countM = 0;
                    foreach (var v in config.ModelInfo)
                    {
                        countM++;
                        int downCount = 1;
                        foreach (var m in v.Value)
                        {
                            ////https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/121014.png?ver=1.0.1
                            // //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/model/2101/2101043.png?ver=1.0.1
                            //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                            //Instance.AddLog($"{downCount}/{config.ModelInfo.Count * v.Value.Count} subUrl: {subUrl}  filePath:model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}", Color.Black);

                            //四方向
                            for (int i = 0; i < 5; i++)
                            {
                                string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}";
                                string filePath = Path.Combine(directory, $"resource/model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}");
                                //Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                                if (i == 0)
                                    Instance.AddLog($"{downCount}/{v.Value.Count} Model {countM}/{config.ModelInfo.Count} subUrl: {subUrl}  filePath:model/{m.Value.Id}/{m.Value.Id}{(m.Value.Action).ToString("D2")}{i}.json", Color.Black);
                                DownloadFileAsync(subUrl + ".png", filePath + ".png");
                                DownloadFileAsync(subUrl + ".json", filePath + ".json");
                            }

                            if (downCount % 5 == 1)
                            {
                                await Task.Delay(1);
                            }

                            downCount++;

                        }
                        await Task.Delay(1);
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;
                    }

                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误3: {ex.Message}", Color.Red);
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
        static async Task DoneRes_Resversion(string directory)
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
                    Instance.AddLog($"-----版本表资源下载:{config.AdditionalData.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;

                    foreach (var m in config.AdditionalData)
                    {
                        ////https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/121014.png?ver=1.0.1
                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/124013.png?ver=1.0.1
                        if (!m.Key.Contains("."))
                        {
                            Console.WriteLine($"版本 是文件夹不是文件: {m.Key} ");
                            downCount++;
                            continue;
                        }

                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/{m.Key}";
                        string filePath = Path.Combine(directory, $"resource/{m.Key}");

                        if (File.Exists(filePath))
                        {
                            Console.WriteLine($"版本 文件已经存在: {m.Key} ");
                            downCount++;
                            continue;
                        }

                        //Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        //await
                        //Instance.AddLog($"{downCount}/{config.AdditionalData.Count} subUrl: {subUrl}  filePath:{m.Key}", Color.Black);
                        DownloadFileAsync(subUrl, filePath, true);

                        subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/{m.Value}/{m.Key}";
                        //filePath = Path.Combine(directory, $"resource/{m.Value}/{m.Key}");

                        //Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        Instance.AddLog($"{downCount}/{config.AdditionalData.Count} 版本资源 subUrl: {subUrl}  filePath:{m.Key}", Color.Black);
                        //await 
                        DownloadFileAsync(subUrl, filePath, true);

                        downCount++;
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;
                    }

                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误4: {ex.Message}", Color.Red);
            }
        }





        /// <summary>
        /// 怪物头像下载
        /// </summary>
        /// <param name="directory"></param>
        static async Task DoneRes_head(string directory)
        {
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                //string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809";//modelinfo
                //string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.24591.2.json?v=20250530185809";
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.25209.2.json?v=20250620170332";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/0config{Instance.txt_resVer.Text}.json?v=20250620170332";
                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    //response.EnsureSuccessStatusCode(); //此句会在非200状态时抛出异常

                    // 修改为：不主动抛出异常，改为手动判断状态码
                    if (!response.IsSuccessStatusCode)
                    {
                        Instance.AddLog($"请求失败: {url}，状态码: {response.StatusCode}", Color.Red);
                        return; // 直接返回，不继续处理，也不抛出异常
                    }

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    Res_Custom config = JsonConvert.DeserializeObject<Res_Custom>(json);
                    Console.WriteLine($"文件num: {config.Monsters.Count}");
                    Instance.AddLog($"-----怪物头像:{config.Monsters.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }

                    int downCount = 0;
                    string strCurr = "";
                    foreach (var m in config.Monsters)
                    {
                        if (strCurr == m.Value.Head)
                        {
                            downCount++;
                            continue;
                        }
                        ////https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/121014.png?ver=1.0.1
                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/fashion/124013.png?ver=1.0.1
                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/head_00086.png?ver=1.0.1
                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/head_00139.png?ver=1.0.1
                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/head_00050.png?ver=1.0.1

                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/head_4274.png?ver=1.0.1

                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/{m.Value.Head}.png?ver=1.0.1";
                        string filePath = Path.Combine(directory, $"resource/icon/head/{m.Value.Head}.png");
                        //Console.WriteLine($"subUrl: {subUrl}  filePath:{filePath}");
                        Instance.AddLog($"{downCount}/{config.Monsters.Count} subUrl: {subUrl}  filePath:icon/head/{m.Value.Head}.png", Color.Black);
                        //await 
                        DownloadFileAsync(subUrl, filePath);
                        strCurr = m.Value.Head;

                        downCount++;
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;
                    }

                    //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误5: {ex.Message}", Color.Red);
            }
        }



        // 定义与JSON结构对应的类
        class Root_map
        {
            [JsonProperty("map")]
            public Dictionary<string, Cfg_Map> Items { get; set; }
        }

        public class Cfg_Map
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("img")]
            public long img { get; set; }

            [JsonProperty("fmap")]
            public string fmap { get; set; }

            [JsonProperty("data")]
            public long data { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("width")]
            public int width { get; set; }
            public int height { get; set; }


        }
        /// <summary>
        /// 计算24位像素图像的内存占用
        /// </summary>
        /// <param name="width">图像宽度（像素）</param>
        /// <param name="height">图像高度（像素）</param>
        /// <param name="depthOrFrames">深度（3D纹理）或帧数（视频序列），默认1</param>
        /// <returns>格式化的内存占用字符串</returns>
        public static string Calculate24BitImageMemory(int width, int height, int depthOrFrames = 1)
        {
            if (width <= 0 || height <= 0 || depthOrFrames <= 0)
                throw new ArgumentException("宽度、高度和深度/帧数必须大于0");

            // 核心计算
            long totalBytes = (long)width * height * depthOrFrames * 3; // 24位 = 3字节/像素

            // 单位转换和格式化
            if (totalBytes >= 1024L * 1024 * 1024)
                return $"{totalBytes / (1024.0 * 1024 * 1024):F2} GB";
            if (totalBytes >= 1024L * 1024)
                return $"{totalBytes / (1024.0 * 1024):F2} MB";
            if (totalBytes >= 1024)
                return $"{totalBytes / 1024.0:F2} KB";

            return $"{totalBytes} Bytes";
        }
        static Dictionary<int, Size> dicConst = new Dictionary<int, Size>()
        {
            { 154, new Size(512 * 93, 256 * 100) },
            { 1025, new Size(512 * 5, 256 * 7) },
            { 2701, new Size(512 * 13, 256 * 20) },
            { 4902, new Size(512 * 9, 256 * 11) },
            { 5002, new Size(512 * 37, 256 * 50) },
            { 5003, new Size(512 * 37, 256 * 50) },
            { 5005, new Size(512 * 37, 256 * 50) },
            { 5562, new Size(512 * 14, 256 * 23) },
            { 5563, new Size(512 * 26, 256 * 35) },
            { 5564, new Size(512 * 14, 256 * 19) },
            { 5565, new Size(512 * 12, 256 * 17) },
            //{ 5564, new Size(512*5,256*7) }, // 注释掉的重复键
            { 5566, new Size(512 * 18, 256 * 30) },
            { 5567, new Size(512 * 7, 256 * 10) },
            { 5568, new Size(512 * 7, 256 * 10) },
            { 5569, new Size(512 * 7, 256 * 10) },
            { 5570, new Size(512 * 10, 256 * 13) },
            { 5571, new Size(512 * 15, 256 * 23) },
            { 5572, new Size(512 * 15, 256 * 20) },
            { 5573, new Size(512 * 5, 256 * 8) },
            { 5574, new Size(512 * 19, 256 * 23) },
            { 5592, new Size(512 * 22, 256 * 31) },
            { 6110, new Size(512 * 21, 256 * 28) },
            { 6211, new Size(512 * 16, 256 * 25) },
            { 6213, new Size(512 * 19, 256 * 25) },
            { 6217, new Size(512 * 37, 256 * 50) },
            { 6224, new Size(512 * 28, 256 * 50) },
            { 6242, new Size(512 * 28, 256 * 37) },
            { 6259, new Size(512 * 9, 256 * 12) },
            { 9401, new Size(512 * 37, 256 * 65) },
            //{ 12558, new Size(512*18,256*7) }, // 注释掉的重复键
            { 12558, new Size(512 * 6, 256 * 8) },
            { 12574, new Size(512 * 7, 256 * 11) },
            { 12582, new Size(512 * 7, 256 * 10) },
            { 20000, new Size(512 * 20, 256 * 27) },
            { 150000, new Size(512 * 8, 256 * 16) }
        };



        static async Task DoneRes_Map(string directory)
        {

            if (Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("系统是64位，支持更大的内存分配");
                Instance.AddLog("系统是64位，支持更大的内存分配");
            }
            else
            {
                Console.WriteLine("系统是32位，内存分配有限制");
                Instance.AddLog("系统是32位，内存分配有限制");
            }

            Process currentProcess = Process.GetCurrentProcess();

            // 获取系统内存信息
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            float availableRam = ramCounter.NextValue();

            //Console.WriteLine("=== 系统内存信息 ===");
            //Console.WriteLine($"可用内存: {availableRam:F2} MB");
            Instance.AddLog($"可用内存: {availableRam / 1024:F2} GB");            
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.24591.2.json?v=20250530185809";//modelinfo
                url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.28929.3.json?v=20251017185057";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/0config{Instance.txt_resVer.Text}.json?v=20251017185057";

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();
                    //if (!response.IsSuccessStatusCode) 
                    //{
                    //    return;
                    //}

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    Root_map config = JsonConvert.DeserializeObject<Root_map>(json);
                    Console.WriteLine($"文件num: {config.Items.Count}");
                    Instance.AddLog($"-----Map 资源下载:{config.Items.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }


                    int downCount = 1;
                    int downCountInfact = 1;
                    string strCurr = "";
                    foreach (var m in config.Items)
                    {
                        //break;
                        if (m.Value.Id != 202)
                        {
                            downCount++;
                            continue;
                        }

                        //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/5286.jpg?ver=1.0.1

                        string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/{m.Value.img}.jpg?ver=1.0.1";
                        string filePath = Path.Combine(directory, $"resource/minimap/{m.Value.img}.jpg");

                        int countTiles = (int)(Math.Ceiling((double)m.Value.width / 512) * Math.Ceiling((double)m.Value.height / 256));
                        Size size;
                        if (dicConst.TryGetValue((int)m.Value.Id, out size)) 
                        {
                            countTiles = size.Width*size.Height/512/256;
                        }

                        //Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                        Instance.AddLog($"{downCount}/{config.Items.Count} map {downCountInfact}({countTiles}) subUrl: {subUrl}  filePath:resource/minimap/{m.Value.img}.jpg", Color.Green);
                        DownloadFileAsync(subUrl, filePath);

                        var dir2 = Path.Combine(directory, $"resource/map/{m.Value.Id}");
                        // 创建保存目录（如果不存在）
                        if (Directory.Exists(dir2))
                        {
                            if (!Directory.EnumerateFileSystemEntries(dir2).Any())
                            {
                                Directory.Delete(dir2);
                            }
                        }


                        // if (File.Exists(filePath))
                        {
                            bool success = false;
                            int ck_num = 5;
                            for (int i = 1; countTiles >= i; i++)
                            {
                                if (i > ck_num && !success)
                                {
                                    break;
                                }
                                //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/map/5286/114.jpg?ver=1.0.1
                                subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/map/{m.Value.Id}/{i}.jpg?ver=1.0.1";
                                filePath = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");
                                if (i % 10 == 1)
                                    Instance.AddLog($"{i}/{countTiles} mapTile subUrl: {subUrl}  filePath:resource/map/{m.Value.Id}/{i}.jpg", Color.Black);

                                if (i <= ck_num)
                                {
                                    bool b = await DownloadFileAsync(subUrl, filePath);
                                    success = b || success;
                                }
                                else
                                {
                                   await DownloadFileAsync(subUrl, filePath);
                                }

                            }
                            //转换为png
                            bool isPNG = false;
                            if (isPNG)
                            {
                                await Task.Delay(1000);
                                for (int i = 1; countTiles >= i; i++)
                                {

                                    string jpgFilePath = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");
                                    string pngFilePath = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.png");

                                    try
                                    {
                                        // 打开JPG文件
                                        using (Image image = Image.FromFile(jpgFilePath))
                                        {
                                            // 保存为PNG格式
                                            image.Save(pngFilePath, ImageFormat.Png);
                                            Console.WriteLine("转换成功！");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"转换失败: {ex.Message}");
                                    }

                                }
                            }

                        }

                        //if (downCount % 5 == 1)
                        {
                            await Task.Delay(1);
                        }

                        downCount++;
                        downCountInfact++;
                        if (downCount >= DebugLimitCount && IsDebug)
                            break;

                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message} ");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误3: {ex.Message}", Color.Red);
            }
        }


        static async Task DoneRes_MapCombine(string directory)
        {           
            string errorlist = "";
            if (Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("系统是64位，支持更大的内存分配");
                Instance.AddLog("系统是64位，支持更大的内存分配");
            }
            else
            {
                Console.WriteLine("系统是32位，内存分配有限制");
                Instance.AddLog("系统是32位，内存分配有限制");
            }

            Process currentProcess = Process.GetCurrentProcess();

            // 获取系统内存信息
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            float availableRam = ramCounter.NextValue();

            //Console.WriteLine("=== 系统内存信息 ===");
            //Console.WriteLine($"可用内存: {availableRam:F2} MB");
            Instance.AddLog($"可用内存: {availableRam / 1024:F2} GB");


            
            try
            {
                //https://cdn.ascq.zlm4.com/aoshi_20240419/resourceVersion.json?v=?v=20250530185809//resourceveresion
                string url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.24591.2.json?v=20250530185809";//modelinfo
                url = "https://cdn.ascq.zlm4.com/aoshi_20240419/0config1.28929.3.json?v=20251017185057";
                url = $"https://cdn.ascq.zlm4.com/aoshi_20240419/0config{Instance.txt_resVer.Text}.json?v=20251017185057";

                using (var httpClient = new HttpClient())
                {
                    // 发送HTTP请求并获取响应
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();
                    //if (!response.IsSuccessStatusCode) 
                    //{
                    //    return;
                    //}

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"文件num: {json}");

                    // 反序列化为配置对象
                    Root_map config = JsonConvert.DeserializeObject<Root_map>(json);
                    Console.WriteLine($"文件num: {config.Items.Count}");
                    Instance.AddLog($"-----Map 资源下载:{config.Items.Count}", Color.Green);
                    if (config == null)
                    {
                        throw new InvalidOperationException("配置文件内容为空或格式不正确");
                    }
  
                    await Task.Delay(15);

                    int errorCount = 0;
                    int caseINT = 3;
                    #region //合图方案一，bitmap 一般图4K
                    if (caseINT == 1)
                    {
                        foreach (var m in config.Items)
                        {
                            //if (m.Value.Id != 6242) { continue; }
                            //if (
                            //    //m.Value.Id != 103 &&
                            //    m.Value.Id != 154 &&
                            //    m.Value.Id != 5267 &&
                            //    m.Value.Id != 5417 &&
                            //    m.Value.Id != 5418 &&
                            //    //m.Value.Id != 6227 &&
                            //    m.Value.Id != 7149 &&
                            //    m.Value.Id != 7151 &&
                            //    //m.Value.Id != 8053 &&
                            //    m.Value.Id != 8074 &&
                            //    m.Value.Id != 8075 &&
                            //    //m.Value.Id != 8078 &&
                            //    m.Value.Id != 9221 &&
                            //    m.Value.Id != 9401
                            //    )
                            //{

                            //    continue;
                            //}
                            await Task.Delay(1);

                            //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/5286.jpg?ver=1.0.1

                            string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/{m.Value.img}.jpg?ver=1.0.1";
                            string mapDir = Path.Combine(directory, $"resource/map/{m.Value.Id}");
                            string outputPath = Path.Combine(directory, $"resource/map/{m.Value.Id}.jpg");

                            int countTiles = (int)(Math.Ceiling((double)m.Value.width / 512) * Math.Ceiling((double)m.Value.height / 256));
                            int newWidth = (int)(m.Value.width);
                            int newHeight = (int)(m.Value.height);

                            Size size;
                            if (dicConst.TryGetValue((int)m.Value.Id, out size))
                            {
                                countTiles = size.Width * size.Height / 512 / 256;
                                newWidth = size.Width;
                                newHeight = size.Height;
                            }
                            //if (newWidth > 10000) continue;
                            if (!Directory.Exists(mapDir)) continue;





                            int xCount = (int)(Math.Ceiling((double)newWidth / 512));
                            int yCount = (int)(Math.Ceiling((double)newHeight / 256));

                           
                           
                            if (File.Exists(outputPath))
                            {
                                if (!JpegChecker.IsLossyJpeg(outputPath))
                                {
                                    Instance.AddLog($"已有的大图有损，删除后继续 {outputPath}");
                                    File.Delete(outputPath);
                                }
                                else
                                {
                                    Instance.AddLog($"已经生成的大图，跳过 {outputPath}");
                                    continue;
                                }

                            }




                            Instance.AddLog($"{iPro} map合图({countTiles}个切片图({Calculate24BitImageMemory(newWidth, newHeight)})，（{newWidth},{newHeight}）) {mapDir} ", Color.Black);

                            try
                            {
                                Instance.AddLog($"有损资源下载中{mapDir}... ", Color.Black);
                                //int totalFiles = files.Length;
                                // 有损资源下载
                                for (int i = 1; i <= countTiles; i++)
                                {
                                    string file = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");
                                   
                                    try
                                    {
                                        if (!JpegChecker.IsLossyJpeg(file))
                                        {
                                            string urlFile2 = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/map/{m.Value.Id}/{i}.jpg?ver=1.0.1";
                                            Instance.AddLog($"{i}/{countTiles} 重下有损资源: {urlFile2} ", Color.Black);
                                            Console.WriteLine($"{i}/{countTiles} 重下有损资源: {urlFile2}");

                                            // 下载文件并覆盖
                                            await DownloadFileAsync(urlFile2, file, true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // 单个文件处理失败不中断整体循环
                                        Instance.AddLog($"处理文件 {file} 失败: {ex.Message}", Color.Orange);
                                        continue;
                                    }

                                    // 每处理100个文件强制回收一次内存（根据实际情况调整频率）
                                    if (i % 1000 == 0 && i > 0)
                                    {
                                        GC.Collect(); // 强制回收托管内存
                                        GC.WaitForPendingFinalizers(); // 等待非托管资源释放
                                        Instance.AddLog($"已处理 {i} 个文件，触发内存回收", Color.Gray);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Instance.AddLog($"有损资源处理错误: {ex.Message}", Color.Red);
                            }

                            //合图
                            try
                            {
                                Console.WriteLine($"newWidth: {newWidth}   newHeight:{newHeight}");
                                Instance.AddLog($"newWidth: {newWidth}   newHeight:{newHeight}", Color.Brown);
                                using (Bitmap outBmp = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
                                using (Graphics g = Graphics.FromImage(outBmp))
                                {


                                    //设置高质量绘制
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;// System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                    g.Clear(Color.Black); // 清除画布

                                    // 处理每个文件
                                    for (int i = 1; i <= countTiles; i++)
                                    {
                                        string tilePath = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");
                                       
                                        using (FileStream tileFs = new FileStream(tilePath, FileMode.Open, FileAccess.Read))
                                        using (Image imgagetile = Image.FromStream(tileFs))
                                        {
                                            // 计算切片位置
                                            int srcX = ((i - 1) % xCount) * 512;
                                            int srcY = ((i - 1) / xCount) * 256;

                                            // 确保不超出目标图像边界
                                            int drawWidth = Math.Min(imgagetile.Width, newWidth - srcX);
                                            int drawHeight = Math.Min(imgagetile.Height, newHeight - srcY);

                                            if (drawWidth > 0 && drawHeight > 0)
                                            {
                                                g.DrawImage(imgagetile, new Rectangle(srcX, srcY, drawWidth, drawHeight));
                                            }
                                        }


                                    }
                                    // 保存合并后的图片
                                    string outputDir = Path.GetDirectoryName(outputPath);
                                    if (!Directory.Exists(outputDir))
                                    {
                                        Directory.CreateDirectory(outputDir);
                                    }

                                    outBmp.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    Instance.AddLog($"图片已保存: {outputPath}", Color.Green);

                                }
                            }
                            catch (Exception e)
                            {
                                errorCount++;
                                Console.WriteLine($"{errorCount}合图发生错误: {e.Message} {m.Value.Id} ({newWidth},{newHeight})");
                                Instance.AddLog($"{errorCount}合图发生错误: {e.Message} {m.Value.Id}  ({newWidth},{newHeight})", Color.Red);
                                errorlist += "\n" + $"{errorCount}合图发生错误: {e.Message} {m.Value.Id}  ({newWidth},{newHeight})";
                            }
                            //break;
                            Console.WriteLine($"--------合图---------------------");
                            //await Task.Delay(1);
                        }
                        //Console.WriteLine($"文件数： {config.Items.Count}  下载数： {downCount}");
                    }
                    #endregion

                    #region // 合图方案二 大图
                    if (caseINT == 2)
                    {
                        // 合图（内存映射方案）
                        foreach (var m in config.Items)
                        {
                            //if (
                            //    //m.Value.Id != 103 &&
                            //    m.Value.Id != 154 &&
                            //    m.Value.Id != 5267 &&
                            //    m.Value.Id != 5417 &&
                            //    m.Value.Id != 5418 &&
                            //    //m.Value.Id != 6227 &&
                            //    m.Value.Id != 7149 &&
                            //    m.Value.Id != 7151 &&
                            //    //m.Value.Id != 8053 &&
                            //    m.Value.Id != 8074 &&
                            //    m.Value.Id != 8075 &&
                            //    //m.Value.Id != 8078 &&
                            //    m.Value.Id != 9221 &&
                            //    m.Value.Id != 9401
                            //    )
                            //{

                            //    continue;
                            //}

                            await Task.Delay(1);

                            string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/{m.Value.img}.jpg?ver=1.0.1";
                            string mapDir = Path.Combine(directory, $"resource/map/{m.Value.Id}");
                            string outputPath = Path.Combine(directory, $"resource/map/{m.Value.Id}.jpg");

                            int countTiles = (int)(Math.Ceiling((double)m.Value.width / 512) * Math.Ceiling((double)m.Value.height / 256));
                            int newWidth = (int)(m.Value.width);
                            int newHeight = (int)(m.Value.height);

                            Size size;
                            if (dicConst.TryGetValue((int)m.Value.Id, out size))
                            {
                                countTiles = size.Width * size.Height / 512 / 256;
                                newWidth = size.Width;
                                newHeight = size.Height;
                            }

                            if (!Directory.Exists(mapDir)) continue;

                            int xCount = (int)(Math.Ceiling((double)newWidth / 512));
                            int yCount = (int)(Math.Ceiling((double)newHeight / 256));

                           
                            if (File.Exists(outputPath))
                            {
                                if (!JpegChecker.IsLossyJpeg(outputPath))
                                {
                                    Instance.AddLog($"已有的大图有损，删除后继续 {outputPath}");
                                    File.Delete(outputPath);
                                }
                                else
                                {
                                    Instance.AddLog($"已经生成的大图，跳过 {outputPath}");
                                    continue;
                                }

                            }



                            Instance.AddLog($"{iPro} map合图({countTiles}个切片图({Calculate24BitImageMemory(newWidth, newHeight)})，（{newWidth},{newHeight}）) {mapDir} ", Color.Black);

                            try
                            {
                                Instance.AddLog($"有损资源下载中{mapDir}... ", Color.Black);
                                
                                // 有损资源下载
                                for (int i = 1; i <= countTiles; i++)
                                {
                                    string file = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");                                  

                                    try
                                    {

                                        if (!JpegChecker.IsLossyJpeg(file))
                                        {
                                            string urlFile2 = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/map/{m.Value.Id}/{i}.jpg?ver=1.0.1";
                                            Instance.AddLog($"{i}/{countTiles} 重下有损资源: {urlFile2} ", Color.Black);
                                            Console.WriteLine($"{i}/{countTiles} 重下有损资源: {urlFile2}");

                                            // 下载文件并覆盖
                                            await DownloadFileAsync(urlFile2, file, true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // 单个文件处理失败不中断整体循环
                                        Instance.AddLog($"处理文件 {file} 失败: {ex.Message}", Color.Orange);
                                        continue;
                                    }

                                    // 每处理100个文件强制回收一次内存（根据实际情况调整频率）
                                    if (i % 1000 == 0 && i > 0)
                                    {
                                        GC.Collect(); // 强制回收托管内存
                                        GC.WaitForPendingFinalizers(); // 等待非托管资源释放
                                        Instance.AddLog($"已处理 {i} 个文件，触发内存回收", Color.Gray);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Instance.AddLog($"有损资源处理错误: {ex.Message}", Color.Red);
                            }

                            try
                            {
                                Console.WriteLine($"newWidth: {newWidth}   newHeight:{newHeight}");
                                Instance.AddLog($"newWidth: {newWidth}   newHeight:{newHeight}", Color.Brown);

                                // 计算24位图像所需字节数 (宽 × 高 × 3字节/像素)
                                long imageDataSize = (long)newWidth * newHeight * 3;
                                string tempMapFile = Path.GetTempFileName();

                                try
                                {
                                    // 创建内存映射文件
                                    using (var mmf = MemoryMappedFile.CreateFromFile(
                                        tempMapFile,
                                        FileMode.Create,
                                        null,
                                        imageDataSize,
                                        MemoryMappedFileAccess.ReadWrite))
                                    {
                                        // 获取内存映射视图访问器
                                        using (var accessor = mmf.CreateViewAccessor(0, imageDataSize, MemoryMappedFileAccess.ReadWrite))
                                        {
                                            // 初始化内存区域为黑色
                                            byte[] blackBuffer = new byte[4096];
                                            for (int i = 0; i < blackBuffer.Length; i++)
                                            {
                                                blackBuffer[i] = 0; // 初始化黑色(0,0,0)
                                            }

                                            long remaining = imageDataSize;
                                            long position = 0;
                                            while (remaining > 0)
                                            {
                                                int writeSize = (int)Math.Min(remaining, blackBuffer.Length);
                                                accessor.WriteArray(position, blackBuffer, 0, writeSize);
                                                position += writeSize;
                                                remaining -= writeSize;
                                            }

                                            // 处理每个切片
                                            for (int i = 1; i <= countTiles; i++)
                                            {
                                                string file = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");

                                                // 计算切片位置
                                                int srcX = ((i - 1) % xCount) * 512;
                                                int srcY = ((i - 1) / xCount) * 256;

                                                // 读取切片图像并锁定像素
                                                using (var tileBmp = new Bitmap(file))
                                                {
                                                    Rectangle tileRect = new Rectangle(0, 0, tileBmp.Width, tileBmp.Height);
                                                    System.Drawing.Imaging.BitmapData tileData = tileBmp.LockBits(
                                                        tileRect,
                                                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                                                    try
                                                    {
                                                        // 确保不超出目标图像边界
                                                        int drawWidth = Math.Min(tileBmp.Width, newWidth - srcX);
                                                        int drawHeight = Math.Min(tileBmp.Height, newHeight - srcY);

                                                        if (drawWidth <= 0 || drawHeight <= 0)
                                                            continue;

                                                        // 每行像素的字节数
                                                        int tileStride = tileData.Stride;
                                                        IntPtr tileScan0 = tileData.Scan0;

                                                        // 复制每行像素数据到内存映射区域
                                                        for (int y = 0; y < drawHeight; y++)
                                                        {
                                                            long destRowOffset = (srcY + y) * newWidth * 3 + srcX * 3;
                                                            if (destRowOffset + drawWidth * 3 > imageDataSize)
                                                                break;

                                                            // 复制一行像素数据到临时缓冲区
                                                            byte[] rowBuffer = new byte[drawWidth * 3];
                                                            System.Runtime.InteropServices.Marshal.Copy(tileScan0 + y * tileStride, rowBuffer, 0, drawWidth * 3);

                                                            // 显式指定类型参数为 byte，将缓冲区数据写入内存映射区域
                                                            accessor.WriteArray<byte>(destRowOffset, rowBuffer, 0, drawWidth * 3);
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        tileBmp.UnlockBits(tileData);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    // 从内存映射文件创建Bitmap并保存
                                    using (var finalBmp = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                                    {
                                        Rectangle rect = new Rectangle(0, 0, newWidth, newHeight);
                                        System.Drawing.Imaging.BitmapData bmpData = finalBmp.LockBits(
                                            rect,
                                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                            finalBmp.PixelFormat);

                                        try
                                        {
                                            // 从临时文件读取数据到Bitmap
                                            using (var fs = new FileStream(tempMapFile, FileMode.Open, FileAccess.Read))
                                            {
                                                byte[] buffer = new byte[bmpData.Stride * newHeight];
                                                fs.Read(buffer, 0, buffer.Length);
                                                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
                                            }
                                        }
                                        finally
                                        {
                                            finalBmp.UnlockBits(bmpData);
                                        }

                                        // 保存最终图像
                                        string outputDir = Path.GetDirectoryName(outputPath);
                                        if (!Directory.Exists(outputDir))
                                            Directory.CreateDirectory(outputDir);

                                        finalBmp.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        Instance.AddLog($"图片已保存: {outputPath}", Color.Green);
                                    }
                                }
                                finally
                                {
                                    // 清理临时文件
                                    if (File.Exists(tempMapFile))
                                    {
                                        File.Delete(tempMapFile);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                errorCount++;
                                Console.WriteLine($"{errorCount}合图发生错误: {e.Message} {m.Value.Id} ({newWidth},{newHeight})");
                                Instance.AddLog($"{errorCount}合图发生错误: {e.Message} {m.Value.Id}  ({newWidth},{newHeight})", Color.Red);
                                errorlist += "\n" + $"{errorCount}合图发生错误: {e.Message} {m.Value.Id}  ({newWidth},{newHeight})";
                            }

                            Console.WriteLine($"--------合图完成---------------------");
                        }
                    }
                    #endregion


                    #region //合图方案三  超大图
                    if (caseINT == 3)
                    {
                        int index = 0;
                        foreach (var m in config.Items)
                        {
                            //if (m.Value.Id!= 6242) { continue; }
                            index++;

                            //if (
                            //    //m.Value.Id != 103 &&
                            //    m.Value.Id != 154 &&
                            //    m.Value.Id != 5267 &&
                            //    m.Value.Id != 5417 &&
                            //    m.Value.Id != 5418 &&
                            //    //m.Value.Id != 6227 &&
                            //    m.Value.Id != 7149 &&
                            //    m.Value.Id != 7151 &&
                            //    //m.Value.Id != 8053 &&
                            //    m.Value.Id != 8074 &&
                            //    m.Value.Id != 8075 &&
                            //    //m.Value.Id != 8078 &&
                            //    m.Value.Id != 9221 &&
                            //    m.Value.Id != 9401
                            //    )
                            //{

                            //    continue;
                            //}
                            GC.Collect();
                                await Task.Delay(1);

                                //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/5286.jpg?ver=1.0.1

                                string subUrl = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/minimap/{m.Value.img}.jpg?ver=1.0.1";
                                string mapDir = Path.Combine(directory, $"resource/map/{m.Value.Id}");
                                string outputPath = Path.Combine(directory, $"resource/map/{m.Value.Id}.jpg");

                                int countTiles = (int)(Math.Ceiling((double)m.Value.width / 512) * Math.Ceiling((double)m.Value.height / 256));
                                int newWidth = (int)(m.Value.width);
                                int newHeight = (int)(m.Value.height);

                                Size size;
                                if (dicConst.TryGetValue((int)m.Value.Id, out size))
                                {
                                    countTiles = size.Width * size.Height / 512 / 256;
                                    newWidth = size.Width;
                                    newHeight = size.Height;
                                }
                                //if (newWidth > 10000) continue;
                                if (!Directory.Exists(mapDir)) continue;
                            if (File.Exists(outputPath)) 
                            {
                                if (!JpegChecker.IsLossyJpeg(outputPath))
                                {
                                    Instance.AddLog($"已有的大图有损，删除后继续 {outputPath}");
                                    File.Delete(outputPath);
                                }
                                else 
                                {
                                    Instance.AddLog($"已经生成的大图，跳过 {outputPath}");
                                    continue;
                                }
                              
                            }


                                int xCount = (int)(Math.Ceiling((double)newWidth / 512));
                                int yCount = (int)(Math.Ceiling((double)newHeight / 256));

                              
                                Instance.AddLog($"{iPro} map合图({countTiles}个切片图({Calculate24BitImageMemory(newWidth, newHeight)})，（{newWidth},{newHeight}）) {mapDir} ", Color.Black);

                            try
                            {
                                Instance.AddLog($"有损资源下载中{mapDir}... ", Color.Black);
                                
                                // 有损资源下载
                                for (int i = 1; i <= countTiles; i++)
                                {
                                    string file = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");

                                    try
                                    {

                                        if (!JpegChecker.IsLossyJpeg(file))
                                        {
                                            string urlFile2 = $"https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/map/{m.Value.Id}/{i}.jpg?ver=1.0.1";
                                            Instance.AddLog($"{i}/{countTiles} 重下有损资源: {urlFile2} ", Color.Black);
                                            Console.WriteLine($"{i}/{countTiles} 重下有损资源: {urlFile2}");

                                            // 下载文件并覆盖
                                            await DownloadFileAsync(urlFile2, file, true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // 单个文件处理失败不中断整体循环
                                        Instance.AddLog($"处理文件 {file} 失败: {ex.Message}", Color.Orange);
                                        continue;
                                    }

                                    // 每处理100个文件强制回收一次内存（根据实际情况调整频率）
                                    if (i % 1000 == 0 && i > 0)
                                    {
                                        GC.Collect(); // 强制回收托管内存
                                        GC.WaitForPendingFinalizers(); // 等待非托管资源释放
                                        Instance.AddLog($"已处理 {i} 个文件，触发内存回收", Color.Gray);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Instance.AddLog($"有损资源处理错误: {ex.Message}", Color.Red);
                            }


                            try
                            {
                                using (var magickImage = new MagickImage(MagickColors.Black, newWidth, newHeight))
                                {
                                    for (int i = 1; i <= countTiles; i++)
                                    {
                                        string file = Path.Combine(directory, $"resource/map/{m.Value.Id}/{i}.jpg");


                                        using (var tile = new ImageMagick.MagickImage(file))
                                        {
                                            int x = ((i - 1) % xCount) * 512;
                                            int y = ((i - 1) / xCount) * 256;

                                            magickImage.Composite(tile, x, y, ImageMagick.CompositeOperator.Over);
                                            if (i % 100 == 1)
                                                Instance.AddLog($"ImageMagick处理切片: {file}/{countTiles}", Color.Black);
                                            Console.WriteLine($"ImageMagick处理切片: {file}");

                                            //// 每处理100个文件强制回收一次内存（根据实际情况调整频率）
                                            if (i % 500 == 0 && i > 0)
                                            {
                                                GC.Collect(); // 强制回收托管内存
                                                GC.WaitForPendingFinalizers(); // 等待非托管资源释放
                                                Instance.AddLog($"已合并 {i}/{countTiles} 个切图，触发内存回收", Color.Gray);
                                            }
                                        }
                                    }
                                   
                                    magickImage.Write(outputPath,MagickFormat.Jpg);
                                }
                                iPro++;
                                Instance.AddLog($"({index}/{config.Items.Count})图片已保存: {outputPath}", Color.Green);
                               
                            }
                            catch (Exception ex)
                            {
                                Instance.AddLog($"ImageMagick处理错误: {ex.Message}", Color.Red);
                            }
                            //return;
                        }

                       
                    }
                    #endregion
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message} ");
                Instance.AddLog($"HTTP请求错误: {ex.Message}", Color.Red);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"解析JSON配置文件时出错: {ex.Message}");
                Instance.AddLog($"解析JSON配置文件时出错: {ex.Message}", Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误3: {ex.Message}", Color.Red);
            }
            Instance.AddLog(errorlist,Color.Red);
        }

        
        static int iPro = 0;
        // 递归处理目录及其子目录
        static void ProcessDirectory(string directoryPath)
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
                if(files.Length > 0)
                    Instance.AddLog($"{iPro} 切文件夹({files.Length}个) {directoryPath} ", Color.Black);

                // 处理每个文件
                foreach (string filePath in files)
                {
                    // 检查文件是否为JSON文件
                    if (Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        if (iPro % 100 == 0)
                        {
                            Instance.AddLog($"{iPro}---- 切图 {filePath}", Color.Green);
                            //await Task.Delay(1);
                        }
                        iPro++;

                       
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
                Instance.AddLog($"访问被拒绝: {directoryPath}", Color.Red);
            }
            catch (PathTooLongException)
            {
                Console.WriteLine($"路径太长: {directoryPath}");
                Instance.AddLog($"路径太长: {directoryPath}", Color.Red);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"目录未找到: {directoryPath}");
                Instance.AddLog($"目录未找到: {directoryPath}", Color.Red);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"处理目录时发生IO错误: {directoryPath}, 错误: {ex.Message}");
                Instance.AddLog($"处理目录时发生IO错误: {directoryPath}, 错误: {ex.Message}", Color.Red);
            }

        }
        
        private static bool ProcessJsonFile(string jsonPath)
        {            
            if (!File.Exists(jsonPath.Replace(".json", ".png"))) 
            {
                Instance.AddLog($"-----切图不存在路径:{jsonPath.Replace(".json", ".png")}", Color.Yellow);
                return false;
            }
            try
            {
                //await Task.Delay(1);
                string jsonText = File.ReadAllText(jsonPath, Encoding.UTF8);
                Bitmap bitmap = null;
                using (var tempImage = Image.FromFile(jsonPath.Replace(".json", ".png")))
                {
                    bitmap = new Bitmap(tempImage);
                    if (bitmap == null)
                    {
                        return false;
                    }
                    //Console.WriteLine($"-----文件名:{jsonPath}");
                    if (jsonText.Contains("frameRate"))
                    {
                        McConfig data = JsonConvert.DeserializeObject<McConfig>(jsonText);
                        if (data != null && data.Mc != null)
                        {
                            //Console.WriteLine($"文件名:{jsonPath}");
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
                                        //if (minX < 0 || minY < 0)
                                        //{
                                        //    File.Delete(path);
                                        //}
                                        FileInfo fileInfo = new FileInfo(path);
                                        if (fileInfo.Length == 0)
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
                                    //Console.WriteLine($"mc path: {path}");
                                    //Instance.AddLog($"mc path: {path}", Color.Black);


                                    // 创建保存目录（如果不存在）
                                    string directory = Path.GetDirectoryName(path);
                                    //if (!Directory.Exists(directory))
                                    //{
                                    //    Directory.CreateDirectory(directory);
                                    //}
                                    //croppedBitmap.Save(path);

                                    //update directory
                                    path = path.Replace("resource", "resource_cut");
                                    directory = Path.GetDirectoryName(path);
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
                    else if (jsonText.Contains("\"count\""))
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
                                    //if (minX < 0 || minY < 0)
                                    //{
                                    //    File.Delete(path);
                                    //}
                                    FileInfo fileInfo = new FileInfo(path);
                                    if (fileInfo.Length == 0)
                                    { 
                                        File.Delete(path); 
                                    }
                                    else
                                    {
                                        return true;
                                    }

                                }
                                //Console.WriteLine($"model path: {Path.GetFileName(path) } bitmap:{bitmap}, OffX:{(int)(fr.OffX - minX)}, OffY:{(int)(fr.OffY - minY)}, rect:{new Rectangle(fr.X, fr.Y, fr.W, fr.H)},W:{maxW}, H:{maxH}");
                                //Instance.AddLog($"model path: {Path.GetFileName(path)} bitmap:{bitmap}, OffX:{(int)(fr.OffX - minX)}, OffY:{(int)(fr.OffY - minY)}, rect:{new Rectangle(fr.X, fr.Y, fr.W, fr.H)},W:{maxW}, H:{maxH}", Color.Black);
                                
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
                                //if (!Directory.Exists(directory))
                                //{
                                //    Directory.CreateDirectory(directory);
                                //}
                                //croppedBitmap.Save(path);

                                //update directory
                                path = path.Replace("resource", "resource_cut");
                                directory = Path.GetDirectoryName(path);
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
                                //Console.WriteLine($"ui path: {path}");
                                //Instance.AddLog($"ui path: {path}", Color.Black);
                                
                                // 创建保存目录（如果不存在）
                                string directory = Path.GetDirectoryName(path);
                                //if (!Directory.Exists(directory))
                                //{
                                //    Directory.CreateDirectory(directory);
                                //}
                                //croppedBitmap.Save(path);

                                path = path.Replace("resource", "resource_cut");
                                directory = Path.GetDirectoryName(path);
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
                Instance.AddLog($"JSON解析错误: {ex.Message}", Color.Red);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Instance.AddLog($"发生错误6: {ex.Message}", Color.Red);
                return false;
            }
            return true;
        }
        private void FormCutAtlasJson_Load(object sender, EventArgs e)
        {

        }
        // 下载文件并保存到指定目录
        private async static Task<bool> DownloadFileAsync(string url, string savePath,bool isDeletExists=false)
        {
            try
            {

                if (File.Exists(savePath)) 
                {
                    if (isDeletExists)
                    {
                        File.Delete(savePath);
                    }
                    else
                    {
                        return true;
                    }
                }
              
                using (var httpClient = new HttpClient())
                {
                    // 设置超时时间（可选）
                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                    // 发送HTTP请求并获取响应流
                    using (HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // 确保请求成功
                        //response.EnsureSuccessStatusCode();


                        // 修改为：不主动抛出异常，改为手动判断状态码
                        if (!response.IsSuccessStatusCode)
                        {
                            //Instance.AddLog($"请求失败: {url}，状态码: {response.StatusCode}", Color.Red);
                            Console.WriteLine($"请求失败: {url}，状态码: {response.StatusCode}");
                            return false; // 直接返回，不继续处理，也不抛出异常
                        }
                        // 创建保存目录（如果不存在）
                        string directory = Path.GetDirectoryName(savePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }


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
                Instance.AddLog($"下载失败: 网络错误 - {ex.Message}   {url}",Color.Red);
                Console.WriteLine($"下载失败: 网络错误 - {ex.Message}  {url}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (TaskCanceledException)
            {
                //MessageBox.Show("下载已取消或超时", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"下载已取消或超时    {url}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Instance.AddLog($"下载已取消或超时    {url}", Color.Red);
                return false;
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"下载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"下载失败: {ex.Message}     {url}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Instance.AddLog($"下载失败: {ex.Message}     {url}", Color.Red);
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
        private string SelectFilePath()
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "请选择文件";
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileDialog.Filter = "所有文件 (*.*)|*.*"; // 可以自定义文件类型筛选

                if (fileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
                {
                    return fileDialog.FileName;
                }
                return null;
            }
        }
        private async void btn_allmanifest_Click(object sender, EventArgs e)
        {
            string selectedPath = this.txt_dir.Text;

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
            string selectedPath = this.txt_dir.Text;

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
            string selectedPath = this.txt_dir.Text;

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
            string selectedPath = this.txt_dir.Text;

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
        /// <summary>
        /// 下载怪头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_icon1_Click(object sender, EventArgs e)
        {
            string selectedPath = this.txt_dir.Text;

            if (selectedPath != null)
            {
                btn_resv.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => FormCutAtlasJson.DoneRes_head(selectedPath)) ;
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
            string selectedPath = Path.Combine(this.txt_dir.Text, "resource");
            //string selectedPath = "F:\\wa7eDoc\\图片\\download\\xxxxx\\resource";//\\model\\125000";// SelectSaveDirectory();

            if (selectedPath != null)
            {
                btn_cut.Enabled = false;
                try
                {
                    // 调用异步方法
                    await Task.Run(() => ProcessDirectory(selectedPath)) ;
                   // MessageBox.Show("配置加载完成", $"{selectedPath} 成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_cut.Enabled = true;
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

        private async  void btn_cut1_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectFilePath();

            //https://cdn.ascq.zlm4.com/aoshi_20240419/assets/resource/icon/head/head_04267.png?ver=1.0.1

            //string selectedPath  = "F:\\wa7eDoc\\图片\\download\\xxxxx\\resource\\assets\\gameui4\\window-sheet\\y_common-sheet.json";
            //string selectedPath = "F:\\wa7eDoc\\图片\\download\\xxxxx\\resource";//\\model\\125000";// SelectSaveDirectory();
            //Console.WriteLine("selectedPath:", selectedPath);
            if (selectedPath != null)
            {
                btn_cut1.Enabled = false;
                try
                {
                    // 检查文件是否为JSON文件
                    if (Path.GetExtension(selectedPath).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        // 处理JSON文件
                        ProcessJsonFile(selectedPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复按钮状态
                    btn_cut1.Enabled = true;
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

       
        /// <summary>
        /// 自动区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ck_00_CheckedChanged(object sender, EventArgs e)
        {
            this.ck_01.Checked = !this.ck_00.Checked;

            this.groupBox1.Visible = this.ck_00.Checked;
            this.groupBox2.Visible = this.ck_01.Checked;
        }
        /// <summary>
        /// 手动区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ck_01_CheckedChanged(object sender, EventArgs e)
        {
            this.ck_00.Checked = !this.ck_01.Checked;

            this.groupBox1.Visible = this.ck_00.Checked;
            this.groupBox2.Visible = this.ck_01.Checked;

        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
           this.txt_dir.Text = SelectSaveDirectory();
        }

        private void txt_dir_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.saveDir_diwang = this.txt_dir.Text;
            Console.WriteLine($"保存目录修改{this.txt_dir.Text}");
            Settings.Default.Save();
        }
        private void txt_resVer_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.resVersion_diwang = this.txt_resVer.Text;
            Settings.Default.Save();
        }
        /// <summary>
        /// 一键开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_onekey_Click(object sender, EventArgs e)
        {
            // 记录总开始时间
            var totalStartTime = DateTime.Now;
            AddLog($"开始执行全部任务，开始时间：{totalStartTime:yyyy-MM-dd HH:mm:ss}", Color.Blue);

            if (this.ck_1.Checked)
            {
                // 下载Allmanifest
                var step1Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_AllManifest(this.txt_dir.Text));
                var step1End = DateTime.Now;
                AddLog($"完成 下载Allmanifest，耗时：{(step1End - step1Start).TotalSeconds:F2}秒", Color.Green);
                this.img_1.Visible = true;
            }

            if (this.ck_2.Checked)
            {
                // 下载Items图标
                var step2Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_Item(this.txt_dir.Text));
                var step2End = DateTime.Now;
                AddLog($"完成 下载Items图标，耗时：{(step2End - step2Start).TotalSeconds:F2}秒", Color.Green);
                this.img_2.Visible = true;
            }

            if (this.ck_3.Checked)
            {
                // 下载Models序列图资源
                var step3Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_Model(this.txt_dir.Text));
                var step3End = DateTime.Now;
                AddLog($"完成 下载Models序列图资源，耗时：{(step3End - step3Start).TotalSeconds:F2}秒", Color.Green);
                this.img_3.Visible = true;
            }         

            if (this.ck_5.Checked)
            {
                // 下载怪物头像图
                var step5Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_head(this.txt_dir.Text));
                var step5End = DateTime.Now;
                AddLog($"完成 下载怪物头像图，耗时：{(step5End - step5Start).TotalSeconds:F2}秒", Color.Green);
                this.img_5.Visible = true;
            }
            if (this.ck_7.Checked)
            {
                // 下载怪物头像图
                var step6Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_Map(this.txt_dir.Text));
                var step6End = DateTime.Now;
                AddLog($"完成 下载地图，耗时：{(step6End - step6Start).TotalSeconds:F2}秒", Color.Green);
                this.img_7.Visible = true;
            }

            if (this.ck_8.Checked)
            {
                // 下载怪物头像图
                var step7Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_MapCombine(this.txt_dir.Text));
                var step7End = DateTime.Now;
                AddLog($"完成 合并地图成大图，耗时：{(step7End - step7Start).TotalSeconds:F2}秒", Color.Green);
                this.img_8.Visible = true;
            }

            if (this.ck_4.Checked)
            {
                // 下载版本资源图
                var step4Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.DoneRes_Resversion(this.txt_dir.Text));
                var step4End = DateTime.Now;
                AddLog($"完成 下载版本资源图，耗时：{(step4End - step4Start).TotalSeconds:F2}秒", Color.Green);
                this.img_4.Visible = true;
            }


            //--------------------------以下切图-----------------------------------------------------
            if (this.ck_6.Checked)
            {
                // 图集切割
                var step6Start = DateTime.Now;
                await Task.Run(() => FormCutAtlasJson.ProcessDirectory(Path.Combine(this.txt_dir.Text, "resource")));
                var step6End = DateTime.Now;
                AddLog($"完成 图集切割，耗时：{(step6End - step6Start).TotalSeconds:F2}秒", Color.Green);
                this.img_6.Visible = true;
            }

            // 总耗时统计
            var totalEndTime = DateTime.Now;
            AddLog($"--------------全部资源下载处理完成！！总耗时：{(totalEndTime - totalStartTime).TotalHours:F2}小时，结束时间：{totalEndTime:yyyy-MM-dd HH:mm:ss}", Color.Green);

            // 启动资源管理器并指定目录
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{this.txt_dir.Text}\"", // 使用引号包裹路径，处理包含空格的路径
                UseShellExecute = true
            });
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            string targetUrl = "https://share.note.youdao.com/s/NvMOTG5"; // 替换为你要打开的 URL

            try
            {
                // 关键代码：调用系统默认浏览器打开 URL
                Process.Start(new ProcessStartInfo(targetUrl)
                {
                    UseShellExecute = true // 确保以Shell方式启动，适配不同系统
                });
            }
            catch (Exception ex)
            {
                // 异常处理：如 URL 格式错误、无默认浏览器等
                MessageBox.Show($"打开链接失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    public class Res_Custom
    {
        [JsonProperty("monster")]
        public Dictionary<string, Res_Custom_sub> Monsters { get; set; }
    }
    public class Res_Custom_sub
    {
        [JsonProperty("head")]
        public string Head { get; set; }
    }

    #endregion
}
