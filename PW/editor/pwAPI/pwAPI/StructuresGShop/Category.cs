using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pwApi.StructuresGShop
{
    public class Category
    {
        public String name; // 128 Byte Unicode
        public int sub_cats_count;
        public List<String> sub_cats = new List<String>();

        public Category(BinaryReader br)
        {
            name = Encoding.Unicode.GetString(br.ReadBytes(128));
            sub_cats_count = br.ReadInt32();
            for (int i = 0; i < this.sub_cats_count; i++)
                sub_cats.Add(Encoding.Unicode.GetString(br.ReadBytes(128)));
        }
        public Category(BinaryWriter bw, Category cat)
        {
            bw.Write(Encoding.Unicode.GetBytes(cat.name), 0, 128);
            bw.Write(cat.sub_cats_count);
            foreach (String s in cat.sub_cats)
                bw.Write(Encoding.Unicode.GetBytes(s), 0, 128);
        }
    }
}
