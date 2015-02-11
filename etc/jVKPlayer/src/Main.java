
import Play.Controller;
import Structures.Song;
import UI.Methods;
import Util.Utils;
import javazoom.jl.decoder.JavaLayerException;
import org.json.JSONException;


import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.UnsupportedAudioFileException;
import java.io.*;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Collections;

public class Main {
    private static Thread _ts;
    public static void main(String[] args) throws IOException, URISyntaxException, JSONException, InterruptedException, UnsupportedAudioFileException, ClassNotFoundException, LineUnavailableException, JavaLayerException {
        Controller.set_songs(Utils.getList("88112285"));
        System.out.println("hello");
        while (true){
            print_menu();
    }
    }

    private static void print_menu() throws IOException, LineUnavailableException, UnsupportedAudioFileException, JavaLayerException, InterruptedException, ClassNotFoundException, URISyntaxException, JSONException {
        if(Controller.get_query() == null)
            Controller.set_query(new ArrayList<Song>());
        while (true) {
            String res = Utils.get_line();
            if(res.contains("play "))
            {
                Controller.get_query().add(0, Controller.get_songs().get(Integer.parseInt(res.split(" ")[1])));
                Controller.get_player().close();
            }
            else if(res.contains("add_pl ")) {
                Utils.add_playlist(res.split(" ")[1]);
            }
                else if(res.contains("set_user ")) {
                Controller.set_songs(Utils.getList(res.split(" ")[1]));
                Controller.play_all();
            }
            else switch (res) {
                case "find_s" :
                    System.out.println("Artist or Track");
                        Methods.search_track(Utils.get_line(),Controller.get_songs());
                break;
                case "skip" :
                    System.out.println("Tracks to skip : ");
                    Methods.skip(Integer.parseInt(Utils.get_line()));
                    break;
                case "5":
                    Methods.print(Controller.get_query());
                    break;
                case "status" :
                    Controller.status();
                    break;
                case "next" :
                    Controller.get_player().close();
                    break;
                case "ls":
                    Methods.print(Controller.get_songs());
                    break;
                case "ls_q" :
                    Methods.print(Controller.get_query());
                    break;
                case "add":
                    Controller.play_all();
                    break;
                case "shuffle" :
                    Collections.shuffle(Controller.get_query());
                    break;
                case "find_q" :
                    System.out.println("Artist or Track");
                    Methods.search_track(Utils.get_line(),Controller.get_query());
                    break;
                case "help" :
                    System.out.println("===================");
                    System.out.println("ls - list songs");
                    System.out.println("ls_q - list queue");
                    System.out.println("add - add all songs to queue");
                    System.out.println("add_pl [USERID] - add user playlist");
                    System.out.println("start - start player");
                    System.out.println("play [POS] - move song to start");
                    System.out.println("shuffle - shuffle list");
                    System.out.println("find_q - search in queue");
                    System.out.println("find_s - search in songs");
                    System.out.println("set_user [ID] - set new VK user");
                    System.out.println("Commands : next,prev,status,find");
                    break;
                case "start":
                    if(_ts == null) {
                        _ts = new Thread(new Runnable() {
                            @Override
                            public void run() {
                                try {
                                    Controller.play_query();
                                } catch (FileNotFoundException e) {
                                    e.printStackTrace();
                                } catch (JavaLayerException e) {
                                    e.printStackTrace();
                                } catch (UnsupportedAudioFileException e) {
                                    e.printStackTrace();
                                } catch (LineUnavailableException e) {
                                    e.printStackTrace();
                                } catch (IOException e) {
                                    e.printStackTrace();
                                } catch (InterruptedException e) {
                                    e.printStackTrace();
                                }
                            }
                        });
                        _ts.start();
                    }
                    break;
                default:
                    System.err.println("Command not found ( Help -h )");
            }

        }
    }

}
