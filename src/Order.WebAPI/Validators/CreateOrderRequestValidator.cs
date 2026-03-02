using FluentValidation;
using Order.Model.DTO;

namespace Order.WebAPI.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.ResellerId)
            .NotEmpty().WithMessage("ResellerId is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemRequestValidator());
    }
}