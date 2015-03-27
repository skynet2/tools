using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace pwApi.Utils
{
    public class UtilsIO
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static byte[] GenerateArray(byte[] arr, int size)
        {
            var newArr = new byte[size];
            for (int i = 0; i < arr.Length; i++)
                newArr[i] = arr[i];
            return newArr;
        }

        public static string NormalizeString(string input)
        {
            return input.Replace("\0", "");
        }
    }
}
