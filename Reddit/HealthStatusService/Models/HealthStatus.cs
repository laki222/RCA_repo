using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthStatusService.Models
{
    public class HealthStatus
    {
        public double UptimeLast24Hours { get; set; }
        public double UptimeLastHour { get; set; }
        public List<string> Last24HoursStatuses { get; set; } 
        public List<string> LastHourStatuses { get; set; }
    }
}