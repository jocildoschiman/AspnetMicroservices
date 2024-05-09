using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {

            //Validação para o usuário que faz a compra:
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            //Validação para o e-mail do usuário:
            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required");

            //Validação para o total geral do pedido:
            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");

        }
    }
}
