package Structure;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.List;

/**
 * Created by io on 07.10.14.
 */
public class Data
{
    public LinkedHashMap<String,String> values = new LinkedHashMap<String,String>();

    public Data(String text) throws JSONException {
        JSONObject obj = new JSONObject(text);
        Iterator<String> itr = obj.keys();
        while (itr.hasNext())
        {
            String temp = itr.next();
            values.put(temp,obj.getString(temp));
        }
    }
}
