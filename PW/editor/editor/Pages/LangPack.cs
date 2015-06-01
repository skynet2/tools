using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace editor.Pages
{

    class LangLoader
    {
        public static Dictionary<string,string> current = new Dictionary<string, string>(); 
        public static void LoadLang(string file)
        {
            current = JsonConvert.DeserializeObject < Dictionary<string, string>>(File.ReadAllText(file, Encoding.UTF8));
        }

        public static void Default()
        {
            current.Add("MenuItem1","Файл");
            current.Add("Menu1SubItem1", "Открыть");
            current.Add("Menu1SubItem2", "Сохранить");
            current.Add("MenuItem2", "Конвертер");
            current.Add("MenuItem3", "Инфо");
            current.Add("MenuItem4", "Язык");
            current.Add("GridValues", "Значения");
            current.Add("GridDecrypt", "Расшифровка");
            current.Add("ContextMenu0", "Удалить");
            current.Add("ContextMenu1", "Клонировать");
            current.Add("ContextMenu2", "Экспорт");
            current.Add("ContextMenu3", "Импорт");
            File.WriteAllText(Path.Combine("languages", "Russian.json"), JsonConvert.SerializeObject(current), Encoding.UTF8);
        }
        public static string GetLangItem(string key)
        {
            return current.ContainsKey(key) ? current[key] : key;
        }
    }
}
