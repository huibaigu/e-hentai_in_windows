using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static ewebsite.变量;

namespace ewebsite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void 线程方法(object s)
        {
            数据 k= (数据) s;
            CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(500);
            var wc1 = new WebClient();
            wc1.Credentials = CredentialCache.DefaultCredentials;
            wc1.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            wc1.Headers.Add("cookie", "__cfduid=da53676e4e2d0127fc0430b7a32b601141585825584");
            wc1.Headers.Add("content-type", "text/html; charset=UTF-8");
            byte[] vb1 = null;
            try
            {
                vb1 = wc1.DownloadData($"{k.URL}");
            }
            catch (Exception)
            {
                for (int i = 1; i <= 基本设置.重试次数; i++)
                {
                    try
                    {
                        Thread.Sleep(基本设置.重试毫秒);
                        vb1 = wc1.DownloadData($"{k.URL}");
                    }
                    catch (Exception)
                    {
                        if (i != 基本设置.重试次数) continue;
                        MessageBox.Show($"连不上,请再次尝试");
                        return;
                    }
                    break;
                }
            }
            var dt1 = Encoding.UTF8.GetString(vb1);
            HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
            doc1.LoadHtml(dt1);
            try
            {
                var htmlnode1 = doc1.DocumentNode.SelectSingleNode($"/html/body/div[2]/div[4]/div[1]/div[3]/table/tr[6]/td[2]");
                k.页面数 = htmlnode1.InnerText.Split(' ')[0];
                Convert.ToInt32(k.页面数);
                htmlnode1 = doc1.DocumentNode.SelectSingleNode($"/html/body/div[2]/div[4]/div[1]/div[4]/table/tr[2]/td");
                k.评分 = htmlnode1.InnerText.Split(':')[1];
                htmlnode1 = doc1.DocumentNode.SelectSingleNode($"/html/body/div[6]");
                HtmlNodeCollection s1 = htmlnode1.SelectNodes("div");
                foreach (HtmlNode ls1 in s1)
                {
                    try
                    {
                        string kkk = ls1.OuterHtml.Split('"')[5].Split('(', ')')[1];
                        下载图片 临时 = new 下载图片();
                        临时.下载url = kkk;
                        临时.保存路径 = Application.StartupPath + "\\temp\\" + k.gid + kkk.Split('-')[1];
                        if (!k.其他信息1.Contains(临时.保存路径))
                        {
                            k.其他信息1.Add(临时.保存路径);
                            if(!File.Exists(临时.保存路径)) 临时.DownPic();
                        }
                        k.其他信息2.Add(ls1.OuterHtml.Split('"')[7]);
                    }
                    catch (Exception)
                    {

                    }
                }
                缓存目录.Add(k.kye.ToString(), k);
            }
            catch (Exception l)
            {
                textBox2.AppendText($"错误编号2--错误信息:[{k.URL}]{l.Message}\r\n");
            }
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
                vb = wc.DownloadData($"https://e-hentai.org/?page={页数}&f_cats=1021");
            }
            catch(Exception)
            {
                for(int i=1;i<= 基本设置.重试次数;i++)
                {
                    try
                    {
                        Thread.Sleep(基本设置.重试毫秒);
                        vb = wc.DownloadData($"https://e-hentai.org/?page={页数}&f_cats=1021");
                    }
                    catch (Exception)
                    {
                        if (i != 基本设置.重试次数) continue;
                        MessageBox.Show($"连不上,请再次尝试");
                        return;
                    }
                    break;
                }
            }
            var dt = Encoding.UTF8.GetString(vb);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(dt);
            try
            {
                var htmlnode = doc.DocumentNode.SelectSingleNode($"/html[1]/body[1]/div[2]/div[2]/table[2]");
                HtmlNodeCollection s = htmlnode.SelectNodes("tr");
                ListViewItem kn0 = new ListViewItem();
                foreach (HtmlNode ls in s)
                {
                    try
                    {
                        string[] st = ls.OuterHtml.Split('"');
                        数据 k = new 数据();
                        try
                        {
                            k.名称 = st[21];
                            k.图片 = st[25];
                            string[] npt = st[31].Split('=', '&', (char)39);
                            k.URL = $"https://e-hentai.org/g/{npt[2]}/{npt[4]}";
                            k.时间 = st[34].Split('>', '<')[1];
                            k.gid = npt[2];
                            k.kye = (++缓存数量).ToString();
                        }
                        catch
                        {
                            k.名称 = st[23];
                            k.图片 = st[27];
                            string[] npt = st[33].Split('=', '&', (char)39);
                            k.URL = $"https://e-hentai.org/g/{npt[2]}/{npt[4]}";
                            k.时间 = st[36].Split('>', '<')[1];
                            k.gid = npt[2];
                            k.kye = (++缓存数量).ToString();
                        }
                        kn0 = new ListViewItem();
                        kn0.Text = 缓存数量.ToString();
                        kn0.SubItems.Add(k.名称);
                        kn0.SubItems.Add(k.URL);
                        listView1.Items.Add(kn0);
                        //////////////////////////////////
                        //down1(k);
                        Thread ssa = new Thread(线程方法);
                        ssa.IsBackground = true;
                        ssa.Start(k);
                        //////////////////////////////////下载所有略缩图
                    }
                    catch(Exception)
                    {

                    }
                }
                Text = $"当前第{++页数}页";
            }
            catch (Exception k)
            {
                textBox2.AppendText($"错误编号1--错误信息:{k.Message}\r\n");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "当前第0页";
            button8.Enabled = false;
            button9.Enabled = false;
            Directory.CreateDirectory($"{Application.StartupPath}\\temp");
            Directory.CreateDirectory($"{Application.StartupPath}\\down");
            XMLFiles xml = new XMLFiles($"{Application.StartupPath}\\set.xml");
            if (!xml.ExistINIFile())
            {
                xml.CreateXmlFile();
                xml.setXmlValue("重试次数", 基本设置.重试次数.ToString());
                xml.setXmlValue("重试毫秒", 基本设置.重试毫秒.ToString());
            }
            else
            {
                基本设置.重试次数 = Convert.ToInt32(xml.getXmlValue("基本设置", "重试次数"));
                基本设置.重试毫秒 = Convert.ToInt32(xml.getXmlValue("基本设置", "重试毫秒"));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            while(listView1.Items.Count!=0)listView1.Items[0].Remove();
            缓存目录.Clear();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = false;
            button9.Enabled = false;
            button8.Enabled = false;
            if (listView1.SelectedItems.Count == 0)return;
            button9.Enabled = true;
            button8.Enabled = true;
            if (listView1.SelectedItems[0].Index == listView1.Items.Count - 1)button8.Enabled = false;
            if (listView1.SelectedItems.Count !=1) return;
            textBox1.Text = string.Empty;
            填充(listView1.SelectedItems[0].Text);
            button10.Enabled = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Items[listView1.SelectedItems[0].Index+1].Selected = true;
            listView1.Items[listView1.SelectedItems[0].Index].Selected = false;
        }
        数据 选中项数据 = new 数据();
        private void 填充(string key)
        {
            选中项数据 = 缓存目录[key];
            图片页数 = 0;
            textBox1.AppendText($"标题:{选中项数据.名称}\r\n\r\n");
            textBox1.AppendText($"时间:{选中项数据.时间}\r\n\r\n");
            textBox1.AppendText($"评分:{选中项数据.评分}\r\n\r\n");
            textBox1.AppendText($"GID:{选中项数据.gid}\r\n\r\n");
            textBox1.AppendText($"ID:{选中项数据.kye}\r\n\r\n");
            textBox1.AppendText($"页面数:{选中项数据.页面数}");
            pictureBox1.ImageLocation = $"{选中项数据.图片}";
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            缓存目录.Remove(listView1.SelectedItems[0].ToString());
            int 索引=listView1.SelectedItems[0].Index;
            listView1.SelectedItems[0].Remove();
            if (listView1.SelectedItems.Count > 索引 + 1)
            {
                listView1.Items[索引].Selected = true;
            }
            else if(listView1.SelectedItems.Count !=0)
            {
                listView1.Items[索引 - 1].Selected = true;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                下载目录.Add(listView1.SelectedItems[0].Text, 选中项数据);
                ListViewItem 项 = new ListViewItem(listView1.SelectedItems[0].Text);
                项.SubItems.Add("等待");
                listView2.Items.Add(项);
                Thread 线程 = new Thread(选中项数据.下载);
                线程.IsBackground = true;
                线程.Start();
            }
            catch(Exception)
            {
                MessageBox.Show("重复添加");
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (图片页数 >= 选中项数据.其他信息1.Count) 图片页数 = 0;
            try
            {
                pictureBox1.Image = Image.FromFile(选中项数据.其他信息1[图片页数++]);
            }
            catch(Exception)
            {
                MessageBox.Show("请稍等,图片正在缓冲中");
            }
        }
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.AppendText("未避免无法打开浏览器,请手动打开网页:<https://github.com/huibaigu/e-hentai_in_windows>\r\n");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox1()).ShowDialog();
        }
    }
    class 变量
    {
        public static class 基本设置
        {
            /// <summary>  
            /// 重试次数
            /// </summary>
            public static int 重试次数 = 1;
            /// <summary>  
            /// 重试暂停多少毫秒
            /// </summary>
            public static int 重试毫秒 = 1000;
        }
        /// <summary>  
        /// 缓存了几本
        /// </summary>
        public static int 缓存数量 = 0;
        /// <summary>  
        /// 当前临时浏览到略缩图的第几页
        /// </summary>
        public static int 图片页数 = 0;
        /// <summary>  
        /// 所有的本的数据
        /// </summary>
        public static Dictionary<string, 数据> 缓存目录 = new Dictionary<string, 数据>();
        /// <summary>  
        /// 当前正在下载的本数据
        /// </summary>
        public static Dictionary<string, 数据> 下载目录 = new Dictionary<string, 数据>();
        public class 数据
        {
            public string 名称;
            /// <summary>  
            /// 封面url地址
            /// </summary>
            public string 图片;
            public string 评分;
            public string 页面数;
            public string 时间;
            /// <summary>  
            /// 详情页面的url地址
            /// </summary>
            public string URL;
            /// <summary>  
            /// 在网上的GID
            /// </summary>
            public string gid;
            /// <summary>  
            /// 略缩图信息
            /// </summary>
            public List<string> 其他信息1 = new List<string>();
            /// <summary>  
            /// 每一张的信息
            /// </summary>
            public List<string> 其他信息2 = new List<string>();
            /// <summary>  
            /// 在软件中的ID
            /// </summary>
            public string kye;
            public void 下载()
            {
                Form1 form1 = new Form1();
                if (Directory.Exists($"{Application.StartupPath}\\down\\{gid}")) return;
                foreach (string ul in 其他信息2)
                {
                    try
                    {
                        下载图片 临时 = new 下载图片();
                        临时.下载url = ul;
                        Directory.CreateDirectory($"{Application.StartupPath}\\down\\{gid}");
                        临时.保存路径 = $"{Application.StartupPath}\\down\\{gid}\\{ul.Split('-')[2]}.jpg";
                        if (File.Exists($"{Application.StartupPath}\\down\\{gid}\\{ul.Split('-')[2]}.jpg")) continue;
                        临时.DownPic();
                    }
                    catch(Exception l)
                    {
                        form1.textBox2.AppendText($"错误编号3-[已经再次添加到下载列表]-错误信息:[{URL}]{l.Message}\r\n");
                        其他信息2.Add(ul);
                    }
                }
                for (int i = 0; i < form1.listView2.Items.Count; i++)
                {
                    if (form1.listView2.Items[i].Text == kye)
                    {
                        form1.listView2.Items[i].Remove();
                    }
                }
            }
        }
        /// <summary>  
        /// 当前获取到第几页了  
        /// </summary>
        public static int 页数 = 0;
        /// <summary>  
        /// 异步下载图片  
        /// </summary>
        public class 下载图片
        {
            /// <summary>  
            /// 下载url  
            /// </summary>  
            public string 下载url { get; set; }
            /// <summary>  
            /// 保存路径  
            /// </summary>  
            public string 保存路径 { get; set; }
            /// <summary>  
            /// 异步读取流的回调函数  
            /// </summary>  
            /// <param name="asyncResult">用于在回调函数当中传递操作状态</param>  
            private void ReadCallback(IAsyncResult asyncResult)
            {
                RequestState requestState = (RequestState)asyncResult.AsyncState;
                int read = requestState.ResponseStream.EndRead(asyncResult);
                if (read > 0)
                {
                    //将缓冲区的数据写入该文件流  
                    requestState.FileStream.Write(requestState.BufferRead, 0, read);
                    //开始异步读取流  
                    requestState.ResponseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length, ReadCallback, requestState);
                }
                else
                {
                    requestState.Response.Close();
                    requestState.FileStream.Close();
                }
            }
            /// <summary>  
            /// 请求资源方法的回调函数  
            /// </summary>  
            /// <param name="asyncResult">用于在回调函数当中传递操作状态</param>  
            private void ResponseCallback(IAsyncResult asyncResult)
            {
                RequestState requestState = (RequestState)asyncResult.AsyncState;
                requestState.Response = (HttpWebResponse)requestState.Request.EndGetResponse(asyncResult);
                Stream responseStream = requestState.Response.GetResponseStream();
                requestState.ResponseStream = responseStream;
                //开始异步读取流  
                responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length, ReadCallback, requestState);
            }
            /// <summary>  
            /// 异步下载图片
            /// </summary>  
            public void DownPic()
            {
                //------------------------开始异步下载图片
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(下载url));
                //设置下载相关参数  
                RequestState requestState = new RequestState();
                requestState.BUFFER_SIZE = 1024;
                requestState.BufferRead = new byte[requestState.BUFFER_SIZE];
                requestState.Request = request;
                requestState.SavePath = 保存路径;
                requestState.FileStream = new FileStream(requestState.SavePath, FileMode.OpenOrCreate);
                //开始异步请求资源
                request.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
            }
        }
        /// <summary>  
        /// 请求状态  
        /// </summary>  
        public class RequestState
        {
            /// <summary>  
            /// 缓冲区大小  
            /// </summary>  
            public int BUFFER_SIZE { get; set; }
            /// <summary>  
            /// 缓冲区  
            /// </summary>  
            public byte[] BufferRead { get; set; }
            /// <summary>  
            /// 保存路径  
            /// </summary>  
            public string SavePath { get; set; }
            /// <summary>  
            /// 请求流  
            /// </summary>  
            public HttpWebRequest Request { get; set; }
            /// <summary>  
            /// 响应流  
            /// </summary>  
            public HttpWebResponse Response { get; set; }
            /// <summary>  
            /// 流对象  
            /// </summary>  
            public Stream ResponseStream { get; set; }
            /// <summary>  
            /// 文件流  
            /// </summary>  
            public FileStream FileStream { get; set; }
        }
        public class XMLFiles
        {
            public string xmlName;
            /// <summary> 
            /// 构造方法 
            /// </summary> 
            /// <param name="XMLName">文件路径</param> 
            public XMLFiles(string XMLName)
            {
                xmlName = XMLName;
            }
            public XMLFiles() { }
            /// <summary>  
            /// 设置XMl文件指定元素的指定属性的值  
            /// </summary>  
            /// <param name="xmlElement">指定元素</param>  
            /// <param name="xmlAttribute">指定属性</param>  
            /// <param name="xmlValue">指定值</param>  
            public void setXmlValue(string xmlAttribute, string xmlValue)
            {
                XDocument xmlDoc = XDocument.Load(xmlName);
                xmlDoc.Element("设置").Element("基本设置").Element(xmlAttribute).SetValue(xmlValue);
                xmlDoc.Save(xmlName);
            }
            /// <summary>  
            /// 返回XMl文件指定元素的指定属性值  
            /// </summary>  
            /// <param name="xmlElement">指定元素</param>  
            /// <param name="xmlAttribute">指定属性</param>  
            /// <returns></returns>  
            public string getXmlValue(string xmlElement, string xmlAttribute)
            {
                XDocument xmlDoc = XDocument.Load(xmlName);
                var results = from c in xmlDoc.Descendants(xmlAttribute) select c;
                string s = "";
                foreach (var result in results)
                {
                    s = result.Value.ToString();
                }
                return s;
            }
            /// <summary>  
            /// 创建xml
            /// </summary>  
            /// <returns></returns>
            public void CreateXmlFile()
            {
                XDocument document = new XDocument();
                XElement root = new XElement("设置");
                XElement book = new XElement("基本设置");
                book.SetElementValue("重试次数", 基本设置.重试次数.ToString());
                book.SetElementValue("重试毫秒", 基本设置.重试毫秒.ToString());
                root.Add(book);
                root.Save(xmlName);
            }
            /// <summary>    
            /// 创建节点    
            /// </summary>    
            /// <param name="xmldoc"></param>  xml文档  
            /// <param name="parentnode"></param>父节点    
            /// <param name="name"></param>  节点名  
            /// <param name="value"></param>  节点值  
            public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
            {
                XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
                node.InnerText = value;
                parentNode.AppendChild(node);
            }
            /// <summary> 
            /// 验证文件是否存在 
            /// </summary> 
            /// <returns>布尔值</returns> 
            public bool ExistINIFile()
            {
                return File.Exists(xmlName);
            }
        }
    }
}
