using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Tasks;

namespace TaskFlow.Application.Validators.Tasks
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}
