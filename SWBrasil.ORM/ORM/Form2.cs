using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Form2_Resize(null, null);
        }

        public string PreviewCode
        {
            set { txtPreview.Text = value; }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            txtPreview.Width = Convert.ToInt32(this.Width * 0.95);
            txtPreview.Height = Convert.ToInt32(this.Height * 0.85);
        }
    }
}
