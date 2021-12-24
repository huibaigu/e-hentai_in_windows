using System;
using System.IO;
using System.Windows.Forms;
using static ewebsite.变量;

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
            XMLFiles xml = new XMLFiles($"{Application.StartupPath}\\set.xml");
            if (!xml.ExistINIFile())File.Create(xml.xmlName).Close();
            xml.setXmlValue("重试次数", textBox1.Text);
            xml.setXmlValue("重试毫秒", textBox2.Text);
            MessageBox.Show("已经更改,请重启软件以得到更新");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            XMLFiles xml = new XMLFiles($"{Application.StartupPath}\\set.xml");
            if (!xml.ExistINIFile())
            {
                File.Create(xml.xmlName).Close();
                xml.setXmlValue("重试次数", 基本设置.重试次数.ToString());
                xml.setXmlValue("重试毫秒", 基本设置.重试毫秒.ToString());
            }
            textBox1.Text = xml.getXmlValue("基本设置", "重试次数");
            textBox2.Text = xml.getXmlValue("基本设置", "重试毫秒");
        }
    }
}
