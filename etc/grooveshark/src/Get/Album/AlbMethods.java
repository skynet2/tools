package Get.Album;

import Structure.Data;
import Utils.JSONSerelizer;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 07.10.14.
 */
public class AlbMethods
{
    public static List<Data> getAlbumSongs(int id, int limit) throws IOException, JSONException {
        JSONSerelizer j = new JSONSerelizer("getAlbumSongs").addNewParam("albumID",String.valueOf(id)).addNewParam("limit",String.valueOf(limit));
        JSONArray arr = new JSONObject(Utils.Base.SendPost(j,true)).getJSONObject("result").getJSONArray("songs");
        List<Data> songs = new ArrayList<Data>();
        for(int i = 0; i < arr.length(); i++) {
            songs.add(new Data(arr.getString(i)));
        }
        return songs;
    }
    public static List<Data> getAlbumSearchResults(String q,int limit) throws IOException, JSONException {
        JSONSerelizer j = new JSONSerelizer("getAlbumSearchResults").addNewParam("query",q).addNewParam("limit",String.valueOf(limit));
        JSONArray arr = new JSONObject(Utils.Base.SendPost(j,true)).getJSONObject("result").getJSONArray("albums");
        List<Data> albums = new ArrayList<Data>();
        for(int i = 0; i < arr.length(); i++) {
            albums.add(new Data(arr.getString(i)));
        }
        return albums;
    }
    public static List<Data> getAlbumsInfo(int id) throws IOException, JSONException {
        JSONSerelizer j = new JSONSerelizer("getAlbumsInfo").addNewParam("albumIDs", String.valueOf(id));
        JSONArray arr = new JSONObject(Utils.Base.SendPost(j,true)).getJSONObject("result").getJSONArray("albums");
        List<Data>albumInfo = new ArrayList<Data>();
        for(int i = 0; i < arr.length(); i++) {
            albumInfo.add(new Data(arr.getString(i)));
        }
        return albumInfo;
    }

    public static void main(String[] args) throws IOException, JSONException {
        getAlbumsInfo(321);
    }
}
