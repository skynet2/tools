import java.io.*;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.MappedByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.Charset;

/**
 * Created by stas on 11.02.2015.
 */
public class BinWriter extends FileOutputStream {

    public static int pos;
    public BinWriter(String name) throws FileNotFoundException {
        super(name);
        pos = 0;
    }

    ;

    public void Write(byte[] bytes) throws IOException {
        pos += bytes.length;
        write(ByteBuffer.wrap(bytes)
                .order(ByteOrder.LITTLE_ENDIAN).array());
    }
    public void WriteShort(short val) throws IOException {
        pos += 2;
        write(ByteBuffer.allocate(2).order(ByteOrder.LITTLE_ENDIAN)
                .putShort(val).array());
    }

    public void writeInt32(int val) throws IOException {
        pos +=4;
        write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
                .putInt(val).array());
    }

    public void writeFloat(float val) throws IOException {
        pos +=4;
        write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
                .putFloat(val).array());
    }

    public void writeUTF16(String value, int lenght) throws IOException {
        try
        {
            value = value.replace("\\u0000","");
            pos += lenght;
            write(ByteBuffer.allocate(lenght).order(ByteOrder.LITTLE_ENDIAN)
                    .put(value.getBytes("UTF-16LE")).array());
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }

    }
    public void writeGBK(String value, int lenght) throws IOException {
        pos +=lenght;
        write(ByteBuffer.allocate(lenght).order(ByteOrder.LITTLE_ENDIAN)
                .put(value.getBytes("cp936")).array());
    }
}
