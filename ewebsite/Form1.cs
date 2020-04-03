using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ewebsite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            var wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            wc.Headers.Add("cookie","__cfduid=da53676e4e2d0127fc0430b7a32b601141585825584");
            wc.Headers.Add("content-type","text/html; charset=UTF-8");
            byte[] vb=null;
            try
            {
                vb = wc.DownloadData($"https://e-hentai.org/?page={变量.页数}&f_cats=1021");
            }
            catch(Exception)
            {
                MessageBox.Show($"连不上,请再次尝试");
                return;
            }
            var dt = Encoding.UTF8.GetString(vb);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(dt);
            try
            {
                var htmlnode = doc.DocumentNode.SelectSingleNode($"/html[1]/body[1]/div[2]/div[2]/table[2]");
                HtmlNodeCollection s = htmlnode.SelectNodes("tr");
                ListViewItem kn0 = new ListViewItem();
                ListViewItem.ListViewSubItem kn1 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem kn2 = new ListViewItem.ListViewSubItem();
                int jia = 1;
                foreach (HtmlNode ls in s)
                {
                    if(jia==1)
                    {
                        jia = 0;
                        continue;
                    }
                    try
                    {
                        string[] st = ls.OuterHtml.Split('"');
                        变量.数据 k = new 变量.数据();
                        k.名称 = st[21];
                        k.图片 = st[25];
                        string[] npt = st[31].Split('=', '&', (char)39);
                        k.URL = $"https://e-hentai.org/g/{npt[2]}/{npt[4]}";
                        k.gid = npt[2];
                        k.时间 = st[34].Split('>', '<')[1];
                        k.kye = (++变量.缓存数量).ToString();
                        变量.缓存目录.Add(变量.缓存数量.ToString(), k);
                        kn0 = new ListViewItem();
                        kn0.Text = 变量.缓存数量.ToString();
                        kn1.Text = k.名称;
                        kn2.Text = k.URL;
                        kn0.SubItems.Add(kn1);
                        kn0.SubItems.Add(kn2);
                        listView1.Items.Add(kn0);
                    }
                    catch(Exception)
                    {
                        try
                        {
                            string[] st = ls.OuterHtml.Split('"');
                            变量.数据 k = new 变量.数据();
                            k.名称 = st[23];
                            k.图片 = st[27];
                            string[] npt = st[33].Split('=', '&', (char)39);
                            k.URL = $"https://e-hentai.org/g/{npt[2]}/{npt[4]}";
                            k.时间 = st[36].Split('>', '<')[1];
                            k.gid = npt[2];
                            k.kye = (++变量.缓存数量).ToString();
                            变量.缓存目录.Add(变量.缓存数量.ToString(), k);
                            kn0 = new ListViewItem();
                            kn0.Text = 变量.缓存数量.ToString();
                            kn1.Text = k.名称;
                            kn2.Text = k.URL;
                            kn0.SubItems.Add(kn1);
                            kn0.SubItems.Add(kn2);
                            listView1.Items.Add(kn0);
                        }
                        catch(Exception k)
                        {
                            MessageBox.Show($"致命错误:{k.Message}");
                        }
                    }
                }
                Text = $"当前第{++变量.页数}页";
            }
            catch (Exception)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "当前第0页";
            button8.Enabled = false;
            button9.Enabled = false;
            button4.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while(listView1.Items.Count!=0)listView1.Items[0].Remove();
            变量.缓存目录.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            while (listView2.Items.Count != 0) listView2.Items[0].Remove();
            变量.下载目录.Clear();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = false;
            if (listView1.SelectedItems.Count == 0)
            {
                button9.Enabled = false;
                button8.Enabled = false;
                return;
            }
            else
            {
                button9.Enabled = true;
                button8.Enabled = true;
            }
            if (listView1.SelectedItems[0].Index == listView1.Items.Count - 1)button8.Enabled = false;
            else button8.Enabled = true;
            if (listView1.SelectedItems.Count !=1) return;
            textBox1.Text = string.Empty;
            try
            {
                填充(listView1.SelectedItems[0].Text);
                button10.Enabled = true;
            }
            catch (Exception )
            {

            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Items[listView1.SelectedItems[0].Index+1].Selected = true;
            listView1.Items[listView1.SelectedItems[0].Index].Selected = false;
        }
        变量.数据 kbl = new 变量.数据();
        private void 填充(string key)
        {
            kbl = 变量.缓存目录[key];
            textBox1.AppendText($"标题:{kbl.名称}\r\n\r\n");
            textBox1.AppendText($"时间:{kbl.时间}\r\n\r\n");
            Thread.Sleep(500);
            var wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            wc.Headers.Add("cookie", "__cfduid=da53676e4e2d0127fc0430b7a32b601141585825584");
            wc.Headers.Add("content-type", "text/html; charset=UTF-8");
            byte[] vb;
            try
            {
                vb = wc.DownloadData($"{kbl.URL}");
            }
            catch(Exception)
            {
                MessageBox.Show($"连不上,请再次尝试");
                return;
            }
            var dt = Encoding.UTF8.GetString(vb);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(dt);
            try
            {
                var htmlnode = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/div[4]/div[1]/div[3]/table/tr[6]/td[2]");
                kbl.页面数=htmlnode.InnerText.Split(' ')[0];
                Convert.ToInt32(kbl.页面数);
                htmlnode = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/div[4]/div[1]/div[4]/table/tr[2]/td");
                kbl.评分 = htmlnode.InnerText.Split(':')[1];
                htmlnode = doc.DocumentNode.SelectSingleNode($"/html/body/div[6]");
                HtmlNodeCollection s = htmlnode.SelectNodes("div");
                foreach (HtmlNode ls in s)
                {
                    try
                    {
                        string kkk = ls.OuterHtml.Split('"')[5].Split('(', ')')[1];
                        try
                        {
                            if (kbl.其他信息1[kbl.其他信息1.Count - 1] != kkk) kbl.其他信息1.Add (kkk);
                        }
                        catch (Exception)
                        {
                            kbl.其他信息1.Add(kkk);
                        }

                        kbl.其他信息2.Add(ls.OuterHtml.Split('"')[7]);
                    }
                    catch (Exception)
                    {

                    }
                }
                变量.缓存目录[key] = kbl;
            }
            catch (Exception)
            {

            }
            变量.图片页数 = 0;
            textBox1.AppendText($"评分:{kbl.评分}\r\n\r\n");
            textBox1.AppendText($"页面数:{kbl.页面数}");
            pictureBox1.ImageLocation = $"{kbl.图片}";
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            变量.缓存目录.Remove(listView1.SelectedItems[0].ToString());
            int ls=listView1.SelectedItems[0].Index;
            listView1.SelectedItems[0].Remove();
            if (listView1.SelectedItems.Count >ls+1)
            {
                listView1.Items[ls].Selected = true;
            }
            else if(listView1.SelectedItems.Count !=0)
            {
                listView1.Items[ls - 1].Selected = true;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count != 1) return;
            变量.下载目录.Remove(listView2.SelectedItems[0].ToString());
            int ls = listView2.SelectedItems[0].Index;
            listView2.SelectedItems[0].Remove();
            if (listView2.SelectedItems.Count > ls + 1)
            {
                listView2.Items[ls].Selected = true;
            }
            else if (listView2.SelectedItems.Count != 0)
            {
                listView2.Items[ls - 1].Selected = true;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ListViewItem kn0 = new ListViewItem();
            ListViewItem.ListViewSubItem kn1 = new ListViewItem.ListViewSubItem();
            try
            {
                变量.下载目录.Add(listView1.SelectedItems[0].Text, kbl);
                kn0.Text = listView1.SelectedItems[0].Text;
                kn1.Text = "等待";
                kn0.SubItems.Add(kn1);
                listView2.Items.Add(kn0);
            }
            catch(Exception)
            {

            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (变量.图片页数 >= kbl.其他信息1.Count) 变量.图片页数 = 0;
            try
            {
                pictureBox1.ImageLocation = kbl.其他信息1[变量.图片页数++];
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*button5.Enabled = false;
             button6.Enabled = false;
             button3.Enabled = false;
             button4.Enabled = true;
             try
             {
                 变量.download = new ManualResetEvent(true);
                 变量.download.Set();
             }
             catch (Exception)
             {

             }
             */
            for (int i=0;i<listView2.Items.Count;i++)
            {
                try
                {
                    变量.数据 kilikili = 变量.下载目录[listView2.Items[i].Text];
                    Thread k = new Thread(kilikili.下载);
                    k.Start();
                    变量.下载目录.Remove(listView2.Items[i].Text);
                }
                catch(Exception)
                {

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
            button5.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            try
            {
                变量.download = new ManualResetEvent(false);
            }
            catch (Exception)
            {

            }
            */
        }
    }
    class 变量
    {
        public static ManualResetEvent download;
        public static int 缓存数量 = 0;
        public static int 图片页数 = 0;
        public static Dictionary<string, 数据> 缓存目录 = new Dictionary<string, 数据>();
        public static Dictionary<string, 数据> 下载目录 = new Dictionary<string, 数据>();
        public class 数据
        {
            public string 名称;
            public string 图片;
            public string 评分;
            public string 页面数;
            public string 时间;
            public string URL;
            public string gid;
            public List<string> 其他信息1 = new List<string>();
            public List<string> 其他信息2 = new List<string>();
            public string kye;
            private void dfs(int l,string ll)
            {
                if (l >= 10) return;
                Thread.Sleep(2000);
                try
                {
                    //download.WaitOne();
                    Thread.Sleep(500);
                    var wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    wc.Headers.Add("cookie", "__cfduid=da53676e4e2d0127fc0430b7a32b601141585825584");
                    wc.Headers.Add("content-type", "text/html; charset=UTF-8");
                    byte[] vb;
                    try
                    {
                        vb = wc.DownloadData($"{ll}");
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    var dt = Encoding.UTF8.GetString(vb);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(dt);
                    var htmlnode = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[2]/img");
                    WebRequest request = WebRequest.Create(htmlnode.InnerText.Split(' ')[0]);
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream reader = response.GetResponseStream())
                        {
                            Directory.CreateDirectory($"{Application.StartupPath}\\down\\{gid}");
                            if (File.Exists($"{Application.StartupPath}\\down\\{gid}\\{ll.Split('-')[2]}.jpg"))File.Delete($"{Application.StartupPath}\\down\\{gid}\\{ll.Split('-')[2]}.jpg");
                            using (FileStream writer = new FileStream($"{Application.StartupPath}\\down\\{gid}\\{ll.Split('-')[2]}.jpg", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                byte[] buff = new byte[512];
                                int c = 0; //实际读取的字节数
                                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                                {
                                    download.WaitOne();
                                    writer.Write(buff, 0, c);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    dfs(l+1,ll);
                }
            }
            public void 下载()
            {
                foreach(string ul in 其他信息2)
                {
                    try
                    {
                        //download.WaitOne();
                        Thread.Sleep(500);
                        var wc = new WebClient();
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        wc.Headers.Add("cookie", "__cfduid=da53676e4e2d0127fc0430b7a32b601141585825584");
                        wc.Headers.Add("content-type", "text/html; charset=UTF-8");
                        byte[] vb;
                        try
                        {
                            vb = wc.DownloadData($"{ul}");
                        }
                        catch (Exception)
                        {
                            return;
                        }
                        var dt = Encoding.UTF8.GetString(vb);
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(dt);
                        var htmlnode = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[2]/a");
                        WebRequest request = WebRequest.Create(htmlnode.InnerHtml.Split('"')[3]);
                        using (WebResponse response = request.GetResponse())
                        {
                            using (Stream reader = response.GetResponseStream())
                            {
                                Directory.CreateDirectory($"{Application.StartupPath}\\down\\{gid}");
                                if (File.Exists($"{Application.StartupPath}\\down\\{gid}\\{ul.Split('-')[2]}.jpg")) File.Delete($"{Application.StartupPath}\\down\\{gid}\\{ul.Split('-')[2]}.jpg");
                                using (FileStream writer = new FileStream($"{Application.StartupPath}\\down\\{gid}\\{ul.Split('-')[2]}.jpg", FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    byte[] buff = new byte[512];
                                    int c = 0; //实际读取的字节数
                                    while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                                    {
                                        writer.Write(buff, 0, c);
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception )
                    {
                        try
                        {
                            dfs(1, ul);
                        }
                        catch(Exception a)
                        {
                            MessageBox.Show($"ID-{kye}错误:{a.Message}");
                            continue;
                        }
                    }
                }
                Form1 form1 = new Form1();
                for (int i = 0; i < form1.listView2.Items.Count; i++)
                {
                    if (form1.listView2.Items[i].Text == kye)
                    {
                        form1.listView2.Items[i].Remove();
                        break;
                    }
                }
            }
        }
        public static int 页数 = 0;
    }
}
