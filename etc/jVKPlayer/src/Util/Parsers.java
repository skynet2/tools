package Util;

import Structures.Song;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 11/13/14.
 */
public class Parsers
{
    public static List<Song> songsList(String resp) throws JSONException {
        List<Song> result = new ArrayList<>();
        JSONObject base = new JSONObject(resp);
        JSONArray array = base.getJSONArray("response");
        for(int i = 0; i < array.length(); i++)
            result.add(new Song((JSONObject) array.get(i)));
        return result;
    }
    public static void UserInfo(String resp) throws JSONException {
        JSONObject base = new JSONObject(resp);
        JSONArray array = base.getJSONArray("response");
        JSONObject obj =  (JSONObject)array.get(0);
        System.out.println(String.format("Name : %s %s ID : %s ",obj.get("first_name"),obj.get("last_name"),obj.get("uid")));
    }
}
