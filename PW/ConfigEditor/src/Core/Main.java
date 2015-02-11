package Core;

import org.json.JSONException;

import javax.swing.text.BadLocationException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

/**
 * Created by io on 13.10.14.
 */
public class Main {
    public static HashSet<String> All_Types = new HashSet<>();
    public static void main(String[] args) throws IOException, BadLocationException, JSONException {
        new UI();
    }
}

