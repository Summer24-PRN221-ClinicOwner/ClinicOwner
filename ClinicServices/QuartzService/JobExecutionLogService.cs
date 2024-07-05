using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClinicServices.QuartzService
{
    public class JobExecutionLogService : IJobExecutionLogService
    {
        private readonly string _logFilePath;

        public JobExecutionLogService(IConfiguration configuration)
        {
            var baseDirectory = Directory.GetCurrentDirectory();
            _logFilePath = Path.Combine(baseDirectory, "jobExecutionLogs.json");
        }

        public async Task LogExecutionTimeAsync(string jobName, DateTime executionTime)
        {
            var logEntries = await ReadLogEntriesAsync();
            logEntries[jobName] = executionTime;
            await WriteLogEntriesAsync(logEntries);
        }

        public async Task<DateTime?> GetLastExecutionTimeAsync(string jobName)
        {
            var logEntries = await ReadLogEntriesAsync();
            return logEntries.ContainsKey(jobName) ? logEntries[jobName] : (DateTime?)null;
        }

        private async Task<Dictionary<string, DateTime>> ReadLogEntriesAsync()
        {
            if (!File.Exists(_logFilePath))
            {
                return new Dictionary<string, DateTime>();
            }

            try
            {
                var json = await File.ReadAllTextAsync(_logFilePath);
                return JsonSerializer.Deserialize<Dictionary<string, DateTime>>(json) ?? new Dictionary<string, DateTime>();
            }
            catch (JsonException)
            {
                // Handle JSON deserialization errors
                return new Dictionary<string, DateTime>();
            }
        }

        private async Task WriteLogEntriesAsync(Dictionary<string, DateTime> logEntries)
        {
            var json = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_logFilePath, json);
        }
    }
}
