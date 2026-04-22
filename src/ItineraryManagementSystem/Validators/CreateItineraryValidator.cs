using FluentValidation;
using ItineraryManagementSystem.DTOs;

namespace ItineraryManagementSystem.Validators
{
    /// <summary>
    /// Validate the itinerary attributes.
    /// </summary>
    public class CreateItineraryValidator : AbstractValidator<CreateItineraryDto>
    {
        /// <summary>
        /// Validates the itinerary attributes before creating.
        /// </summary>
        public CreateItineraryValidator()
        {
            RuleFor(item => item.Destination)
                .NotEmpty().WithMessage("Destination is required")
                .MaximumLength(100);

            RuleFor(item => item.TravelDate)
                .GreaterThan(DateTime.Now).WithMessage("Travel date must be in future");

            RuleFor(item => item.DurationDays)
                .InclusiveBetween(1, 30);
        }
    }
}
