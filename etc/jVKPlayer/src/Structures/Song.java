package Structures;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.Map;

public class Song implements Serializable
{
    public Map<String,String> values;
    public Song(JSONObject obj) throws JSONException {
            Iterator<String> iter = obj.keys();
        values = new LinkedHashMap<>();
            while(iter.hasNext()) {
                String key = iter.next();
                if(!key.equals("url"))
                values.put(key,obj.getString(key));
                else
                    values.put(key,obj.get(key).toString()
                            .substring(0,obj.get(key).toString().indexOf("?")));
            }
    }
    public String getParam(String key)
    {
        return values.get(key);
    }

    }
