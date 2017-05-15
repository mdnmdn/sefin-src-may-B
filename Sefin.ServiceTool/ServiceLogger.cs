using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.ServiceTool
{
    class ServiceLogger 
    {

        #region Singleton

        static ServiceLogger _instance;

        static public ServiceLogger Instance
        {
            get
            {
                //return _instance ?? (_instance = new ServiceLogger());
                if (_instance == null)
                {
                    _instance = new ServiceLogger();
                }

                return _instance;
            }
        }

        #endregion

        DateTime _lastTimeStamp = DateTime.Now;

        public Action<string> Logger { get; set; }

        public void Log(string message)
        {
            if (Logger == null) return;

            var delta = DateTime.Now.Subtract(_lastTimeStamp);
            _lastTimeStamp = DateTime.Now;

            System.Diagnostics.Trace.WriteLine(message);

            if (Logger != null) {
                var msg = String.Format("{0:0.00}] {1} ", delta.TotalMilliseconds / 1000, message);
                Logger(msg);
            }
        }
    }
}
