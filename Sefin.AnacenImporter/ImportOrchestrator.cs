using Sefin.Importer.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
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

        ConcurrentDictionary<string, ProcessWrapper> _processRegistry 
            = new ConcurrentDictionary<string, ProcessWrapper>();


        private const int MaxRunnigThreads = 5;


        public void Process()
        {

            DBHelper.Instance.Init(ConfigurationManager.ConnectionStrings["MainConnection"]);

            try
            {
                if (_processRegistry.Count >= MaxRunnigThreads) return;

                var files = ListFileToProcess();
                if (files.Length > 0)
                {
                    // Log("File da processare: " + String.Join(", ", files));

                    foreach(var file in files)
                    {
                        if (_processRegistry.Count >= MaxRunnigThreads) return;

                        var importFileInfo = PreprocessFile(file);

                        if (_processRegistry.ContainsKey(importFileInfo.Abi)) continue;

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

            var processWrapper = new ProcessWrapper(importFileInfo);
            processWrapper.SetLogger(_logger);

            processWrapper.OnTerminate += (sender, terminatingImportFile) =>
            {
                    ProcessWrapper tmp = null;
                    _processRegistry.TryRemove(terminatingImportFile.Abi, out tmp);
            };

            //Interlocked.Increment(ref _numRunningThread);
            _processRegistry[importFileInfo.Abi] = processWrapper;
            processWrapper.Start();
        }

        private void PrintRunningThreads()
        {
            Log("Running threads: " + _processRegistry.Count);
        }

        public void RequestStop()
        {
            var keys = _processRegistry.Keys;
            foreach(var key in keys)
            {
                ProcessWrapper wrapper = null;
                if (_processRegistry.TryGetValue(key, out wrapper))
                {
                    wrapper.Importer.RequestStop();
                }
            }
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

            return files.OrderBy(f => f).ToArray();
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
