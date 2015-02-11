package Structures;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 13.10.14.
 */
public class ConfigList
{
    public String _name;
    public List<Type> _types = new ArrayList<Type>();

    public ConfigList(String name,String values,String types)
    {
        _name = name;
        String[] vals = values.split(";");
        String[] tty = types.split(";");
        for(int i = 0; i < vals.length;i++)
            _types.add(new Type(vals[i],tty[i]));
    }
    public int getSize()
    {
        int result = 0;
        for(Type t : this._types)
            result += t._size;
        return result;
    }
}
