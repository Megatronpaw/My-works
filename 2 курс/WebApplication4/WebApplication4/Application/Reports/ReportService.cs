using System.Data;
using WebApplication4.Infrastructure.Reports;
using Dapper;

namespace WebApplication4.Application.Reports
{
        public class ReportService : IReportService
        {
            private readonly IDbConnection _connection;
            public ReportService(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetGroupLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter filter)
            {
                return await _connection.QueryAsync<GroupLeadersAndLaggardsItem>(
                    ReportQueries.GroupLeadersAndLaggards, filter);
            }

            public async Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter filter)
            {
                return await _connection.QueryAsync<StudentTestResultsItem>(
                  ReportQueries.StudentTestResults, filter);
            }
        }
}


