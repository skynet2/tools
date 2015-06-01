using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pwApi.StructuresRegion;

namespace pwApi.Readers
{
    public class RegionReader
    {
        public RegionStruct Region { get; set; }
        public RegionReader(string path)
        {
            try
            {
                Region = new RegionStruct
                {
                    Regions = new List<Region>(),
                    Transp = new List<Transport>()
                };
                using (var br = new BinaryReader(File.OpenRead(path)))
                {
                    Region.Version = br.ReadInt32();
                    Region.iNumRegion = br.ReadInt32();
                    if (Region.Version > 1)
                        Region.iNumTrans = br.ReadInt32();
                    if (Region.Version > 3)
                        Region.TimeStamp = br.ReadInt32();

                    for (int i = 0; i < Region.iNumTrans + Region.iNumRegion; i++)
                    {
                        int type = br.ReadInt32();
                        if (type == 0)
                            Region.Regions.Add(new Region(br, i));
                        else Region.Transp.Add(new Transport(br, Region.Version,i));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void SaveAsText(string path)
        {
            using (var wr = new StreamWriter(path, false, Encoding.GetEncoding(1200)))
            {
                wr.WriteLine("version  5");
                wr.WriteLine("1312962951");
                wr.WriteLine();
                foreach (var region in Region.Regions)
                {
                    region.SaveAsText(wr);
                    wr.WriteLine();
                }
                foreach (var trans in Region.Transp)
                {
                    trans.SaveAsText(wr);
                    wr.WriteLine();
                }
            }
            
        }
        public void Save(string path)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(path)))
            {
                bw.Write(Region.Version);
                bw.Write(Region.Regions.Count);
                if(Region.Version > 1)
                    bw.Write(Region.Transp.Count);
                if(Region.Version > 3)
                    bw.Write(Region.TimeStamp);
                foreach (var reg in Region.Regions)
                {
                    bw.Write(0);
                    reg.Save(bw);
                }
                foreach (var tra in Region.Transp)
                {
                    bw.Write(1);
                    tra.Save(bw,Region.Version);
                }
            }
        }
    }
}
