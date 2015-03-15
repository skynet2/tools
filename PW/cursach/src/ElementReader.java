import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

/**
 * Created by io on 23.02.2015.
 */
public class ElementReader {
    public List<ItemEntry> _items;
    private BinReader _br;
    private List<ConfigList> _confList;
    // SAVERS
    private Hashtable<Byte, List<byte[]>> _somevals;

    public ElementReader(String configPath, String elementPath) throws IOException {
        _confList = ConfigList.ParseList(configPath);
        _items = new ArrayList<>();
        _br = new BinReader(elementPath);
        _somevals = new Hashtable<>();
        List<byte[]> bb = new ArrayList<>();
        bb.add(_br.ReadBytes(8));
        _somevals.put((byte) 0, bb);

        for (int i = 0; i < _confList.size(); i++) {
            switch (i) {
                case 20:
                    _somevals.put((byte) i, offset20());
                    _items.add(new ItemEntry(_confList.get(i).Name, list(i)));
                    break;
                case 58:
                    _somevals.put((byte) i, list58());
                    break;
                case 100:
                    _somevals.put((byte) i, offset100());
                    list(i);
                    break;
                default:
                    _items.add(new ItemEntry(_confList.get(i).Name, list(i)));
                    break;
            }
        }
        _br.close();
        System.out.println("Readed");
    }

    private Item[] list(int i) throws IOException {
        int count = _br.readInt32();
        Item[] items = new Item[count];
        for (int z = 0; z < count; z++)
            items[z] = Item.ParseItem(_confList.get(i), _br);
        return items;
    }

    private List<byte[]> offset100() throws IOException {
        List<byte[]> off100 = new ArrayList<>();
        off100.add(_br.ReadBytes(4));
        int count = _br.readInt32();
        off100.add(IntToByte(count));
        off100.add(_br.ReadBytes(count));
        return off100;
    }

    private List<byte[]> list58() throws IOException {
        List<byte[]> list59 = new ArrayList<>();
        System.out.println("POS : " + _br.getPos());
        int count = _br.readInt32();
        list59.add(IntToByte(count));
        for (int i = 0; i < count; i++) {
            list59.add(_br.ReadBytes(132));
            int count2 = _br.readInt32();
            list59.add(IntToByte(count2));
            for (int i2 = 0; i2 < count2; i2++) {
                list59.add(_br.ReadBytes(8));
                int l = _br.readInt32();
                list59.add(IntToByte(l));
                list59.add(_br.ReadBytes(l * 2));
                int count3 = _br.readInt32();
                list59.add(IntToByte(count3));
                list59.add(_br.ReadBytes(count3 * 136));
            }
        }
        return list59;
    }

    private static byte[] IntToByte(int val) {
        return ByteBuffer.allocate(4).putInt(val).array();
    }

    private List<byte[]> offset20() throws IOException {
        List<byte[]> off20 = new ArrayList<>();
        off20.add(_br.ReadBytes(4));
        int count = _br.readInt32();
        _br.setPos(_br.getPos() - 4);
        off20.add(_br.ReadBytes(count + 8));
        return off20;
    }

    public void Save(String newPath) throws IOException {
        BinWriter bw = new BinWriter(newPath);
        WriteList(bw, _somevals.get((byte)0));
        ConfigList l58 = _confList.get(58);
        _confList.remove(l58);
        for (int i = 0; i < _items.size(); i++)
        {
            if (i == 20)
            {
                WriteList(bw, _somevals.get((byte)20));
            }
            if (i == 58)
            {
                WriteList(bw, _somevals.get((byte)58));
            }
            if (i == 99)
            {
                WriteList(bw, _somevals.get((byte)100));
            }
            bw.writeInt32(_items.get(i).Items.length);
            for (Item item : _items.get(i).Items)
            try
            {
                item.Save(bw, _confList.get(i));
            }
                catch (Exception e)
                {
                    System.out.println(e.toString());
                }

        }
        _confList.add(58, l58);
    }

    private static void WriteList(BinWriter bw, List<byte[]> ls) throws IOException {
        for(byte[] bb : ls)
            bw.write(bb);
    }
}

