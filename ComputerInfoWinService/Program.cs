using System.ServiceProcess;

namespace ComputerInfoWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ComputerInfoService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
