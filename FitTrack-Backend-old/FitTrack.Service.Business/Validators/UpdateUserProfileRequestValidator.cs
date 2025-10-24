using FitTrack.Data.Contract.Helpers.Requests;
using FluentValidation;

namespace FitTrack.Service.Business.Validators;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required!")
            .Length(2, 100).WithMessage("First name must be between 2 and 100 characters!");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required!")
            .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters!");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required!");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be a positive number!");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be a positive number!");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender must be a valid value!");
    }
}
