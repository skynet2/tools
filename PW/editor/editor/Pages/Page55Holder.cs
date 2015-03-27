using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Windows.Forms;
using pwApi.StructuresElement;

namespace editor.Pages
{
    public class Page55Holder
    {
        public TextBox Name { get; set; }
        public TextBox Skill { get; set; }
        public TextBox Produce { get; set; }
        public Label Desk { get; set; }
        public PictureBox pict { get; set; }
        public TextBox text { get; set; }
        public string param { get; set; }
        public Item it { get; set; }
    }

    public class Page55
    {
        public static GlobalSelector _globalSelector { get; set; }
        public static TextBox skill;
            public static bool Drowed { get; set; }
        public void SetValues55(ref TabControl tabControl1, ref ListBox listBox1)
        {
            if (_55Holders == null)
            {
                Make55Page(ref tabControl1, ref listBox1);
            }
            int i = 0;
            Item it = (Item)listBox1.SelectedItem;
            for (int k = 0; k < 8; k++)
            {
                string str = ((string)it.GetByKey(string.Format("pages_{0}_page_title", (k + 1)))).Replace("\0", "");
                tabControl1.TabPages[k].Controls["pan"].Controls["NameBox"+k].Text = str;

                tabControl1.TabPages[k].Text = str;
            }
            /// Skills 
            /// 
            
            Drowed = true;
            for (int z = 0; z < it.Values.Length / 2; z++)
            {
                if (Convert.ToString(it.Values[z, 0]).Contains("id_goods"))
                {
                    _55Holders[i].param = it.Values[z, 0];
                    _55Holders[i].text.Text = it.Values[z, 1].ToString();
                    _55Holders[i].it = it;
                    _55Holders[i].Skill.Text = it.GetByKey("id_make_skill").ToString();
                    _55Holders[i].Produce.Text = it.GetByKey("produce_type").ToString();
                    var val = it.Values[z, 1];
                    if (Convert.ToInt32(it.Values[z, 1]) != 0)
                    {
                        _55Holders[i].pict.Image =
                            Helper.GetImage(Convert.ToInt32(it.Values[z, 1]));
                    }
                    //Graphic.GetImage(Helper._img,Helper._elReader.GetIcon(int.Parse(it.Values[z, 1].ToString())));
                    else
                    {
                        _55Holders[i].pict.Image = Helper._cropped.ElementAt(0).Value;
                    }

                    i++;
                }
            }
            
            Drowed = false;
        }

        static void text_TextChanged(object sender, EventArgs e)
        {
            if(Drowed)
                return;
            foreach (var it in _55Holders)
            {
                if (it.text == (TextBox) sender)
                {
                    it.it.SetByKey(it.param,int.Parse(((TextBox)sender).Text));
                    it.pict.Image = Helper.GetImage((Convert.ToInt32(((TextBox) sender).Text)));
                }
            }
            
        }
        public static List<Page55Holder> _55Holders { get; set; }
        public static void Make55Page(ref TabControl tabControl1, ref ListBox listBox1)
        {
                listBox1.SelectionMode = SelectionMode.One;
                tabControl1.TabPages.Clear();
                _55Holders = new List<Page55Holder>();
                for (int i = 0; i < 8; i++)
                {
                    var page = new TabPage();
                    page.Name = "Page";
                    var pan = new FlowLayoutPanel
                    {
                        Width = 395,
                        Height = 550,
                        FlowDirection = FlowDirection.LeftToRight,
                        Name = "pan"
                    };
                    var name = new TextBox() { Width = 390, Name = "NameBox"+i };
                    var skill = new TextBox() {Width = 30, Name = "Skill", BackColor = Color.DarkOrange};
                    var deskSkill = new Label() { Width = 320, Name = "SkillDesk_" + i, Text = "158 Кузнец || 159 Портной || 160 Ремесленник || 161 Аптекарь" };
                    var type = new TextBox() {Width = 30, Name = "Type_" + i};
                    var typeD = new Label() {Width = 320,Text = "Тип крафта : 1 - перетянуть, 3 - регрейд, 4 - дух стихий"};
                    pan.Controls.Add(name);
                    pan.Controls.Add(skill);
                    pan.Controls.Add(deskSkill);
                    pan.Controls.Add(type);
                    pan.Controls.Add(typeD);
                    for (int z = 0; z < 32; z++)
                    {
                        var holder = new TableLayoutPanel();
                        _55Holders.Add(new Page55Holder()
                        {
                            pict = new PictureBox() { Height = 32, Width = 32, BackColor = Color.Black , WaitOnLoad = true},
                            text = new TextBox() { Width = 34, Name = "Value" + i + "_" + z, BackColor = Color.DarkOrange },
                            Skill = skill,
                            Produce = type,
                            Desk = deskSkill,
                        });
                        holder.Controls.Add(_55Holders.Last().pict);
                        holder.Controls.Add(_55Holders.Last().text);
                        _55Holders.Last().text.TextChanged += text_TextChanged;
                        _55Holders.Last().Skill.TextChanged += Skill_TextChanged;
                        _55Holders.Last().Skill.MouseDoubleClick += Skill_MouseDoubleClick;
                        _55Holders.Last().Produce.TextChanged += Produce_TextChanged;
                        _55Holders.Last().text.MouseDoubleClick += text_MouseDoubleClick;
                        holder.Height = 60;
                        holder.Width = 42;
                        pan.Controls.Add(holder);
                    }
                    page.Controls.Add(pan);
                    tabControl1.TabPages.Add(page);

                }
        }

        static void Skill_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_globalSelector == null)
                _globalSelector = new GlobalSelector();
            TextBox tt = (TextBox)sender;
            if (!_globalSelector.Opened)
            {
                _globalSelector.Display(55, ref tt,true);
            }
        }

        static void text_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (_globalSelector == null)
                _globalSelector = new GlobalSelector();
            TextBox tt = (TextBox) sender;
            if (!_globalSelector.Opened)
            {
                _globalSelector.Display(69,ref tt);
            }
        }

        static void Produce_TextChanged(object sender, EventArgs e)
        {
            if (Drowed)
                return;
            ((Item)Program.f.listBox1.SelectedItem).SetByKey("produce_type", ((TextBox)sender).Text);
        }

        static void Skill_TextChanged(object sender, EventArgs e)
        {
            if (Drowed)
                return;
            ((Item)Program.f.listBox1.SelectedItem).SetByKey("id_make_skill",((TextBox)sender).Text);
        }

    }

}
