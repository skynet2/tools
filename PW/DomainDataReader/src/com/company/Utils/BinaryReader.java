package com.company.Utils;

import java.io.*;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

public class BinaryReader extends FilterInputStream
{
    public long lenght;
    public long position = 0;

    public BinaryReader(InputStream in) throws IOException {
        super(in);
        lenght = in.available();
    }

    public int ReadInt32() throws IOException {
        return  ByteBuffer.wrap(this.ReadBytes(4))
                .order(ByteOrder.LITTLE_ENDIAN)
                .getInt();
    }

    public byte[] ReadBytes(int _length) throws IOException {
        position += _length;
        byte[] bytes = new byte[_length];
        this.read(bytes);
        return bytes;
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
        return new String(val,"UTF-16LE");
    }
}
