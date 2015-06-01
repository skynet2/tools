using System.Collections.Generic;
using System.IO;

namespace pwApi.StructuresElement
{
    public class ConfigList
    {
        public string Name;
        public string Skip;
        public Type[] Types;

        public ConfigList(string name, string skip, string values, string types)
        {
            Name = name;
            Skip = skip;
            var vals = values.Split(';');
            var tty = types.Split(';');
            Types = new Type[vals.Length];
            for(var i = 0; i < vals.Length; i++)
                Types[i] = new Type(vals[i], tty[i]);
        }
    }

    public class ConfigLists
    {
        public byte NpcTalk;
        public List<ConfigList> Lists;

        public void ParseList(string path)
        {
            Lists = new List<ConfigList>();
            var args = File.ReadAllLines(path);
            var count = int.Parse(args[0]);
            NpcTalk = byte.Parse(args[1]);
            var line = 2;
            for(var i = 0; i < count; i++)
            {
                while(args[line] == "")
                    line++;
                Lists.Add(new ConfigList(args[line], args[line + 1], args[line + 2], args[line + 3]));
                line += 4;
            }
        }
    }
}
