using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ecmCopy
{
    internal class Logic
    {
      //  private static List<string> GlobalFiles { get; set; }
        private static Dictionary<string, HashSet<string>> GlobalFiles;
        private static int counter;

        public static void ReadFile(string path, int encoding = 936)
        {
            try
            {
                var fileContent = new List<string>(File.ReadLines(path, Encoding.GetEncoding(encoding)));
                foreach (var line in fileContent)
                {
                    if (!line.Contains("FxFilePath:")) continue;

                    var sp = line.Split(':');
                    var val = sp[1].TrimStart();
                    string ext = Path.GetExtension(val);
                    switch (ext)
                    {
                        case ".gfx":
                            var p =
                                Path.Combine(
                                    MainWindow.PathBuilder(true, "gfx", App.Current.Dispatcher.Invoke(() => MainWindow.Window.PckCheckBox.IsChecked)),
                                    val);
                            AddValue(ext,p);
                            ParseGfx(p);
                            break;
                        case ".wav":
                            AddValue(ext,
                                Path.Combine(
                                    MainWindow.PathBuilder(true, "sfx", App.Current.Dispatcher.Invoke(() => MainWindow.Window.PckCheckBox.IsChecked)),
                                    val));
                            break;
                        default:
                            MessageBox.Show(Path.GetExtension(val));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MainWindow.AddtoLogBox(e.Message);
            }
        }


        public static void CopyFile(string from, string to)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var finalPath = from.Replace(MainWindow.elementPath, to);
                if (!MainWindow.Window.DestCheckBox.IsChecked.GetValueOrDefault())
                    finalPath = RemovepckFiles(finalPath);

                var finalDir = Path.GetDirectoryName(finalPath);

                if (finalDir != null && !Directory.Exists(finalDir))
                    Directory.CreateDirectory(finalDir);
                try
                {
                    MainWindow.SetValueProgress(++counter);
                    File.Copy(from, finalPath, true);
                }
                catch (Exception e)
                {
                    MainWindow.AddtoLogBox(e.Message);
                }
            });
        }

        private static string RemovepckFiles(string path)
        {
            var sp = path.Split('\\');
            for (int i = 0; i < sp.Length ; i++)
            {
                if (sp[i].Contains(".pck.files"))
                    sp[i] = "";
                if (sp[i].Contains(":"))
                    sp[i] += "\\";
            }
            return Path.Combine(sp);
        }
        private static void ParseGfx(string path)
        {
                var fileContent = new List<string>(File.ReadLines(path, Encoding.GetEncoding(936)));
                foreach (var line in fileContent)
                {
                    if (!line.Contains("TexFile:")) continue;
                    var sp = line.Split(':');
                    if(sp[1].Contains("."))
                        AddValue(Path.GetExtension(sp[1]),Path.Combine(MainWindow.gfxPath, "textures", sp[1].TrimStart()));
                }

        }
        public static void ProcessEcm(HashSet<string> ecmFiles)
        {
            GlobalFiles = new Dictionary<string, HashSet<string>>();
            foreach (var ll in ecmFiles)
                if (Path.GetExtension(ll) != "")
                    ParseFolder(Path.GetDirectoryName(ll));
            else ParseFolder(ll);

            foreach (var key in GlobalFiles.ToList())
            {
                foreach (var file in key.Value.ToList())
                {
                    switch (key.Key)
                    {
                        case ".ecm" :
                            ReadFile(file);
                            break;
                        case ".gfx":
                            ParseGfx(file);
                            break;
                    }
                }

            }
            PrepereUi();
            foreach (var file in GlobalFiles.SelectMany(key => key.Value))
                CopyFile(file,MainWindow.GetTextValue(1));
            MainWindow.AddtoLogBox("Копирование завершено");
        }

        private static void PrepereUi()
        {
            MainWindow.AddtoLogBox("Файлы для копирования :");
            MainWindow.SetMaxProgress(GlobalFiles.Count);
            foreach (var key in GlobalFiles)
                MainWindow.AddtoLogBox(string.Format("[{0}] - {1}", key.Key, key.Value.Count));

            MainWindow.SetValueProgress(0);
            counter = 0;
            MainWindow.AddtoLogBox("Копирование началось");
        }
        private static void AddValue(string key, string val)
        {
            if(!GlobalFiles.ContainsKey(key))
                GlobalFiles.Add(key, new HashSet<string>(new[] { val }));
            else GlobalFiles[key].Add(val);
        }

        private static void ParseFolder(string path)
        {
            try
            {
                foreach (var f in Directory.GetFiles(path))
                {
                    AddValue(Path.GetExtension(f),f);
                }
                foreach (var d in Directory.GetDirectories(path))
                {
                    ParseFolder(d);
                }
            }
            catch (Exception e)
            {
                MainWindow.AddtoLogBox(e.Message);
            }

        }
    }
}
