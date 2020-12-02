using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Docker
{
    public partial class CreateImage : Form
    {
        private readonly DockerClient _context;
        public CreateImage(DockerClient context)
        {
            InitializeComponent();
            _context = context;
        }

        private void CreateImage_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imagenames = textBox1.Text;
            string dnses = textBox2.Text;
            string ports = textBox3.Text;
            string domainname = textBox4.Text;
            string workdir = textBox5.Text;
            if (imagenames == null)
            {
                MessageBox.Show("必须输入容器名称", "Error", MessageBoxButtons.OK);
                return;
            }
            CreateImageInLocal(imagenames, dnses.Split(','), ports.Split(','), domainname, workdir);
        }

        private async void CreateImageInLocal(string imagename,string[] dnslist,string[]portlist,string domainname,string workdirlist)
        {
            List<string> dnslis = new List<string>();
            Regex dns = new Regex("^(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|[1-9])\\." + "(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)\\." + "(1\\d{2} 2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)\\." + "(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)$");
            foreach (string ip in dnslist)
            {
                if(dns.IsMatch(ip))
                {
                    dnslis.Add(ip);
                }
            }
            HostConfig hc = new HostConfig
            {
                DNS = dnslis.ToArray()
            };
            IDictionary<string, EmptyStruct> idd = new Dictionary<string, EmptyStruct>();
            foreach (string port in portlist)
            {
                if (int.TryParse(port, out int a))
                    idd.Add(a.ToString(), new EmptyStruct());
            }
            string[] cmdlist = textBox6.Text.Split(',');
            IList<string> cmdli = new List<string>();
            foreach (string cmd in cmdlist)
            {
                cmdli.Add(cmd);
            }
            CreateContainerParameters ccp = new CreateContainerParameters
            {
                HostConfig = hc,
                Image = imagename,
                ExposedPorts = idd,
                Domainname = domainname,
                WorkingDir = workdirlist,
                Cmd = cmdli
            };
            try
            {
                var result = await _context.Containers.CreateContainerAsync(ccp);
                string id = result.ID.Substring(0, 12);
                MessageBox.Show("创建ID为：" + id, "Success", MessageBoxButtons.OK);
            }
            catch (Exception e)
            {
                if(e.Message.Contains("No such"))
                {
                    MessageBox.Show("没有名字为:" + imagename + " 的镜像", "Error", MessageBoxButtons.OK);
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
