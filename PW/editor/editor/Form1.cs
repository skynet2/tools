using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using editor.Pages;
using Newtonsoft.Json;
using pwApi.Readers;
using pwApi.StructuresElement;
using pwApi.Utils;

namespace editor
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> GetVals;
        public static GlobalSelector _globalSelector;
        public static SurfacesChanger _SurfacesChanger;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns.Add("Test", "header");
            var priva = dataGridView1.Columns["Test"];
            if (priva != null) priva.ReadOnly = true;
            dataGridView1.Columns.Add("Value", "Values");
            Helper.LoadSurfaces();
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox1.MouseDown += listBox1_MouseClick;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Minimum = 0;
            textBox3.MouseDoubleClick += textBox3_MouseDoubleClick;
            CheckForIllegalCrossThreadCalls = false;
        }


        void textBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
         //   if(_SurfacesChanger == null)
                _SurfacesChanger = new SurfacesChanger();
            if (!SurfacesChanger.Opened)
            {
                SurfacesChanger.Path = textBox3.Text;
                _SurfacesChanger.Display(ref textBox3);
                
            }
        }

        void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(SelectedIndex == -1)
                return;
            if(e.Button == MouseButtons.Right)
            {
                ContextMenuStrip m = new ContextMenuStrip();
                m.Items.Add("Удалить");
                m.Items.Add("Клонировать");
                m.Items.Add("Экспорт");
                m.Items.Add("Импорт");
                m.Show(listBox1,new Point(e.X, e.Y));
                m.ItemClicked += m_ItemClicked;
            }
        }

        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Удалить":
                {
                    foreach (var it in listBox1.SelectedItems)
                    {
                        Helper._elReader.RemoveItem(GetCurrentList(),(Item)it);
                    }
                    break;
                }

                case "Клонировать":
                {
                    foreach (var ss in listBox1.SelectedItems)
                    {
                        AddItems((Item)ss);
                    }                    
                    break;
                }
                case "Импорт":
                {
                    OpenFileDialog f = new OpenFileDialog();
                    f.Multiselect = true;
                    f.ShowDialog();
                    foreach (var file in f.FileNames)
                    {
                        var id = ((KeyValuePair<string, Item[]>) comboBox1.SelectedItem).Key;
                        int list = GetCurrentList();
                        Item it = JsonConvert.DeserializeObject<Item>(File.ReadAllText(file, Encoding.UTF8));
                        Helper._elReader.AddItem(list, it);
                    }
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;

                    break;
                }
                case "Экспорт":
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.ShowDialog();
                    Random ra = new Random();
                    foreach (Item ss in listBox1.SelectedItems)
                    {
                        File.WriteAllText(Path.Combine(fbd.SelectedPath,string.Format("list{0}id{1}rand{2}.json",
                            (comboBox1.SelectedIndex+1),ss.GetByKey("ID"),ra.Next(0,3000))),JsonConvert.SerializeObject(ss),
                            Encoding.UTF8);
                    }
                    break;
                }
            }
            ListBoxRepaint(true);
        }

        private void AddItems(Item old)
        {
            var oldIt = old;
            var newIt = UtilsIO.DeepClone(oldIt);
            Helper._elReader.AddItem((comboBox1.SelectedIndex + 1), newIt);
        }

        private static Dictionary<short, string> _versions = new Dictionary<short, string>(); 
        private void откритьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var vv in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(),"configs"),"*v*.*",SearchOption.AllDirectories))
            {
                var name = Path.GetFileName(vv);
                var key = short.Parse(Regex.Match(name, "v(\\d*)[.]").Groups[1].Value);
                if(!_versions.ContainsKey(key))
                    _versions.Add(key,vv);
            }
            OpenFileDialog diag = new OpenFileDialog();
            diag.ShowDialog();
            var res = diag.FileName;
            short version;
            if (string.IsNullOrEmpty(res))
                return;
            using (BinaryReader br = new BinaryReader(File.OpenRead(res)))
            {
                version = br.ReadInt16();
                br.Close();
            }
            if (!_versions.ContainsKey(version))
            {
                MessageBox.Show("Конфиг не найден. Версия елемента : " + version);
                return;
            }
            Helper._elReader = new ElementReader(_versions[version], res);
            comboBox1.DataSource = new BindingSource(Helper._elReader.Items, null);
            comboBox1.DisplayMember = "Key";
        }

        private static int SelectedIndex { get; set; }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex == -1)
                return;
            SelectedIndex = comboBox1.SelectedIndex;
            ListBoxRepaint();
            SetInfo();
        }

        private void SetInfo()
        {
            label1.Text = string.Format("Кол-во итемов - {0}", listBox1.Items.Count);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            dataGridView1.Rows.Clear();
            var cl = Helper._elReader.Items.ElementAt(SelectedIndex).Value[listBox1.SelectedIndex];
            dataGridView1.Rows.Add(""); // temp

            // ID Name Surfaces
            numericUpDown1.Value = cl.GetByKey("ID");
            textBox1.Text = cl.GetByKey("Name");
            var surfaces = cl.GetByKey("file_icon");
            if (surfaces != null)
            {
                textBox3.Text = surfaces;
            }
            else
            {
                textBox3.Text = "";
            }
                        
            if((comboBox1.SelectedIndex+1) == 55)
            {
                Page55 pp = new Page55();
                pp.SetValues55(ref tabControl1, ref listBox1);
                return;
            }
            else if(tabControl1.TabPages.Count > 2)
            {
                tabControl1.TabPages.Clear();
                var page = new TabPage();
                page.Controls.Add(dataGridView1);
                page.Text = "Значения";
                tabControl1.TabPages.Add(page);
                var page2 = new TabPage();
                page2.Controls.Add(listBox2);
                page2.Text = "Связи";
                tabControl1.TabPages.Add(page2);
                listBox1.SelectionMode = SelectionMode.MultiExtended;
            }
            for(int i = 2; i < cl.Values.Length/2; i++)
            {
                
                var row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                if (cl.Values[i, 0] == "file_icon")
                    continue;
                row.Cells[0].Value = cl.Values[i, 0];
                row.Cells[1].Value = cl.Values[i, 1];
                if(cl.Values[i,0].Contains("character_combo"))
                {
                    row.Cells[1].Style.BackColor = Color.DarkOrange;
                }
                if (cl.Values[i, 0] == "id_sub_type")
                {
                    row.Cells[1].Style.BackColor = Color.DarkOrange;
                }

                dataGridView1.Rows.Add(row);
            }
            dataGridView1.Rows.RemoveAt(0);
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            numericUpDown1.ValueChanged +=numericUpDown1_ValueChanged;
            GetConnections();
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
                return;

            if ((string) dataGridView1[0, e.RowIndex].Value == "proc_type")
                MessageBox.Show("Trie");
            if ((string)dataGridView1[0, e.RowIndex].Value == "id_sub_type")
            {
                if(_globalSelector == null)
                    _globalSelector = new GlobalSelector();
                if(!_globalSelector.Opened)
                {
                    var dataGridViewCell = dataGridView1[1,e.RowIndex];
                    _globalSelector.Display(comboBox1.SelectedIndex,ref dataGridViewCell);
                }
            }
        }

        private void GetConnections()
        {
            listBox2.Items.Clear();
            foreach (var gg in Helper.DropSearcher((Item)listBox1.SelectedItem))
            {
                listBox2.Items.Add(gg);
            }
            foreach (var gg in Helper.CraftSearch((Item) listBox1.SelectedItem))
            {
                listBox2.Items.Add(gg);
            }
            listBox2.DoubleClick += listBox2_DoubleClick;
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;
            var sp = listBox2.SelectedItem.ToString().Split(' ');
            int newList;
            var list = int.Parse(sp[2].Trim());
            var id = sp[5];
            var it = Helper.SearchItem(id, (list - 1), out newList, false, false);
            if (newList >= 59)
                newList -= 1;
            Setter(newList, it);
        }

        private int GetCurrentList()
        {
            return int.Parse(((KeyValuePair<string, Item[]>) comboBox1.SelectedItem).Key.Split(' ')[0]);
        }
        private void ListBoxRepaint(bool full = false)
        {
                if (full)
                    listBox1.DataSource = null;
                listBox1.DataSource = Helper._elReader.Items.ElementAt(SelectedIndex).Value;
                listBox1.DisplayMember = "EditorView";
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            foreach (Item it in listBox1.SelectedItems)
            {
                it.SetByKey((string)dataGridView1[0, e.RowIndex].Value, dataGridView1[1, e.RowIndex].Value);
            }
         //   MessageBox.Show(string.Format("Column : {0}\n Row : {1}", e.ColumnIndex, e.RowIndex));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
                ((Item)listBox1.SelectedItem).SetByKey("ID",Convert.ToInt32(numericUpDown1.Value));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int newList = (comboBox1.SelectedIndex + 1);
            var it = Helper.SearchItem(textBox4.Text, comboBox1.SelectedIndex, out newList, checkBox2.Checked, checkBox3.Checked);
            Setter(newList, it);
        }

        private void Setter(int newList,Item it)
        {
            if (newList != (comboBox1.SelectedIndex + 1))
                comboBox1.SelectedIndex = (newList - 1);
            listBox1.SelectedItem = it;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Item tt in listBox1.SelectedItems)
            {
                AddItems(tt);
            }
            ListBoxRepaint();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var id = Helper.FindCoord(textBox3.Text);
            if (Helper._cropped != null && Helper._cropped.ContainsKey(Path.GetFileName(textBox3.Text)) &&
                textBox3.Text != "")
                pictureBox1.Image = Helper._cropped[Path.GetFileName(textBox3.Text)];
            else pictureBox1.Image = null;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog dd = new SaveFileDialog();
            dd.ShowDialog();
            if(File.Exists(dd.FileName))
                File.Delete(dd.FileName);
            Helper._elReader.Save(dd.FileName);
        }

        private void инфоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var gg = new InfoForm();
            gg.Show();
        }
    }
}
