/**
 * Created by stas on 11.03.2015.
 */
public class Type
{
    public String get_name() {
        return Name;
    }

    public String Name;

    public String get_type() {
        return _type;
    }

    public String _type;


    public int Size;

    public Type(String name, String type)
    {
        Name = name;
        _type = type;
        if(!_type.contains(":")) {
            switch (_type) {
                case "int32":
                    break;
                case "float":
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
                Size = Integer.parseInt(val[1]);
            }
            catch(Exception e)
            {
                Size = 0;
            }
        }
    }
}