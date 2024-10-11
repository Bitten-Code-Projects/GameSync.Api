using EnsureThat;
using FluentValidation;
using GameSync.Domain.Shared.Commands;
using MediatR;
using GameSync.Domain.Examples.Entities;
using GameSync.Domain.Examples.ValueObjects;
using GameSync.Application.Examples.Interfaces;

namespace GameSync.Application.Examples.UseCases.CreateExample;

public class CreateExampleHandler : IRequestHandler<CreateExampleCommand, CommandResult>
{
    private readonly IValidator<CreateExampleCommand> _validator;
    private readonly IExampleRepository _exampleRepository;

    public CreateExampleHandler(IValidator<CreateExampleCommand> validator, IExampleRepository exampleRepository)
    {
        _validator = validator;
        _exampleRepository = exampleRepository;
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

        var result = await _exampleRepository.CreateExample(example, cancellationToken);

        return CommandResult.SuccessWithData(result.Id);
    }
}
