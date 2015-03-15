import java.io.IOException;

public class Main {

    public static void main(String[] args) throws IOException {
        ElementReader el = new ElementReader("E:\\Dropbox\\ServerDev\\Extended sELedit 1.0\\configs\\PW_1.5.1_v101.cfg","E:\\Dropbox\\PW\\UniPW\\Обновы\\Обнова 20.10\\elements.data");
        el.Save("E:\\Dropbox\\PW\\UniPW\\Обновы\\Обнова 20.10\\elements1.data");
      //  el.
      System.out.println(((double) ((double) (Runtime.getRuntime().totalMemory() / 1024) / 1024)) - ((double) ((double) (Runtime.getRuntime().freeMemory() /1024)/1024)));
    }
}
