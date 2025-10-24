using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FitTrack.Service.Business.Validators;

public class ValidationActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationActionFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var firstArg = context.ActionArguments.Values.FirstOrDefault();
        if (firstArg == null)
        {
            await next();
            return;
        }

        var validatorType = typeof(IValidator<>).MakeGenericType(firstArg.GetType());
        var validator = _serviceProvider.GetService(validatorType) as IValidator;

        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(new FluentValidation.ValidationContext<object>(firstArg));

            if (!validationResult.IsValid)
            {
                var firstError = validationResult.Errors.First();

                throw new ValidationException(firstError.ErrorMessage);
            }
        }

        await next();
    }
}
