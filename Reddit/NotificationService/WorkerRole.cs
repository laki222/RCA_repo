using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ServiceBus.Messaging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Azure;
using RedditService;
using RedditService.Models;
using NotificationService.Models;
using RedditService.Repository;
using RedditService.HealthMonitoring;
using System.ServiceModel;
using Common;
using PostmarkDotNet;


namespace NotificationService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private QueueClient queueClient;

      
        private const string QueueName = "test";
        private const string StorageConnectionString = "DataConnectionString";
        private const string SendGridApiKey = "YourSendGridApiKey";
        // private readonly CommentRepository commentRepository=new CommentRepository();
        private CloudQueue queue;
        private CloudQueue queueAdmins;

        public override void Run()
        {
            Trace.TraceInformation("NotificationService is running");

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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Initialize the queue client
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("notifications");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
            CloudQueueClient queueClientAdmin = storageAccount.CreateCloudQueueClient();

            queueAdmins = queueClientAdmin.GetQueueReference("admins");
            queueAdmins.CreateIfNotExists();



            ServiceHost serviceHost;
            serviceHost = new ServiceHost(typeof(HealthMonitoringService));
            NetTcpBinding binding = new NetTcpBinding();

            serviceHost.AddServiceEndpoint(typeof(IHealthMonitoring), binding, new
            Uri("net.tcp://localhost:10100/HealthMonitoring"));

            serviceHost.Open();


            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("NotificationService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("NotificationService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
           

            while (!cancellationToken.IsCancellationRequested)
            {
                CloudQueueMessage message = await queue.GetMessageAsync();
                CloudQueueMessage messageAdmin = await queueAdmins.GetMessageAsync();
                if (messageAdmin != null)
                {
                    try
                    {

                       


                        SendEmailsAdminsAsync(messageAdmin.AsString);

                        await queueAdmins.DeleteMessageAsync(messageAdmin.Id, messageAdmin.PopReceipt);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Error processing message: {0}", ex.Message);

                    }
                }

                if (message != null)
                {
                    try
                    {

                        var comment = Newtonsoft.Json.JsonConvert.DeserializeObject<CommentEntity>(message.AsString);
                        await ProcessMessageAsync(comment);

                        await queue.DeleteMessageAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Error processing message: {0}", ex.Message);
                       
                    }
                }

                await Task.Delay(1000);
            }
        }

        private async Task ProcessMessageAsync(CommentEntity comment)
        {
            //CommentEntity comment=await commentRepository.GetComment(commentId);
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            var _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);

            var table = tableClient.GetTableReference("Reactions");
            var query = new TableQuery<ReactionEntity>()
                .Where(TableQuery.GenerateFilterCondition("PostId", QueryComparisons.Equal, comment.PostId));
            var result = await table.ExecuteQuerySegmentedAsync(query, null);

            var emails = result.Results.FindAll(sub => sub.Reaction=="UPVOTE").ToList();


           // Assume this method fetches the comment text
            await SendEmailsAsync(emails, comment.Content);

            // Log the notification details
            var notificationLog = tableClient.GetTableReference("NotificationLog");
            var logEntity = new NotificationLogEntity
            {
                PartitionKey = "NotificationLog",
                RowKey = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                CommentId = comment.RowKey,
                EmailCount = emails.Count
            };
            await notificationLog.ExecuteAsync(TableOperation.Insert(logEntity));
        }



        public async Task SendEmailsAsync(List<ReactionEntity> emails, string commentText)
        {
           
            var client = new PostmarkClient("7cc441a8-8816-4664-93a5-9e10833ecc0b");

            foreach (var email in emails)
            {
                var message = new PostmarkMessage
                {
                    From = "brankovic.pr121.2020@uns.ac.rs",
                    To = email.SubscribedUser,
                    Subject = "New Comment",
                    TextBody = commentText,
                };

                var response = await client.SendMessageAsync(message);
                Trace.TraceInformation($"Email sent to {email.SubscribedUser} with status {response.Status}");
            }
        }

        public void SendEmailsAdminsAsync(string email)
        {

            var client = new PostmarkClient("7cc441a8-8816-4664-93a5-9e10833ecc0b");

           
                var message = new PostmarkMessage
                {
                    From = "brankovic.pr121.2020@uns.ac.rs",
                    To = email,
                    Subject = "PROBLEM",
                    TextBody = "DOWN",
                };

                var response =  client.SendMessageAsync(message);
               
            
        }

    }
}
