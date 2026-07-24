using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Projects
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
