using System;
using System.IO;
using System.Windows.Forms;

namespace ewebsite
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            变量.IniFiles ini = new 变量.IniFiles($"{Application.StartupPath}\\set.ini");
            if (!ini.ExistINIFile())
            {
                File.Create(ini.inipath);
                ini.IniWriteValue("基础设置", "重试次数", textBox1.Text);
                ini.IniWriteValue("基础设置", "重试毫秒", textBox2.Text);
            }
            else
            {
                ini.IniWriteValue("基础设置", "重试次数", textBox1.Text);
                ini.IniWriteValue("基础设置", "重试毫秒", textBox2.Text);
            }
            MessageBox.Show("已经更改,请重启软件以得到更新");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            变量.IniFiles ini = new 变量.IniFiles($"{Application.StartupPath}\\set.ini");
            if (!ini.ExistINIFile())
            {
                File.Create(ini.inipath);
                ini.IniWriteValue("基础设置", "重试次数", 变量.基本设置.重试次数.ToString());
                ini.IniWriteValue("基础设置", "重试毫秒", 变量.基本设置.重试毫秒.ToString());
                textBox1.Text = ini.IniReadValue("基础设置", "重试次数");
                textBox2.Text = ini.IniReadValue("基础设置", "重试毫秒");
            }
            else
            {
                textBox1.Text = ini.IniReadValue("基础设置", "重试次数");
                textBox2.Text = ini.IniReadValue("基础设置", "重试毫秒");
            }
        }
    }
}
