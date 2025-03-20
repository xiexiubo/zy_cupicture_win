using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public static class MessageDisplayer
    {
        public static void ShowMessage(string message, int durationInSeconds)
        {
            Thread messageThread = new Thread(() =>
            {
                Form messageForm = new Form();
                messageForm.FormBorderStyle = FormBorderStyle.None;
                messageForm.BackColor = Color.Black;
                messageForm.Opacity = 0.7;
                messageForm.TopMost = true;
                messageForm.ShowInTaskbar = false;

                Label messageLabel = new Label();
                messageLabel.Text = message;
                messageLabel.ForeColor = Color.White;
                messageLabel.Font = new Font("Arial", 24);
                messageLabel.AutoSize = true; // 启用自动调整大小
                messageLabel.Anchor = AnchorStyles.None;

                messageForm.Controls.Add(messageLabel);

                // 使用Load事件确保控件已完成布局
                messageForm.Load += (sender, e) =>
                {
                    // 计算并设置窗体大小（包含边距）
                    int padding = 20;
                    messageForm.ClientSize = new Size(
                        messageLabel.Width + padding,
                        messageLabel.Height + padding
                    );

                    // 居中标签
                    messageLabel.Location = new Point(
                        (messageForm.ClientSize.Width - messageLabel.Width) / 2,
                        (messageForm.ClientSize.Height - messageLabel.Height) / 2
                    );

                    // 获取鼠标当前位置
                    Point mousePosition = Control.MousePosition;
                    // 设置窗体位置为鼠标位置
                    messageForm.Location = new Point(mousePosition.X, mousePosition.Y);
                };

                // 持续时间定时器
                System.Windows.Forms.Timer durationTimer = new System.Windows.Forms.Timer();
                durationTimer.Interval = durationInSeconds * 1000;
                durationTimer.Tick += (s, e) =>
                {
                    durationTimer.Stop();

                    // 渐隐定时器
                    System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();
                    fadeTimer.Interval = 50;
                    fadeTimer.Tick += (fs, fe) =>
                    {
                        if (messageForm.Opacity > 0.05)
                        {
                            messageForm.Opacity -= 0.05;
                        }
                        else
                        {
                            fadeTimer.Stop();
                            messageForm.Close();
                        }
                    };
                    fadeTimer.Start();
                };
                durationTimer.Start();

                // 启动消息循环
                Application.Run(messageForm);
            });

            // 配置线程
            messageThread.SetApartmentState(ApartmentState.STA);
            messageThread.IsBackground = true;
            messageThread.Start();
        }
    }
}