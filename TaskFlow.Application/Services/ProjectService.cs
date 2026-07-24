using FluentValidation;
using Raven.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Projects;
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
        public ProjectService(IProjectRepository projectRepository,IProjectMemberRepository projectMemberRepository,
            IValidator<CreateProjectDto> createProjectValidator)
        {
            _projectMemberRepo = projectMemberRepository;
            _projectRepository = projectRepository;
            _createProjectValidator = createProjectValidator;
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
                OwnerId = userId
            }; 
            await _projectRepository.AddAsync(project);

            var projectMember = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = userId,
                ProjectRole = ProjectRole.Owner
            };
            await _projectMemberRepo.AddAsync(projectMember);

            return project.ToResponseDto();
        }

        public Task DeleteAsync(int projectId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectResponseDto> GetByIdAsync(int projectId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectResponseDto>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectResponseDto> UpdateAsync(int projectId, int userId, UpdateProjectDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
