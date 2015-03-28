using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using pwApi.Readers;
using pwApi.StructuresElement;

namespace editor
{
    public partial class ElementConverter : Form
    {
        public ElementConverter()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(textBox1.Text)))
                {
                    button1.Text = br.ReadInt16().ToString();
                }

            }
            catch (Exception e1)
            {

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(textBox2.Text)))
                {
                    button2.Text = br.ReadInt16().ToString();
                }
            }
            catch (Exception)
            {
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            new Thread(() =>
            {
                try
                {
                    UISetter(true);
                    var to = new ElementReader(Helper._versions[Convert.ToInt16(button1.Text)], textBox1.Text);
                    progressBar1.Maximum = to.Items.Count;
                    var from = new ElementReader(Helper._versions[Convert.ToInt16(button2.Text)], textBox2.Text);
                    for (int i = 1; i <= to.Items.Count; i++)
                    {
                        label3.Text = string.Format("Конвертируется лист - {0}", i);
                        if (i == 59)
                            i += 1;
                        progressBar1.Value = i;
                        var removable = new List<Item>();
                        foreach (var it in to.GetListById(i))
                        {
                            removable.Add(it);
                        }
                        if (removable.Count == 0)
                            continue;
                        removable.RemoveAt(0);
                        foreach (var it in removable)
                        {
                            to.RemoveItem(i, it);
                        }
                        // Cleaning
                        foreach (var item in from.GetListById(i))
                        {
                            to.AddItem(i, item, false);
                        }
                        to.RemoveItem(i, to.GetFirstInList(i));
                        //    to.RemoveItem(i, to.GetLastInList(i));
                    }

                    if (File.Exists(textBox1.Text + ".bak"))
                        File.Delete(textBox1.Text + ".bak");

                    File.Move(textBox1.Text,textBox1.Text+".bak");
                    label3.Text = "Сохраняем...";
                    to.Save(textBox1.Text);
                    label3.Text = "Сохранено :)";
                    MessageBox.Show(string.Format("Сохранено в файл {0}\nБекап - {1}", textBox1.Text,
                      Path.GetFileName(textBox1.Text) + ".bak"));
                    UISetter(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    UISetter(false);
                }

            }).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetPath();
        }

        private string GetPath()
        {
            OpenFileDialog ff = new OpenFileDialog();
            ff.ShowDialog();
            return ff.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetPath();
        }

        private void UISetter(bool val)
        {
            textBox1.ReadOnly = val;
            textBox2.ReadOnly = val;
            button1.Enabled = !val;
            button2.Enabled = !val;
            button3.Enabled = !val;
        }
    }
}
