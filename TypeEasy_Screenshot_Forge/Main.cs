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

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            int speed = (int)ExtractConsecutiveIntegers(this.textBox3.Text);
            this.label1.Text = ((int)(speed * (ExtractConsecutiveIntegers(this.textBox4.Text) * 0.01))).ToString() + "字/分";
            int value = (int)ExtractConsecutiveIntegers(this.textBox3.Text);  // 只调用一次

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
            else if (value >= 100)
            {
                this.label2.Text = "三级（共十级）";
                pictureBox2.Image = Image.FromStream(new MemoryStream(Properties.Resources.三级));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3_TextChanged(textBox3, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 获取 pictureBox1 的位置和大小
            Rectangle rect = pictureBox1.Bounds;

            // 创建一个与控件区域大小相同的 Bitmap 对象
            Bitmap screenshot = new Bitmap(rect.Width, rect.Height);

            // 创建一个 Graphics 对象用于从屏幕复制图像
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                // 获取控件所在区域的屏幕截图
                g.CopyFromScreen(pictureBox1.PointToScreen(Point.Empty), Point.Empty, rect.Size);
            }

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
        }
    }
}
