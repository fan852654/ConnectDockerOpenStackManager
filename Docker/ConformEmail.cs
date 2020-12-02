using System;
using System.Linq;
using System.Windows.Forms;

namespace Docker
{
    public partial class ConformEmail : Form
    {
        int truecode = 0;
        string user = string.Empty;
        string emails = string.Empty;
        bool isphonenum;
        public ConformEmail(int code,string username,string email,bool isphone)
        {
            InitializeComponent();
            truecode = code;
            user = username;
            emails = email;
            isphonenum = isphone;
        }

        private void ConformEmail_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int inputcode = 0;
            bool change = int.TryParse(textBox1.Text,out inputcode);
            if(!change)
            {
                MessageBox.Show("请输入正确的验证码", "Error", MessageBoxButtons.OK);
                return;
            }
            if (inputcode == truecode)
            {
                DockerUserControllerEntities context = new DockerUserControllerEntities();
                var result = context.EditSet.SingleOrDefault(m => m.Username == user);
                if(result == null)
                {
                    MessageBox.Show("错误！！！！", "Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    if(isphonenum)
                    {
                        result.Phone = emails;
                    }
                    else
                    {
                        result.Email = emails;
                    }
                    context.SaveChanges();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("请输入正确的验证码", "Error", MessageBoxButtons.OK);
                return;
            }
        }
    }
}
