using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService
{
    public class HealthMonitoringService : IHealthMonitoring
    {
        public string IAmAlive()
        {
            return "ALIVEEEEEEEEE";
        }
    }
}
