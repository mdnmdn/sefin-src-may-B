using System;
using System.Configuration.Install;
using System.Reflection;

namespace Sefin.ServiceTool
{
    public static class ServiceTools
    {

        private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

        public static bool Install()
        {
            try {
                ManagedInstallerClass.InstallHelper(new string[] { exePath });

                ServiceLogger.Instance.Log("Service installed correctly.");
                return true;
            } catch(Exception ex) {
                ServiceLogger.Instance.Log("Error installing service: " + ex);

                return false;
            }
        }

        public static bool Uninstall()
        {
            try {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
                ServiceLogger.Instance.Log("Service uninstalled correctly.");
                return true;
            } catch(Exception ex) {
                ServiceLogger.Instance.Log("Error uninstalling service: " + ex);
                return false;
            }
        }


    }
}
