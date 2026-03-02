using System.Linq;
using FluentValidation;
using Order.Model.DTO;

namespace Order.WebAPI.Validators;

public class StatusValidator : AbstractValidator<StatusFilter>
{
    private readonly string[] _allowedStatuses =
    [
        "In Progress",
        "Created",
        "Shipped",
        "Failed"
    ];
    
    public StatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("NewStatus cannot be empty")
            .Must(status => _allowedStatuses.Contains(status))
            .WithMessage($"NewStatus must be one of the following: {string.Join(", ", _allowedStatuses)}");
    }
}