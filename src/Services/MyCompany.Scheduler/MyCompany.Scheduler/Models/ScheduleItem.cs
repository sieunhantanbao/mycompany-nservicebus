using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Models
{
    public class ScheduleItem
    {
        public string JobName { get; set; }
        public string Cron { get; set; }
        public string Command { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName ="Enabled", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Enabled { get; set; }

        public ScheduleItem()
        {
            JobName = string.Empty;
            Cron = string.Empty;
            Command = string.Empty;
            Enabled = true;
        }
    }
}
