using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace editor
{
    public partial class AddonSelector : Form
    {
        private static DataGridViewCell _cell;
        public static bool Opened { get; set; }
        public AddonSelector()
        {
            InitializeComponent();
            this.Closing += AddonSelector_Closing;
        }

        void AddonSelector_Closing(object sender, CancelEventArgs e)
        {

        }

        private void AddonSelector_Load(object sender, EventArgs e)
        {

        }

        public void Display(int listID, ref DataGridViewCell cell)
        {
            Opened = true;
            Dictionary<string, HashSet<string>> source = null;
            switch (listID)
            {
                case 4 :
                    source = Helper.Bonus4Page;
                    break;
                case 7 :
                    source = Helper.Bonus4Page;
                    break;
                case 10 :
                    source = Helper.Bonus4Page;
                    break;
            }
            listBox1.DataSource = new BindingSource(source, null);
            listBox1.DisplayMember = "Key";
            listBox1.SelectedValueChanged+= listBox1_TabIndexChanged;
            _cell = cell;
            Show();
        }

        private void listBox1_TabIndexChanged(object sender, EventArgs e)
        {
            var kk = (KeyValuePair<string, HashSet<string>>) listBox1.SelectedItem;
            listBox2.DataSource = new BindingSource( kk.Value, null);
            listBox2.SelectedValueChanged += listBox2_SelectedValueChanged;
            listBox2.SelectedIndex = 0;
        }

        void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox2.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Выберите бонус или закройте окно");
                return;
            }
            _cell.Value = textBox1.Text;
            Opened = false;
            this.Hide();
            this.Dispose();
        }
    }
}
