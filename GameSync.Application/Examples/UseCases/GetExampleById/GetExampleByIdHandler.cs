namespace GameSync.Application.Examples.UseCases.GetExampleById;

using AutoMapper;
using EnsureThat;
using FluentValidation;
using GameSync.Domain.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetExampleByIdHandler : IRequestHandler<GetExampleByIdQuery, GetExampleByIdResult>
{
    private readonly IValidator<GetExampleByIdQuery> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<GetExampleByIdHandler> _logger; // no needed here by now, just an example

    public GetExampleByIdHandler(IValidator<GetExampleByIdQuery> validator, IMapper mapper, ILogger<GetExampleByIdHandler> logger)
    {
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetExampleByIdResult> Handle(GetExampleByIdQuery request, CancellationToken cancellationToken)
    {
        Ensure.That(request).IsNotNull();

        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        _logger.LogInformation("Example with id {Id} was requested", request.Id);

        return new GetExampleByIdResult()
        {
            Id = 1,
            Name = "asd",
            Surname = "sss",
            Address = new Domain.Examples.ValueObjects.ExampleAddress()
            {
                Street = "asdss",
                City = "sss",
                HouseNumber = "sss",
            },
        };
        //var result = await _exampleRepository.GetExampleById(request.Id, cancellationToken);
        //if (result is null)
        //{
        //    throw new NotFoundException($"Example with id {request.Id} was not found");
        //}

        //return _mapper.Map<GetExampleByIdResult>(result);
    }
}
