using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Jobs.FaiCose
{
    public class FaiCoseJob : IJob
    {
        public void RunJob()
        {
            Log("Sono partito!!!");
        }

        public override string ToString()
        {
            return "FaiCoseJob!!!";
        }



        #region logging

        ILogger _logger;

        void Log(string message)
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
