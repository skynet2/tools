using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using pwApi.StructuresGShop;

namespace pwApi.Readers
{
    public class GShopReader
    {
        public int Timestamp;
        public int ItemCount;
        public List<ShopItem> Items;
        public List<Category> Categories = new List<Category>();
        public void Find(int id)
        {
            foreach (var i in Items.Where(i => i.ItemId == id))
            {
                PrintInfo(i);
            }
        }
        public void Find(String name)
        {
            foreach (var i in Items.Where(i => i.Name == name))
            {
                PrintInfo(i);
            }
        }

        private void PrintInfo(ShopItem i, bool flag = false)
        {
            var cat = Categories[i.CatIndex];
            Console.WriteLine("================");
            Console.WriteLine("ID : {0}{1}Name : {2}{1}Category : {3}{1}SubCat : {4}",
                i.ItemId, Environment.NewLine, i.Name.Replace("\0", ""), cat.name.Replace("\0", ""), cat.sub_cats[i.SubCatIndex].Replace("\0", ""));
        }

        public void RemoveItem(ShopItem i, bool print = false)
        {
            if (print) PrintInfo(i);
            Items.Remove(i);
        }
        public GShopReader(string gShopPath)
        {
            var br = new BinaryReader(File.OpenRead(gShopPath));
            Timestamp = br.ReadInt32();
            ItemCount = br.ReadInt32();
            Items = new List<ShopItem>();
            for (var i = 0; i < ItemCount; i++)
            {
                Items.Add(new ShopItem(br));
            }
            for (var i = 0; i < 8; i++)
                Categories.Add(new Category(br));
            Console.WriteLine("ShopReaded");
            br.Close();
        }

        public void AddItem(int id, int cat, int subcat, string name, string icon, bool flag = false)
        {
            var i = new ShopItem(id, cat, subcat, id, name, icon);
            Items.Add(i);
            if (flag) PrintInfo(i);
        }

        public void ChangeDescription(ShopItem i, string newDesk)
        {
            i.Description = Encoding.UTF8.GetString(Utils.UtilsIO.GenerateArray(Encoding.UTF8.GetBytes(newDesk), 1024));
        }
        public void Save(string newPath)
        {
            var bw = new BinaryWriter(File.OpenWrite(newPath));
            bw.Write(Timestamp);
            bw.Write(Items.Count);
            foreach (ShopItem it in Items)
                new ShopItem(bw, it, true);
            foreach (Category cat in Categories)
                new Category(bw, cat);
        }
    }
}
