using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Docker
{
    public partial class OpenStackCreateUser : Form
    {
        string url;
        string auth;
        public OpenStackCreateUser(string url,string auth)
        {
            this.url = url;
            this.auth = auth;
            InitializeComponent();
        }

        private void OpenStackCreateUser_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string email = textBox3.Text;
            string project = textBox4.Text;
            string domain = textBox5.Text;
            string description = textBox6.Text;
            IOpenStack ios = new OpenstackController();
            int num = ios.AddUser(url, auth, project, domain, username, password, description, email);
            if (num == 404)
                MessageBox.Show("404 NOT FOUND 没有找到网页", "Error", MessageBoxButtons.OK);
            else if (num == 401)
                MessageBox.Show("用户信息或者权限错误请检查", "Error", MessageBoxButtons.OK);
            else if (num == 403)
                MessageBox.Show("用户无权访问", "Error", MessageBoxButtons.OK);
            else
                MessageBox.Show("错误的返回代码", "Error", MessageBoxButtons.OK);
            return;
        }
    }
}
