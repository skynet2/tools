import java.io.*;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.MappedByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.Charset;

/**
 * Created by stas on 11.02.2015.
 */
public class BinWriter {
    private FileChannel _channel;
    private MappedByteBuffer _buff;

    public BinWriter(String path) throws IOException {
        File f = new File(path);
        _channel = new RandomAccessFile(f, "rw").getChannel();
        long bufferSize=8*1000;
        _buff = _channel.map(FileChannel.MapMode.READ_WRITE, 0, bufferSize);
    }

    public void writeInt32(int val) {
        write(ByteBuffer.allocate(4).
                order(ByteOrder.LITTLE_ENDIAN).
                putInt(val).array());
    }
    public void writeFloat(float val) {
        write(ByteBuffer.allocate(4).
            order(ByteOrder.LITTLE_ENDIAN).
            putFloat(val).array());

    }
    public void writeUTF16(String val) {
        byte[] bb = val.getBytes(Charset.forName("UTF-16LE"));
        write(ByteBuffer.wrap(bb)
                .order(ByteOrder.LITTLE_ENDIAN)
                .array());
    }
    public void write(byte[] bb)
    {
        _buff.put(bb);
    }
    public void close() throws IOException {
        _channel.force(false);
        _channel.close();
        _channel = null;
        _buff = null;
        System.gc();
    }
}
