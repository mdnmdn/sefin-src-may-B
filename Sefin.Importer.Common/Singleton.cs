using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Importer.Common
{
    public class Singleton<K> where K : new()
    {
        static K _instance;

        static public K Instance
        {
            get
            {
                // non thread safe
                if (_instance == null)
                {
                    _instance = new K();
                }
                return _instance;
            }
        }
    }


    public class ThreadSingleton<K> where K : new()
    {
        [ThreadStatic]
        static K _instance;

        static public K Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new K();
                }
                return _instance;
            }
        }
    }

    public class RequestSingleton<K> where K : new()
    {
        static public K Instance
        {
            get
            {
                var key = "___singleton_" + typeof(K);
                if (HttpContext.Current.Items[key] == null)
                {
                    HttpContext.Current.Items[key] = new K();
                }
                return (K) HttpContext.Current.Items[key];
            }
        }
    }
}
