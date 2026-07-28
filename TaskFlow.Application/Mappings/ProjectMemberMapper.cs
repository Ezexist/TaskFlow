using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Members;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Mappings
{
    public static class ProjectMemberMapper
    {
        public static ProjectMemberResponseDto ToResponseDto(this ProjectMember member)
        {
            return new ProjectMemberResponseDto
            {
                UserId = member.UserId,
                UserName = member.User.UserName,
                ProjectRole = member.Role,
                JoinedAt = member.JoinedAt
            };
        }
    }
}
