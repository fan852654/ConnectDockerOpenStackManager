using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Docker
{
    public delegate void TransfDelegate(String value);

    public partial class UserLogin : Form
    {
        DockerUserControllerEntities context;
        string cookie = string.Empty;
        public event TransfDelegate TransfEvent;
        Thread checknet;
        private int keynumber = 0;
        public UserLogin()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            AcceptButton = button1;
            checknet = new Thread(CheckOnline);
            checknet.Start();
            context = new DockerUserControllerEntities();
        }
        
        private void UserLogin_Load(object sender, EventArgs e)
        {
            Random r = new Random();
            for(int i = 0;i<8;i++)
            {
                int nu = r.Next(0, 9);
                for(int a = 0;a<i;a++)
                {
                    nu = nu * 10;
                }
                keynumber = keynumber + nu;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TransfEvent(textBox1.Text);
            if(!online)
            {
                MessageBox.Show("未能成功连接至服务器", "Error", MessageBoxButtons.OK);
                return;
            }
            string user = textBox1.Text;
            string pass = textBox2.Text;
            if(user != "" && pass != "")
            {
                Regex r = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                bool ismail = r.IsMatch(user);
                if(ismail)
                {
                    var resultforemail = context.EditSet.SingleOrDefault(m => m.Email == user);
                    if(resultforemail==null)
                    {
                        MessageBox.Show("没有此邮箱", "Error", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        string trueusername = resultforemail.Username;
                        var resultforlogin = context.UserSet.SingleOrDefault(m => m.Username == trueusername);
                        if(resultforlogin.Password != pass)
                        {
                            MessageBox.Show("错误的密码", "Error", MessageBoxButtons.OK);
                            return;
                        }
                        else
                        {
                            cookie = EnDeCode.EncryptString(resultforlogin.Username, resultforlogin.EncodeKey) + "^" + resultforlogin.Username;
                            TransfEvent(cookie);
                            context.Dispose();
                            Close();
                        }
                    }
                }
                else
                {
                    var result = context.UserSet.SingleOrDefault(m => m.Username == user);
                    if(result==null)
                    {
                        MessageBox.Show("没有此用户", "Error", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        if(pass == result.Password)
                        {
                            cookie = EnDeCode.EncryptString(user, result.EncodeKey) + "^" + user;
                            TransfEvent(cookie);
                            context.Dispose();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("密码错误", "Error", MessageBoxButtons.OK);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (user == "")
                    MessageBox.Show("用户名不能为空", "Error", MessageBoxButtons.OK);
                else if (pass == "")
                    MessageBox.Show("密码不能为空", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!online)
            {
                MessageBox.Show("未能成功连接至服务器", "Error", MessageBoxButtons.OK);
                return;
            }
            string user = textBox1.Text;
            string pass = textBox2.Text;
            if(user != "" && pass != "")
            {
                var result = context.UserSet.SingleOrDefault(m => m.Username == user);
                if(result != null)
                {
                    MessageBox.Show("已经有相同的用户名", "Error", MessageBoxButtons.OK);
                    return;
                }
                ConfromPass cp = new ConfromPass();
                cp.TransfEvent += ConformP;
                cp.ShowDialog();
                if(conformpass != pass)
                {
                    MessageBox.Show("密码不相符", "Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    UserSet us = new UserSet
                    {
                        Username = user,
                        Password = pass,
                        EncodeKey = keynumber.ToString()
                    };
                    context.UserSet.Add(us);
                    EditSet es = new EditSet
                    {
                        Username = user,
                        Identity = 3                        
                    };
                    context.EditSet.Add(es);
                    CloudEditSet ces = new CloudEditSet
                    {
                        Username = user
                    };
                    context.CloudEditSet.Add(ces);
                    context.SaveChanges();
                    cookie = EnDeCode.EncryptString(user, keynumber.ToString()) + "^" + user;
                    TransfEvent(cookie);
                    Close();
                }
            }
            else
            {
                if(pass == "")
                    MessageBox.Show("密码不能为空", "Error", MessageBoxButtons.OK);
                else if(user == "")
                    MessageBox.Show("用户名不能为空", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private string conformpass = string.Empty;
        void ConformP(string value)
        {
            conformpass = value;
        }

        private bool online = false;
        private void CheckOnline()
        {
            while (!online)
            {
                Ping p = new Ping();
                PingReply pr = p.Send("8.8.8.8");
                if (pr.Status != IPStatus.Success)
                    online = false;
                else
                    online = true;
            }
        }
    }
}
