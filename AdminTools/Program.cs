using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace AdminTools
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            ServiceHost serviceHost;
            serviceHost = new ServiceHost(typeof(HealthMonitoringService));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IHealthMonitoring), binding, new
            Uri("net.tcp://localhost:6000/HealthMonitoring"));

            serviceHost.AddServiceEndpoint(typeof(IHealthMonitoring), binding, new
           Uri("net.tcp://localhost:6000/HealthMonitoring"));

            serviceHost.Open();
            Console.WriteLine("Server ready and waiting for requests.");
        }
    } 
}

