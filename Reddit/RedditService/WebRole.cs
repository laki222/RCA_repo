using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using RedditService.HealthMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace RedditService
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            ServiceHost serviceHost;
            serviceHost = new ServiceHost(typeof(HealthMonitoringService));
            NetTcpBinding binding = new NetTcpBinding();

            serviceHost.AddServiceEndpoint(typeof(IHealthMonitoring), binding, new
           Uri("net.tcp://localhost:8081/HealthMonitoring"));

            serviceHost.Open();
          

            return base.OnStart();
        }
    }
}
