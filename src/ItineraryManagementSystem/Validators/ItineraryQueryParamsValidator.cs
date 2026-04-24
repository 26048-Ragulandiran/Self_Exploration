using FluentValidation;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Validators
{
    public class ItineraryQueryParamsValidator : AbstractValidator<ItineraryQueryParams>
    {
        public ItineraryQueryParamsValidator()
        {
            RuleFor(item => item.TravelDate)
                .GreaterThan(DateTime.Now).WithMessage("Travel date must be in future");

            RuleFor(item => item.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be non-negative");

            RuleFor(item => item.PageSize)
                .GreaterThan(0).WithMessage("Page Size must be non-negative.");
        }
    }
}
