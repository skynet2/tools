import java.io.IOException;

public class Main {

    public static void main(String[] args) throws IOException {
        ElementReader el = new ElementReader("E:\\Dropbox\\ServerDev\\Extended sELedit 1.0\\configs\\PW_1.5.1_v101.cfg",
                "D:\\update8\\element\\data\\elements.data");
        el.Save("D:\\update8\\element\\data\\el3.data");
        System.out.println("Hello World!");
    }
}
