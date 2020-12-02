using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Docker
{
    public partial class GetConnectDkOST : Form
    {
        public bool openordocker;
        public GetConnectDkOST(bool isdocker)
        {
            InitializeComponent();
            openordocker = isdocker;
            if (!isdocker)
                textBox1.Text = "e.g  http://104.130.30.68";
        }
        public event TransfDelegate TransfEvent;
        public string x509ssl = string.Empty;
        private void GetConnectDkOST_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            if(openordocker)
            {
                label3.Visible = false;
                textBox4.Visible = false;
                button2.Visible = false;
                label2.Visible = false;
                textBox3.Visible = false;
            }
            else
            {
                comboBox1.Visible = false;
                textBox3.Visible = true;
                label2.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 1)
            {
                button2.Visible = true;
                label2.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                button2.Visible = false;
                label2.Visible = false;
                textBox3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            Regex ckurl = new Regex("(((news|telnet|nttp|file|http|ftp|https)://)|(www|ftp|wap))[-A-Za-z0-9//.]+(:[0-9]*)?(/[-A-Za-z0-9_//$//.//+//!//*//(//),;:@&=//?/~//#//%]*[^]'//.}>//) ,///\"])*");
            if (!ckurl.IsMatch(url))
                return;
            if (openordocker)
            {
                if(comboBox1.SelectedIndex == 0)
                {
                    if(string.IsNullOrWhiteSpace(textBox1.Text))
                        return;
                    if(!string.IsNullOrWhiteSpace(textBox2.Text))
                        TransfEvent(textBox2.Text);
                    else
                        TransfEvent("Docker");
                    TransfEvent("docker");
                    TransfEvent("false");
                    TransfEvent(textBox1.Text);
                }else
                {
                    if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox1.Text) || x509ssl == string.Empty)
                        return;
                    if (!string.IsNullOrWhiteSpace(textBox2.Text))
                        TransfEvent(textBox2.Text);
                    else
                        TransfEvent("Docker");
                    TransfEvent("docker");
                    TransfEvent("true");
                    TransfEvent(textBox1.Text);
                    TransfEvent(x509ssl);
                    TransfEvent(textBox3.Text);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                    return;
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                    TransfEvent(textBox2.Text);
                else
                    TransfEvent("Openstack");
                TransfEvent("openstack");
                TransfEvent(textBox1.Text);
                TransfEvent(textBox4.Text);
                TransfEvent(textBox3.Text);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filepath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog
            {
                ValidateNames = true,
                CheckPathExists = true,
                CheckFileExists = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                StreamReader sr = new StreamReader(filepath, Encoding.Default);
                x509ssl = sr.ReadToEnd();
                sr.Close();
                MessageBox.Show("成功", "Success", MessageBoxButtons.OK);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "e.g  http://104.130.30.68:5000/v2.0")
                textBox1.Text = "http://";
        }
    }
}
