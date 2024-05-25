using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Models
{
    public class NotificationLogEntity:TableEntity
    {

        public DateTime Date { get; set; }
        public string CommentId { get; set; }
        public int EmailCount { get; set; }

    }
}
