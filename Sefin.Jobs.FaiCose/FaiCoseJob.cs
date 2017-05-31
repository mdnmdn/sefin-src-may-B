using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Jobs.FaiCose
{
    
    public class FaiCoseJob : IJob, ILogEnabled
    {
        public FaiCoseJob()
        {
            Abi = "<No abi>";
            ConstructorType = "0";
        }

        public FaiCoseJob(string filename, string abi)
        {
            Abi = abi;
            ConstructorType = "2";
        }

        public FaiCoseJob(string abi, string filename, bool backupFile, int timeout)
        {
            Abi = abi;
            ConstructorType = "4";
        }

        public void RunJob()
        {
            Log($"Runinng abi: {Abi} ({ConstructorType})");
        }

        public override string ToString()
        {
            return $"FaiCoseJob ABI: {Abi}";
        }



        #region logging

        ILogger _logger;

        public string Abi { get; private set; }
        public string ConstructorType { get; private set; }

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
