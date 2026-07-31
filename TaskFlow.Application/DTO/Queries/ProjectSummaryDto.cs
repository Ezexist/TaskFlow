using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Queries
{
    public class ProjectSummaryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Tasks { get; set; }

        public int Completed { get; set; }

        public int Members { get; set; }
    }
}
