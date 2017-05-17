using System;
using Sefin.Importer.Common;
using System.Threading;

namespace Sefin.AnacenImporter
{
    internal class FileImporter
    {
        private ImportFileInfo importFileInfo;

        private bool _requestStop = false;

        public FileImporter(ImportFileInfo importFileInfo)
        {
            this.importFileInfo = importFileInfo;
        }

        internal void Process()
        {
            Log("Processing " + importFileInfo);

            var _lock = new object();

            for (long i = 0; i < 1000L; i++)
            {
                for (long j = 0; j < 1000000L; j++)
                {
                    var res = Math.Sqrt(i * j);
                }

                if (_requestStop)
                {
                    Log("Request stop " + importFileInfo);
                    Thread.Sleep(3000 + new Random().Next(5) * 1000);
                    Log("Stopped " + importFileInfo);
                    return;
                }
                //for (long j = 0; j < 5000L; j++)
                //{
                //    var res = Math.Sqrt(i * j);
                //}
                //Thread.Sleep(5);
            }

            importFileInfo.MoveToFolder(ServiceConfiguration.Instance.CompleteFilePath);

            Log("Completed " + importFileInfo);
        }

        public void RequestStop()
        {
            _requestStop = true;
        }


        #region logging

        ILogger _logger;

        void Log(string message)
        {
            if (_logger != null)
                _logger.Log(Thread.CurrentThread.ManagedThreadId + "] " + message);
        }

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        #endregion
    }
}