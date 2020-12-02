using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Docker
{
    public partial class UserEdit : Form
    {
        string cookiemsg = string.Empty;
        DockerUserControllerEntities context;
        Thread control;
        public UserEdit(string cookie)
        {
            InitializeComponent();
            cookiemsg = cookie;
            control = new Thread(ControlForm);
            control.Start();
            textBox1.Text = cookie.Split('^')[1];
        }

        private void UserEdit_Load(object sender, EventArgs e)
        {
            string[] list = cookiemsg.Split('^');
            string user = list[1];
            if (context != null)
                context.Dispose();
            context = new DockerUserControllerEntities();
            var results = context.EditSet.SingleOrDefault(m => m.Username == user);
            if(results == null)
            {
                MessageBox.Show("错误的登录方式", "Error", MessageBoxButtons.OK);
                Close();
            }
            else
            {
                if(results.Email != null)
                {
                    textBox2.Text = results.Email;
                    button2.Visible = false;
                }
                if(results.Phone != null)
                {
                    textBox3.Text = results.Phone;
                    button3.Visible = false;
                }
                textBox4.Text = results.Money.ToString();
                textBox5.Text = results.Identity.ToString();
            }
            if (context != null)
                context.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int GetCode(int length)
        {
            int keynumber = 0;
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                int nu = r.Next(0, 9);
                for (int a = 0; a < i; a++)
                {
                    nu = nu * 10;
                }
                keynumber = keynumber + nu;
            }
            return keynumber;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (context != null)
                context.Dispose();
            context = new DockerUserControllerEntities();
            bool result = await context.EditSet.AllAsync(m => m.Email == textBox2.Text);
            if(result)
            {
                MessageBox.Show("该邮箱已经被注册过", "Error", MessageBoxButtons.OK);
                return;
            }
            string[] list = cookiemsg.Split('^');
            string user = list[1];
            if (textBox2.Text == "")
            {
                MessageBox.Show("不能为空", "Error", MessageBoxButtons.OK);
                return;
            }
            int code = GetCode(6);
            bool sended = EmailSender.SendEmail(textBox2.Text.ToString(),"验证码","验证码是:"+code.ToString());
            if(sended)
            {
                ConformEmail ce = new ConformEmail(code,user,textBox2.Text.ToString(),false);
                ce.ShowDialog();
            }
            else
            {
                MessageBox.Show("未能发送邮件", "Error", MessageBoxButtons.OK);
                return;
            }
            Flaush();
            if (context != null)
                context.Dispose();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string[] list = cookiemsg.Split('^');
            string user = list[1];
            if (textBox3.Text == "")
            {
                MessageBox.Show("手机号码不能为空", "Error", MessageBoxButtons.OK);
                return;
            }
            int code = GetCode(6);
            await SMSSender.SendSMS(textBox3.Text, "验证码:" + code.ToString(), "Code");
            ConformEmail ce = new ConformEmail(code, user, textBox3.Text.ToString(),true);
            ce.ShowDialog();
            Flaush();
            if (context != null)
                context.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Flaush();
        }

        private void Flaush()
        {
            string[] list = cookiemsg.Split('^');
            string user = list[1];
            if (context != null)
                context.Dispose();
            context = new DockerUserControllerEntities();
            var result = context.EditSet.SingleOrDefault(m => m.Username == user);
            if (result == null)
            {
                MessageBox.Show("错误的登录方式", "Error", MessageBoxButtons.OK);
                Close();
            }
            else
            {
                if (result.Email != null)
                {
                    textBox2.Text = result.Email;
                    button2.Visible = false;
                }
                if (result.Phone != null)
                {
                    textBox3.Text = result.Phone;
                    button3.Visible = false;
                }
                textBox4.Text = result.Money.ToString();
                textBox5.Text = result.Identity.ToString();
            }
            if (context != null)
                context.Dispose();
        }

        private void ControlForm()
        {
            DockerUserControllerEntities th = new DockerUserControllerEntities();
            while (true)
            {
                string[] list = cookiemsg.Split('^');
                string user = list[1];
                var result = th.EditSet.SingleOrDefault(m => m.Username == user);
                if (result == null)
                {
                    MessageBox.Show("错误的登录方式", "Error", MessageBoxButtons.OK);
                    Close();
                }
                else
                {
                    if (result.Email != null)
                    {
                        textBox2.Text = result.Email;
                        button2.Visible = false;
                    }
                    if (result.Phone != null)
                    {
                        textBox3.Text = result.Phone;
                        button3.Visible = false;
                    }
                    textBox4.Text = result.Money.ToString();
                    textBox5.Text = result.Identity.ToString();
                }
                Thread.Sleep(600);
            }
        }
    }
}
