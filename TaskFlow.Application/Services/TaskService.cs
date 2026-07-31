using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings.Tasks;
using TaskFlow.Domain.Enities;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateTaskDto> _createTaskValidator;
        private readonly IValidator<UpdateTaskDto> _updateTaskValidator;
        private readonly IValidator<UpdateTaskStatusDto> _updateTaskStatusValidator;

        public TaskService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IProjectMemberRepository projectMemberRepository,
            IUserRepository userRepository,
            IValidator<CreateTaskDto> createTaskValidator,
            IValidator<UpdateTaskDto> updateTaskDto,
            IValidator<UpdateTaskStatusDto> updateTaskStatusValidator)
            
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _userRepository = userRepository;
            _createTaskValidator = createTaskValidator;
            _updateTaskValidator = updateTaskDto;
            _updateTaskStatusValidator = updateTaskStatusValidator;
        }

        public async Task<TaskResponseDto> AssignUserAsync(int taskId, int userId, AssignTaskDto dto)
        {
            var task = await GetTaskForUserAsync(taskId, userId);
            if (dto.AssignedUserId is null)
            {
                task.AssignedUserId = null;

                await _taskRepository.UpdateAsync(task);

                return await GetTaskResponseAsync(task.Id);
            }

            await EnsureAssignedUserAsync(task.ProjectId, dto.AssignedUserId);

            task.AssignedUserId = dto.AssignedUserId;

            await _taskRepository.UpdateAsync(task);

            return await GetTaskResponseAsync(task.Id);
        }

        public async Task<TaskResponseDto> CreateAsync(int projectId, int userId, CreateTaskDto dto)
        {
            var validationResult = await _createTaskValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(Environment.NewLine,
                    validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            var project = await _projectRepository.GetDetailsByIdAsync(projectId);
            if(project == null)
            {
                throw new NotFoundException("Project not found");
            }

            EnsureMember(project, userId);

           await EnsureAssignedUserAsync(projectId, userId);

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Deadline = dto.Deadline,

                Status = TaskStatus.Todo,
                CreatedAt = DateTimeOffset.UtcNow,
                ProjectId = projectId,
                CreatedById = userId,
                AssignedUserId = dto.AssignedUserId
            };

            await _taskRepository.AddAsync(task);

            return await GetTaskResponseAsync(task.Id);
        }
        public async Task DeleteAsync(int taskId, int userId)
        {
            var task = await GetTaskForUserAsync(taskId, userId);

            await _taskRepository.DeleteAsync(task);
        }

        public async Task<TaskResponseDto> GetByIdAsync(int taskId, int userId)
        {
            var task = await GetTaskForUserAsync(taskId, userId);

            return task.ToResponseDto();
        }

        public async Task<IEnumerable<TaskResponseDto>> GetByProjectAsync(int projectId, int userId)
        {
            var projectr =  await _projectRepository.GetDetailsByIdAsync(projectId);
            if (projectr is null)
            {
                throw new NotFoundException("Project not found");
            }
            EnsureMember(projectr, userId);

            var tasks = await _taskRepository.GetByProjectIdAsync(projectId);

            return tasks.Select(x => x.ToResponseDto());
        }

        public async Task<IEnumerable<TaskResponseDto>> GetMyTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetByAssignedUserIdAsync(userId);

            return tasks.Select(x => x.ToResponseDto());
        }

        public async Task<IEnumerable<TaskResponseDto>> GetOverdueTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetOverdueTasksAsync(userId);
            return tasks.Select(x => x.ToResponseDto());
        }

        public async Task<TaskResponseDto> UpdateAsync(int taskId, int userId, UpdateTaskDto dto)
        {
            var validatorResult = await _updateTaskValidator.ValidateAsync(dto);

            if (!validatorResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(Environment.NewLine,
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }
            var task = await GetTaskForUserAsync(taskId, userId);
            await EnsureAssignedUserAsync(task.ProjectId,dto.AssignedUserId);

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.Deadline = dto.Deadline;
            task.AssignedUserId = dto.AssignedUserId;

            await _taskRepository.UpdateAsync(task);

            return await GetTaskResponseAsync(task.Id);
        }

        public async Task<TaskResponseDto> UpdateStatusAsync(int taskId, int userId, UpdateTaskStatusDto dto)
        {
            var validatorResult = await _updateTaskStatusValidator.ValidateAsync(dto);
            if (!validatorResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(Environment.NewLine,
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }

            var task = await GetTaskForUserAsync(taskId, userId);

            task.Status = dto.Status;
            await _taskRepository.UpdateAsync(task);

            return await GetTaskResponseAsync(task.Id);
        }



        //PRIVATE
        private void EnsureMember(Project project, int userId)
        {
            var member = project.Members
                .FirstOrDefault(x => x.UserId == userId);

            if (member is null)
            {
                throw new BadRequestException( "You don't have access to this project.");
            }


        }
        private async Task<TaskItem> GetTaskForUserAsync(int taskId,int userId)
        {
            var task = await _taskRepository
                             .GetDetailsByIdAsync(taskId);

            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            EnsureMember(task.Project, userId);

            return task;
        }
        private async Task<TaskResponseDto> GetTaskResponseAsync(int taskId)
        {
            var task = await _taskRepository
                .GetDetailsByIdAsync(taskId);

            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            return task.ToResponseDto();
        }
        private async Task EnsureAssignedUserAsync(int projectId,int? assignedUserId)
        {
            if (!assignedUserId.HasValue)
            {
                return;
            }

            var assignedUser = await _userRepository
                .GetByIdAsync(assignedUserId.Value);

            if (assignedUser is null)
            {
                throw new NotFoundException( "Assigned user not found");
            }

            var isProjectMember =
                await _projectMemberRepository
                    .IsProjectMemberAsync(projectId,assignedUserId.Value);

            if (!isProjectMember)
            {
                throw new BadRequestException( "Assigned user is not project member");
            }
        }
    }
}
