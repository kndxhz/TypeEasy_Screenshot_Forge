using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TypeEasy_Screenshot_Forge
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        
        public static List<int> ExtractConsecutiveIntegers(string input)
        {
            var integers = new List<int>();

            // 使用正则表达式匹配连续的数字
            var matches = Regex.Matches(input, @"\d+");

            foreach (Match match in matches)
            {
                // 将每个匹配的字符串转换为整数并添加到列表中
                integers.Add(int.Parse(match.Value));
            }

            return integers;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }


    }
}
