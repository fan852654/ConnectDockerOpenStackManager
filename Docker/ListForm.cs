using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Docker
{
    public partial class ListForm : Form
    {
        Dictionary<string, string> dic;
        public ListForm(Dictionary<string,string> a)
        {
            InitializeComponent();
            dic = new Dictionary<string, string>();
            dic = a;
        }
        public int max = 0;
        private void ListForm_Load(object sender, EventArgs e)
        {
            foreach(KeyValuePair<string, string> kv in dic)
            {
                listBox1.Items.Add(kv.Key);
                if (kv.Key.Length > max)
                    max = kv.Key.Length;
            }
            listBox1.ColumnWidth = max * 9;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            string thing = listBox1.SelectedItem.ToString();
            string value = dic[thing];
            MessageBox.Show(thing + "\r\n" + value, "描述", MessageBoxButtons.OK);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
