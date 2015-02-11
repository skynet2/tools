package Utils;

import Core.UI;
import Structures.Config;
import Structures.ConfigList;

import javax.swing.text.*;
import java.awt.*;
import java.io.FileInputStream;
import java.io.IOException;

/**
 * Created by io on 13.10.14.
 */
public class Tester {
    private BinaryReader br;
    private Config _cfg;

    public Tester(Config cfg,String path) throws IOException, BadLocationException {
        _cfg = cfg;
        br = new BinaryReader(new FileInputStream(path));
        br.ReadBytes(8);
        for (int i = 0; i < _cfg.getCount(); i++) {
            switch (i) {
                case 20:
                    offset20();
                    ConfigList temp = _cfg.lists.get(i);
                    list(temp.getSize(),temp._name,i);
                    break;
                case 58:
                    list59();
                    break;
                case 100:
                    offset100();
                    temp = _cfg.lists.get(i);
                    list(temp.getSize(),temp._name,i);
                    break;
                default:
                    temp = _cfg.lists.get(i);
                    list(temp.getSize(),temp._name,i);
                    break;
            }
        }
    }

    private void offset20() throws IOException {
        br.ReadInt32();
        int count = br.ReadInt32();
        br.ReadBytes(count + 4);
    }
    private void offset100() throws IOException {
        br.ReadInt32();
        int count = br.ReadInt32();
        br.ReadBytes(count);
    }

    private void list(int size,String name,int i) throws IOException, BadLocationException {
        int c1 = br.ReadInt32();
        if(c1 <= 0 || c1 > 5000 )
            add_to_log(String.format("List : %s Size : %s Count : %s\n",name,size,c1), Color.RED);
        else
            add_to_log(String.format("List : %s Size : %s Count : %s\n", name, size, c1), Color.DARK_GRAY);
        System.out.println(String.format("List : %s Size : %s Count : %s", name, size, c1));
        br.ReadBytes(c1 * size);
    }
    private void add_to_log(String textm,Color col) throws BadLocationException {
        StyledDocument doc = UI.LOG.getStyledDocument();
        StyleContext context = new StyleContext();
        Style style = context.addStyle("test", null);
        StyleConstants.setForeground(style, col);
        doc.insertString(doc.getLength(), textm, style);
    }
    private void list59() throws IOException {
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            br.ReadBytes(132);
            int count2 = br.ReadInt32();
            for (int i2 = 0; i2 < count2; i2++)
            {
                br.ReadBytes(8);
                int l = br.ReadInt32();
                br.ReadBytes(l*2);
                int count3 = br.ReadInt32();
                br.ReadBytes(count3*136);
            }
        }
    }
}
