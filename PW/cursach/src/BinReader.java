import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.MappedByteBuffer;
import java.nio.channels.FileChannel;

/**
 * Created by stas on 11.02.2015.
 */
public class BinReader {
    private MappedByteBuffer _buffer;
    private FileChannel _channel;
    private RandomAccessFile _file;
    public BinReader(String path) throws IOException {
        File temp = new File(path);
        _file = new RandomAccessFile(temp,"r");
        _channel = _file.getChannel();
        _buffer = _channel.map(FileChannel.MapMode.READ_ONLY, 0, temp.length());
    }
    public byte[] ReadBytes(int length) throws IOException {
        byte[] bytes = new byte[length];
        for(int i = 0; i < length; i++)
            bytes[i] = _buffer.get();
        return bytes;
    }
    public float readFloat() throws IOException {
        return ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getFloat();
    }
    public float readSingle() throws IOException {
        return ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getFloat();
    }
    public String readUnicode() throws IOException {
        int _lenght = readInt32();
        byte[] val = new byte[_lenght];
        ByteBuffer.wrap(ReadBytes(_lenght))
                .order(ByteOrder.LITTLE_ENDIAN)
                .get(val);
        return new String(val, "UTF-16LE");
    }
    public String readUnicode(int _lenght) throws IOException {
        byte[] val = new byte[_lenght];
        ByteBuffer.wrap(ReadBytes(_lenght))
                .order(ByteOrder.LITTLE_ENDIAN)
                .get(val);
        return new String(val, "UTF-16LE");
    }
    public String readGBK(int _lenght) throws IOException {
        byte[] val = new byte[_lenght];
        ByteBuffer.wrap(ReadBytes(_lenght))
                .order(ByteOrder.LITTLE_ENDIAN)
                .get(val);
        return new String(val, "cp936");
    }
    public int readInt32() throws IOException {
        return ByteBuffer.wrap(ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getInt();
    }

    public int getPos()
    {
        return _buffer.position();
    }
    public int getLenght()
    {
        return _buffer.capacity();
    }
    public void setPos(int pos)
    {
        _buffer.position(pos);
    }
    public void close() throws IOException {
        _channel.force(false);
        _channel.close();
        _channel = null;
        _file.close();
        _file = null;
        _buffer = null;
        System.gc();
    }
}
