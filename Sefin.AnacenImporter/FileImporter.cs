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
        const int MaxDataWorker = 4;
        const int MaxDbWorker = 6;

        int _rowsRead = 0;
        int _blocksRead = 0;

        int _blockSize = 50;

        BlockingCollection<string> _inputQueue = new BlockingCollection<string>();        
        BlockingCollection<string> _ouputQueue = new BlockingCollection<string>();

        BlockingCollection<List<string>> _inputQueueWithBlocks = new BlockingCollection<List<string>>();
        BlockingCollection<List<string>> _outputQueueWithBlocks = new BlockingCollection<List<string>>();

        List<Task> _workerTasks = new List<Task>();

        public FileImporter(ImportFileInfo importFileInfo)
        {
            this.importFileInfo = importFileInfo;
        }


        internal void Process()
        {
            //ProcessByRow();
            ProcessWithBlocks();
        }

        internal void ProcessByRow()
        {
            var startTimestamp = DateTime.Now;
            Log("Processing " + importFileInfo);

            var readingtask = Task.Factory.StartNew(ReadImportFile);
            _workerTasks.Add(readingtask);
            

            for(int i = 1; i <= MaxDataWorker; i++)
            {
                var workerName = "validateworker-" + i;
                
                var worker = Task.Factory.StartNew(name => Validation((string)name), workerName);
                _workerTasks.Add(worker);
            }


            for (int i = 1; i <= MaxDbWorker; i++)
            {
                var workerName = "dbworker-" + i;
                var worker = Task.Factory.StartNew(name => WriteToDb((string)name), workerName);
                _workerTasks.Add(worker);
            }


            Task.WaitAll(_workerTasks.ToArray());

            var durationMs = (int)DateTime.Now.Subtract(startTimestamp).TotalMilliseconds;
            Log("Completed " + importFileInfo + " in " + durationMs + "ms");
            
        }

        internal void ProcessWithBlocks()
        {
            var startTimestamp = DateTime.Now;
            Log("Processing with blocks " + importFileInfo);

            var readingtask = Task.Factory.StartNew(ReadImportFileByBlock);
            _workerTasks.Add(readingtask);

            for (int i = 1; i <= MaxDataWorker; i++)
            {
                var workerName = "validateworker[bl]-" + i;

                var worker = Task.Factory.StartNew(name => ValidationWithBlocks((string)name), workerName);
                _workerTasks.Add(worker);
            }


            for (int i = 1; i <= MaxDbWorker; i++)
            {
                var workerName = "dbworker[bl]-" + i;
                var worker = Task.Factory.StartNew(name => WriteToDbWithBlocks((string)name), workerName);
                _workerTasks.Add(worker);
            }


            Task.WaitAll(_workerTasks.ToArray());

            var durationMs = (int)DateTime.Now.Subtract(startTimestamp).TotalMilliseconds;
            Log("Completed " + importFileInfo + " in " + durationMs + "ms");

        }

        private void Validation(string name)
        {
            int count = 0;
            Log(name + ": Starting");
            while (!(_inputQueue.Count == 0 && _readComplete))
            {
                string data = null;
                if (_inputQueue.TryTake(out data, 500))
                {
                    count++;
                    LogQueueStatus();

                    var rowTs = DateTime.Now;
                    FakeMixedRowProcessing(30, 10);
                    var rowDuration = DateTime.Now.Subtract(rowTs).TotalMilliseconds;
                    Log(name + ": processed in " + rowDuration + "ms");
                    _ouputQueue.Add(data);
                }
            }

            Log(name + ": Terminated processing " + count + " rows");
        }

        private void WriteToDb(string name)
        {
            int count = 0;
            Log(name + ": Starting");
            while (!(_ouputQueue.Count == 0 && _readComplete))
            {
                string data = null;
                if (_ouputQueue.TryTake(out data, 500))
                {
                    count++;
                    LogQueueStatus();

                    var rowTs = DateTime.Now;
                    FakeMixedRowProcessing(5, 18);
                    var rowDuration = DateTime.Now.Subtract(rowTs).TotalMilliseconds;
                    Log(name + ": processed in " + rowDuration + "ms");
                }
            }

            Log(name + ": Terminated processing " + count + " rows");
        }

        private void ValidationWithBlocks(string name)
        {
            int count = 0;
            Log(name + ": Starting");
            while (!(_inputQueueWithBlocks.Count == 0 && _readComplete))
            {
                List<string> data = null;
                if (_inputQueueWithBlocks.TryTake(out data, 500))
                {
                    count++;
                    LogQueueBlockStatus();

                    var rowTs = DateTime.Now;
                    for (int r = 0; r < data.Count; r++)
                        FakeMixedRowProcessing(30, 10);

                    var rowDuration = DateTime.Now.Subtract(rowTs).TotalMilliseconds;
                    Log(name + ": processed in " + rowDuration + "ms");
                    _outputQueueWithBlocks.Add(data);
                }
            }

            Log(name + ": Terminated processing " + count + " rows");
        }

        private void WriteToDbWithBlocks(string name)
        {
            int count = 0;
            Log(name + ": Starting");
            while (!(_outputQueueWithBlocks.Count == 0 && _readComplete))
            {
                List<string> data = null;
                if (_outputQueueWithBlocks.TryTake(out data, 500))
                {
                    count++;
                    LogQueueBlockStatus();

                    var rowTs = DateTime.Now;
                    for (int r = 0; r < data.Count; r++)
                        FakeMixedRowProcessing(2, 8);

                    var rowDuration = DateTime.Now.Subtract(rowTs).TotalMilliseconds;
                    Log(name + ": processed in " + rowDuration + "ms");
                }
            }

            Log(name + ": Terminated processing " + count + " rows");
        }

        private void LogQueueBlockStatus()
        {
            Log(String.Format("Rows read: {0} Blcoks read: {3} Input queue (bl): {1}   Output queue (bl): {2}",
                _rowsRead, _inputQueueWithBlocks.Count, _outputQueueWithBlocks.Count,_blocksRead));
        }

        private void LogQueueStatus()
        {
            Log(String.Format("Rows read: {0}  Input queue: {1}   Output queue: {2}", _rowsRead, _inputQueue.Count, _ouputQueue.Count));
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
                            Log("reading, queue: " + _inputQueue.Count);
                            _inputQueue.Add(line);
                            _rowsRead++;
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

        private void ReadImportFileByBlock()
        {
            try
            {
                using (var fs = File.Open(importFileInfo.FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        List<string> block = null;
                        while (!reader.EndOfStream)
                        {
                            if (block == null) block = new List<string>();
                            var line = reader.ReadLine();
                            block.Add(line);
                            if (block.Count == _blockSize)
                            {
                                // da leggere un blocco di righe
                                Log("reading, queue: " + _inputQueue.Count);
                                _inputQueueWithBlocks.Add(block);
                                _rowsRead += _blockSize;
                                _blocksRead++;
                                block = null;
                            }
                        }

                        if (block != null)
                        {
                            Log("reading, queue: " + _inputQueue.Count);
                            _inputQueueWithBlocks.Add(block);
                            _blocksRead++;
                            _rowsRead += block.Count;
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