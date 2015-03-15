import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

/**
 * Created by stas on 11.03.2015.
 */
class Holder {
    public String Name;
    public Item[] Items;
    public Holder(String n,Item[] it) {
    Name = n;
        Items = it;
    }
}
class EtcBytes {
    public byte Pos;
    public List<byte[]> Bytes;
    public EtcBytes(byte p,List<byte[]> bb) {
        Pos = p;
        Bytes = bb;
    }
}
public class ElementReader {
    public List<Holder> Items;
    private BinaryReader _br;
    private List<ConfigList> _confList;
    public HashSet<Integer> ExistingId;
    private String _path;
    // SAVERS
    private List<EtcBytes> _somevals;

    public ElementReader (String configPath, String elementPath) throws IOException {
        _path = elementPath;
        _confList = ConfigList.ParseList (configPath);
        Items = new ArrayList<>();
        _br = new BinaryReader (elementPath);
        _somevals = new ArrayList<>();
        List<byte[]> bb = new ArrayList<>();
        bb.add(_br.ReadBytes(8));
        _somevals.add(new EtcBytes((byte) 0, bb));

        for (int i = 0; i < _confList.size(); i++) {
            switch (i) {
                case 20:
                    _somevals.add (new EtcBytes((byte)i, Offset20 ()));
                    Items.add(new Holder(_confList.get(i).Name, List(i)));
                    break;
                case 58:
                    _somevals.add(new EtcBytes((byte) i, List58()));
                    break;
                case 100:
                    _somevals.add(new EtcBytes((byte) i, Offset100()));
                    Items.add(new Holder(_confList.get(i).Name, List(i)));
                    break;
                default:
                    Items.add(new Holder(_confList.get(i).Name, List(i)));
                    break;
            }
        }
        _br.close();
        System.out.println("Readed");
    }

 /*   public int GetFreeId ()
    {
        if (ExistingId == null)
            ElementUtils.GetExsistingIDs (this);
        var ra = new Random ();
        int id;
        do {
            id = ra.Next (0, 55000);
        } while(ExistingId.Contains(id));
        ExistingId.Add (id);
        return id;
    }
    */

    public Item[] GetListById (int id)
    {
        return Items.get(GetListKey(id)).Items;
    }

    private int GetListKey (int id)
    {
        for (int i = 0; i < this.Items.size(); i++) {
            String key = this.Items.get(i).Name;
            int list = Integer.parseInt((key.split(" ")[0]));
            if (list == id)
                return i;
        }
        return -1;
    }

    private Item[] List (int i) throws IOException {
        int count = _br.ReadInt32 ();
        Item[] items = new Item[count];
        for (int z = 0; z < count; z++)
            items [z] = Item.ParseItem (_confList.get(i), _br);
        return items;
    }

    private List<byte[]> Offset100 () throws IOException {
        List<byte[]> off100 = new ArrayList<>();
        off100.add(_br.ReadBytes(4));
        int count = _br.ReadInt32 ();
        off100.add(IntToByte(count));
        off100.add(_br.ReadBytes(count));
        return off100;
    }

    private List<byte[]> List58 () throws IOException {
        int count = _br.ReadInt32 ();
        List<byte[]> list59 = new ArrayList<>();
        list59.add(IntToByte(count));
        for (int i = 0; i < count; i++) {
            list59.add(_br.ReadBytes(132));
            int count2 = _br.ReadInt32 ();
            list59.add(IntToByte(count2));
            for (int i2 = 0; i2 < count2; i2++) {
                list59.add(_br.ReadBytes(8));
                int l = _br.ReadInt32 ();
                list59.add(IntToByte(l));
                list59.add(_br.ReadBytes(l * 2));
                int count3 = _br.ReadInt32 ();
                list59.add(IntToByte(count3));
                list59.add(_br.ReadBytes(count3 * 136));
            }
        }
        return list59;
    }

    public Item GetFirstInList(int listID)
    {
        return GetListById(listID)[0];
    }
  /*  public void AddItem (string key, Item newItem)
    {
        var arr = new Item[Items [key].Length + 1];
        newItem.GetByKey("ID");
        Array.Copy (Items [key], arr, Items [key].Length);
        arr [arr.Length - 1] = newItem;
        Items [key] = arr;
    }
*/
  /*  public Item FindInList(int listID, int id)
    {
        foreach (var it in GetListById(listID).Where(it => it.GetByKey("ID") == id))
        return it;
        return null;
    }

    public int AddItem (int id, Item newItem,bool print = false)
    {
        if (ExistingId == null)
            ElementUtils.GetExsistingIDs(this);
        var key = GetListKey (id);
        if(ExistingId.Contains(newItem.GetByKey("ID")))
            newItem.SetByKey("ID",GetFreeId());
        if(print) PrintInfo(newItem);
        AddItem (key, newItem);
        return newItem.GetByKey("ID");
    }

    private static void PrintInfo(Item i)
    {
        Console.WriteLine("ID {0}{1} Name {2}",i.GetByKey("ID"),Environment.NewLine,
                UtilsIO.NormalizeString(i.GetByKey("Name")));
    }
    */
  /*  public Item GetItem (String key, int pos)
    {
        return UtilsIO.DeepClone (Items [key] [pos]);
    }
*/
    private static byte[] IntToByte (int val)
    {
        return ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN).putInt(val).array();
    }

    private List<byte[]> Offset20 () throws IOException {
        List off20 = new ArrayList<>();
        off20.add(_br.ReadBytes(4));
        int count = _br.ReadInt32 ();
        _br.setPos(_br.get_position()-4);
        off20.add(_br.ReadBytes(count + 8));
        return off20;
    }

    public void Save (String newPath) throws IOException {
        if (newPath == null) newPath = _path;
        BinaryWriter bw = new BinaryWriter (newPath);
        WriteList (bw, _somevals.get(0));
        ConfigList l58 = _confList.get(58);
        _confList.remove(l58);
        for (int i = 0; i < Items.size(); i++) {
            if (i == 20) {
                WriteList (bw, _somevals.get(1));
            }
            if (i == 58) {
                WriteList (bw, _somevals.get(2));
            }
            if (i == 99) {
                WriteList (bw, _somevals.get(3));
            }
            bw.WriteInt32(Items.get(i).Items.length);
            for (Item item : Items.get(i).Items)
            item.Save(bw, _confList.get(i));

        }
        _confList.add(58,l58);

    }

    private static void WriteList (BinaryWriter bw, EtcBytes hh ) throws IOException {
        for (byte[] bb : hh.Bytes)
            bw.Write(ByteBuffer.wrap(bb).order(ByteOrder.LITTLE_ENDIAN).array());
    }
}