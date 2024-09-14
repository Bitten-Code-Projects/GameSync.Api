using FluentValidation;

namespace GameSync.Api.Application.Examples.UseCases.GetExampleById;

public class GetExampleByIdValidator : AbstractValidator<GetExampleByIdQuery>
{
    public GetExampleByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.Id)
            .Must(x => x > 0)
            .WithMessage("Id must be positive.");
    }
}
