using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pwApi.StructuresRegion
{
    public class Transport
    {
        public int m_idInst { get; set; }
        public int m_idSrcInst { get; set; }
        public int iLevelLmt { get; set; }
        public List<Point> Points { get; set; }
        public int id { get; set; }
        public Transport(BinaryReader br, int version,int id)
        {
            this.id = id;
            m_idInst = br.ReadInt32();
            if (version > 2)
                m_idSrcInst = br.ReadInt32();
            if (version > 4)
                iLevelLmt = br.ReadInt32();

            Points = new List<Point>();
            for(int i = 0; i < 3; i++)
                Points.Add(new Point(br));
        }

        public Transport(int version)
        {
            id = new Random().Next(0,1000);
            m_idInst = 0;
            if (version > 2)
                m_idSrcInst = 0;
            if (version > 4)
                iLevelLmt = 0;
            Points.Add(new Point());
                Points.Add(new Point(5,5,5));
                Points.Add(new Point());
        }
        public void SaveAsText(StreamWriter wr)
        {
            wr.WriteLine("[trans]");
            wr.WriteLine("{0}  {1}  {2}", m_idInst, m_idSrcInst, iLevelLmt);
            foreach (var point in Points)
            {
                point.SaveAsText(wr);
            }
        }
        public void Save(BinaryWriter bw,int version)
        {
            bw.Write(m_idInst);
            if(version > 2)
                bw.Write(m_idSrcInst);
            if(version > 4)
                bw.Write(iLevelLmt);
            foreach (var p in Points)
            {
                p.Save(bw);
            }
        }
    }

    public class Point
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Point(BinaryReader br)
        {
            x = br.ReadSingle();
            y = br.ReadSingle();
            z = br.ReadSingle();
        }

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point(int br, int i, int i1)
        {
            x = br;
            y = i;
            z = i1;
        }

        public void SaveAsText(StreamWriter wr)
        {
            wr.WriteLine(string.Format("{0}  {1}  {2}", x, y, z).Replace(",", "."));
        }
        public void Save(BinaryWriter bw)
        {
            bw.Write(x);
            bw.Write(y);
            bw.Write(z);
        }
    }
}
