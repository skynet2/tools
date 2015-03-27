using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace editor
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            var n = new DateTime(2015, 04, 10, 0, 0, 0);
            if (DateTime.UtcNow.Date > n)
                label2.Text = "SkyDev (0x31F)";
            else
                label2.Text = "SkyDev";
            pictureBox1.Image = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "images", "logo.png"));

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}
