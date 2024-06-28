using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.QuartzService
{
    public interface IJobExecutionLogService
    {
        Task<DateTime?> GetLastExecutionTimeAsync(string jobName);
        Task LogExecutionTimeAsync(string jobName, DateTime executionTime);
    }
}
