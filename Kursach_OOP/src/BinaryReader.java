import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.MappedByteBuffer;
import java.nio.channels.FileChannel;

/**
 * Created by stas on 11.03.2015.
 */
public class BinaryReader {
    private MappedByteBuffer _buffer;
    private long _lenght;
    private RandomAccessFile _file;
    private FileChannel _channel;

    public int get_position() {
        return _position;
    }

    private static int _position;

    public BinaryReader(String path) throws IOException {
        File temp = new File(path);
        _file = new RandomAccessFile(temp, "rw");
        _channel = _file.getChannel();
        _buffer = _channel.map(FileChannel.MapMode.READ_WRITE, 0, temp.length());
        _position = 0;
        _lenght = _file.length();
    }

    public int ReadInt32() throws IOException {
        return ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getInt();
    }

    public void setPos(int new_pos) throws IOException {
        _position = new_pos;
        _buffer.position(_position);
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

    public long get_lenght() {
        return _lenght;
    }

    public byte[] ReadBytes(int length) throws IOException {
        _position += length;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
            bytes[i] = _buffer.get();
        return ByteBuffer.wrap(bytes).order(ByteOrder.LITTLE_ENDIAN)
                .array();
    }

    public float ReadFloat() throws IOException {
        return ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getFloat();
    }

    public float ReadSingle() throws IOException {
        return ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getFloat();
    }

    public String ReadUnicode(int _lenght) throws IOException {
        byte[] val = new byte[_lenght];
        ByteBuffer.wrap(ReadBytes(_lenght))
                .order(ByteOrder.LITTLE_ENDIAN)
                .get(val);
        return new String(val, "UTF-16LE");
    }

    public String ReadGBK(int _lenght) throws IOException {
        byte[] val = new byte[_lenght];
        ByteBuffer.wrap(ReadBytes(_lenght))
                .order(ByteOrder.LITTLE_ENDIAN)
                .get(val);
        return new String(val, "cp936");
    }
}