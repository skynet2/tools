using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pwApi.StructuresRegion
{
    public class Region
    {
        public int id { get; set; }
        public List<Point> Points { get; set; }

        public Region(BinaryReader br,int id)
        {
            this.id = id;
            Points = new List<Point>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Points.Add(new Point(br));
            }
        }

        public Region()
        {
            id = new Random().Next(0,1000);
            Points = new List<Point>();
            Points.Add(new Point());
        }

        public void SaveAsText(StreamWriter wr)
        {
            wr.WriteLine("[region]");
            wr.WriteLine("\"SkyDev няшка\"");
            wr.WriteLine("{0}  {1}",1,(Points.Count));
            foreach (var point in Points)
            {
                point.SaveAsText(wr);
            }
        }
        public void Save(BinaryWriter bw)
        {
            bw.Write(Points.Count);
            foreach (var p in Points)
            {
                p.Save(bw);
            }
        }
    }

}
