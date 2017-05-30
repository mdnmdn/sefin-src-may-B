using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sefin.ServiceTool
{
    public class JobManager
    {
        List<JobDefinition> _jobs = new List<JobDefinition>();

        public void LoadXmlConfig(string fileName)
        {
            var doc = XDocument.Load(fileName);

            var jobs = doc.Root.Element("jobs").Elements("job");

            foreach(var jobElement in jobs)
            {
                _jobs.Add(new JobDefinition {
                    Name = jobElement.Attribute("name").Value,
                    ClassName = jobElement.Attribute("class").Value,
                    AssemblyName = jobElement.Attribute("assembly").Value,
                });
            }
        }


        internal void RunJobs()
        {
            List<Task> runningJobs = new List<Task>();
            
            foreach(var job in _jobs)
            {
                var task = Task.Factory.StartNew(j => {
                    ExecuteJob((JobDefinition)j);
                },job);

                runningJobs.Add(task);
            }

            Task.WaitAll(runningJobs.ToArray());


            //Parallel.ForEach(_jobs, j => ExecuteJob(j));
            //Parallel.ForEach(_jobs, ExecuteJob);


        }

        private void ExecuteJob(JobDefinition job)
        {

            //var jobInstance = Activator.CreateInstance(job.AssemblyName, job.ClassName);
            var asm = Assembly.Load(job.AssemblyName);
            var type = asm.GetType(job.ClassName);

            var constructor = type.GetConstructor(new Type[0]);
            var instance = constructor.Invoke(new object[0]);

            var setLoggerMethod = type.GetMethod("SetLogger");

            if (setLoggerMethod != null)
            {
                setLoggerMethod.Invoke(instance, new object[] { _logger });
            }

            var method = type.GetMethod("RunJob");

            method.Invoke(instance, new object[0]);
            Log("Eseguito: " + method);
        }


        class JobDefinition
        {
            public string AssemblyName { get; internal set; }
            public string ClassName { get; internal set; }
            public string Name { get; internal set; }
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
