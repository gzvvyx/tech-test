using System.Linq;
using FluentValidation;
using Order.Model.DTO;

namespace Order.WebAPI.Validators;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty");

        RuleFor(x => x.Status)
            .SetValidator(new StatusValidator());
    }
}