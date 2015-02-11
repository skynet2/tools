using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Threading;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace ecmCopy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow Window;
        public MainWindow()
        {
            Window = this;
            InitializeComponent();
            loadConfig();
        }

        private void loadConfig()
        {
            try
            {
                string[] ss = File.ReadAllLines("x.cfg");
                FromTextBox.Text = ss[0];
                ToTextBox.Text = ss[1];
            }
            catch (Exception)
            {
                CreateConfig(new []{"From","To"});
                loadConfig();
            }
        }

        private void CreateConfig(string[] paths)
        {
            File.WriteAllLines("x.cfg",paths);
        }

        public HashSet<string> EcmHashList;
        public static string elementPath;
        public static string gfxPath;

        public static string PathBuilder(bool from,string pck,bool? dotPck)
        {
            string path = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                path =  Path.Combine(from ? Window.FromTextBox.Text : Window.ToTextBox.Text,
                    dotPck.GetValueOrDefault() ? string.Format("{0}.pck.files", pck) : "", pck);
            });
            return path;
        }

        public static void AddtoLogBox(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Window.LogBox.AppendText(text + "\r");
                Window.LogBox.ScrollToEnd();
            });
        }

        public static void SetMaxProgress(int max)
        {
            Application.Current.Dispatcher.Invoke(() => Window.ProgressBar1.Maximum = max);
        }

        public static void SetValueProgress(int val)
        {
            Application.Current.Dispatcher.Invoke(() => Window.ProgressBar1.Value = val);
        }

        public static string GetTextValue(int pos)
        {
            string text = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (pos)
                {
                    case 0:
                        text = Window.FromTextBox.Text;
                        break;
                    default :
                        text =  Window.ToTextBox.Text;
                        break;

                }
            });
            return text;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => Window.LogBox.Document.Blocks.Clear());
            CreateConfig(new []{FromTextBox.Text,ToTextBox.Text});
            try
            {
                EcmHashList = new HashSet<string>();
                gfxPath = Path.Combine(FromTextBox.Text, PckCheckBox.IsChecked.GetValueOrDefault() ? "gfx.pck.files" : "",
                    "gfx");
                elementPath = FromTextBox.Text;
                foreach (var text in new TextRange(EcmRichTextBox.Document.ContentStart, EcmRichTextBox.Document.ContentEnd).Text.Split('\n'))
                {
                    var path = text.Replace("\r", "");
                    var val = "";
                    if (PckCheckBox.IsChecked.GetValueOrDefault())
                        val = getPck(path);
                    if (path.Equals("")) continue;
                    EcmHashList.Add(Path.Combine(FromTextBox.Text, val, path));
                }
                AddtoLogBox("Количество .ecm " + EcmHashList.Count);
                new Thread(() => Logic.ProcessEcm(EcmHashList)).Start();
            }
            catch (Exception e1)
            {
                AddtoLogBox(e1.Message);
            }

        }

        private string getPck(string path)
        {
            string[] vals = path.Split('\\');
            return string.Format("{0}.pck.files", vals[0]);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            FromTextBox.Text = dialog.SelectedPath;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            ToTextBox.Text = dialog.SelectedPath;
        }
    }
}
