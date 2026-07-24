using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Projects;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Mappings
{
    public static class ProjectMapper
    {
        public static ProjectResponseDto ToResponseDto(this Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                OwnerUserName = project.Owner.UserName
            };
        }
    }
}
