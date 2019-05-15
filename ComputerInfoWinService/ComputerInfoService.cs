using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Management;
using System.DirectoryServices;

namespace ComputerInfoWinService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
    public partial class ComputerInfoService : ServiceBase
    {
        private int eventId = 1;
        public ComputerInfoService()
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            var timer = new Timer();

            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            //var machineName = Environment.MachineName;
            var domainName = Environment.UserDomainName;
            //var userName = Environment.UserName;


            var list = GetComputerUsers();
            string info = string.Empty;
            list.ForEach(x => info += x + Environment.NewLine);

            eventLog1.WriteEntry($"PC info = {info}", EventLogEntryType.Information, eventId++);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        public static List<string> GetComputerUsers()
        {
            List<string> users = new List<string>();
            var path =
                string.Format("WinNT://{0},computer", Environment.MachineName);

            using (var computerEntry = new DirectoryEntry(path))
                foreach (DirectoryEntry childEntry in computerEntry.Children)
                    if (childEntry.SchemaClassName == "User")
                        users.Add(childEntry.Name);

            return users;
        }
    }
}
