using System;
using Sefin.Importer.Common;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Sefin.AnacenImporter
{
    internal class FileImporter
    {
        private ImportFileInfo importFileInfo;

        private bool _requestStop = false;

        private bool _readComplete;
        private Exception _readException;
        const int MaxDataWorker = 6;

        BlockingCollection<string> _dataQueue = new BlockingCollection<string>(20);
        List<Task> _workerTasks = new List<Task>();

        public FileImporter(ImportFileInfo importFileInfo)
        {
            this.importFileInfo = importFileInfo;
        }



        internal void Process()
        {
            var startTimestamp = DateTime.Now;
            Log("Processing " + importFileInfo);

            var readingtask = Task.Factory.StartNew(ReadImportFile);
            _workerTasks.Add(readingtask);

            var _lock = new object();

            for(int i = 1; i <= MaxDataWorker; i++)
            {
                var workerName = "datawork-" + i;
                var worker = Task.Factory.StartNew(name =>
                {
                    int count = 0;
                    Log(name + ": Starting");
                    while (!(_dataQueue.Count == 0 && _readComplete))
                    {
                        string data = null;
                        if (_dataQueue.TryTake(out data, 500)){
                            count++;
                            Log(name + ": take " + data.Substring(0, 10) + " queue: " + _dataQueue.Count);
                            //Thread.Sleep(300);
                            var rowTs = DateTime.Now;
                            FakeMixedRowProcessing(33,30);
                            var rowDuration = DateTime.Now.Subtract(rowTs).TotalMilliseconds;
                            Log(name + ": processed in " + rowDuration + "ms");
                        }
                    }

                    Log(name + ": Terminated processing " + count + " rows");

                }, workerName);
                _workerTasks.Add(worker);
            }

            Task.WaitAll(_workerTasks.ToArray());

            var durationMs = (int)DateTime.Now.Subtract(startTimestamp).TotalMilliseconds;
            Log("Completed " + importFileInfo + " in " + durationMs + "ms");
            
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
                Log("Reader: Terminated");
            }
        }

        void FakeMixedRowProcessing(int processorTimeMs = 4000, int waitTime = 2000)
        {
            var start = DateTime.Now;
            var endProcessor = DateTime.Now.AddMilliseconds(processorTimeMs);
            while(true)
            {
                for (long j = 0; j < 100000L; j++)
                {
                    var res = Math.Sqrt(j * j);
                }
                if (DateTime.Now > endProcessor) break;
            }

            Thread.Sleep(waitTime);
            //for (long i = 0; i < 1000L; i++)
            //{
            //    Thread.Sleep(1);
            //}

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