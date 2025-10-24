using FitTrack.Data.Contract.Helpers.Requests;
using FluentValidation;

namespace FitTrack.Service.Business.Validators;

public class GetFilteredUsersRequestValidator : AbstractValidator<GetFilteredUsersRequest>
{
    public GetFilteredUsersRequestValidator()
    {
        RuleFor(x => x.Page)
            .NotEmpty().WithMessage("Page is required!")
            .GreaterThan(0).WithMessage("Page must be greater than 0!");

        RuleFor(x => x.PageSize)
            .NotEmpty().WithMessage("Page size is required!")
            .GreaterThan(0).WithMessage("Page size must be greater than 0!")
            .LessThan(100).WithMessage("Page size must be less than 100!");


        RuleFor(x => x.Search)
            .Length(0, 100).WithMessage("Search must be between 0 and 100 characters!");

    }
}
