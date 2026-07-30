using FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Tasks;

namespace TaskFlow.Application.Validators.Tasks
{
    public class UpdateTaskStatusValidator
        : AbstractValidator<UpdateTaskStatusDto>
    {
        public UpdateTaskStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid task status.");
        }
    }
}
