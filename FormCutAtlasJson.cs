using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace zy_cutPicture
{
    public partial class FormCutAtlasJson : Form
    {  
       
        public FormCutAtlasJson()
        {
            InitializeComponent();
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
        public class AllManifest 
        {
            public Dictionary<string, AllManifest_Value> Resources { get; set; }

        }
        public static T LoadJsonResource<T>(string resourceName) where T : new()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        Console.WriteLine($"资源 '{resourceName}' 未找到。");
                        return new T();
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载资源失败: {ex.Message}");
                return new T();
            }
        }
        static void ReadConfig_AllManifest()
        {       
            try
            {    //https://cdn.ascq.zlm4.com/aoshi_20240419/allmanifest1.24591.2.json?v=20250530185809?qufu_version=20
                // 反序列化为配置对象
                AllManifest config = LoadJsonResource<AllManifest>("Resources/allmanifest1.24591.2.json");

                if (config == null)
                {
                    throw new InvalidOperationException("配置文件内容为空或格式不正确");
                }
                foreach ( var kvp in config.Resources) 
                {
                    Console.WriteLine($"文件名: {kvp.Key}   {kvp.Value.v}    {kvp.Value}");
                }

            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("解析JSON配置文件时出错", ex);
            }
        }
        public static bool parse(string json)
        {
            if (parse1(json)) return true;
            if (parse2(json)) return true;
            return false;
        }
        private static bool parse1(string json)
        {
            try
            {
                JsonData1 data = JsonConvert.DeserializeObject<JsonData1>(json);

                Console.WriteLine($"文件名: {data.file}");

                foreach (var frame in data.frames)
                {
                    // Console.WriteLine($"帧名称: {frame.Key}");
                    Console.WriteLine($"位置: ({frame.x}, {frame.y})");
                    Console.WriteLine($"尺寸: {frame.w}x{frame.h}");
                    Console.WriteLine($"偏移: ({frame.offX}, {frame.offY})");
                    Console.WriteLine($"源尺寸: {frame.sourceW}x{frame.sourceH}");
                    Console.WriteLine();
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
        private static bool parse2(string json)
        {
            try
            {
                JsonData2 data = JsonConvert.DeserializeObject<JsonData2>(json);

                Console.WriteLine($"文件名: {data.file}");

                foreach (var frame in data.frames)
                {
                    Console.WriteLine($"帧名称: {frame.Key}");
                    Console.WriteLine($"位置: ({frame.Value.x}, {frame.Value.y})");
                    Console.WriteLine($"尺寸: {frame.Value.w}x{frame.Value.h}");
                    Console.WriteLine($"偏移: ({frame.Value.offX}, {frame.Value.offY})");
                    Console.WriteLine($"源尺寸: {frame.Value.sourceW}x{frame.Value.sourceH}");
                    Console.WriteLine();
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

        private void btn_allmanifest_Click(object sender, EventArgs e)
        {
            ReadConfig_AllManifest();
        }
    }




    public class FrameData
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public int offX { get; set; }
        public int offY { get; set; }
        public int sourceW { get; set; }
        public int sourceH { get; set; }
    }
    /// <summary>
    /// 模型 角色 解读
    /// </summary>
    public class JsonData1
    {
        public int count { get; set; }
        public string file { get; set; }
        public List<FrameData> frames { get; set; }
       
    }
    /// <summary>
    /// ui 解读
    /// </summary>
    public class JsonData2
    {
        public string file { get; set; }
        public Dictionary<string, FrameData> frames { get; set; }
      
    }

}
