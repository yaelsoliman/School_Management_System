using Common.Request.Enrollments;
using FluentValidation;

namespace SchoolManagementSystem.Validators.Enrollments
{
    public class AddEnrollmentRequestValidator : AbstractValidator<AddEnrollmentRequest>
    {
        public AddEnrollmentRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.CourseIds)
                .NotNull().WithMessage("CourseIds list cannot be null.")
                .NotEmpty().WithMessage("At least one CourseId must be provided.");

            RuleForEach(x => x.CourseIds)
                .NotEmpty().WithMessage("CourseId cannot be empty GUID.");

            RuleFor(x => x.EnrolledDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("EnrolledDate cannot be in the future.");
        }
    }
}
