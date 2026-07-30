using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Comments;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings.Comments;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IValidator<CreateCommentDto> _createValidator;
        private readonly IValidator<UpdateCommentDto> _updateValidator;
        public CommentService(
            ICommentRepository commentRepository,
            ITaskRepository taskRepository,
            IValidator<CreateCommentDto> createValidator,
            IValidator<UpdateCommentDto> updateValidator)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        public async Task<CommentResponseDto> CreateAsync(int taskId, int userId, CreateCommentDto dto)
        {
            var validatorResult = await _createValidator.ValidateAsync(dto);
            if( !validatorResult.IsValid)
            {
                throw new BadRequestException(string.Join(
                    Environment.NewLine,
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }

            var task = await _taskRepository.GetDetailsByIdAsync(taskId);
            if(task == null )
            {
                throw new NotFoundException("Task not found");
            }
            var isMember =  task.Project.Members.Any(x => x.UserId == userId);

            if (!isMember)
            {
                throw new BadRequestException("You dont have access to the project");
            }
            var comment = new Comment
            {
                Message = dto.Message,
                CreatedAt = DateTimeOffset.UtcNow,
                TaskItemId = taskId,
                UserId = userId,
            };
            await _commentRepository.AddAsync(comment);
            var result = await _commentRepository
                .GetDetailsByIdAsync(comment.Id);

            return result!.ToResponseDto();
        }

        public async Task DeleteAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment is null)
            {
                throw new NotFoundException("Comment not found.");
            }
            if(comment.UserId != userId)
            {
                throw new BadRequestException( "You can only delete your own comments.");
            }
            await _commentRepository.DeleteAsync(comment);
        }

        public async Task<IEnumerable<CommentResponseDto>> GetByTaskAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetDetailsByIdAsync(taskId);
            if (task is null)
            {
                throw new NotFoundException("Task not found.");
            }
            var isMember = task.Project.Members.Any(x => x.UserId ==userId);
            if (!isMember)
            {
                throw new BadRequestException(
                    "You dont have access to this project.");
            }
            var comments = await _commentRepository.GetByTaskIdAsync(taskId);
            return comments.Select(x => x.ToResponseDto());
        }

        public async Task<CommentResponseDto> UpdateAsync(int commentId, int userId, UpdateCommentDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException(
                    string.Join(
                        Environment.NewLine,
                        validationResult.Errors
                            .Select(x => x.ErrorMessage)));
            }

            var comment = await _commentRepository.GetDetailsByIdAsync(commentId);
            if (comment is null)
            {
                throw new NotFoundException("Comment not found.");
            }
            if(comment.UserId != userId)
            {
                throw new BadRequestException("You can only edit your own comments");
            }

            comment.Message = dto.Message;
            await _commentRepository.UpdateAsync(comment);

            var result =  await _commentRepository.GetDetailsByIdAsync(comment.Id);

            return result!.ToResponseDto();
        }
    }
}
