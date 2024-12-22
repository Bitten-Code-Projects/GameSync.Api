using EnsureThat;
using FluentValidation;
using GameSync.Domain.Examples.Entities;
using GameSync.Domain.Examples.ValueObjects;
using GameSync.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Application.Examples.UseCases.CreateExample;

public class CreateExampleHandler : IRequestHandler<CreateExampleCommand, CommandResult>
{
    private readonly IValidator<CreateExampleCommand> _validator;

    public CreateExampleHandler(IValidator<CreateExampleCommand> validator)
    {
        _validator = validator;
    }

    public async Task<CommandResult> Handle(CreateExampleCommand command, CancellationToken cancellationToken)
    {
        Ensure.That(command).IsNotNull();

        await _validator.ValidateAndThrowAsync(command);

        var example = new Example()
        {
            Id = 0,
            Name = command.Name,
            Surname = command.Surname,
            Address = new ExampleAddress()
            {
                Street = command.Street,
                City = command.City,
                HouseNumber = command.HouseNumber,
            },
        };

        return CommandResult.Success;
    }
}
