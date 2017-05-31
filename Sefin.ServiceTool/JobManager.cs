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
                    Parameters = jobElement.Elements("parameter")
                                .Select(el => new JobParameter
                                {
                                    Name = el.Attribute("name").Value,
                                    Value = el.Attribute("value").Value,
                                    ParameterType = ParseType(el.Attribute("type").Value)
                                })
                                .ToList()
                });

            }
        }

       

        internal void RunJobs()
        {
            List<Task> runningJobs = new List<Task>();
            
            foreach(var job in _jobs)
            {

                //ExecuteJob(job);
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
            // lancia la classe di un eseguibile
            //var exeAssembly = Assembly.LoadFile("SampleConsole.exe");
            //var programType = exeAssembly.GetType("SampleConsole.Program");
            //var mainMethod = programType.GetMethod("Main");
            //mainMethod.Invoke(null, null);

            Log("Processing " + job.Name);
            //var jobInstance = Activator.CreateInstance(job.AssemblyName, job.ClassName);
            var asm = Assembly.Load(job.AssemblyName);
            var type = asm.GetType(job.ClassName);

            //var constructor = type.GetConstructor(new Type[0]);
            //var instance = constructor.Invoke(new object[0]);

            var instance = CreateWithParameters(type, job.Parameters);

            var jobDescription = type.GetCustomAttribute<JobDescriptionAttribute>();
            if (jobDescription != null)
            {
                Log($"Description: {jobDescription.Description}");
            }

            //var prop = type.GetProperty("Name");
            //prop.GetValue(instance);
            //prop.SetValue(instance, "Luca");

            var loggableJob = instance as ILogEnabled;
            if (loggableJob != null) loggableJob.SetLogger(_logger);


            var runnableJob = instance as IJob;
            if (runnableJob == null)
            {
                throw new Exception($"job {job.Name} does not implement IJob");
            }
            runnableJob.RunJob();

            //Log("Eseguito: " + method);


            //var name = instance.GetType().GetProperty("name").GetValue(instance);
            //instance.GetType().GetMethod("RunJob").Invoke(instance,null);
            //instance.GetType().GetCustomAttribute<JobDescriptionAttribute>();

        }

        private object CreateWithParameters(Type type, List<JobParameter> inputParameters)
        {
            
            foreach(var constructor in type.GetConstructors())
            {
                var constructorParameters = constructor.GetParameters();
                if (constructorParameters.Length != inputParameters.Count)
                    continue;

                var parameterList = new List<JobParameter>();
                var match = true;
                
                foreach(var param in constructorParameters)
                {
                    var inputParam = inputParameters.FirstOrDefault(p => String.Compare(p.Name,param.Name,true) == 0
                                    && p.ParameterType == param.ParameterType);

                    if (inputParam == null)
                    {
                        match = false;
                        break;
                    }
                    parameterList.Add(inputParam);
                }

                if (match)
                {
                    var values = parameterList.Select(par => par.GetParsedValue())
                                                .ToArray();

                    return constructor.Invoke(values);
                }
            }

            throw new Exception($"Constructor not found for {type} with {inputParameters.Count} parameters");
        }

        class JobDefinition
        {
            public string AssemblyName { get; internal set; }
            public string ClassName { get; internal set; }
            public string Name { get; internal set; }
            public List<JobParameter> Parameters { get; internal set; }
        }


        private class JobParameter
        {
            public string Name { get; internal set; }
            public Type ParameterType { get; internal set; }
            public string Value { get; internal set; }

            internal object GetParsedValue()
            {
                if (ParameterType == typeof(string)) return Value;
                if (ParameterType == typeof(int)) return Convert.ToInt32(Value);
                if (ParameterType == typeof(bool)) return Convert.ToBoolean(Value);

                throw new Exception("Unsupported type " + ParameterType);
            }
        }

        private Type ParseType(string type)
        {
            if (String.Compare(type, "string", true) == 0)
                return typeof(string);

            if (String.Compare(type, "int", true) == 0)
                return typeof(int);

            if (String.Compare(type, "bool", true) == 0)
                return typeof(bool);

            throw new Exception("Invalid parameter type : " + type);
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
