using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pwApi.StructuresRegion
{
    public class RegionStruct
    {
        public int Version { get; set; }
        public int iNumTrans { get; set; }
        public int iNumRegion { get; set; }
        public int versionkey { get; set; }
        public int TimeStamp { get; set; }
        public List<Region> Regions { get; set; }
        public List<Transport> Transp { get; set; }

    }
}
