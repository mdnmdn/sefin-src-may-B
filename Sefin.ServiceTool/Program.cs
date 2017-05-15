using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Sefin.ServiceTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Init();
            try
            {
                if (IsService())
                {
                    // run as service 
                    Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { new SchedulerService() };
                    ServiceBase.Run(ServicesToRun);
                }
                else
                {
                    // run as application
                    Application.Run(new ServiceControlWindow());
                }
            }
            catch (Exception ex)
            {
				//log4net.LogManager.GetLogger("Service").Error("Main", ex);

            }
        }


        static bool IsService()
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName == Properties.ServiceInfo.ServiceCode
                    && service.Status == ServiceControllerStatus.StartPending)
                {
                    return true;
                }
            }

            return false;
        }

        private static void Init()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

    }
}
