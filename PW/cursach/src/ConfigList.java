import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 23.02.2015.
 */
public class ConfigList {
    public String Name;
    public Type[] Types;
    public ConfigList (String name,String values,String types)
    {
        Name = name;
        String[] vals = values.split(";");
        String[] tty = types.split(";");
        Types = new Type[vals.length];

        for(int i = 0; i < vals.length;i++)
            Types[i] = new Type(vals[i],tty[i]);
    }
    public static List<ConfigList> ParseList(String path) throws IOException {
        List<ConfigList> lists = new ArrayList<ConfigList>();
        List<String> args = Files.readAllLines(Paths.get(path), Charset.defaultCharset());
        for(int i = 2; i<=args.size()-2; i+=5)
        {
            lists.add(new ConfigList(args.get(i+1),args.get(i+3),args.get(i+4)));
        }
        return lists;
    }
}
