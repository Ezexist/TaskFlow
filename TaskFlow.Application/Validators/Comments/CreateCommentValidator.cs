using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Comments;

namespace TaskFlow.Application.Validators.Comments
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
