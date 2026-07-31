using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Queries;

namespace TaskFlow.Application.Interfaces.Others
{
    public interface IProjectQueryService
    {
        Task<IEnumerable<ProjectSummaryDto>> GetSummaryAsync(int userId);
    }
}
