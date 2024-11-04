namespace GameSync.Application.Examples.UseCases.CreateExample;

using FluentValidation;

public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Name)
            .Must(x => x.Length <= 64)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Name has maximum 64 characters.");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname is required");

        RuleFor(x => x.Surname)
            .Must(x => x.Length <= 64)
            .When(x => !string.IsNullOrEmpty(x.Surname))
            .WithMessage("Surname has maximum 64 characters.");

        RuleFor(x => x.HouseNumber)
            .NotEmpty()
            .WithMessage("House number is required.");

        RuleFor(x => x.HouseNumber)
            .Must(x => x.Length <= 64)
            .When(x => !string.IsNullOrEmpty(x.HouseNumber))
            .WithMessage("House number has maximum 64 characters.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(x => x.Street)
            .Must(x => x.Length <= 128)
            .When(x => !string.IsNullOrEmpty(x.Street))
            .WithMessage("Street has maximum 128 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(x => x.City)
            .Must(x => x.Length <= 128)
            .When(x => !string.IsNullOrEmpty(x.City))
            .WithMessage("City has maximum 128 characters.");
    }
}
