using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Projects;

namespace TaskFlow.Application.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters")
                .MaximumLength(250).WithMessage("Project name must not exceed 250 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Descriptiom must not exceed 1000 characters");
                
        }
    }
}
