using System;
using Sefin.Importer.Common;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;

namespace Sefin.AnacenImporter
{
    internal class FileImporter
    {
        private ImportFileInfo importFileInfo;

        private bool _requestStop = false;

        private bool _readComplete;
        private Exception _readException;
        const int MaxDataWorker = 5;

        BlockingCollection<string> _dataQueue = new BlockingCollection<string>();

        public FileImporter(ImportFileInfo importFileInfo)
        {
            this.importFileInfo = importFileInfo;
        }



        internal void Process()
        {
            Log("Processing " + importFileInfo);

            var readingtask = Task.Factory.StartNew(ReadImportFile);

            for(int i = 1; i <= MaxDataWorker; i++)
            {
                var workerName = "datawork-" + i;
                var worker = Task.Factory.StartNew(name =>
                {
                    while (true)
                    {
                        var data = _dataQueue.Take();
                        Log(name + ": take " + data.Substring(0,10) + " queue: " + _dataQueue.Count);
                        Thread.Sleep(200);
                    }

                }, workerName);
            }

            Task.WaitAll(readingtask);

            Log("Completed " + importFileInfo);
        }

        private void ReadImportFile()
        {
            try
            {
                using (var fs = File.Open(importFileInfo.FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            // da leggere un blocco di righe
                            Log("reading, queue: " + _dataQueue.Count);
                            _dataQueue.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _readException = ex;
            }
            finally
            {
                _readComplete = true;
            }
        }

        internal void ProcessFake()
        {
            Log("Processing " + importFileInfo);

            var _lock = new object();

            if (0 == new Random().Next(3))
            {
                Log("errore!!!");
                throw new ImportException();
            }


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