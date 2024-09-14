using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSync.Api.Application.Examples.UseCases.GetExampleById;

public class GetExampleByIdQuery : IRequest<GetExampleByIdResult>
{
    public long Id { get; set; }
}
