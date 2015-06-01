using System;

namespace pwApi.StructuresElement
{
    public class Type
    {
        public String get_name()
        {
            return Name;
        }

        public String Name;

        public String get_type()
        {
            return _type;
        }

        public String _type;


        public int Size;

        public Type(String name, String type)
        {
            Name = name;
            _type = type;
            if (!_type.Contains(":"))
            {
                switch (_type)
                {
                    case "int32":
                        break;
                    case "float":
                        break;
                    default:
                        Console.WriteLine(_type);
                        break;
                }
            }
            else
            {
                var val = _type.Split(':');
                _type = val[0] + ":";
                try
                {
                    Size = int.Parse(val[1]);
                }
                catch (Exception e)
                {
                    Size = 0;
                }
            }
        }
    }
}
