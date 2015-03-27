using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace editor
{
    public partial class SurfacesChanger : Form
    {
        public static bool Opened { get; set; }
        public static string Path { get; set; }
        public static GraphicsState State { get; set; }
        public static bool Selected { get; set; }
        public static int[] OldCoord { get; set; }
        public static TextBox _text;
        public SurfacesChanger()
        {
            InitializeComponent();
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            textBox1.TextChanged += textBox1_TextChanged;
            pictureBox2.LoadProgressChanged += pictureBox2_LoadProgressChanged;
            Closing += SurfacesChanger_Closing;
        }

        void SurfacesChanger_Closing(object sender, CancelEventArgs e)
        {
            Opened = false;
            this.Dispose();
            this.Hide();
        }

        void pictureBox2_LoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }


        void textBox1_TextChanged(object sender, EventArgs e)
        {
        }


        private void DrowRect(int[] val, Graphics g)
        {
            {
                Pen pen = new Pen(Color.Chartreuse, 4);
                Brush brush = new SolidBrush(panel1.BackColor);

                g.DrawRectangle(pen, val[0], val[1], 32, 32);

                pen.Dispose();
            }
        }

        void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int mm = 4096/32; // Image Default size
            int x = e.X/32;
            int y = e.Y/32;
            int val = y == 0 ? x : x+(y*mm);
            var id = Helper.FindCoord(Helper._surfaces[val]);
            pictureBox2.Image = Graphic.CropImage(Helper._img, new Rectangle(id[0], id[1], 32, 32));
            textBox1.Text = Helper._surfaces[val];
            OldCoord = new[] {x*32, y*32};
            pictureBox1.Refresh();
        }

        private void SurfacesChanger_Load(object sender, EventArgs e)
        {
            Opened = true;
            panel1.AutoScroll = true;
            pictureBox1.Paint += PictureBox1OnPaint;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Image = Helper._img;
            var id = Helper.FindCoord(Path);
            panel1.VerticalScroll.Value = id[1]-32;
            if (id[1] < 32)
                id[1] = 33;
            panel1.HorizontalScroll.Value = id[0]-32;

            pictureBox2.Image = Graphic.CropImage(Helper._img, new Rectangle(id[0], id[1], 32, 32));
            
            OldCoord = new[] { id[0], id[1] };
            textBox1.Text = Path;
        }

        private void PictureBox1OnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            DrowRect(OldCoord, paintEventArgs.Graphics);
        }

        public void Display(ref TextBox text)
        {
            _text = text;
            this.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _text.Text = textBox1.Text;
            Opened = false;
            this.Hide();
        }


    }
}
