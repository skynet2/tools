package Structures;

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
    public int count, xz;
    public static List<ConfigList> lists = new ArrayList<ConfigList>();
    public int getCount()
    {
        return count;
    }
    public Config(List<String> args)
    {
        count = Integer.parseInt(args.get(0)); xz = Integer.parseInt(args.get(1));
        for(int i = 2; i<=args.size()-2; i+=5)
        {
            lists.add(new ConfigList(args.get(i+1),args.get(i+3),args.get(i+4)));
        }
        System.out.println("Readed");
    }
    public Config(Config local,String path) throws IOException {
        List<String> result = new ArrayList<String>();
        result.add(String.valueOf(local.count));
        result.add(String.valueOf(local.xz));
        for(ConfigList ls : local.lists)
        {
            result.add("");
            result.add(ls._name);
            result.add("0");
            String[] vals = new String[] { "",""};
            for (Type tty : ls._types)
            {
                vals[0] += tty._name+";";
                System.err.println(vals[1].toString());
                if(tty._type.contains(":"))
                    vals[1] += tty._type+tty._size+";";
                else
                    vals[1] += tty._type + ";";
            }
            result.add(vals[0].substring(0,vals[0].length()-1));
            result.add(vals[1].substring(0,vals[1].length()-1));
            System.out.println("WTF");
        }
        Files.write(new File(path).toPath(), result, Charset.defaultCharset());
    }
}
