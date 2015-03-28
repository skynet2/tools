using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pwApi.StructuresElement;

namespace editor
{
    public partial class ItemSelector : Form
    {
        public static bool Opened { get; set; }
        public ItemSelector()
        {
            InitializeComponent();
            this.Closing += ItemSelector_Closing;
        }

        void ItemSelector_Closing(object sender, CancelEventArgs e)
        {
            Opened = false;
        }

        private static DataGridViewCell _cell;
        public void Display(ref DataGridViewCell cell)
        {
            Opened = true;
            listBox1.DataSource = new BindingSource(Helper._elReader.Items, null);
            listBox1.DisplayMember = "Key";
            listBox1.SelectedValueChanged += listBox1_SelectedValueChanged;
            _cell = cell;
            this.Show();
        }

        void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var it = (KeyValuePair<string, Item[]>) listBox1.SelectedItem;
            listBox2.SelectedValueChanged += listBox2_SelectedValueChanged;
            listBox2.DataSource = new BindingSource(it.Value, null);
            listBox2.DisplayMember = "EditorView";
        }

        void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            Item it = (Item) listBox2.SelectedItem;
            label1.Text = it.GetByKey("Name");
            string val = ((string) it.GetByKey("file_icon"));
            if (val == null)
            {
                pictureBox1.Image = Helper._cropped.ElementAt(0).Value;
                return;
            }
            val = val.Replace("\0", "");
            val = Path.GetFileName(val);
            if (Helper._cropped.ContainsKey(val))
                pictureBox1.Image = Helper._cropped[val];
            else pictureBox1.Image = Helper._cropped.ElementAt(0).Value;
        }
        private void ItemSelector_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _cell.Value = ((Item) listBox2.SelectedItem).GetByKey("ID");
            Hide();
            Opened = false;
        }
    }
}
