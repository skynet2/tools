import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

/**
 * Created by stas on 11.03.2015.
 */
public class BinaryWriter extends FileOutputStream {
    public static int pos;
    public BinaryWriter(String name) throws FileNotFoundException {
        super(name);
        pos = 0;
    }

    ;

    public void Write(byte[] bytes) throws IOException {
        if(pos == 30475228)
            pos = 30475228;
        pos += bytes.length;
        write(ByteBuffer.wrap(bytes)
                .order(ByteOrder.LITTLE_ENDIAN).array());
    }
    public void WriteShort(short val) throws IOException {
        Write(ByteBuffer.allocate(2).order(ByteOrder.LITTLE_ENDIAN)
                .putShort(val).array());
    }

    public void WriteInt32(int val) throws IOException {
        Write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
                .putInt(val).array());
    }

    public void WriteFloat(float val) throws IOException {
        Write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
                .putFloat(val).array());
    }

    public void WriteUnicode(String value, int lenght) throws IOException {
        try
        {
            Write(ByteBuffer.allocate(lenght).order(ByteOrder.LITTLE_ENDIAN)
                    .put(value.getBytes("UTF-16LE")).array());
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }

    }
    public void WriteGBK(String value, int lenght) throws IOException {
        Write(ByteBuffer.allocate(lenght).order(ByteOrder.LITTLE_ENDIAN)
                .put(value.getBytes("cp936")).array());
    }
}
