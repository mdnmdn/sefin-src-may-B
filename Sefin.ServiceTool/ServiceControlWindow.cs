using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace Sefin.ServiceTool
{
    public partial class ServiceControlWindow : Form
    {

        WindowStatus currentStatus = WindowStatus.LoadingInfo;
        protected WindowStatus CurrentStatus
        {
            get { return currentStatus; }
            set
            {
                WindowStatus oldStatus = currentStatus;
                currentStatus = value;
                if (currentStatus != oldStatus)
                {
                    RefreshWindow();
                }
            }
        }

        public ServiceControlWindow()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
        }

        private void ServiceControlWindow_Load(object sender, EventArgs e)
        {
            ServiceLogger.Instance.Logger = m =>  TxtLog.AppendText(m + Environment.NewLine);

            BtnRefresh_Click(null, null);
        }

        
        
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            CurrentStatus = WindowStatus.LoadingInfo;
            ThreadPool.QueueUserWorkItem(RefreshInfo);
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (CurrentStatus == WindowStatus.ServiceNotInstalled)
            {
                ThreadPool.QueueUserWorkItem(StartProgram);
            }
            else if (CurrentStatus == WindowStatus.ProgramRunning)
            {
                ThreadPool.QueueUserWorkItem(StopProgram);
            }
            else
            {
                CurrentStatus = WindowStatus.Processing;
                if (BtnStartStop.Text == "Start")
                {
                    ThreadPool.QueueUserWorkItem(StartService);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(StopService);
                }
            }
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            CurrentStatus = WindowStatus.Processing;

            if (BtnInstall.Text == "Install")
            {
                ThreadPool.QueueUserWorkItem(InstallService);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(UnInstallService);
            }
        }


        void RefreshWindow()
        {
            switch (currentStatus)
            {
                case WindowStatus.LoadingInfo:
                    LblStatusBar.Text = "Refreshing...";
                    LblStatus.Text = String.Empty;
                    LblInfo.Text = String.Empty;
                    LblInstall.Text = String.Empty;
                    BtnStartStop.Enabled = false;
                    BtnRefresh.Enabled = false;
                    BtnInstall.Enabled = false;

                    break;
                case WindowStatus.Processing:
                    LblStatusBar.Text = "Refreshing...";
                    BtnStartStop.Enabled = false;
                    BtnRefresh.Enabled = false;
                    BtnInstall.Enabled = false;

                    break;
                case WindowStatus.ServiceNotInstalled:
                    LblStatusBar.Text = "Done.";
                    LblStatus.Text = "N/A";
                    LblInstall.Text = "Not installed";
                    BtnStartStop.Enabled = true;
                    BtnStartStop.Text = "Start";
                    BtnRefresh.Enabled = true;
                    BtnInstall.Enabled = true;
                    BtnInstall.Text = "Install";

                    break;
                case WindowStatus.ServiceRunning:
                    LblStatusBar.Text = "Ready.";
                    LblStatus.Text = "Service Running";
                    LblInstall.Text = "Installed";
                    BtnStartStop.Enabled = true;
                    BtnStartStop.Text = "Stop";
                    BtnRefresh.Enabled = true;
                    BtnInstall.Enabled = false;
                    BtnInstall.Text = "Uninstall";

                    break;

                case WindowStatus.Waiting:
                    LblStatusBar.Text = "Ready.";
                    LblInstall.Text = "Installed";
                    BtnStartStop.Enabled = false;
                    BtnRefresh.Enabled = true;
                    BtnInstall.Enabled = false;
                    BtnInstall.Text = "Uninstall";

                    break;

                case WindowStatus.ServiceStopped:
                    LblStatusBar.Text = "Ready.";
                    LblStatus.Text = "Service Stopped";
                    LblInstall.Text = "Installed";
                    BtnStartStop.Enabled = true;
                    BtnStartStop.Text = "Start";
                    BtnRefresh.Enabled = true;
                    BtnInstall.Enabled = true;
                    BtnInstall.Text = "Uninstall";

                    break;
                case WindowStatus.ProgramRunning:
                    LblStatusBar.Text = "Program started..";
                    LblStatus.Text = "Program running";
                    BtnStartStop.Enabled = true;
                    BtnStartStop.Text = "Stop";
                    BtnInstall.Enabled = false;

                    break;
            }
        }



        protected void RefreshInfo(object stateObject)
        {
            ServiceController svc = GetService();
            if (svc == null)
            {
                if (service != null && service.IsRunning)
                {
                    CurrentStatus = WindowStatus.ProgramRunning;
                }
                else
                {
                    CurrentStatus = WindowStatus.ServiceNotInstalled;
                }
            }
            else
            {
                LblInfo.Text = String.Format("{0} ({1})", svc.DisplayName, svc.ServiceName);
                LblStatus.Text = svc.Status.ToString();

                switch (svc.Status)
                {
                    case ServiceControllerStatus.Running:
                        CurrentStatus = WindowStatus.ServiceRunning;
                        break;
                    case ServiceControllerStatus.Stopped:
                        CurrentStatus = WindowStatus.ServiceStopped;
                        break;
                    default:
                        CurrentStatus = WindowStatus.Waiting;
                        break;
                }
            }
        }

        protected void StartService(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Starting...";
                ServiceController svc = GetService();
                svc.Start();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                Log("Error starting service: " + ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }
        }

        protected void StopService(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Stopping...";
                ServiceController svc = GetService();
                svc.Stop();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                Log("Error stopping service: " + ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }

        }

        protected void InstallService(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Installing service";
                ServiceTools.Install();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                //Logger.Error("Install:", ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }
        }

        protected void UnInstallService(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Uninstalling service";
                ServiceTools.Uninstall();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                //Logger.Error("Uninstall:", ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }
        }


        protected ServiceController GetService()
        {
            foreach (ServiceController svc in ServiceController.GetServices())
            {
                if (svc.ServiceName == Properties.ServiceInfo.ServiceCode)
                {
                    return svc;
                }
            }
            return null;
        }


        SchedulerService service = null;

        protected void StartProgram(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Starting...";
                if (service == null)
                {
                    service = new SchedulerService();
                }

                service.StartService();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                Log("Error starting:" + ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }
        }

        protected void StopProgram(object stateObject)
        {
            try
            {
                LblStatusBar.Text = "Stopping...";
                service.StopService();
                RefreshInfo(null);
            }
            catch (Exception ex)
            {
                Log("Error stopping:" + ex);
                RefreshInfo(null);
                LblStatusBar.Text = "Error in current operation";
            }

        }

        private void ServiceControlWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            TxtLog.Text = String.Empty;
        }

        private void Log(string msg)
        {
            ServiceLogger.Instance.Log(msg);
        }
    }

    public enum WindowStatus
    {
        LoadingInfo,
        ServiceNotInstalled,
        Processing,
        Disabled,
        Waiting,
        ServiceStopped,
        ServiceRunning,
        ProgramRunning
    }
}