using Common.Request.GradeAssignmentSubmissions;
using FluentValidation;

namespace SchoolManagementSystem.Validators.GradeAssignmentSubmissions
{
    public class GradeAssignmentSubmissionsValidator : AbstractValidator<AddGradeAssignmentSubmissionRequest>
    {
        public GradeAssignmentSubmissionsValidator()
        {
            RuleFor(x => x.AssignmentSubmissionId)
            .NotEmpty().NotNull().WithMessage("UserId is required.");

            RuleFor(x => x.Score)
                .NotNull().GreaterThanOrEqualTo(0);

            RuleForEach(x => x.Feedback)
                .NotEmpty();
        }
    }
}
