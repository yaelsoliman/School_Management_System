using Common.Request.Courses;
using FluentValidation;

namespace Common.Validators.Courses
{
    public class AddCourseRequestValidator : AbstractValidator<AddCourseRequest>
    {
        public AddCourseRequestValidator()
        {
            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(40).WithMessage("Course name must not exceed 40 characters");

            RuleFor(x => x.CourseDescription)
                .NotEmpty().WithMessage("Course Description is required")
                .MaximumLength(256).WithMessage("Course name must not exceed 256 characters");


            RuleFor(x => x.StartDate)
                       .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

            RuleFor(x => x.EndDate)
                .Must(endDate => endDate.ToDateTime(new TimeOnly(0, 0)) > DateTime.Now)
                .WithMessage("End date must be in the future");

                 RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("End date must be after start date");
        }
    }

}
