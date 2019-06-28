using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace NotificationSystem
{
    static class Program
    {
        public enum ServiceCustomCommands { StopWorker = 128, RestartWorker, CheckWorker };
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            string strNotificationSystemName = Properties.EndarProcessorSettings.Default.ServiceName;
            LogHandler erh = null;
           
            try
            {
                ServiceController[] scServices;
                scServices = ServiceController.GetServices();

                erh = new LogHandler("", "Program.Main()");
               
                System.ServiceProcess.ServiceBase[] ServicesToRun;                            

                NotificationService nsc = new NotificationService();
                erh = new LogHandler(nsc.ServiceName, "program main");
                erh.LogWarning("starting");

                ServicesToRun = new System.ServiceProcess.ServiceBase[] { nsc };

                erh.WindowsServiceName = nsc.ServiceName;
                erh.LogWarning(nsc.ServiceName);
                System.ServiceProcess.ServiceBase.Run(ServicesToRun);

                foreach (ServiceController scTemp in scServices)
                {
                    //erh.LogWarning("service  = " + scTemp.ServiceName + " " + scTemp.Status.ToString());

                    if (scTemp.ServiceName == strNotificationSystemName)
                    {
                        ServiceController sc = new ServiceController(strNotificationSystemName);
                                          
                        erh.LogWarning(nsc.ExitCode.ToString());
                      

                        if (sc.Status == ServiceControllerStatus.Stopped && 
                            nsc.ExitCode == NotificationService.EXIT_NEED_RESTART)
                        {
                            sc.Start();
                            //RestartService(strNotificationSystemName, 1000);                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHandler erh1 = new LogHandler("EnDARDatabaseNotificationService", "program main");
                erh1.LogWarning("error in program main" + ex.ToString());
                erh1.LogError("Program.main()", ex);       
            }

        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            LogHandler erh = null;
          
            try
            {
                int millisec1 = Environment.TickCount;
                erh = new LogHandler("", "RestartService");
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                LogHandler erh1 = new LogHandler("EnDARDatabaseNotificationService", "RestartService");
                erh1.LogWarning("error in RestartService" + ex.ToString());
                erh1.LogError("RestartService", ex);       
            }
        }
    }
}
