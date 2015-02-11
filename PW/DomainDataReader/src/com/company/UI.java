package com.company;

import com.company.Structure.DomainData;
import com.company.Structure.DomainSev;
import com.company.Utils.BinaryReader;
import com.company.Utils.BinaryWriter;
import com.company.Utils.Config;
import org.json.JSONException;

import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;

/**
 * Created by io on 12.10.14.
 */
public class UI
{
    public DomainData dom;
    public DomainSev sev;
    private JTree tree;
    public JTextField _ter_name,_ter_type,path;
    private int selected;
    private DefaultTreeModel model;
    private JButton save,get_file;
    private JFrame frame;
    private JFileChooser choose;
    private JComboBox<String> type;
    private boolean isData;
    DefaultMutableTreeNode root;

    public UI() throws IOException, JSONException {
        frame = new JFrame("Editor");
        JButton load = new JButton("Load");
        JButton save = new JButton("Save");
        _ter_name = new JTextField("Null");
        _ter_type = new JTextField("Null");
        tree = new JTree(new DefaultMutableTreeNode("Domain.data"));
        get_file = new JButton("...");

        model = (DefaultTreeModel) tree.getModel();
        root = (DefaultMutableTreeNode)model.getRoot();
        /// Values
        Box bv = new Box(BoxLayout.Y_AXIS);
        bv.add(new JLabel("     Types       "));
        bv.add(new JLabel("0 = Flag capture"));
        bv.add(new JLabel("1 = Bridge"));
        bv.add(new JLabel("2 = Сrystals"));
        /// Mode
        type = new JComboBox<String>(new String[]{".data", ".sev"});
        path = new JTextField(Config.read()[0]);
        type.setSelectedItem(Config.read()[1]);
        /// Editor
        frame.add(new JLabel("Name")).setBounds(225, 40, 100, 20);
        frame.add(_ter_name).setBounds(225,60,170,25);
        frame.add(new JLabel("Type")).setBounds(225,90,100,20);
        frame.add(_ter_type).setBounds(225,120,170,25);
        frame.add(bv).setBounds(225,160,170,80);
        frame.add(save).setBounds(225,230,170,140);
        // Main
        frame.setLayout(null);
        frame.add(type).setBounds(291,22,105,18);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setBounds(0, 0, 400, 400);
        frame.add(path).setBounds(0, 0, 290, 20);
        frame.add(get_file).setBounds(291,0,29,20);
        frame.add(load).setBounds(325,0,70,20);
        frame.add(new JScrollPane(tree)).setBounds(0, 23, 220, 370);
        frame.setVisible(true);
        // listeners
        get_file.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                choose = new JFileChooser();
                choose.showDialog(frame,"Open file");
                path.setText(String.valueOf(choose.getSelectedFile()));
            }
        });
        save.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                BinaryWriter bw = null;
                try {
                        bw = new BinaryWriter(path.getText());
                } catch (FileNotFoundException e) {
                    e.printStackTrace();
                }
                try {
                    if(isData)
                    {
                        DomainData res = new DomainData(bw, dom);
                    }
                    else
                    {
                        DomainSev _sev = new DomainSev(bw,sev);
                    }

                } catch (IOException e) {
                    e.printStackTrace();
                }
                JOptionPane.showMessageDialog(frame,"Saved to " + path.getText());
            }

        });
        _ter_type.getDocument().addDocumentListener(new DocumentListener() {
            @Override
            public void insertUpdate(DocumentEvent documentEvent) {
                change();
            }

            @Override
            public void removeUpdate(DocumentEvent documentEvent) {
            }

            @Override
            public void changedUpdate(DocumentEvent documentEvent) {
                change();
            }
            private void change()
            {
                if(isData)
                    dom.domains.get(selected)._battletype = Integer.parseInt(_ter_type.getText().trim());
                else
                    sev.zones.get(selected)._battletype = Integer.parseInt(_ter_type.getText().trim());
            }
        });
        _ter_name.getDocument().addDocumentListener(new DocumentListener() {
            @Override
            public void insertUpdate(DocumentEvent documentEvent) {
                change();
            }

            @Override
            public void removeUpdate(DocumentEvent documentEvent) {
            }

            @Override
            public void changedUpdate(DocumentEvent documentEvent) {
                change();
            }

            private void change() {
                System.err.println(_ter_name.getText());
                DefaultMutableTreeNode node = (DefaultMutableTreeNode) model.getChild(root, (selected));
                node.setUserObject(_ter_name.getText());
                model.reload(node);
                if (isData)
                    dom.domains.get(selected)._name = _ter_name.getText();
                else {
                    sev.zones.get(selected)._id = Integer.parseInt(_ter_name.getText().trim());
                }
            }
        });
        tree.addTreeSelectionListener(new TreeSelectionListener() {
            @Override
            public void valueChanged(TreeSelectionEvent treeSelectionEvent) {
                selected = tree.getMinSelectionRow() - 1;
                if (selected != -1) {
                    if (isData) {
                        _ter_name.setText(dom.domains.get(selected)._name);
                        System.out.println("Type : " + String.valueOf(dom.domains.get(selected)._battletype));
                        _ter_type.setText(String.valueOf(dom.domains.get(selected)._battletype).trim());
                    } else {
                        System.err.println("Selected id " + selected);
                        System.out.println("ID : " + String.valueOf(sev.zones.get(selected)._id).trim());
                        _ter_name.setText(String.valueOf(sev.zones.get(selected)._id).trim());
                        _ter_type.setText(String.valueOf(sev.zones.get(selected)._battletype).trim());
                    }
                }
                System.out.println(tree.getMinSelectionRow());
            }
        });
        load.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                try {
                    isData = type.getSelectedIndex()==0 ? true : false;
                    if(new File(path.getText()).exists()) {
                        BinaryReader br = new BinaryReader(new FileInputStream(path.getText()));
                        if(isData) {
                            dom = new DomainData(br);
                            for (DomainData.Domain d : dom.domains)
                                root.insert(new DefaultMutableTreeNode(d._name), root.getChildCount());
                            tree.expandRow(0);
                        }
                        else
                        {
                            sev = new DomainSev(br);
                            for(DomainSev.Domain_zone zone : sev.zones)
                                root.insert(new DefaultMutableTreeNode(zone._id),root.getChildCount());
                            tree.expandRow(0);
                        }
                        Config.write(path.getText(), isData ? ".data" : ".sev");
                    }
                    else JOptionPane.showMessageDialog(frame,String.format("File %s not found",path.getText()));
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        });
    }
}
