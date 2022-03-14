using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace SqlExample
{
    public static class SqlExample
    {
        public static void Run(
            [SqlTrigger("[dbo].[Employees]", ConnectionStringSetting = "SqlConnectionString")]
            IEnumerable<SqlChangeTrackingEntry<Employee>> changes,
            ILogger logger)
        {
            foreach (var change in changes)
            {
                Employee employee = change.Data;
                logger.LogInformation($"Change occurred to Employee table row: {change.ChangeType}");
                logger.LogInformation($"EmployeeID: {employee.EmployeeId}, FirstName: {employee.FirstName}, LastName: {employee.LastName}, Company: {employee.Company}, Team: {employee.Team}");
            }
        }
    }
}