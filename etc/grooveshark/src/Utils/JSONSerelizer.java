package Utils;

/**
 * Created by mermoldy on 10/7/14.
 */

import Main.Main;
import org.json.JSONException;
import org.json.JSONObject;

public class JSONSerelizer
{
    private JSONObject myjson = null;
    private JSONObject header = null;
    private JSONObject param  = null;

    public JSONSerelizer(String methodname)
    {
        myjson   = new JSONObject();
        header   = new JSONObject();
        param    = new JSONObject();

        try
        {
            myjson.put("method", methodname);
            myjson.put("parameters", param);
            myjson.put("header", header);
            header.put("wsKey" , Main.Key);
            header.put("sessionID", Main.sessinID);
        }
        catch (JSONException e)
        {
            e.printStackTrace();
        }
    }

    public JSONSerelizer addNewParam(String name, String value)
    {
        try
        {
            param.put(name, value);
        }
        catch (JSONException e)
        {
            e.printStackTrace();
        }
        return this;
    }

    public String toString ()
    {
        return myjson.toString();
    }


}
