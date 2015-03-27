using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pwApi.StructuresElement;

namespace editor
{
    public partial class GlobalSelector : Form
    {
        public bool Opened { get; set; }

        public GlobalSelector()
        {
            InitializeComponent();
        }

        private DataGridViewCell cell;
        private TextBox Result { get; set; }
        public void Display(int list,ref DataGridViewCell val)
        {
            Result = null;
            cell = null;
            Opened = true;
            Show();
            listBox1.DataSource = new BindingSource(Helper._elReader.Items.ElementAt(list - 1).Value, null);
            listBox1.DisplayMember = "EditorView";
            cell = val;
        }

        public void Display(int list, ref TextBox vv)
        {
            Result = null;
            cell = null;
            Opened = true;
            Show();
            listBox1.DataSource = new BindingSource(Helper._elReader.Items.ElementAt(list - 1).Value, null);
            listBox1.DisplayMember = "EditorView";
            Result = vv;
        }

        public void Display(int list,ref TextBox tt,bool val)
        {
            Result = null;
            cell = null;
            Opened = true;
            var ls = new List<string>();
            if (list == 55)
            {
                ls.Add("158 Кузнец");
                ls.Add("159 Портной" );
                ls.Add("160 Ремесленник");
                ls.Add("161 Аптекарь");
            }
            listBox1.DataSource = new BindingSource(ls, null);
            Result = tt;
            this.Show();
        }
        private void GlobalSelector_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dynamic selected;
            try
            {
                selected = (Item)listBox1.SelectedItem;
                if (cell != null)
                    cell.Value = selected.GetByKey("ID").ToString();
                if (Result != null)
                    Result.Text = selected.GetByKey("ID").ToString();
            }
            catch (Exception)
            {
                Result.Text = ((string)listBox1.SelectedItem).Split(' ')[0];
                
            }
            Opened = false;
            this.Hide();
        }

        private void GlobalSelector_Load_1(object sender, EventArgs e)
        {

        }
    }
}
