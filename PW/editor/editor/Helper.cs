using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using pwApi.Readers;
using pwApi.StructuresElement;

namespace editor
{
    class Helper
    {
        public static ElementReader _elReader;
        public static Image _img;
        public static List<string> _surfaces;
        public static Dictionary<string, Image> _cropped;

        public static Image GetImage(int id,bool is70Page = true)
        {
            try
            {
                string path = "";
                if (is70Page)
                    path = _elReader.GetIcon70(Convert.ToInt32(id));
                else path = _elReader.GetIcon(id);

                if(string.IsNullOrEmpty(path))
                    return _cropped.ElementAt(0).Value;

                path = Path.GetFileName(path.Replace("\0", ""));
                if (_cropped.ContainsKey(path))
                    return _cropped[path];
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.ToString());
            }
            return _cropped.ElementAt(0).Value;

        }
        public static void CropImages()
        {
            _cropped = new Dictionary<string, Image>();
            foreach (var ll in _surfaces)
            {
                _cropped.Add(ll,Graphic.GetImage(_img,ll));
            }
            Graphic.bmpImage.Dispose();
            Graphic.bmpImage = null;
        }
        public static List<string> DropSearcher(Item it)
        {
            var ls = new List<string>();
            foreach (var line in _elReader.GetListById(39))
            {
                var id = it.GetByKey("ID");
                for (int i = 189; i < 252; i++)
                {
                    var zz = (string)line.Values[i, 0];
                    if (zz.Contains("drop_matters_") && zz.Contains("id"))
                    {
                        if (id == (int)line.Values[i, 1])
                            ls.Add(String.Format("List : {0} ID : {1} Name : {2} Шанс : {3}", 39, line.GetByKey("ID"),
                                ((string)line.GetByKey("Name")).Replace("\0",""), line.Values[++i, 1]));
                    }
                }
            }
            return ls;
        }

        public static List<string> CraftSearch(Item it)
        {
            var ls = new List<string>();
            int id = Convert.ToInt32(it.GetByKey("ID"));
            foreach (var item in _elReader.GetListById(70))
            {
                for(int i = 1; i < 4; i++)
                    if (Convert.ToInt32(item.GetByKey(String.Format("targets_{0}_id_to_make", i))) == id)
                        ls.Add(String.Format("List : {0} ID : {1} Name : {2} Рецепт крафта : {3}",70,
                            item.GetByKey("ID"),item.GetByKey("Name"),i));
            }
            return ls;
        }
        public static Item SearchItem(string param, int currList, out int newList, bool full , bool mat,Item selected)
        {
            currList++;
            newList = currList;
            Item it = null;
            var final = !full ? currList : _elReader.Items.Count;

            for (int i = currList; i <= final; ++i)
            {
                newList = i;
                bool found = false;
                foreach (Item item in _elReader.GetListById(i))
                {
                    if (selected != null)
                    {
                        if (!found)
                        {
                            if (item == selected)
                            {
                                found = true;
                                selected = null;
                            }
                            continue;
                        }
                    }
                    if (item.GetByKey("ID").ToString() == param)
                    {   
                        return item;
                    }
                    if (!mat)
                    {
                        if (((string) item.GetByKey("Name")).Replace("\0","").Contains(param))
                        {
                            return item;
                        }
                    }
                    else if (((string) item.GetByKey("Name")).Replace("\0","") == param)
                    {
                        return item;
                    }


                }
            }
            return it;
        }

        public static void LoadSurfaces()
        {
            _surfaces = new List<string>();
            try
            {
                _img = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "iconlist_ivtrf.png"));
                foreach (var line in File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "iconlist_ivtrf.txt"), Encoding.GetEncoding(936)))
                {
                    if (Path.GetExtension(line) == ".dds")
                        _surfaces.Add(line);
                }
                new Thread(CropImages).Start();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.ToString());
            }


        }

        public static int[] FindCoord(string val)
        {
            int i = 0;
            int x = 0;
            int y = 0;
            if (val == null)
                return new[] {0, 0};
            val = val.Replace("\0","");
            if (i >= _surfaces.Count)
                return new[] { 0, 0 };
            while (Path.GetFileName(val) != _surfaces[i])
            {
                x += 32;
                if (x >= 4096)
                {
                    x = 0;
                    y += 32;
                }
                i++;
                if (i >= _surfaces.Count)
                {
                    return new[] {32, 0};
                }
            }
            return new [] {x, y};
        }

        public static Dictionary<short, string> _versions = new Dictionary<short, string>();

        public static void LoadElementConfigs()
        {
            foreach (var vv in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "configs"), "*v*.*", SearchOption.AllDirectories))
            {
                var name = Path.GetFileName(vv);
                var key = Int16.Parse(Regex.Match(name, "v(\\d*)[.]").Groups[1].Value);
                if (!Helper._versions.ContainsKey(key))
                    Helper._versions.Add(key, vv);
            }
        }

        public static Dictionary<string, HashSet<string>> Bonus4Page { get; set; }
        public static Dictionary<string, HashSet<string>> Bonus7Page { get; set; }
        public static Dictionary<string, HashSet<string>> Bonus10Page { get; set; }

        public static string GetBonus(int list,string bonusid)
        {
            try
            {
                Dictionary<string, HashSet<string>> temp = null;
                switch (list)
                {
                    case 4:
                        temp = Helper.Bonus4Page;
                        break;
                    case 7:
                        temp = Helper.Bonus7Page;
                        break;
                    case 10:
                        temp = Helper.Bonus10Page;
                        break;
                }
                foreach (var it in temp)
                {
                    foreach (var val in it.Value)
                    {
                        if (val == bonusid)
                            return it.Key;
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
