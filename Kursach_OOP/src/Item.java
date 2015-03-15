import java.io.IOException;
import java.lang.reflect.Array;

/**
 * Created by stas on 11.03.2015.
 */
public class Item {
    public String[][] Values;
    public Item (String[][] items)
    {
        Values = items;
    }
    public void Save(BinaryWriter bw,ConfigList ls) throws IOException {
        System.out.println(Values[0][1]);
        System.out.println(ls.Name);
        for (int i = 0; i < Values.length ; i++) {

            Type type = ls.Types [i];
            if (type.Size == 236)
                type.Size = 236;
            switch (type._type)
            {
                case "wstring:":
                    bw.WriteUnicode (Values [i][1], type.Size);
                    break;
                case "string:":
                    bw.WriteGBK (Values [i][1], type.Size);
                    break;
                case "int32":
                    bw.WriteInt32(Integer.parseInt(Values [i][1]));
                    break;
                case "float":
                    bw.WriteFloat (Float.parseFloat(Values [i][1]));
                    break;
                default:
                    bw.WriteFloat (Float.parseFloat(Values [i][1]));
                    break;
            }
        }
    }

    public String GetByKey(String key)
    {
        for (int i = 0; i < Values.length; i++)
            if (Values [i][0].equals(key))
        return Values [i][1];
        return null;
    }
    public void SetByKey(String key,String val)
    {
        for (int i = 0; i < Values.length; i++)
        if (Values [i][0].equals(key)) {
        Values [i][1] = val;
        return;
    }
    }


    public String GetByPos(int pos)
    {
        return Values [pos][1];
    }
    public int GetPos(String key)
    {
        for (int i = 0; i < Values.length; i++)
            if (Values [i][0].equals(key))
        return i;
        return -1;
    }
    public static Item ParseItem(ConfigList cfg,BinaryReader br) throws IOException {
        String[][] vvv = new String[cfg.Types.length][2];
        int i = 0;
        for (Type tt : cfg.Types)
        {
            switch (tt._type) {
                case "int32":
                    vvv [i][0] = tt.Name;
                    vvv [i][1] = String.valueOf(br.ReadInt32 ());
                    break;
                case "string:":
                    vvv [i][0] = tt.Name;
                    vvv [i][1] = br.ReadGBK(tt.Size);
                    break;
                case "wstring:":
                    vvv [i][0] = tt.Name;
                    vvv[i][1] = br.ReadUnicode(tt.Size);
                    break;
                case "float":
                    vvv [i][0] = tt.Name;
                    vvv [i][1] = String.valueOf(br.ReadSingle());
                    break;
                default :
                    System.err.println("unknow type : " + tt._type);
                    break;
            }
            i++;
        }
        return new Item(vvv);
    }
}

