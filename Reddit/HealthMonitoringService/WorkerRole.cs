using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common;
using HealthMonitoringService.Model;
using HealthMonitoringService.Repository;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;



namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private readonly HealthCheckRepository checkRepository=new HealthCheckRepository();


        public override void Run()
        {
            

            Trace.TraceInformation("JobWorker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("JobWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("JobWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("JobWorker has stopped");
        }

        private IHealthMonitoring proxyNotfication;

        private IHealthMonitoring proxyReddit;
        public void Connect()
        {
            var bindingNot = new NetTcpBinding();
            ChannelFactory<IHealthMonitoring> factoryNotification = new
            ChannelFactory<IHealthMonitoring>(bindingNot, new
            EndpointAddress("net.tcp://localhost:10100/HealthMonitoring"));
            proxyNotfication = factoryNotification.CreateChannel();

            var bindingRedd = new NetTcpBinding();
            ChannelFactory<IHealthMonitoring> factoryReddit = new
            ChannelFactory<IHealthMonitoring>(bindingRedd, new
            EndpointAddress("net.tcp://localhost:8081/HealthMonitoring"));
            proxyReddit = factoryReddit.CreateChannel();




        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Connect();
                    proxyReddit.IAmAlive();
                    HealthCheckEntity healthCheck=new HealthCheckEntity("OK",DateTime.Now,"reddit");
                    await LogToTable(healthCheck);
                    Trace.TraceInformation("Service is alive.");
                }
                catch
                {
                    HealthCheckEntity healthCheck = new HealthCheckEntity("NOT OK", DateTime.Now, "reddit");
                    await LogToTable(healthCheck);
                    Trace.TraceWarning("Service not alive anymore!");
                }
                try
                {
                    proxyNotfication.IAmAlive();
                    HealthCheckEntity healthCheck = new HealthCheckEntity("OK", DateTime.Now, "notification");
                    await LogToTable(healthCheck);
                    Trace.TraceInformation("Service is alive.");
                }
                catch
                {
                    HealthCheckEntity healthCheck = new HealthCheckEntity("NOT OK", DateTime.Now, "notification");
                    await LogToTable(healthCheck);
                    Trace.TraceWarning("Service not alive anymore!");
                }


                Random r = new Random();
                await Task.Delay(r.Next(1000,5000));
            }
        }

        private async Task LogToTable(HealthCheckEntity healthCheck)
        {
           await checkRepository.AddCheckAsync(healthCheck);

        }


    }
}
