package UI;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by io on 14.11.2014.
 */
public class Notify extends JFrame {
    private static final SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss");
    private final Date now = new Date();

    private final JLabel text = new JLabel();

    public Notify(String text1,String text2,String text3) {
        super("TranslucentWindow");
        setLayout(null);

        setSize(300,200);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
        double width = screenSize.getWidth();
        double height = screenSize.getHeight();
        add(new JLabel(text1)).setBounds(10,10,300,25);
        add(new JLabel(text2)).setBounds(10,35,300,25);
        add(new JLabel(text3)).setBounds(10,55,300,25);
        setBounds((int) (width - 300), (int) (height - 265), 300, 200);
        setAlwaysOnTop(true);
    }

    public static void Generate(final String text_1, final String text_2, final String text_3) {
        // Determine if the GraphicsDevice supports translucency.
        GraphicsEnvironment ge =
                GraphicsEnvironment.getLocalGraphicsEnvironment();
        GraphicsDevice gd = ge.getDefaultScreenDevice();
        JFrame.setDefaultLookAndFeelDecorated(true);
        SwingUtilities.invokeLater(new Runnable() {
            @Override
            public void run() {
                Notify tw = new Notify(text_1,text_2,text_3);

                // Set the window to 55% opaque (45% translucent).
                tw.setOpacity(0.45f);

                // Display the window.
                tw.setVisible(true);
            }
        });
    }

}
