package Utils;

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
public class Cfg {
    static File f;

    public static String[] read() throws IOException, JSONException {
        f = new File("config.json");
        if(f.exists()) {
            JSONObject obj = new JSONObject(Files.readAllLines(f.toPath(), Charset.defaultCharset()).get(0));
            if (obj.length() == 3)
                return new String[]{obj.getString("txt_conf"), obj.getString("txt_element"),obj.getString("txt_save")};
        }
        else {
            write("Path to config.cfg","Path to element","Path for new config");
        }
        return read();
    }
    public static void write(String txt_conf,String txt_element,String txt_save) throws JSONException, IOException {
        List<String> val = new ArrayList<String>();
        JSONObject obj = new JSONObject();
        val.add(obj.put("txt_conf", txt_conf).put("txt_element",txt_element).put("txt_save",txt_save).toString());
        Files.write(f.toPath(), val, Charset.defaultCharset());
    }

}
