using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Domain.Enities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepo;
        private readonly IValidator<CreateProjectDto> _createProjectValidator;
        private readonly IValidator<UpdateProjectDto> _updateProjectValidator;
        public ProjectService(IProjectRepository projectRepository,IProjectMemberRepository projectMemberRepository,
            IValidator<CreateProjectDto> createProjectValidator, IValidator<UpdateProjectDto> updateProjectValidator)
        {
            _projectMemberRepo = projectMemberRepository;
            _projectRepository = projectRepository;
            _createProjectValidator = createProjectValidator;
            _updateProjectValidator = updateProjectValidator;
        }
        public async Task<ProjectResponseDto> CreateAsync(int userId, CreateProjectDto dto)
        {
            var validatorResult = await _createProjectValidator.ValidateAsync(dto);
            if (!validatorResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(Environment.NewLine,
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId,
                CreatedAt = DateTimeOffset.UtcNow
            }; 
            await _projectRepository.AddAsync(project);

            var projectMember = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = ProjectRole.Owner,
                JoinedAt = DateTimeOffset.UtcNow,
                
            };
            await _projectMemberRepo.AddAsync(projectMember);

            var createdProject = await _projectRepository.GetDetailsByIdAsync(project.Id);
            //loaded navigation property
            if(createdProject is null)
            {
                throw new BadRequestException("Project was not found after creation");
            }

            return createdProject.ToResponseDto();
        }

        public async Task DeleteAsync(int projectId, int userId)
        {
            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if (project is null)
            {
                throw new BadRequestException("Project not found");
            }
            var member = EnsureMember(project, userId);
            EnsureOwner(member);

            await _projectRepository.DeleteAsync(project);
        }

        public async Task<ProjectResponseDto> GetByIdAsync(int projectId, int userId)
        {

            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if (project is null)
            {
                throw new NotFoundException("Project not found");
            }
    
            EnsureMember(project, userId);

            return project.ToResponseDto();
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetByUserIdAsync(int userId)
        {
            var projects =await _projectRepository.GetByUserIdAsync(userId);

            return projects.Select(x => x.ToResponseDto());
        }

        public async Task<ProjectResponseDto> UpdateAsync(int projectId, int userId, UpdateProjectDto dto)
        {
            var validatorResult = await _updateProjectValidator.ValidateAsync(dto);
            if (!validatorResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(Environment.NewLine,
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }

            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if(project == null)
            {
                throw new NotFoundException("Project not found.");
            }
            var member = EnsureMember(project,userId);
            EnsureOwner(member);

            project.Name = dto.Name;
            project.Description = dto.Description;

            await _projectRepository.UpdateAsync(project);

            return project.ToResponseDto();
        }

        //PRIVATE 
        private ProjectMember EnsureMember(Project project,int userId)
        {
            var member = project.Members
                .FirstOrDefault(x => x.UserId == userId);
            if (member == null)
            {
                throw new BadRequestException("You dont have access to this project");
            }
            return member;
        }

        private void EnsureOwner(ProjectMember member)
        {
               if(member.Role != ProjectRole.Owner)
            {
                throw new BadRequestException("Only project owner can update the project");
            }
        }
    }
}
