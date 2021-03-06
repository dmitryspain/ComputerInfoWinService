﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices;
using System.Management;
using System.DirectoryServices;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using ComputerInfo.Models.Models;

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
        System.Timers.Timer _timer = new System.Timers.Timer();
        private PCInfo _pcInfo = new PCInfo();

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
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 10000
            };

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(ServiceHandle, ref serviceStatus);


            _timer.Elapsed += new ElapsedEventHandler(OnTimer);
            _timer.Interval = TimeSpan.FromSeconds(3).TotalMilliseconds;
            _timer.Interval = 3000; // 3 sec for the first entrance 
            _timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
        }

        public async void OnTimer(object sender, ElapsedEventArgs args)
        {
            _timer.Interval = TimeSpan.FromMinutes(30).TotalMilliseconds; // 30 min for the next requests

            SelectQuery query = new SelectQuery(@"Select * from Win32_ComputerSystem");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject process in searcher.Get())
                {
                    var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    var ramCounter = new PerformanceCounter("Memory", "Available MBytes", string.Empty, Environment.MachineName);

                    cpuCounter.NextValue();
                    Thread.Sleep(500);

                    _pcInfo.CpuLoad = (int)cpuCounter.NextValue();
                    _pcInfo.RamLoad = (int)ramCounter.NextValue();
                    _pcInfo.Name = process["Name"].ToString();
                    _pcInfo.Manufacturer = process["Manufacturer"].ToString();
                    _pcInfo.Model = process["Model"].ToString();
                    _pcInfo.Users = MapToUsers(GetComputerUsers());
                    _pcInfo.DateTime = DateTime.Now;
                }
            }

            var response = string.Empty;

            var myContent = JsonConvert.SerializeObject(_pcInfo);
            HttpContent content = new StringContent(myContent, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                HttpResponseMessage result = await client.PostAsync(new Uri("https://localhost:44338/api/pcinfo/add"), content);
                if (result.IsSuccessStatusCode)
                {
                    response = result.StatusCode.ToString();
                }
            }

            eventLog1.WriteEntry(response);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private static List<string> GetComputerUsers()
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

        private List<User> MapToUsers(IEnumerable<string> userList)
        {
            var users = new List<User>();

            foreach(var userName in userList)
            {
                users.Add(new User
                {
                    Name = userName,
                });
            }

            return users;
        }
    }
}
