using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sefin.AnacenImporter
{
    public class ImportOrchestrator
    {
        object _lock = new object();

        private const int MaxRunnigThreads = 5;

        private int _numRunningThread = 0;

        public void Process()
        {
            try
            {
                if (_numRunningThread >= MaxRunnigThreads) return;

                var files = ListFileToProcess();
                if (files.Length > 0)
                {
                    // Log("File da processare: " + String.Join(", ", files));

                    foreach(var file in files)
                    {
                        if (_numRunningThread >= MaxRunnigThreads) return;

                        var importFileInfo = PreprocessFile(file);

                        ProcessFile(importFileInfo);
                    }
                }
            }catch(Exception ex)
            {
                Log("Error processing: " + ex);
            }
        }

        private void ProcessFile(ImportFileInfo importFileInfo)
        {
            importFileInfo.MoveToFolder(ServiceConfiguration.Instance.StagingFilePath);

            var importer = new FileImporter(importFileInfo);
            importer.SetLogger(_logger);

            var thread = new Thread(() =>
            {
                importer.Process();

                //Interlocked.Decrement(ref _numRunningThread);

                lock (_lock)
                {
                    _numRunningThread--;
                }
                PrintRunningThreads();

            });

            thread.Start();
            //Interlocked.Increment(ref _numRunningThread);
            lock (_lock)
            {
                _numRunningThread++;
            }

            PrintRunningThreads();


        }

        private void PrintRunningThreads()
        {
            Log("Running threads: " + _numRunningThread);
        }

        private ImportFileInfo PreprocessFile(string file)
        {
            var path = Path.GetDirectoryName(file);
            var abi = Path.GetFileName(path);
            return new ImportFileInfo()
            {
                Abi = abi,
                FilePath = file
            };
        }

        private string[] ListFileToProcess()
        {
            var files = Directory.GetFiles(ServiceConfiguration.Instance.ImportFilePath, "*.txt", 
                                    SearchOption.AllDirectories);

            return files;
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
