package Structures;

import Core.Main;

public class Type
{
    public String _name,_type;
    public int _size;

    public Type(String name, String type)
    {
        _name = name;
        _type = type;
        if(!_type.contains(":")) {
            Main.All_Types.add(_type);
            switch (_type) {
                case "int32":
                    _size = 4;
                    break;
                case "float":
                    _size = 4;
                    break;
                default:
                    System.err.println(_type);
                    break;
            }
        }
        else
        {
            String[] val = _type.split(":");
            _type = val[0]+":";
            try {
                _size = Integer.parseInt(val[1]);
            }
            catch(Exception e)
            {
                _size = 0;
            }
            Main.All_Types.add(_type);
         //   System.out.println(String.format("Structures.Type is %s and Size is %s",_type,_size));
        }
    }
}
