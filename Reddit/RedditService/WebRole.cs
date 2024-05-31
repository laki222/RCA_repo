using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using RedditService.HealthMonitoring;
using RedditService.Models;
using RedditService.Repository;
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


        public override async void OnStop()
        {
           PostRepository postRepository = new PostRepository();
            
            
                // Primer: Postavljanje vrednosti u bazi
               
                    List<PostEntity> entities =await postRepository.RetrieveAllPosts();
                    foreach (var entity in entities)
                    {
                        entity.CommentsOpen = false;
                        await postRepository.UpdatePostAsync(entity);
                    }
                  
                
            

            base.OnStop();
        }


    }
}
