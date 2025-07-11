using FitTrack.Data.Contract.Helpers.Requests;
using FluentValidation;

namespace FitTrack.Service.Business.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required!")
            .Length(5, 30).WithMessage("Username must be between 5 and 30 characters!")
            .Matches("^[a-zA-Z0-9._]+$").WithMessage("Username can contain only letters, numbers, dots, and underscores!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required!")
            .MaximumLength(50).WithMessage("Email cannot be longer than 50 characters!")
            .EmailAddress().WithMessage("Invalid email format!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required!")
            .Length(8, 50).WithMessage("Password must be between 5 adn 30 characters!")
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[-+_!@#$%^&*.,?]).+$")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character!");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match!");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required!")
            .Length(2, 100).WithMessage("First name must be between 2 and 100 characters!");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required!")
            .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters!");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required!");

        RuleFor(x => x.WeightKg)
            .GreaterThan(0).WithMessage("Weight must be a positive number!");

        RuleFor(x => x.HeightCm)
            .GreaterThan(0).WithMessage("Height must be a positive number!");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender must be a valid value!");

        RuleFor(x => x.UnitSystem)
            .IsInEnum().WithMessage("Unit system must be a valid value!");

        RuleFor(x => x.AppTheme)
            .IsInEnum().WithMessage("Theme must be a valid value!");
    }
}
