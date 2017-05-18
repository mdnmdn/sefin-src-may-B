using Sefin.Importer.Common;
using System;
using System.Threading;

namespace Sefin.AnacenImporter
{
    internal class ProcessWrapper
    {
        public ImportFileInfo ImportFileInfo { get; private set; }
        public Thread ImportThread { get; private set; }
        public FileImporter Importer { get; private set; }

        public event EventHandler<ImportFileInfo> OnTerminate;

        public ProcessWrapper(ImportFileInfo importFileInfo)
        {
            this.ImportFileInfo = importFileInfo;
        }

        internal void Start()
        {
            Importer = new FileImporter(ImportFileInfo);
            Importer.SetLogger(_logger);

            ImportThread = new Thread(() =>
            {
                try
                {
                    Importer.Process();

                }catch(Exception ex)
                {
                    Log("!!!!Error nell'importazione di " + ImportFileInfo);
                }
                finally
                {
                    if (OnTerminate != null)
                        OnTerminate(this, ImportFileInfo);
                }
            });

            ImportThread.Start();
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