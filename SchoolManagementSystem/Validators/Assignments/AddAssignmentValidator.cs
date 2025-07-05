using Common.Request.Assignments;
using FluentValidation;

namespace SchoolManagementSystem.Validators.Assignments
{
    public class AddAssignmentValidator : AbstractValidator<AddAssignmentRequest>
    {
        public AddAssignmentValidator()
        {
            RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Course name is required")
                    .MaximumLength(100).WithMessage("Course name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Course Description is required")
                .MaximumLength(256).WithMessage("Course name must not exceed 100 characters");


            RuleFor(x => x.DueDate)
                       .GreaterThan(x=>DateTime.Now);

            RuleFor(x => x.CourseId)
                .NotNull().NotEmpty();

        }
    }
}
