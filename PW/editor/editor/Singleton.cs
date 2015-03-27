using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace editor
{
    public class Singleton<T> where T : class
    {
        private static T _instance;

        /// <summary>
        /// Gets the instance of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                OnInit();

                return _instance;
            }
        }

        protected Singleton() { }

        private static void OnInit()
        {
            if (_instance != null)
                return;
            lock (typeof(T))
            {
                _instance = typeof(T).InvokeMember(typeof(T).Name,
                                                   BindingFlags.CreateInstance |
                                                   BindingFlags.Instance |
                                                   BindingFlags.Public |
                                                   BindingFlags.NonPublic,
                                                   null, null, null) as T;
            }
        }
    }
}
