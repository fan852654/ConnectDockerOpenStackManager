using System;
using System.Windows.Forms;

namespace Docker
{
    public partial class ConfromPass : Form
    {
        public ConfromPass()
        {
            InitializeComponent();
        }
        public event TransfDelegate TransfEvent;
        private void ConfromPass_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransfEvent(textBox1.Text);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
