using HealthMonitoringService.Model;
using HealthMonitoringService.Repository;
using HealthStatusService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HealthStatusService.Controllers
{
    public class HomeController : Controller
    {
        private readonly HealthCheckRepository healthCheckRepository=new HealthCheckRepository();

        public async Task<ActionResult> Index()
        {
            DateTime now = DateTime.UtcNow;
            DateTime last24Hours = now.AddHours(-24);
            DateTime lastHour = now.AddHours(-1);

            var last24HoursChecks = await healthCheckRepository.GetHealthChecksAsync(last24Hours);
            var lastHourChecks = await healthCheckRepository.GetHealthChecksAsync(lastHour);

            var uptimeLast24Hours = CalculateUptime(last24HoursChecks);
            var uptimeLastHour = CalculateUptime(lastHourChecks);
            List<string> last24HoursStatuses = new List<string>();
            List<string> lastHourStatuses = new List<string>();
            
            foreach (var item in last24HoursChecks)
            {
                
                    last24HoursStatuses.Add(item.Status);
               
                   
                

            }
            foreach (var item in lastHourChecks)
            {

                lastHourStatuses.Add(item.Status);

              

            }


            var model = new HealthStatus
            {
                UptimeLast24Hours = uptimeLast24Hours,
                UptimeLastHour = uptimeLastHour,
                Last24HoursStatuses = last24HoursStatuses,
                LastHourStatuses = lastHourStatuses
            };

            return View(model);
        }

        private double CalculateUptime(List<HealthCheckEntity> healthChecks)
        {
            if (healthChecks == null || !healthChecks.Any()) return 0;

            int totalChecks = healthChecks.Count;
            int availableChecks = healthChecks.Count(hc => hc.Status == "OK");

            return (double)availableChecks / totalChecks * 100;
        }
    }
}