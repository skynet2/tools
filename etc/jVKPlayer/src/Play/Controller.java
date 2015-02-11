package Play;

import Structures.Song;
import UI.Notify;
import javazoom.jl.decoder.JavaLayerException;
import javazoom.jl.player.Player;
import org.apache.commons.io.FileUtils;

import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.UnsupportedAudioFileException;
import java.io.*;

import java.net.URL;
import java.util.List;

/**
 * Created by io on 11/14/14.
 */
public class Controller {

    private static Song _current;
    private static Song _next;
    private static Song _previous;
    private static List<Song> _songs;
    private static List<Song> _query;
    private static Player _player;

    public static void set_songs(List<Song> m_songs) {
        _songs = m_songs;
    }
    public static List<Song> get_songs() {
        return _songs;
    }


    public static List<Song> get_query() {
        return _query;
    }

    public static void set_query(List<Song> _query) {
        Controller._query = _query;
    }

    public static Player get_player() {
        return _player;
    }


    public static void play_query() throws IOException, JavaLayerException, UnsupportedAudioFileException, LineUnavailableException, InterruptedException {
        while(true) {
                if(_player != null && _player.isComplete())
                    Thread.sleep(3000);
                if(_query.get(1) != null)
                    _next = _query.get(1);
                Song playing = _query.get(0);
                _query.remove(0);
                play(playing);
        }
    }

    private static void play(Song s) throws IOException, UnsupportedAudioFileException, LineUnavailableException, JavaLayerException, InterruptedException {
        FileUtils.copyURLToFile(new URL(s.getParam("url")), new File("temp.mp3"));
        if (_current != null) {
            _previous = _current;
            System.out.println(String.format("Previous : %s - %s", _previous.getParam("artist"), _previous.getParam("title")));
        }
        _current = s;
        System.out.println(String.format("Now Playing : %s - %s", s.getParam("artist"), s.getParam("title")));
        System.out.println(String.format("Next Playing : %s - %s", _next.getParam("artist"), _next.getParam("title")));
       // if(_previous != null)
          //  Notify.Generate(String.format("Now Playing : %s - %s", s.getParam("artist"), s.getParam("title")),
       //             String.format("Next Playing : %s - %s", _next.getParam("artist"), _next.getParam("title")),
       //             String.format("Previous : %s - %s", _previous.getParam("artist"), _previous.getParam("title")));
        _player = new Player(new FileInputStream("temp.mp3"));
        _player.play();
    }
    public static void status()
    {
        int count = (_player.getPosition()*100/(Integer.parseInt(_current.getParam("duration"))*1000))/10;
        System.out.println("Status : ");
            System.out.println(String.format("Val : %s Dur : %s Pos :%s",count,Integer.parseInt(_current.getParam("duration"))*1000,_player.getPosition()));

        System.out.print("[");
        for(int i = 0; i < 10; i++)
            if(i < count)
                System.out.print("X");
                else System.out.print(" ");
        System.out.println("]");
    }
    public static void play_all() throws IOException, LineUnavailableException, UnsupportedAudioFileException, JavaLayerException, ClassNotFoundException {
        _query = (List<Song>) deepcopy(_songs);
    }

    private static Object deepcopy(Object one) throws IOException, ClassNotFoundException {
        ObjectOutputStream obj = new ObjectOutputStream(new FileOutputStream("temp.obj"));
        obj.writeObject(one);
        ObjectInputStream str = new ObjectInputStream(new FileInputStream("temp.obj"));
        return str.readObject();
    }
}
