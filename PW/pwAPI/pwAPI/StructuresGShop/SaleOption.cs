using System.IO;

namespace pwApi.StructuresGShop
{
    public class SaleOption
    {
        int price;
        int expire_date;
        int duration;
        int start_date;
        int control_type;
        int day;
        int status;
        int flags;

        public SaleOption(BinaryReader br)
        {
            price = br.ReadInt32();
            expire_date = br.ReadInt32();
            duration = br.ReadInt32();
            start_date = br.ReadInt32();
            control_type = br.ReadInt32();
            day = br.ReadInt32();
            status = br.ReadInt32();
            flags = br.ReadInt32();
        }
        public SaleOption()
        {
            price = 1;
            expire_date = 0;
            duration = 0;
            start_date = 0;
            control_type = -1;
            day = 0;
            status = 0;
            flags = 0;
        }
        public SaleOption(BinaryWriter bw, int count)
        {
            if (count == 0)
            {
                bw.Write(100);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(-1);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
            }
            else
            {
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
            }
        }

    }
}
