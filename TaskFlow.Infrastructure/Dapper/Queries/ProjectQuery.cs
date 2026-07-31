using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskFlow.Application.DTO.Queries;
using TaskFlow.Application.Interfaces.Others;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Infrastructure.Dapper.Queries
{
    public class ProjectQuery : IProjectQuery
    {
        private readonly TaskFlowDbContext _context;
        public ProjectQuery(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectSummaryDto>> GetSummaryAsync(int userId)
        {
            var connection = _context.Database.GetDbConnection();
            var completed = (int)TaskStatus.Done;

            const string sql =
                """
                SELECT
                    p."Id",
                    p."Name",

                    COUNT(DISTINCT t."Id") AS Tasks,

                    COUNT(DISTINCT CASE
                            WHEN t."Status" = @CompletedStatus
                            THEN t."Id"
                        END
                    ) AS Completed,

                    COUNT(DISTINCT pm."Id") AS Members
                FROM "Projects" p

                LEFT JOIN "Tasks" t
                    ON t."ProjectId" = p."Id"

                LEFT JOIN "ProjectMembers" pm
                    ON pm."ProjectId" = p."Id"

                JOIN "ProjectMembers" pmUser
                    ON pmUser."ProjectId" = p."Id"

                WHERE pmUser."UserId" = @UserId

                GROUP BY
                    p."Id",
                    p."Name"

                ORDER BY p."Name";
                """;

            var result = await connection.QueryAsync<ProjectSummaryDto>(sql, new
            {
                UserId = userId,
                CompletedStatus = completed
            });

            return result;  
        }
    }
}
