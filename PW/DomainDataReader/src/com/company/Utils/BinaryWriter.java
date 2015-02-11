package com.company.Utils;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.math.BigInteger;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.charset.Charset;


/**
 * Created by io on 12.10.14.
 */
public class BinaryWriter extends FileOutputStream {
    public BinaryWriter(String name) throws FileNotFoundException {
        super(name);
    };

    public void Write(byte[] bytes) throws IOException {
        write(ByteBuffer.wrap(bytes)
                .order(ByteOrder.LITTLE_ENDIAN).array());
    }
    public void Write(int val) throws IOException {
        write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
        .putInt(val).array());
    }
    public void Write(float val) throws IOException {
        write(ByteBuffer.allocate(4).order(ByteOrder.LITTLE_ENDIAN)
                .putFloat(val).array());
    }
    public void WriteUnicode(String value) throws IOException {
        write(ByteBuffer.allocate(32).order(ByteOrder.LITTLE_ENDIAN)
                .put(value.getBytes("UTF-16LE")).array());
    }


}
