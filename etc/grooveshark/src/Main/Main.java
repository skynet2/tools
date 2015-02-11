package Main;

import Core.CoreMethods;
import Structure.Data;
import Utils.Base;
import Utils.JSONSerelizer;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;

public class Main {
    public static String Key = "plists_vksync";
    public static String Secret = "7090d181494057e1947ef8bff3408ebf";
    public static String sessinID = null;
    public static Data UserInfo;
    public static JSONObject Country;

    public static void main(String[] args) throws IOException, InvalidKeyException, NoSuchAlgorithmException, JSONException {
        sessinID = Base.getSession();
        Country = CoreMethods.getCountry();
        CoreMethods.authenticate("lol","lol2"); // => UserInfo // TODO
        /*
        JSONSerelizer auth = new JSONSerelizer("authenticate");
        auth.addNewParam("login",  "mermoldy");
        auth.addNewParam("password", "b8ea4c759eead50f8ed239379f8850d8");

        String resauth = Utils.Base.SendPost(auth,true);
        System.err.println(auth);
        System.err.println(resauth);

        JSONSerelizer jj = new JSONSerelizer("getCountry");
        jj.addNewParam("ip",  "77.47.190.232");
        jj.addNewParam("UserID",  "25521757");

        String res = Utils.Base.SendPost(jj,true);

        JSONSerelizer jj2 = new JSONSerelizer("getStreamKeyStreamServer");
        jj2.addNewParam("songID",     "321");
        jj2.addNewParam("country",  new JSONObject(res).getString("result"));
        jj2.addNewParam("lowBitrate", "1");

        System.err.println(jj);

        String res2 = Utils.Base.SendPost(jj2,true);
        System.out.println(res);

        System.out.println(res2);
*/
    }
}
