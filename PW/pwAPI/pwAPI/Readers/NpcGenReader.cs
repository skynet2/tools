namespace pwApi.Readers
{
    public class NpcGenReader
    {
        // TODO
        /*public List<AREA> m_aAreas;
        public List<RESAREA> m_aResAreas;
        public List<DYNOBJ> m_aDynObjs;
        public List<CONTROLLER> m_aControllers;
        public List<FLYAREA> m_aFlyAreas;

        uint version;
        int iNumAIGen;
        int iNumResArea;
        int iNumDynObj;
        int iNumNPCCtrl;
        int iNumFlyArea;
        int iNumMineralCube;
        int iNumMineralSphere;
        int export_time;
        short export_vss_name;
        short export_computer_name;
        byte[] unk;

        public NpcGenReader (string filePath)
        {
            BinaryReader br = new BinaryReader (File.OpenRead(gShopPath));
            version = br.ReadUInt32();
            iNumAIGen = br.ReadInt32();
            iNumResArea = br.ReadInt32();

            if (version >= 6)
                iNumDynObj = br.ReadInt32();
            if (version >= 7)
                iNumNPCCtrl = br.ReadInt32();
            if (version >= 16)
                iNumFlyArea = br.ReadInt32();
            if (version == 16) {
                iNumMineralCube = br.ReadInt32();
                iNumMineralSphere = br.ReadInt32();
            }
            if (version > 20 && version < 23) {
                export_time = br.ReadInt32();
                export_vss_name = br.ReadInt16();
                export_computer_name = br.ReadInt16();
            }
            if (version == 23)
                unk = br.ReadBytes(272);

            m_aAreas = new List<AREA>();
            for (int i = 0; i < iNumAIGen; i++)
                m_aAreas.Add(new AREA(br, version));

            m_aResAreas = new List<RESAREA>();
            for (int i = 0; i < iNumResArea; i++) {
                m_aResAreas.Add(new RESAREA(br, version));
            }

            m_aDynObjs = new List<DYNOBJ>();
            for (int i = 0; i < iNumDynObj; i++) {
                m_aDynObjs.Add(new DYNOBJ(br, version));
            }
            
            m_aControllers = new List<CONTROLLER>();
            for (int i = 0; i < iNumNPCCtrl; i++)
                m_aControllers.Add(new CONTROLLER(br, version));

            m_aFlyAreas = new List<FLYAREA>();
            for (int i = 0; i < iNumFlyArea; i++)
                m_aFlyAreas.Add(new FLYAREA(br));
        }
    }
    */
    }
}