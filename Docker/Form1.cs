using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.DotNet.X509;
using net.openstack.Core.Domain;
using net.openstack.Core.Providers;
using OpenStack.Compute.v2_1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Docker.OpenStackUserentity;

namespace Docker
{
    public partial class Form1 : Form
    {
        private string loginmsg = string.Empty;
        Thread checklogin;
        Thread changemenu;
        DockerUserControllerEntities context;
        List<string> GetConnetcFromForm = new List<string>();
        List<List<string>> DockerConnectionStrs = new List<List<string>>();
        List<List<string>> OpenstackConnectionStrs = new List<List<string>>();
        DockerClient client;
        public bool conOpenstack = false;
        List<string> conOpenS = new List<string>();
        DataTable OpenstackUserDT = new DataTable();
        List<string> willadd = new List<string>();
        public string useOpenstackuser = string.Empty;
        public string useOpenstackpass = string.Empty;
        public string useOpenstackhttp = string.Empty;
        public string selectedopensuser = string.Empty;
        public string openstackloginauth = string.Empty;
        OpenStackUserInfo osui = null;
        List<UsersItem> osusers = null;
        List<OpenStackImage> imagelist = null;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            checklogin = new Thread(checkonline);
            checklogin.Start();
            changemenu = new Thread(changelogin);
            changemenu.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            用户ToolStripMenuItem1.Visible = false;
            注销ToolStripMenuItem.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            DataColumn OpenstackDC1 = new DataColumn("username", Type.GetType("System.String"));
            DataColumn OpenstackDC2 = new DataColumn("id", Type.GetType("System.String"));
            DataColumn OpenstackDC3 = new DataColumn("email", Type.GetType("System.String"));
            DataColumn OpenstackDC4 = new DataColumn("active", Type.GetType("System.String"));
            DataColumn OpenstackDC5 = new DataColumn("domain", Type.GetType("System.String"));
            OpenstackUserDT.Columns.Add(OpenstackDC1);
            OpenstackUserDT.Columns.Add(OpenstackDC2);
            OpenstackUserDT.Columns.Add(OpenstackDC3);
            OpenstackUserDT.Columns.Add(OpenstackDC4);
            OpenstackUserDT.Columns.Add(OpenstackDC5);
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserLogin Ul = new UserLogin();
            Ul.TransfEvent += Ul_TransfEvent;
            Ul.ShowDialog();
            changelogin();
        }

        void Ul_TransfEvent(string value)
        {
            loginmsg = value;
        }

        void GCDOST_TransfEvent(string value)
        {
            GetConnetcFromForm.Add(value);
        }

        private void checkonline()
        {
            context = new DockerUserControllerEntities();
            while (true)
            {
                Ping p = new Ping();
                PingReply pr = p.Send("8.8.8.8");
                if (pr.Status != IPStatus.Success)
                {
                    Thread.Sleep(500);
                    continue;
                }
                if (loginmsg == string.Empty)
                {
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    string[] cookie = loginmsg.Split('^');
                    string username = cookie[1];
                    var result = context.UserSet.SingleOrDefault(m => m.Username == username);
                    if (result == null)
                        loginmsg = "";
                    else
                    {
                        string decode = EnDeCode.DecryptString(cookie[0], result.EncodeKey);
                        if (decode != cookie[1])
                        {
                            loginmsg = "";
                        }
                    }
                }
                Thread.Sleep(600000);
            }
        }

        private void changelogin()
        {
            if (loginmsg != string.Empty)
            {
                string[] log = loginmsg.Split('^');
                登录ToolStripMenuItem.Visible = false;
                用户ToolStripMenuItem1.Visible = true;
                用户ToolStripMenuItem1.Text = log[1];
                注销ToolStripMenuItem.Visible = true;
                label2.Text = log[1];
                button2.Visible = true;
                button3.Visible = true;
                button1.Visible = false;
            }
            else
            {
                登录ToolStripMenuItem.Visible = true;
                用户ToolStripMenuItem1.Visible = false;
                用户ToolStripMenuItem1.Text = "用户";
                注销ToolStripMenuItem.Visible = false;
                label2.Text = "匿名者";
                button2.Visible = false;
                button3.Visible = false;
                button1.Visible = true;
            }
            Thread.Sleep(600);
        }

        private void changeloginnothread()
        {
            if (loginmsg != string.Empty)
            {
                string[] log = loginmsg.Split('^');
                登录ToolStripMenuItem.Visible = false;
                用户ToolStripMenuItem1.Visible = true;
                用户ToolStripMenuItem1.Text = log[1];
                注销ToolStripMenuItem.Visible = true;
                label2.Text = log[1];
                button2.Visible = true;
                button3.Visible = true;
                button1.Visible = false;
            }
            else
            {
                登录ToolStripMenuItem.Visible = true;
                用户ToolStripMenuItem1.Visible = false;
                用户ToolStripMenuItem1.Text = "用户";
                注销ToolStripMenuItem.Visible = false;
                label2.Text = "匿名者";
                button2.Visible = false;
                button3.Visible = false;
                button1.Visible = true;
            }
        }

        private void 用户ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UserEdit ue = new UserEdit(loginmsg);
            ue.Show();
        }

        private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginmsg = string.Empty ;
            changeloginnothread();
            changelogin();
            MessageBox.Show("退出成功", "Success", MessageBoxButtons.OK);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            登录ToolStripMenuItem_Click(sender,e);
        }

        private void 转到控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            用户ToolStripMenuItem1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            注销ToolStripMenuItem_Click(sender, e);
        }

        private void 转到窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetConnetcFromForm = new List<string>();
            GetConnectDkOST gcdo = new GetConnectDkOST(true);
            gcdo.TransfEvent += GCDOST_TransfEvent;
            gcdo.ShowDialog();
            if(GetConnetcFromForm.Count ==0)
            {
                return;
            }
            AddConn(GetConnetcFromForm);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            int chooise = comboBox1.SelectedIndex;
            if(chooise == -1)
            {
                return;
            }
            List<string> getconn = DockerConnectionStrs[chooise];
            if (client != null)
                client.Dispose();
            if (getconn[2] == "false")
            {
                client = new DockerClientConfiguration(new Uri(getconn[3])).CreateClient();
                label3.Text = "已经连接至" + comboBox1.SelectedItem.ToString();
            }
            else if(getconn[2] == "true")
            {
                var credentials = new CertificateCredentials(new X509Certificate2(getconn[4],getconn[5]));
                var config = new DockerClientConfiguration(new Uri(getconn[3]), credentials);
                client = config.CreateClient();
                label3.Text = "已经连接至" + comboBox1.SelectedItem.ToString();
            }
            else
            {
                label3.Text = "数据错误";
            }
            try
            {
                listBox1.Items.Clear();
                if (client != null)
                {
                    IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
                    foreach (ContainerListResponse c in containers)
                    {
                        listBox1.Items.Add(c.ID.Substring(0, 12));
                    }
                }
                else
                    return;
            }
            catch
            {
                MessageBox.Show("连接失败", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        public void AddConn(List<string> ls)
        {
            willadd = ls;
            if(willadd[1] == "docker")
            {
                DockerConnectionStrs.Add(willadd);
                comboBox1.Items.Add(willadd[0]);
            }
            else if(willadd[1] == "openstack")
            {
                OpenstackConnectionStrs.Add(willadd);
                comboBox2.Items.Add(willadd[0]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int num = comboBox1.SelectedIndex;
            if(num==-1)
            {
                return;
            }
            comboBox1.Items.RemoveAt(num);
            DockerConnectionStrs.RemoveAt(num);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (client != null)
                client.Dispose();
            label3.Text = "已经断开";
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                if (client != null)
                {
                    IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
                    foreach (ContainerListResponse c in containers)
                    {
                        listBox1.Items.Add(c.ID.Substring(0,12));
                    }
                }
                else
                    return;
            }
            catch
            {
                ;
            }
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string hostsid = listBox1.SelectedItem.ToString();
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
            var result = containers.SingleOrDefault(m => m.ID.Substring(0,12) == hostsid);
            if(result == null)
            {
                button8_Click(sender,e);
                return;
            }
            textBox1.Text = "ID:" + result.ID + "\r\n镜像名:" + result.Image + "\r\n镜像编号:" + result.ImageID
                    + "\r\n容器名:" + result.Names[0] + "\r\n容器状态:" + result.Status;
            if (result.Status.Contains("Exit") || result.Status.Contains("Created"))
                button9.Text = "开启容器";
            else if (result.Status.Contains("Up"))
                button9.Text = "关闭容器";
            else
                button9.Text = "操作";
            foreach(Port a in result.Ports)
            {
                textBox1.Text += "\r\nPublicPort:" + a.PublicPort + "  PrivatePort" + a.PrivatePort;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            if (button9.Text == "操作")
                return;
            int selectednum = listBox1.SelectedIndex;
            if (selectednum == -1)
                return;
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
            string hostsid = listBox1.SelectedItem.ToString();
            if(button9.Text == "开启容器")
            {
                ContainerStartParameters cp = new ContainerStartParameters();
                await client.Containers.StartContainerAsync(hostsid, cp);
                MessageBox.Show("已经发送指令", "Success", MessageBoxButtons.OK);
            }
            else if(button9.Text == "关闭容器")
            {
                ContainerStopParameters csp = new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 10
                };
                var stopped = await client.Containers.StopContainerAsync(hostsid, csp, CancellationToken.None);
                string stopstr = stopped.ToString();
                if(stopstr == "True")
                    MessageBox.Show("已经关闭", "Success", MessageBoxButtons.OK);
                else
                    MessageBox.Show("出现错误", "Error", MessageBoxButtons.OK);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(client == null)
            {
                MessageBox.Show("没有连接机器", "Error", MessageBoxButtons.OK);
                return;
            }
            CreateImage ci = new CreateImage(client);
            ci.Show();
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            int selectednum = listBox1.SelectedIndex;
            if (selectednum == -1)
                return;
            string id = listBox1.SelectedItem.ToString();
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
            foreach(ContainerListResponse c in containers)
            {
                if(c.ID.Substring(0,12) == id)
                {
                    id = c.ID;
                }
            }
            if(id.Length == 12)
            {
                MessageBox.Show("出现错误", "Error", MessageBoxButtons.OK);
                return;
            }
            ContainerRemoveParameters crp = new ContainerRemoveParameters
            {
                Force = true,
                RemoveVolumes = true
            };
            await client.Containers.RemoveContainerAsync(id, crp);
            MessageBox.Show("已经发出指令", "Success", MessageBoxButtons.OK);
        }

        private async void 本地ImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagesListParameters ilp = new ImagesListParameters
            {
                All = true
            };
            if(client == null)
            {
                MessageBox.Show("未连接任何机器", "Success", MessageBoxButtons.OK);
                return;
            }
            IList<ImagesListResponse> containers = await client.Images.ListImagesAsync(ilp);
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach(ImagesListResponse i in containers)
            {
                if (i.RepoDigests == null)
                    continue;
                if (i.RepoDigests[0] == "<none>@<none>")
                    continue;
                string[] list = i.RepoDigests[0].Split('@');
                if (d.Keys.Contains(list[0]))
                    continue;
                d.Add(list[0], list[1]);
            }
            ListForm lf = new ListForm(d) ;
            lf.Show();
        }

        private void 官方ImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://hub.docker.com/explore/");
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            int selectednum = listBox1.SelectedIndex;
            if (selectednum == -1)
                return;
            string id = listBox1.SelectedItem.ToString();
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                        new ContainersListParameters()
                        {
                            Limit = 10,
                        });
            foreach (ContainerListResponse c in containers)
            {
                if (c.ID.Substring(0, 12) == id)
                {
                    id = c.Image;
                }
            }
            try
            {
                Stream s = await client.Images.SaveImageAsync(id);
                MessageBox.Show("提示结束之前请误关闭程序", "Message", MessageBoxButtons.OK);
                while (!s.CanRead)
                {
                    Thread.Sleep(10);
                }
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = @"|*"
                };
                string path = string.Empty;
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                s.CopyTo(fs);
                fs.Flush();
                fs.Close();
                s.Close();
                MessageBox.Show("结束：已经导出到:" + path + "文件", "Message", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source, "Error", MessageBoxButtons.OK);
            }            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            GetConnetcFromForm = new List<string>();
            GetConnectDkOST gcdo = new GetConnectDkOST(false);
            gcdo.TransfEvent += GCDOST_TransfEvent;
            gcdo.ShowDialog();
            if (GetConnetcFromForm.Count == 0)
            {
                return;
            }
            AddConn(GetConnetcFromForm);
        }
        
        private void Button14_Click(object sender, EventArgs e)
        {
            int num = comboBox2.SelectedIndex;
            if (num == -1)
                return;
            List<string> getconnopk = OpenstackConnectionStrs[num];
            useOpenstackhttp = getconnopk[2];
            useOpenstackuser = getconnopk[3];
            useOpenstackpass = getconnopk[4];
            IOpenStack ios = new OpenstackController();
            openstackloginauth = ios.GetAuth(useOpenstackhttp,useOpenstackuser,useOpenstackpass,"default");
            if (!string.IsNullOrEmpty(openstackloginauth))
            {
                label4.Text = "成功连接";
            }
        }

        private void GetOpenStackEdit(WebBrowser wb)
        {
            if (wb == null)
                return;
            HtmlDocument hd = wb.Document;
            HtmlElement el = hd.GetElementById("navbar-collapse");
            foreach(HtmlElement he in el.Children[0].Children)
            {
                user_combox.Items.Add(he.FirstChild.InnerText);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            button18.Text = "启动";
            if(string.IsNullOrEmpty(useOpenstackhttp)||string.IsNullOrEmpty(openstackloginauth))
            {
                MessageBox.Show("未连接至任何服务", "Error", MessageBoxButtons.OK);
                return;
            }
            IOpenStack iso = new OpenstackController();
            osui = iso.GetUserList(useOpenstackhttp,openstackloginauth);
            if(osui == null)
                return;
            user_combox.Items.Clear();
            osusers = osui.users;
            foreach(var item in osusers)
            {
                user_combox.Items.Add(item.name);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(useOpenstackhttp) || string.IsNullOrEmpty(openstackloginauth))
            {
                MessageBox.Show("未连接至任何服务", "Error", MessageBoxButtons.OK);
                return;
            }
            if (user_combox.SelectedIndex == -1)
                return;
            if(button18.Text == "启动" || button18.Text == "禁用")
            {
                if (osui == null)
                {
                    MessageBox.Show("没有数据", "Warning", MessageBoxButtons.OK);
                    return;
                }
                string name = user_combox.SelectedItem.ToString();
                var result = osui.users.SingleOrDefault(m => m.name == name);
                string msg = "用户名：" + result.name + "\r\n项目id：" + result.default_project_id + "\r\n域id：" + result.domain_id + "\r\n账户状态：" + result.enabled + "\r\n";
                textBox3.Text = msg;
            }
            else
            {
                if(imagelist == null)
                {
                    MessageBox.Show("没有数据", "Warning", MessageBoxButtons.OK);
                    return;
                }
                string name = user_combox.SelectedItem.ToString();
                var result = imagelist.SingleOrDefault(m => m.name == name);
                string msg = "镜像名：" + result.name + "\r\n所有者id：" + result.owner + "\r\n创建最小硬盘容量："
                    + result.min_disk + "\r\n创建最小内存：" 
                    + result.min_ram + "\r\n镜像大小：" + result.size + "\r\n上传时间：" + result.updated_at + "\r\n";
                textBox3.Text = msg;
            }            
        }

        private void user_combox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(button18.Text == "启动" || button18.Text == "禁用")
            {
                string name = user_combox.SelectedItem.ToString();
                var result = osui.users.SingleOrDefault(m => m.name == name);
                if (result.enabled == "false")
                {
                    button18.Text = "启动";
                }
                else
                {
                    button18.Text = "禁用";
                }
            }
            else
            {
                string name = user_combox.SelectedItem.ToString();
                var result = imagelist.SingleOrDefault(m => m.name == name);
                if (result.protecte)
                {
                    button18.Text = "不能删除";
                }
                else
                {
                    button18.Text = "删除";
                }
            }            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            OpenStackCreateUser oscu = new OpenStackCreateUser(useOpenstackhttp,openstackloginauth);
            oscu.ShowDialog();
            button15_Click(sender,e);
        }

        List<string> GetOpenstackuser = new List<string>();
        void Getopenstackuserpass(List<string> list)
        {
            GetOpenstackuser = list;
        }
        
        private void button18_Click(object sender, EventArgs e)
        {
            if (user_combox.SelectedIndex == -1)
                return;
            if (osui == null)
            {
                MessageBox.Show("没有数据", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (button18.Text == "启动" || button18.Text == "禁用")
            {
                string name = user_combox.SelectedItem.ToString();
                var result = osui.users.SingleOrDefault(m => m.name == name);
                if (button18.Text == "启动")
                {
                    IOpenStack iso = new OpenstackController();
                    int num = iso.StartUser(useOpenstackhttp, openstackloginauth, result.id);
                    CheckHttpCode(num);
                }
                else if (button18.Text == "禁用")
                {
                    IOpenStack iso = new OpenstackController();
                    int num = iso.StopUser(useOpenstackhttp, openstackloginauth, result.id);
                    CheckHttpCode(num);
                }
                else
                {
                    MessageBox.Show("错误的程序内容", "Error", MessageBoxButtons.OK);
                }
                button15_Click(sender, e);
                button16_Click(sender, e);
            }
            else
            {
                if (button18.Text == "不能删除")
                    return;
                else
                {
                    string name = user_combox.SelectedItem.ToString();
                    var result = imagelist.SingleOrDefault(m => m.name == name);
                    IOpenStack ios = new OpenstackController();
                    int num = ios.DelImage(useOpenstackhttp, openstackloginauth, result.id);
                    CheckHttpCode(num);
                    button22_Click(sender, e);
                    button16_Click(sender, e);
                }
            }
        }

        private void CheckHttpCode(int num)
        {
            if (num == 404)
                MessageBox.Show("404 NOT FOUND 没有找到网页", "Error", MessageBoxButtons.OK);
            else if (num == 401)
                MessageBox.Show("用户信息或者权限错误请检查", "Error", MessageBoxButtons.OK);
            else if (num == 403)
                MessageBox.Show("用户无权访问", "Error", MessageBoxButtons.OK);
            else if (num == 200)
                MessageBox.Show("成功", "Success", MessageBoxButtons.OK);
            else
                MessageBox.Show("错误的返回代码", "Error", MessageBoxButtons.OK);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            List<List<List<string>>> peizhi = new List<List<List<string>>>();
            peizhi.Add(DockerConnectionStrs);
            peizhi.Add(OpenstackConnectionStrs);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < peizhi.Count; i++)
            {
                for(int j = 0;j<peizhi[i].Count;j++)
                {
                    for(int k =0;k<peizhi[i][j].Count;k++)
                    {
                        sb.Append(","+peizhi[i][j][k]);
                    }
                }
            }
            textBox2.Text = sb.ToString().Substring(1, sb.Length - 1);
        }

        private async void button20_Click(object sender, EventArgs e)
        {
            if(loginmsg == "")
            {
                MessageBox.Show("必须登录才能生成加密文件", "Error", MessageBoxButtons.OK);
                return;
            }
            string[] log = loginmsg.Split('^');
            string username = log[1];
            var result = await context.UserSet.SingleOrDefaultAsync(m=>m.Username == username);
            if(result == null)
            {
                loginmsg = "";
                return;
            }
            List<List<List<string>>> peizhi = new List<List<List<string>>>();
            peizhi.Add(DockerConnectionStrs);
            peizhi.Add(OpenstackConnectionStrs);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < peizhi.Count; i++)
            {
                for (int j = 0; j < peizhi[i].Count; j++)
                {
                    for (int k = 0; k < peizhi[i][j].Count; k++)
                    {
                        sb.Append("," + peizhi[i][j][k]);
                    }
                }
            }
            string neirong = sb.ToString().Substring(1, sb.Length - 1);
            string encodeedneirong = EnDeCode.EncryptString(neirong, result.EncodeKey);
            textBox2.Text = encodeedneirong;
        }

        private async void button21_Click(object sender, EventArgs e)
        {
            string[] neirong = textBox2.Text.Split(',');
            if(neirong.Count() == 1)
            {
                if(loginmsg == "")
                {
                    MessageBox.Show("必须以导出用户登录才能导入", "Error", MessageBoxButtons.OK);
                    return;
                }
                string username = loginmsg.Split('^')[1];
                var result = await context.UserSet.SingleOrDefaultAsync(m => m.Username == username);
                string key = result.EncodeKey;
                string decodeed = string.Empty;
                try
                {
                    decodeed = EnDeCode.DecryptString(textBox2.Text, key);
                }
                catch
                {
                    MessageBox.Show("必须以导出用户登录才能导入", "Error", MessageBoxButtons.OK);
                    return;
                }
                neirong = decodeed.Split(',');
            }
            List<string> aa = new List<string>();
            aa = neirong.ToList();
            //aa,docker,false,http://www,adsf,docker,true,http://wwwss,123,qwe,wq,openstack,http://www1,1,1
            restart: for (int i = 0; i < aa.Count(); i++)
            {
                if(aa.Count() == 0)
                {
                    break;
                }
                if(aa[i+1] == "docker")
                {
                    if(aa[i+2] == "false")
                    {
                        List<string> constr = new List<string>();
                        constr.Add(aa[i]);
                        constr.Add(aa[i + 1]);
                        constr.Add(aa[i + 2]);
                        constr.Add(aa[i + 3]);
                        DockerConnectionStrs.Add(constr);
                        aa.RemoveRange(0, 4);
                        goto restart;
                    }
                    else if(aa[i+2]=="true")
                    {
                        List<string> constr = new List<string>();
                        constr.Add(aa[i]);
                        constr.Add(aa[i + 1]);
                        constr.Add(aa[i + 2]);
                        constr.Add(aa[i + 3]);
                        constr.Add(aa[i + 4]);
                        constr.Add(aa[i + 5]);
                        DockerConnectionStrs.Add(constr);
                        aa.RemoveRange(0, 6);
                        goto restart;
                    }
                }
                else if(aa[i+1]=="openstack")
                {
                    List<string> constr = new List<string>();
                    constr.Add(aa[i]);
                    constr.Add(aa[i + 1]);
                    constr.Add(aa[i + 2]);
                    constr.Add(aa[i + 3]);
                    constr.Add(aa[i + 4]);
                    OpenstackConnectionStrs.Add(constr);
                    aa.RemoveRange(0, 5);
                    goto restart;
                }
            }
            MessageBox.Show("成功", "MSG", MessageBoxButtons.OK);
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach(List<string> a in DockerConnectionStrs)
            {
                comboBox1.Items.Add(a[0]);
            }
            foreach (List<string> a in OpenstackConnectionStrs)
            {
                comboBox2.Items.Add(a[0]);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            button18.Text = "开启";
            user_combox.Items.Clear();
            IOpenStack ios = new OpenstackController();
            var images = ios.GetImageList(useOpenstackhttp, openstackloginauth);
            imagelist = images.images;
            foreach(var item in imagelist)
            {
                user_combox.Items.Add(item.name);
            }
        }
    }
}