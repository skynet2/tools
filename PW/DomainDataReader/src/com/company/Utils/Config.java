package com.company.Utils;

import com.company.UI;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 13.10.14.
 */
public class Config
{
    static File f;
    public static String[] read() throws IOException, JSONException {
        f = new File("conf.json");
        if(f.exists()) {
            JSONObject obj = new JSONObject(Files.readAllLines(f.toPath(), Charset.defaultCharset()).get(0));
            if (obj.length() == 2)
                return new String[]{obj.getString("folder"), obj.getString("type")};
        }
        else {
            write(System.getProperty("user.dir") + File.separator + "domain2.data", ".data");
        }
            return read();
    }
    public static void write(String path,String type) throws JSONException, IOException {
        List<String> val = new ArrayList<String>();
        JSONObject obj = new JSONObject();
        val.add(obj.put("folder", path).put("type",type).toString());
        Files.write(f.toPath(), val, Charset.defaultCharset());
    }
}
