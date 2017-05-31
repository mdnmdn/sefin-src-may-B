using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Importer.Common
{
    public abstract class BaseJob : IJob, ILogEnabled
    {
        public abstract void RunJob();



        #region logging

        ILogger _logger;
        
        protected void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }


        #endregion
    }
}
