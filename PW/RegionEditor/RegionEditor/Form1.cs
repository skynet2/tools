using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using pwApi.Readers;
using pwApi.StructuresRegion;
using pwApi.Utils;
using Point = pwApi.StructuresRegion.Point;
using Region = pwApi.StructuresRegion.Region;

namespace RegionEditor
{
    public partial class dest : Form
    {
        public dest()
        {
            InitializeComponent();
        }

        private RegionReader _reader;
        private List<Config> _configs;
        private Image BeforeDot { get; set; }
        private bool ImageChanged { get; set; }
        private Image BeforeDot2 { get; set; }
        private Pen _pen = new Pen(Color.Green, 3);

        private void UpdateMaps()
        {
            comboBox3.Items.Clear();
            foreach (var it in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "maps")))
            {
                comboBox3.Items.Add(Path.GetFileName(it));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sourcebox.LostFocus += textBox4_TextChanged;
            destbox.LostFocus += textBox4_TextChanged;
            lvlbox.LostFocus += textBox4_TextChanged;
            UpdateMaps();
            panel1.AutoScroll = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            tabControl1.Selecting += tabControl1_TabIndexChanged;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.CellMouseClick += dataGridView1_CellMouseClick;
            var column0 = new DataGridViewTextBoxColumn()
            {
                Name = "column0",
                HeaderText = "#",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            };
            var column1 = new DataGridViewTextBoxColumn()
            {
                Name = "column1",
                HeaderText = "X",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            var column2 = new DataGridViewTextBoxColumn()
            {
                Name = "column2",
                HeaderText = "Y",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            var column3 = new DataGridViewTextBoxColumn()
            {
                Name = "column3",
                HeaderText = "Z",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dataGridView1.Columns.Add(column0);
            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            _reader = new RegionReader(textBox1.Text);
            listBox1.DataSource = _reader.Region.Regions;
            listBox1.DisplayMember = "id";
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var box = (TextBox) sender;
                var it = (Transport) listBox1.SelectedItem;
                switch (box.Name)
                {
                    case "sourcebox":
                        it.m_idSrcInst = int.Parse(box.Text);
                        break;
                    case "destbox":
                        it.m_idInst = int.Parse(box.Text);
                        break;
                    case "lvlbox":
                        it.iLevelLmt = int.Parse(box.Text);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var it = comboBox3.SelectedItem.ToString();
                var image = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "maps", it));
                BeforeDot = (Image) image.Clone();
                pictureBox1.Image = image;
                ImageChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
                return;
            if (pictureBox1.Image != null)
            {
                if (BeforeDot2 == null || ImageChanged)
                {
                    BeforeDot2 = UtilsIO.DeepClone(pictureBox1.Image);
                    ImageChanged = false;
                }
                else
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = (Image) BeforeDot2.Clone();
                    pictureBox1.Update();
                }
            }
            dynamic val;
            if (tabControl1.SelectedIndex == 0)
                val = (Region) listBox1.SelectedItem;
            else val = (Transport) listBox1.SelectedItem;

            Point p = ((List<Point>) val.Points)[dataGridView1.CurrentCell.RowIndex];
            if (pictureBox1.Image == null) return;
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.FillEllipse(new SolidBrush(Color.Crimson), GetX(p.x) - 5, GetZ(p.z) - 5, 10, 10);
            }
            panel1.VerticalScroll.Value = 0;
            panel1.HorizontalScroll.Value = 0;
            if (Convert.ToInt32(GetZ(p.z) - 70) > panel1.VerticalScroll.Maximum ||
                Convert.ToInt32(GetX(p.x) - 60) > panel1.HorizontalScroll.Maximum )
                return;
            panel1.VerticalScroll.Value = Convert.ToInt32(GetZ(p.z) - 70);
            panel1.HorizontalScroll.Value = Convert.ToInt32(GetX(p.x) - 60);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }


        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Parent = tabControl1.SelectedTab;
            listBox1.Parent = tabControl1.SelectedTab;
            RepaintList();
            dataGridView1.Rows.Clear();
        }

        private void RepaintList(bool force = false)
        {
            if (force)
            {
                listBox1.DataSource = null;
            }
            if (tabControl1.SelectedIndex == 0)
            {
                listBox1.DataSource = _reader.Region.Regions;
                groupBox2.Visible = false;
            }
            else
            {
                listBox1.DataSource = _reader.Region.Transp;
                groupBox2.Visible = true;
            }
            listBox1.DisplayMember = "id";
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var reg = ((Region) listBox1.SelectedItem).Points[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 1:
                        reg.x = float.Parse(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
                        break;
                    case 2:
                        reg.y = float.Parse(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
                        break;
                    case 3:
                        reg.z = float.Parse(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
                        break;
                }
                pictureBox1.Image = (Image) BeforeDot.Clone();
                DataRepaint();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (listBox1.SelectedIndex < 0)
                    return;
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = (Image) BeforeDot.Clone();
                }
                DataRepaint();
                if (tabControl1.SelectedIndex == 0)
                    return;
                var it = (Transport) listBox1.SelectedItem;
                sourcebox.Text = it.m_idSrcInst.ToString();
                destbox.Text = it.m_idInst.ToString();
                lvlbox.Text = it.iLevelLmt.ToString();
        }

        private float GetX(float currX)
        {
            if (currX < 0)
                currX = Math.Abs(Math.Abs(currX) - 4096);
            else currX = Math.Abs(currX) + 4096;
            return currX/4;
        }

        private float GetZ(float currZ)
        {
            if (currZ > 0)
                currZ = Math.Abs(Math.Abs(currZ) - 5632);
            else currZ = Math.Abs(currZ) + 5632;
            return currZ/4;
        }

        private void DataRepaint()
        {
            dynamic reg;
            if (tabControl1.SelectedIndex == 0)
            {
                reg = (Region) listBox1.SelectedItem;
            }
            else
            {
                reg = (Transport) listBox1.SelectedItem;
            }
            dataGridView1.Rows.Clear();
            Point last = null;
            var path = new GraphicsPath();
            path.StartFigure();

            if (pictureBox1.Image != null && reg.Points.Count >= 3)
            {
                if (reg is Region)
                {
                    var ls = new List<PointF>();
                    for (int i = 0; i < reg.Points.Count; i++)
                    {
                        Point pp = reg.Points[i];
                        last = pp;
                        ls.Add(new PointF(GetX(pp.x), GetZ(pp.z)));
                    }
                    path.AddPolygon(ls.ToArray());
                }
                else
                {
                    last = reg.Points[0];
                    path.AddLine(GetX(reg.Points[0].x), GetZ(reg.Points[0].z), GetX(reg.Points[2].x),
                        GetZ(reg.Points[2].z));
                }
                path.CloseFigure();
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawPath(_pen, path);
                }
                BeforeDot2 = null;
            }
            for (int i = 0; i < reg.Points.Count; i++)
            {
                Point pp = reg.Points[i];
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() {Value = i});
                row.Cells.Add(new DataGridViewTextBoxCell() {Value = pp.x});
                row.Cells.Add(new DataGridViewTextBoxCell() {Value = pp.y});
                row.Cells.Add(new DataGridViewTextBoxCell() {Value = pp.z});
                dataGridView1.Rows.Add(row);
            }
            if (pictureBox1.Image == null || reg.Points.Count < 3)
                return;
            panel1.VerticalScroll.Value = 0;
            panel1.HorizontalScroll.Value = 0;
            if (Convert.ToInt32(GetZ(last.z) - 70) > panel1.VerticalScroll.Maximum ||
    Convert.ToInt32(GetX(last.x) - 60) > panel1.HorizontalScroll.Maximum)
                return;
            panel1.VerticalScroll.Value = Convert.ToInt32(GetZ(last.z) - 70);
            panel1.HorizontalScroll.Value = Convert.ToInt32(GetX(last.x) - 60);
        }

private void button5_Click(object sender, EventArgs e)
        {
            dynamic reg;
            if (tabControl1.SelectedIndex == 0)
                reg = (Region) listBox1.SelectedItem;
            else 
                reg = (Transport) listBox1.SelectedItem;
            reg.Points.Add(new Point());
            DataRepaint();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var reg = (Region)listBox1.SelectedItem;
            reg.Points.Remove(reg.Points.Last());
            DataRepaint();
        }

        private int HexToInt(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                var mm = new MemoryWork((Process)comboBox2.SelectedItem);
                var cfg = (Config)comboBox1.SelectedItem;
                var t = mm.ReadInt32(HexToInt(cfg.BaseAddr)) + HexToInt(cfg.Coord);
                var coords = mm.ReadInt32(t);
                var point = new Point
                {
                    x = mm.ReadFloat(coords + Convert.ToInt32(cfg.X)),
                    y = mm.ReadFloat(coords + Convert.ToInt32(cfg.Y)),
                    z = mm.ReadFloat(coords + Convert.ToInt32(cfg.Z))
                };
                dynamic selected;
                if (tabControl1.SelectedIndex == 0)
                    selected = (Region)listBox1.SelectedItem;
                else selected = (Transport)listBox1.SelectedItem;
                var points = ((List<Point>)selected.Points);
                points[dataGridView1.SelectedCells[0].RowIndex] = point;
                DataRepaint();
            }
            catch (Exception ex)
            {
                dynamic reg;
                if (tabControl1.SelectedIndex == 0)
                    reg = (Region)listBox1.SelectedItem;
                else
                    reg = (Transport)listBox1.SelectedItem;
                reg.Points.Add(new Point());
                                DataRepaint();
            }

        }

        private void UpdateProcess()
        {
            var process = Process.GetProcesses().Where(
    proc => !string.IsNullOrEmpty(proc.MainWindowTitle) && !string.IsNullOrWhiteSpace(proc.MainWindowTitle)).ToList();
            comboBox2.DataSource = process;
            comboBox2.DisplayMember = "MainWindowTitle";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            _configs = JsonConvert.DeserializeObject<List<Config>>(File.ReadAllText("coord.json", Encoding.UTF8));
            comboBox2.MouseClick += comboBox2_MouseClick;
            comboBox1.DataSource = _configs;
            comboBox1.DisplayMember = "Name";
            UpdateProcess();
        }

        void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateProcess();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(textBox1.Text,textBox1.Text+".bk");
                _reader.Save(textBox1.Text);
                MessageBox.Show("Файл успешно сохранен : " + textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                _reader.SaveAsText(textBox1.Text.Replace(".sev",".clt"));
                MessageBox.Show("Файл успешно сохранен : " + textBox1.Text.Replace(".sev", ".clt"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            textBox1.Text = dialog.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    _reader.Region.Regions.Add(new Region());
                }
                else
                {
                    _reader.Region.Transp.Add(new Transport(_reader.Region.Version));
                }
                RepaintList(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

    }

    class Config
    {
        public string Name { get; set; }
        public string BaseAddr { get; set; }
        public string Coord { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
    }
}
