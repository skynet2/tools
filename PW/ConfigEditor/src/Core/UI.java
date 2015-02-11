package Core;

import Structures.Config;
import Structures.ConfigList;
import Structures.Type;
import Utils.Cfg;
import Utils.Tester;
import org.json.JSONException;

import javax.swing.*;
import javax.swing.event.TableModelEvent;
import javax.swing.event.TableModelListener;
import javax.swing.table.DefaultTableModel;
import javax.swing.text.BadLocationException;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;

/**
 * Created by io on 13.10.14.
 */
public class UI
{
    private JComboBox<String> lists = new JComboBox<String>();
    private JFrame frame;
    private Config _conf;
    private JTable _table;
    private JButton add,remove;
    private DefaultTableModel model;
    private int _selected_list;
    private JTextArea _txt_config,_txt_element,_txt_save_path;
    private JButton _select_conf,_select_element,_select_save,_loadConfig,_loadElement,_save;
    public static JTextPane LOG;
    public UI() throws IOException, BadLocationException, JSONException {
        model = new DefaultTableModel(new Object[] { "","","" }, 0);
        frame = new JFrame();
        add = new JButton("ADD");
        remove = new JButton("Remove");
        _select_conf = new JButton("...");
        _select_element = new JButton("...");
        _loadConfig = new JButton("Load cfg");
        _loadElement = new JButton("Load Elem");
        _save = new JButton("Save");
        _select_save = new JButton("...");
        LOG = new JTextPane();
        String[] paths = Cfg.read();
        _txt_config = new JTextArea(paths[0]);
        _txt_element = new JTextArea(paths[1]);
        _txt_save_path = new JTextArea(paths[2]);
        _table = new JTable(model);
        frame.setLayout(null);
        frame.setBounds(0, 0, 970, 600);
        frame.setVisible(true);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.add(lists).setBounds(0, 28, 300, 25);
        frame.add(new JScrollPane(_table)).setBounds(0, 60, 500, 450);
        frame.add(add).setBounds(0,515,250,30);
        frame.add(remove).setBounds(255,515,245,30);
        frame.add(_txt_config).setBounds(0,2,420,20);
        frame.add(_select_conf).setBounds(425,2,75,20);
        frame.add(_txt_element).setBounds(505,2,455,20);
        frame.add(_loadConfig).setBounds(305,28,195,25);
        frame.add(_select_element).setBounds(505,28,100,25);
        frame.add(_loadElement).setBounds(610,28,350,25);
        frame.add(_txt_save_path).setBounds(0,550,650,20);
        frame.add(_select_save).setBounds(655,547,95,20);
        frame.add(_save).setBounds(755,547,210,20);
        frame.add(new JScrollPane(LOG)).setBounds(505,60,460,450);
        // Listeners
        _save.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                try {
                    Config cfg = new Config(_conf,_txt_save_path.getText());
                    Cfg.write(_txt_config.getText(),_txt_element.getText(),_txt_save_path.getText());
                    JOptionPane.showMessageDialog(frame,"Saved to " + _txt_save_path.getText());
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        });
        _select_save.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                JFileChooser choose = new JFileChooser();
                choose.showOpenDialog(frame);
                _txt_save_path.setText(choose.getSelectedFile().toString());
            }
        });
        _loadConfig.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                try {
                    _conf = new Config(Files.readAllLines(new File(_txt_config.getText()).toPath(), Charset.defaultCharset()));
                    Cfg.write(_txt_config.getText(),_txt_element.getText(),_txt_save_path.getText());
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                JCombo_add();
                add_to_table(0);
            }
        });
        _select_conf.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                JFileChooser choose = new JFileChooser();
                choose.showOpenDialog(frame);
                _txt_config.setText(choose.getSelectedFile().toString());
            }
        });
        _select_element.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                JFileChooser choose = new JFileChooser();
                choose.showOpenDialog(frame);
                _txt_element.setText(choose.getSelectedFile().toString());
            }
        });
        _loadElement.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                try {
                    LOG.setText("");
                    new Tester(_conf,_txt_element.getText());
                    Cfg.write(_txt_config.getText(),_txt_element.getText(),_txt_save_path.getText());
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (BadLocationException e) {
                    e.printStackTrace();
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        });
        remove.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                int index = _table.getSelectedRow();
                _conf.lists.get(_selected_list)._types.remove(index);
                model.removeRow(_table.getSelectedRow());
                _table.repaint();
            }
        });
        add.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                _conf.lists.get(_selected_list)._types.add(new Type("NULL","int32"));
                model.addRow(new Object[] {"NULL","int32","4" });
            }
        });
        lists.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                _selected_list = lists.getSelectedIndex();
                add_to_table(_selected_list);
            }
        });
        _table.getModel().addTableModelListener(new TableModelListener() {
            @Override
            public void tableChanged(TableModelEvent e) {
                {
                    if(e.getType() == e.UPDATE) {
                        switch (e.getColumn())
                        {
                            case 0 :
                                System.err.println(String.format("[Edited] Name %s => %s",_conf.lists.get(_selected_list)._types.get(e.getFirstRow())._name,
                                        model.getValueAt(e.getFirstRow(), e.getColumn())));
                                _conf.lists.get(_selected_list)._types.get(e.getFirstRow())._name = String.valueOf(model.getValueAt(e.getFirstRow(), e.getColumn()));
                                break;
                            case 1 :
                                System.err.println(String.format("[Edited] Structures.Type %s => %s",_conf.lists.get(_selected_list)._types.get(e.getFirstRow())._type,
                                        model.getValueAt(e.getFirstRow(),e.getColumn())));
                                _conf.lists.get(_selected_list)._types.get(e.getFirstRow())._type = String.valueOf(model.getValueAt(e.getFirstRow(),e.getColumn()));
                                break;
                            case 2 :
                                System.err.println(String.format("[Edited] Size %s => %s",_conf.lists.get(_selected_list)._types.get(e.getFirstRow())._size,
                                        model.getValueAt(e.getFirstRow(),e.getColumn())));
                                _conf.lists.get(_selected_list)._types.get(e.getFirstRow())._size = Integer.parseInt(String.valueOf(model.getValueAt(e.getFirstRow(),e.getColumn())));
                                break;
                        }
                    }

                }
            }
        });
    }
    private void JCombo_add()
    {
        for(ConfigList ls : _conf.lists)
            lists.addItem(ls._name);
    }
    private void add_to_table(int list)
    {
        _table.setRowHeight(20);
        model.setRowCount(0); _table.setModel(model);
            for(Type tty : _conf.lists.get(list)._types )

            {
                model.addRow(new Object[] {tty._name,tty._type,tty._size });
                System.out.println(model.getValueAt(model.getRowCount() - 1, 1));
            }
    }

}
