using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Comments;

namespace TaskFlow.Application.Validators.Comments
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
