using System.ComponentModel;
using System.Configuration.Install;


namespace Sefin.ServiceTool
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();

            svcInstaller.ServiceName = Properties.ServiceInfo.ServiceCode;
            svcInstaller.DisplayName = Properties.ServiceInfo.ServiceDisplayName;
            svcInstaller.Description = Properties.ServiceInfo.ServiceDescription;
        }



    }
}