package UI;

import Play.Controller;
import Structures.Song;
import Util.Utils;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by io on 14.11.2014.
 */
public class Methods {
    public static void print(List<Song> t)
    {
        int z = 0;
        for(Song s : Controller.get_query())
            System.out.println(String.format("POS : %s Name : %s Author : %s",z++,s.getParam("title"),s.getParam("artist")));
    }
    public static void skip(int count)
    {
        System.out.println(String.format("Skipped %s tracks",count));
        for(int i = 0; i < count; i++)
            Controller.get_query().remove(0);
    }

    public static void search_track(String val,List<Song> ls) throws IOException {
        int z = 0;
        List<Song> result = new ArrayList<>();
        for(Song s : ls) {
            if (s.getParam("title").toLowerCase().contains(val.toLowerCase()) || s.getParam("artist").toLowerCase().contains(val.toLowerCase())) {
                System.out.println("Found match in :");
                System.out.println(String.format("POS %s Title : %s Artist : %s",z,s.getParam("title"),s.getParam("artist")));
                result.add(s);
            }
            z++;
        }
        System.out.println("Add all results to playlist? (yes,no,some)");
        switch (Utils.get_line())
        {
            case "yes" :
                System.out.println(String.format("Added %s to playlist",result.size()));
                Controller.get_query().addAll(0,result);
                break;
            case "no" :
                return;
            case "some" :
                System.out.println("Split with witespace");
                String[] params = Utils.get_line().split(" ");
                for(String par : params) {
                    Song res = ls.get(Integer.parseInt(par));
                    Controller.get_query().add(0, res);
                    System.out.println(String.format("Added : %s - %s",res.getParam("title"),res.getParam("name")));
                }
        }
    }
}
