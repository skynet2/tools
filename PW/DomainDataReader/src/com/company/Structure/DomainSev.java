package com.company.Structure;

import com.company.Utils.BinaryReader;
import com.company.Utils.BinaryWriter;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 13.10.14.
 */
public class DomainSev {
    private int count;
    private byte[] version;
    public List<Domain_zone> zones;

    public DomainSev(BinaryReader br) throws IOException {
        version = br.ReadBytes(4);
        count = br.ReadInt32();
        zones = new ArrayList<Domain_zone>();

        for (int i = 0; i < count; i++)
        {
            zones.add(new Domain_zone(br));
        }
    }
    public DomainSev(BinaryWriter bw, DomainSev local) throws IOException {
        bw.write(local.version);
        bw.write(local.zones.size());
        bw.write(new byte[] {0,0,0});
        for(Domain_zone i : local.zones)
            new Domain_zone(bw,i);
    }

    public static class Domain_zone
    {
        public int _id,_points, _owner, _iscapital, _battletype;
        public List<SpawnP> spawnsList;
        List<Touches> toucheses;
        public Domain_zone(BinaryReader br) throws IOException {
            _id = br.ReadInt32();
            _points = br.ReadInt32();
            _battletype = br.ReadInt32();
            _owner = br.ReadInt32();
            _iscapital = br.ReadInt32();
            spawnsList = new ArrayList<SpawnP>();
            for(int i = 0; i < 4; i++)
                spawnsList.add(new SpawnP(br));
            toucheses = new ArrayList<Touches>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                toucheses.add(new Touches(br));

        }

        public Domain_zone(BinaryWriter bw,Domain_zone i) throws IOException {
            bw.Write(i._id);
            bw.Write(i._points);
            bw.Write(i._battletype);
            bw.Write(i._owner);
            bw.Write(i._iscapital);
            for (int z = 0; z < 4; z++)
            {
                bw.Write(i.spawnsList.get(z)._x);
                bw.Write(i.spawnsList.get(z)._y);
                bw.Write(i.spawnsList.get(z)._z);
            }
            bw.Write(i.toucheses.size());
            for (Touches z : i.toucheses)
            {
                bw.Write(z._id);
                bw.Write(z._time);
            }
        }
    }

    public static class SpawnP
    {
        public float _x, _y, _z;

        public SpawnP(BinaryReader br) throws IOException {
            _x = br.ReadSingle();
            _y = br.ReadSingle();
            _z = br.ReadSingle();
        }
    }

    public static class Touches
    {
        public int _id, _time;

        public Touches(BinaryReader br) throws IOException {
            _id = br.ReadInt32();
            _time = br.ReadInt32();
        }
    }
}

