package com.company.Structure;

import com.company.Utils.BinaryReader;
import com.company.Utils.BinaryWriter;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class DomainData {
    public byte[] _header = new byte[8];
    public int _count = 0;
    public List<Domain> domains = new ArrayList<Domain>();
    public byte[] teil;

    public DomainData(BinaryReader br) throws IOException {
        _header = br.ReadBytes(8);
        _count = br.ReadInt32();
        for (int i = 0; i < _count; i++)
            domains.add(new Domain(br));
        teil = new byte[(int) (br.lenght - br.position)];
        long count = br.lenght - br.position;
        teil = br.ReadBytes((int) count);
        br.close();
    }
    public DomainData(BinaryWriter bw,DomainData local) throws IOException {
        bw.Write(local._header);
        bw.Write(local._count);
        for(Domain i : local.domains)
            new Domain(bw,i);
        bw.Write(local.teil);
        bw.close();
    }

    /**
     * Created by io on 13.10.14.
     */
    public static class Domain {
        public String _name;
        public int _id, _points, _battletype, _owner, _iscapital, _count1, _count2, _count3;
        public byte[] myX, myY, xer;
        public List<Coord> coords = new ArrayList<Coord>();
        public List<Neiba> neiba = new ArrayList<Neiba>();

        public Domain(BinaryReader br) throws IOException {
            _name = br.ReadUnicode(32); //Encoding.Unicode.GetString(br.ReadBytes(32));
            _name = _name.replaceAll("[\\u0000-\\u001f]","");
            _id = br.ReadInt32();
            System.out.println("Readed name : " + _name);
            _points = br.ReadInt32();

            _battletype = br.ReadInt32();
            _owner = br.ReadInt32();
            _iscapital = br.ReadInt32();
            for (int i = 0; i < 4; i++)
                coords.add(new Coord(br));
            xer = br.ReadBytes(8);
            _count1 = br.ReadInt32();
            myX = br.ReadBytes(_count1 * 8);
            _count2 = br.ReadInt32();
            myY = br.ReadBytes(_count2 * 12);
            _count3 = br.ReadInt32();
            for (int i = 0; i < _count3; i++)
                neiba.add(new Neiba(br));
        }

        public Domain(BinaryWriter bw, Domain i) throws IOException {
            bw.WriteUnicode(i._name);
            bw.Write(i._id);
            bw.Write(i._points);
            bw.Write(i._battletype);
            bw.Write(i._owner);
            bw.Write(i._iscapital);
            for (int z = 0; z < 4; z++)
                new Coord(bw, i, z);
            bw.Write(i.xer);
            bw.Write(i._count1);
            bw.Write(i.myX);
            bw.Write(i._count2);
            bw.Write(i.myY);
            bw.Write(i._count3);
            for (int z = 0; z < i._count3; z++)
                new Neiba(bw, i, z);
        }
    }

    /**
     * Created by io on 13.10.14.
     */
    static class Coord {
        public float _x, _y, _z;

        public Coord(BinaryReader br) throws IOException {
            _x = br.ReadSingle();
            _y = br.ReadSingle();
            _z = br.ReadSingle();
        }

        public Coord(BinaryWriter bw, Domain i, int pos) throws IOException {
            bw.Write(i.coords.get(pos)._x);
            bw.Write(i.coords.get(pos)._y);
            bw.Write(i.coords.get(pos)._z);
        }
    }

    /**
     * Created by io on 13.10.14.
     */
    static class Neiba {
        public int _xz;

        public Neiba(BinaryReader br) throws IOException {
            _xz = br.ReadInt32();
        }

        public Neiba(BinaryWriter bw, Domain g, int index) throws IOException {
            bw.Write(g.neiba.get(index)._xz);
        }
    }
}
