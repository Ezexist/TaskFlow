
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Members;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Domain.Enities;
using TaskFlow.Domain.Enums;
using ConflictException = TaskFlow.Application.Exceptions.ConflictException;

namespace TaskFlow.Application.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;
        public ProjectMemberService(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository,
            IUserRepository userRepository)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }
        public async Task AddMemberAsync(int projectId, int userId, AddProjectMemberDto dto)
        {
            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if(project == null)
            {
                throw new NotFoundException("Project not found");
            }
            var currentMember = EnsureMember(project, userId);
            EnsureOwner(currentMember);

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }

            var isAlreadyMember = await _projectMemberRepository
                .IsProjectMemberAsync(projectId, user.Id);
            if (isAlreadyMember)
            {
                throw new ConflictException("User is already a member of this project");
            }

            var projectMember = new ProjectMember
            {
                ProjectId = projectId,
                UserId = user.Id,
                Role = ProjectRole.Member,
                JoinedAt = DateTimeOffset.UtcNow

            };
            await _projectMemberRepository.AddAsync(projectMember);

        }

        public async Task<IEnumerable<ProjectMemberResponseDto>> GetMembersAsync(int projectId, int userId)
        {
            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if(project == null)
            {
                throw new BadRequestException("Project not found");
            }
            EnsureMember(project, userId);
            var members = await _projectMemberRepository.GetProjectMembersAsync(projectId);

            return members.Select(x => x.ToResponseDto());
        }

        public async Task RemoveMemberAsync(int projectId, int userId, int memberId)
        {
            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if(project == null)
            {
                throw new NotFoundException("Project not Found");
            }
            var currentMember =  EnsureMember(project, userId);
            EnsureOwner(currentMember);
            var member = await _projectMemberRepository.GetByProjectAndUserId(projectId, memberId);
            if(member == null)
            {
                throw new NotFoundException("Member not found");
            }
            if(member.Role == ProjectRole.Owner)
            {
                throw new BadRequestException("Project owner can`t be removed");
            }

            await _projectMemberRepository.DeleteAsync(member);
        }



        private ProjectMember EnsureMember(Project project, int userId)
        {
            var member = project.Members.
                FirstOrDefault(x => x.UserId == userId);

            if (member is null)
            {
                throw new BadRequestException("You don't have access to this project.");
            }

            return member;
        }
        private void EnsureOwner(ProjectMember member)
        {

            if(member.Role != Domain.Enums.ProjectRole.Owner)
            {
                throw new BadRequestException("Only project owner can perform this action");
            }
        }
    }
}
