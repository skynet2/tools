using System;
using System.Drawing;
using System.IO;
using System.Text;
using pwApi.Utils;

namespace pwApi.StructuresElement
{
    [Serializable]
    public class Item
    {
        public dynamic[,] Values;
        public Item(dynamic[,] items)
        {
            Values = items;
        }
        public string Name
        {
            get
            {
                return Values[1,1];
            }
        }
        public string EditorView
        {
            get
            {
                return string.Format("{0} - {1}",GetByKey("ID"),GetByKey("Name"));
            }
        }
        public void Save(BinaryWriter bw, ConfigList ls)
        {

            for (int i = 0; i < Values.Length / 2; i++)
            {
                var type = ls.Types[i];
                if (type.Size == 236)
                    type.Size = 236;
                switch (type._type)
                {
                    case "wstring:":
                     //   bw.Write(Encoding.Unicode.GetBytes(Values[i, 1]), 0, type.Size);
                        bw.Write(UtilsIO.GenerateArray(Encoding.Unicode.GetBytes(Values[i, 1]), type.Size));
                        break;
                    case "string:":
                   //     bw.Write(Encoding.GetEncoding(936).GetBytes(Values[i, 1]),0,type.Size);
                        bw.Write(UtilsIO.GenerateArray(Encoding.GetEncoding(936).GetBytes(Values[i, 1]), type.Size));
                        break;
                    case "int32":
                        bw.Write(Convert.ToInt32(Values[i, 1]));
                        break;
                    case "float":
                        bw.Write(Convert.ToSingle(Values[i, 1]));
                        break;
                    default:
                        bw.Write(Values[i, 1]);
                        break;
                }
            }
        }
        public dynamic GetByKey(string key)
        {
            try
            {
                for (var i = 0; i < Values.Length; i++)
                    if (Values[i, 0] == key)
                        return Values[i, 1];
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
        public void SetByKey(string key, dynamic val)
        {
            for (int i = 0; i < Values.Length; i++)
                if (Values[i, 0] == key)
                {
                    Values[i, 1] = val;
                    return;
                }
        }


        public dynamic GetByPos(int pos)
        {
            return Values[pos, 1];
        }
        public int GetPos(string key)
        {
            for (int i = 0; i < Values.Length; i++)
                if (Values[i, 0] == key)
                    return i;
            return -1;
        }
        public static Item ParseItem(ConfigList cfg, BinaryReader br)
        {
            var vvv = new dynamic[cfg.Types.Length, 2];
            var i = 0;
            foreach (var tt in cfg.Types)
            {
                switch (tt._type)
                {
                    case "int32":
                        vvv[i, 0] = tt.Name;
                        vvv[i, 1] = br.ReadInt32();
                        break;
                    case "string:":
                        vvv[i, 0] = tt.Name;
                        vvv[i, 1] = Encoding.GetEncoding(936)
                            .GetString(br.ReadBytes(tt.Size));
                        break;
                    case "wstring:":
                        vvv[i, 0] = tt.Name;
                        vvv[i, 1] = Encoding.Unicode.GetString(

                            br.ReadBytes(tt.Size));
                        break;
                    case "float":
                        vvv[i, 0] = tt.Name;
                        vvv[i, 1] = br.ReadSingle();
                        break;
                    default:
                        Console.WriteLine("unknow type : " + tt._type);
                        break;
                }
                i++;
            }
            return new Item(vvv);
        }
    }

}
