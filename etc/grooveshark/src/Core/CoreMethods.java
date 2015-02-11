package Core;

import Main.Main;
import Structure.Data;
import Utils.Base;
import Utils.Cryptography;
import Utils.JSONSerelizer;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import static Utils.Base.getSucess;

/**
 * Created by io on 07.10.14.
 */
public class CoreMethods
{
    public static JSONObject getCountry() throws IOException, JSONException {
        return new JSONObject(Utils.Base.SendPost(new JSONSerelizer("getCountry"),true));
    }
    public static boolean logout() throws IOException, JSONException {
        JSONObject result = new JSONObject(Utils.Base.SendPost(new JSONSerelizer("logout").addNewParam("sessionID", Main.sessinID),true));
        return getSucess(result);
    }
    public static boolean authenticate(String login,String password) throws IOException, JSONException {
        JSONObject result = new JSONObject(Utils.Base.SendPost(new JSONSerelizer("authenticate").addNewParam("login",login).addNewParam("password", Cryptography.MD5(password)),true));
        Main.UserInfo = new Data(result.getJSONObject("result").toString());
        return Main.UserInfo.values.get("success")== "true" ? true : false;
    }
    public static boolean authenticateToken(String token) throws IOException, JSONException {
        JSONObject result = new JSONObject(Utils.Base.SendPost(new JSONSerelizer("logout").addNewParam("token",token),true));
        return getSucess(result);
    }
    public static void main(String[] args) throws IOException, JSONException {
        Main.sessinID  = Base.getSession();
        getCountry();
    }
}
