package Util;

import Play.Controller;
import Structures.Song;
import javazoom.jl.decoder.JavaLayerException;
import org.apache.commons.io.IOUtils;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;

import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.UnsupportedAudioFileException;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 14.11.2014.
 */
public class Utils
{
    public static final String ACCESS_TOKEN = "6587714af6a08ec107a477c2aa49015620ebe392f5d240dc9babdca0656b9e5e637e6163212d9214835b4";

    public static String get_line() throws IOException {
        return new BufferedReader(new InputStreamReader(System.in)).readLine();
    }
    public static void add_playlist(String User) throws JSONException, IOException, URISyntaxException, ClassNotFoundException, UnsupportedAudioFileException, LineUnavailableException, JavaLayerException {
        Controller.get_songs().addAll(getList(User));
        System.out.println("Refresh query? (yes\\no)");
        if(Utils.get_line().equals("yes"))
            Controller.play_all();
    }

    public static String WebExecute(String method, List<NameValuePair> values) throws URISyntaxException, IOException {
        URIBuilder builder = new URIBuilder();
        builder.setScheme("https").setHost("api.vk.com").setPath(method)
                .setParameters(values);
        URI uri = builder.build();
        HttpGet httpget = new HttpGet(uri);
        HttpClient httpclient = new DefaultHttpClient();
        HttpResponse response = httpclient.execute(httpget);
        HttpEntity entity = response.getEntity();
        if (entity != null) {
            InputStream instream = null;
            try {
                instream = entity.getContent();
                return IOUtils.toString(instream);
            } finally {
                if (instream != null)
                    instream.close();
            }

        }
        return null;
    }

    public static List<Song> getList(String user) throws IOException, URISyntaxException, JSONException {
        List<NameValuePair> pair = new ArrayList<>();
        getUser(user);
        pair.add(new BasicNameValuePair("oid",user));
        pair.add(new BasicNameValuePair("need_user","0"));
        pair.add(new BasicNameValuePair("access_token", ACCESS_TOKEN));
        return Parsers.songsList(WebExecute("/method/audio.get", pair));
    }
    public static void getUser(String id) throws IOException, URISyntaxException, JSONException {
        List<NameValuePair> pair = new ArrayList<>();
        pair.add(new BasicNameValuePair("uids",id));
        Parsers.UserInfo(WebExecute("/method/users.get",pair));
    }


}
