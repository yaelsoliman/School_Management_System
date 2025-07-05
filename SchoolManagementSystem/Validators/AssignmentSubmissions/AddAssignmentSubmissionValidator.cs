using Common.Request.AssignmentSubmissions;
using FluentValidation;

namespace SchoolManagementSystem.Validators.AssignmentSubmissions
{
    public class AddAssignmentSubmissionValidator : AbstractValidator<AddAssignmentSubmissionRequest>
    {
        public AddAssignmentSubmissionValidator()
        {
            RuleFor(x => x.AssignmentId)
                .NotNull().WithMessage("assignmentId cannot be null");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("please add note")
                .MaximumLength(256);


        }
    }
}
