using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Services
{
    public interface IScheduleService
    {
        Task ScheduleScan(int frenquency);
    }
}
