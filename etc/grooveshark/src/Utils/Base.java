package Utils;

import Main.Main;
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.PostMethod;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

/**ss
 * Created by io on 06.10.14.
 */
public class Base {
    public static String SendPost(JSONSerelizer JSON,boolean debug) throws IOException {
        HttpClient client = new HttpClient();
        PostMethod base = new PostMethod("https://api.grooveshark.com/ws3.php?sig="+ Cryptography.HMACMD5(JSON.toString(), Main.Secret));
        base.setRequestBody(JSON.toString());

        client.executeMethod(base);
        String resp = base.getResponseBodyAsString();
        if(debug) System.err.println(resp);
        return resp;
    }
    public static String getSession() throws IOException, JSONException {
        JSONSerelizer json = new JSONSerelizer("startSession");
        return new JSONObject(Base.SendPost(json,false)).getJSONObject("result").getString("sessionID");
    }
    public static boolean getSucess(JSONObject obj) throws JSONException {
        return obj.getJSONObject("result").getString("success") == "true" ? true : false;
    }
}
