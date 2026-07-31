using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Queries;
using TaskFlow.Application.Interfaces.Others;

namespace TaskFlow.Application.Services.QueryService
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IProjectQuery _projectQuery;
        public ProjectQueryService(IProjectQuery projectQuery)
        {
            _projectQuery = projectQuery;
        }
        public async Task<IEnumerable<ProjectSummaryDto>> GetSummaryAsync(int userId)
        {
           return await _projectQuery.GetSummaryAsync(userId);
        }
    }
}
