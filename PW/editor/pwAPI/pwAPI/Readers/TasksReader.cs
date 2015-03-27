using System;
using System.Collections.Generic;
using System.IO;
//using JQEditor.Classes;

namespace pwApi.Readers
{
    /*
    public class TasksReader
    {
        public ATaskTemplFixedData[] Quests;
        public List<int> AvailableVersions = new List<int>(new int[] { 105, 108, 111, 118, 9999 });
        public int TimeStamp, FileVersion, QuestsCount, ElementsVersion;
        public string FileName, FileInfo, FileNameSave, ElementsFileName, TestStr;
        private BinaryReader br;
        public TasksReader(String taskPath)
        {
            br = new BinaryReader(File.OpenRead(taskPath));
            TimeStamp = br.ReadInt32();
            FileVersion = br.ReadInt32();
            QuestsCount = br.ReadInt32();
            int[] offset = new int[QuestsCount];
            for (int i = 0; i < QuestsCount; i++)
                offset[i] = br.ReadInt32();
            Quests = new ATaskTemplFixedData[QuestsCount];
            for (int i = 0; i < QuestsCount; i++)
            {
                br.BaseStream.Seek(offset[i], SeekOrigin.Begin);
                Quests[i] = new ATaskTemplFixedData(br, FileVersion);
            }
            br.Close();
        }
        public void Save(string newPath)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(newPath));
            bw.Write(-1819966623);
            bw.Write(FileVersion);
            bw.Write(Quests.Length);
            bw.Write(new byte[Quests.Length * 4]);
            int[] offset = new int[Quests.Length];
            for (int i = 0; i < Quests.Length; ++i)
            {
                offset[i] = (int)bw.BaseStream.Position;
                Quests[i].Write(bw, FileVersion);
            }

            bw.BaseStream.Position = 12;
            for (int i = 0; i < offset.Length; ++i)
                bw.Write(offset[i]);
            bw.Close();
        }
    }
     * */
}
