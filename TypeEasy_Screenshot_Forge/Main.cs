﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.IO;
using System.Threading;

namespace TypeEasy_Screenshot_Forge
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public static long ExtractConsecutiveIntegers(string input)
        {
            long result = 0;
            var matches = Regex.Matches(input, @"\d+");

            foreach (Match match in matches)
            {
                // 将每个匹配的整数拼接起来
                result = result * (long)Math.Pow(10, match.Length) + long.Parse(match.Value);
            }

            return result;
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            // 可以在加载时执行一些异步操作，例如加载数据
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                int speed = (int)ExtractConsecutiveIntegers(this.textBox3.Text);
                _ = this.Invoke((Action)(() =>
                {
                    this.label1.Text = ((int)(speed * (ExtractConsecutiveIntegers(this.textBox4.Text) * 0.01))).ToString() + "字/分钟";
                    int value = (int)ExtractConsecutiveIntegers(this.textBox3.Text);

                    if (value < 50)
                    {
                        this.label2.Text = "一级（共十级）";
                        pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.一级));
                    }
                    else if (value < 100)
                    {
                        this.label2.Text = "二级（共十级）";
                        pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.二级));
                    }
                    else if (value < 150)
                    {
                        this.label2.Text = "三级（共十级）";
                        pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.三级));
                    }
                    else if (value < 200)
                    {
                        this.label2.Text = "四级（共十级）";
                        pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.四级));
                    }
                    else if (value >= 200)
                    {
                        this.label2.Text = "五级（共十级）";
                        pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.五级));
                    }
                    // 获取文章字数和时间
                    int wordCount = (int)ExtractConsecutiveIntegers(this.textBox2.Text);
                    int duration = (int)ExtractConsecutiveIntegers(this.textBox3.Text);
                    if (wordCount==0 || duration ==0){
                        label3.Text = "0分0秒";
                        return;
                    }
                    // 计算结果
                    double time = (double)wordCount / duration; // 每单位时间的字数
                    int time_min = (int)time; // 取整的分钟数
                    int time_sec = (int)((time - time_min) * 60); // 计算剩余秒数

                    // 显示结果
                    this.label3.Text = $"{time_min}分{time_sec}秒"; // 显示分钟和秒



                }));
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                textBox3_TextChanged(textBox3, EventArgs.Empty);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Random rand = new Random();
                int words = randint(1000, 1500, rand);
                int speed = randint(25, 50, rand);
                int rate = randint(95, 100, rand);

                this.Invoke((Action)(() =>
                {
                    this.textBox2.Text = words.ToString() + '字';
                    this.textBox3.Text = speed.ToString() + "字/分";
                    this.textBox4.Text = rate.ToString() + '%';
                }));
            });
        }

        static int randint(int minValue, int maxValue, Random rand)
        {
            return rand.Next(minValue, maxValue + 1); // maxValue + 1因为Next的上限是最大值不包括maxValue
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // 异步执行截图任务
            await Task.Run(() =>
            {
                // 使用Invoke将操作调度到UI线程
                Rectangle rect = (Rectangle)this.Invoke((Func<Rectangle>)(() => pictureBox1.Bounds));

                // 创建一个与控件区域大小相同的 Bitmap 对象
                Bitmap screenshot = new Bitmap(rect.Width, rect.Height);

                // 创建一个 Graphics 对象用于从屏幕复制图像
                using (Graphics g = Graphics.FromImage(screenshot))
                {
                    // 使用Invoke获取 pictureBox1 的屏幕坐标
                    Point location = (Point)this.Invoke((Func<Point>)(() => pictureBox1.PointToScreen(Point.Empty)));

                    // 获取控件所在区域的屏幕截图
                    g.CopyFromScreen(location, Point.Empty, rect.Size);
                }

                // 创建一个新的 STA 线程来处理文件对话框
                Thread staThread = new Thread(() =>
                {
                    // 创建并显示文件保存对话框
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        FileName = "捕获.png", // 默认文件名
                        Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*", // 文件类型过滤
                        DefaultExt = "png", // 默认扩展名
                        AddExtension = true // 自动添加扩展名
                    };

                    // 显示保存文件对话框，如果用户选择了文件
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // 将截图保存为 PNG 文件
                            screenshot.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            MessageBox.Show("截图已保存", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"保存文件时发生错误: {ex.Message}", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                });

                // 设置为 STA 模式
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();  // 等待线程完成
            });
        }



        private void button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Random rand = new Random();
                int words = randint(1000, 3000, rand);
                int speed = randint(50, 100, rand);
                int rate = randint(95, 100, rand);

                this.Invoke((Action)(() =>
                {
                    this.textBox2.Text = words.ToString() + '字';
                    this.textBox3.Text = speed.ToString() + "字/分";
                    this.textBox4.Text = rate.ToString() + '%';
                }));
            });
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                textBox3_TextChanged(textBox3, EventArgs.Empty);
            });
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                textBox3_TextChanged(textBox3, EventArgs.Empty);
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "字数、速度、正确率都可以自己点击输入\n" +
                "总速度、时间和等级是自动计算，不可输入\n" +
                "嫌麻烦的可以直接使用预设，只需要改个字数就行\n" +
                "如果出现计算错误请点击刷新\n" +
                "直接按保存图片就可以截图了\n\n" +
                "github.com/kndxhz/TypeEasy_Screenshot_Forge",
                "帮助", // 标题
                MessageBoxButtons.OK, // 按钮
                MessageBoxIcon.Question // 问号图标
            );

        }
    }
}
