using System.Linq;
using FluentValidation;
using Order.Model.DTO;

namespace Order.WebAPI.Validators;

public class StatusValidator : AbstractValidator<StatusFilter>
{
    public StatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("NewStatus cannot be empty")
            .IsInEnum().WithMessage("NewStatus must be a valid status");
    }
}