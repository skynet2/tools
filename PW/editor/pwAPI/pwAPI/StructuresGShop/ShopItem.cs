using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pwApi.StructuresGShop
{
    public class ShopItem
    {
        public bool Activate;
        public int ShopId;
        public int CatIndex;
        public int SubCatIndex;
        public String SurfacePath; // 128 Byte GBK
        public int ItemId;
        public int ItemAmount;
        public List<SaleOption> SaleOptions = new List<SaleOption>(); // length = 4
        public String Description; // 1024 Byte Unicode
        public String Name; // 64 Byte Unicode
        public int GiftId;
        public int GiftAmount;
        public int GiftDuration;
        public int LogPrice;

        public ShopItem(BinaryReader br)
        {
            Activate = true;
            ShopId = br.ReadInt32();
            CatIndex = br.ReadInt32();
            SubCatIndex = br.ReadInt32();
            SurfacePath = Encoding.GetEncoding(936).GetString(br.ReadBytes(128)); // 128 Byte GBK
            ItemId = br.ReadInt32();
            ItemAmount = br.ReadInt32();
            for (int z = 0; z < 4; z++)
                SaleOptions.Add(new SaleOption(br));
            Description = Encoding.Unicode.GetString(br.ReadBytes(1024));  // 1024 Byte Unicode
            Name = Encoding.Unicode.GetString(br.ReadBytes(64));
            GiftId = br.ReadInt32();
            GiftAmount = br.ReadInt32();
            GiftDuration = br.ReadInt32();
            LogPrice = br.ReadInt32();
        }

        public ShopItem(BinaryWriter bw, ShopItem item, bool client)
        {
            Activate = true;
            bw.Write(item.ShopId);
            bw.Write(item.CatIndex);
            bw.Write(item.SubCatIndex);
            bw.Write(Encoding.GetEncoding(936).GetBytes(item.SurfacePath), 0, 128);
            bw.Write(item.ItemId);
            bw.Write(item.ItemAmount);
            for (int i = 0; i < 4; i++)
                new SaleOption(bw, i);
            if (client)
            {
                bw.Write(Encoding.Unicode.GetBytes(item.Description), 0, 1024);
                bw.Write(Encoding.Unicode.GetBytes(item.Name), 0, 64);
            }
            //Encoding.Unicode.GetBytes(bout.items[z].name));
            bw.Write(item.GiftId);
            bw.Write(item.GiftAmount);
            bw.Write(item.GiftDuration);
            bw.Write(item.LogPrice);
        }

        public ShopItem(int shop_id, int cat_index, int sub_cat_index, int item_id, String name, String surface)
        {
            Activate = true;
            ShopId = shop_id;
            CatIndex = cat_index;
            SubCatIndex = sub_cat_index;
            SurfacePath = surface;
            ItemId = item_id;
            ItemAmount = 1;
            for (int z = 0; z < 4; z++)
                SaleOptions.Add(new SaleOption());
            var temp = new byte[1024];
            // ByteBuffer.wrap(new String("unipw.org").getBytes()).allocate(1024).order(ByteOrder.LITTLE_ENDIAN).get(temp);
            //this.description = new String(temp, Charset.forName("UTF8"));
            Description = Encoding.UTF8.GetString(Utils.UtilsIO.GenerateArray(Encoding.UTF8.GetBytes("unipw.org"), 1024));
            Name = name; // 64 Byte Unicode
            GiftId = 0;
            GiftAmount = 0;
            GiftDuration = 0;
            LogPrice = 0;
        }

    }
}
