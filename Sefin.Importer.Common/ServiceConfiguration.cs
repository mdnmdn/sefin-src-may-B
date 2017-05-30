using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Importer.Common
{
    public class ServiceConfiguration
    {

        #region Singleton

        static object _lock = new object();

        static ServiceConfiguration _instance;

        static public ServiceConfiguration Instance
        {
            get
            {
                //return _instance ?? (_instance = new ServiceConfiguration());
                if (_instance == null)
                {
                    _instance = new ServiceConfiguration();
                }

                return _instance;
            }
        }

        #endregion

        ServiceConfiguration()
        {
            ImportFilePath   = NormalizeConfigPath("Sefin.Importer.ImportFilePath"); ;
            StagingFilePath  = NormalizeConfigPath("Sefin.Importer.StagingFilePath"); ;
            CompleteFilePath = NormalizeConfigPath("Sefin.Importer.CompleteFilePath"); ;
            JobManagerConfigFile = NormalizeConfigPath("Sefin.JobManager.ConfigFile"); ;
        }


        public string ImportFilePath { get; private set; }
        public string StagingFilePath { get; private set; }
        public string CompleteFilePath { get; private set; }
        public string JobManagerConfigFile { get; private set; }

        private static string NormalizeConfigPath(string configKey)
        {
            var importPath = ConfigurationManager.AppSettings[configKey];

            if (!Path.IsPathRooted(importPath))
                importPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, importPath);
            return importPath;
        }
    }
}
