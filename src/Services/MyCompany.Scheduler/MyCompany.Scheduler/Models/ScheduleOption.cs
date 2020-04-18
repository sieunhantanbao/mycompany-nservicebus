using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Models
{
    public class ScheduleOption
    {
        public string ScheduleCron { get; set; }
        public int Frequency { get; set; }
        public string URLGatewayAPI { get; set; }
    }
}
