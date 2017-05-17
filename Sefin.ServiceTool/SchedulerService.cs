using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using Sefin.AnacenImporter;

namespace Sefin.ServiceTool
{
    public partial class SchedulerService : ServiceBase
    {
        /// <summary>
        /// main thread
        /// </summary>
        protected Thread _thread;


        /// <summary>
        /// 
        /// </summary>
        protected bool _continue = true;

        /// <summary>
        /// 
        /// </summary>
        private int _joinTimeMs = 4000;

        /// <summary>
        /// ctor: init component and local data
        /// </summary>
        public SchedulerService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// starts the main thread
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _continue = true;
            _thread = new Thread(Process);
            _thread.IsBackground = true;
            _thread.Start();
        }

        /// <summary>
        /// tells the main thread to stop and waits for its termination
        /// </summary>
        protected override void OnStop()
        {
            //throw new Exception("non mi fermo!!!");

            _continue = false;
            //_thread.Join();  // wait forever
            
            _thread.Join(_joinTimeMs);
            if (_thread.ThreadState != ThreadState.Stopped) { 
                _thread.Abort();
                _thread.Join(_joinTimeMs);
            }
        }

        protected void Process()
        {
            Log("  - Starting process -");

            var orchestrator = new ImportOrchestrator();
            orchestrator.SetLogger(ServiceLogger.Instance);

            while (true)
            {
                try
                {
                    Log("  Performing...");

                    orchestrator.Process();

                } catch(Exception ex)
                {
                    Log("Service unexpected error: " + ex);
                }

                Thread.Sleep(2000);
                if (!_continue)
                {
                    orchestrator.RequestStop();
                    break;
                }
            }

            //Thread.Sleep(20000);
            Log("  - Process completed -");
        }


        public bool IsRunning { get { return _thread != null && _thread.ThreadState == ThreadState.Running; } }

        /// <summary>
        /// starts the service
        /// </summary>
        public void StartService()
        {
            Log("calling startservice");
            this.OnStart(null);
        }

        /// <summary>
        /// stops the service
        /// </summary>
        public void StopService()
        {
            Log("calling stopservice");
            this.OnStop();
        }

        void Log(string message)
        {
            ServiceLogger.Instance.Log(message);
        }
    }
}
