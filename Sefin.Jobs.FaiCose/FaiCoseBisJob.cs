using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Jobs.FaiCose
{
    
    [JobDescription("cose bis", Notes = "sfsdgf")]
    public class FaiCoseBisJob: IJob
    {

        public void RunJob()
        {
            Log("BIS: Sono partito!!!");

            TranslationManager.Instance.Translate("...");

        }

        public override string ToString()
        {
            return "FaiCoseJobBis!!!";
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
