using System;
using System.Collections.Generic;
using System.IO;

namespace pwApi.StructuresElement
{
    public class ConfigList
    {
        public string Name;
        public Type[] Types;
        public ConfigList(String name, String values, String types)
        {
            Name = name;
            var vals = values.Split(';');
            var tty = types.Split(';');
            Types = new Type[vals.Length];

            for (int i = 0; i < vals.Length; i++)
                Types[i] = new Type(vals[i], tty[i]);
        }
        public static List<ConfigList> ParseList(String path)
        {
            List<ConfigList> lists = new List<ConfigList>();
            var args = File.ReadAllLines(path);
            for (int i = 2; i <= args.Length - 2; i += 5)
            {
                lists.Add(new ConfigList(args[i + 1], args[i + 3], args[i + 4]));
            }
            return lists;
        }
    }
}
