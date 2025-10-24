using FluentValidation;
using FitTrack.Data.Contract.Helpers.Requests;

namespace FitTrack.Service.Business.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Credential)
            .NotEmpty().WithMessage("Credential (username or email) is required!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required!");
    }
}
