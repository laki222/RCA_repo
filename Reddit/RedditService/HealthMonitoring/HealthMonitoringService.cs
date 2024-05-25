using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditService.HealthMonitoring
{
    public class HealthMonitoringService : IHealthMonitoring
    {

        public string IAmAlive()
        {
            return "I am OKKKKKK"; 
        }
    }
}