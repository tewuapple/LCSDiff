using System;
using System.Windows.Forms;

namespace LCSDiff
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourceStr = textBox1.Text;
            string destinationStr = textBox2.Text;
            if (string.IsNullOrEmpty(sourceStr)&&string.IsNullOrEmpty(destinationStr))
                return;
            Lcs.LcsAlgorithm(sourceStr.Replace("\r\n", "\n"), destinationStr.Replace("\r\n", "\n"), rtb_DisPlay);
        }
    }
}
