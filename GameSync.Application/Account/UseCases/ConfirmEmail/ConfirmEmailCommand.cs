using GameSync.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Application.Account.UseCases.ConfirmEmail;

#pragma warning disable SA1313 // ParameterNamesMustBeginWithLowerCaseLetter

/// <summary>
/// Command class for confirming user's email address.
/// </summary>
/// <param name="UserId">User's id.</param>
/// <param name="Code">Code for confirmation.</param>
public sealed record ConfirmEmailCommand(
    string UserId,
    string Code)
: IRequest<CommandResult>;

#pragma warning restore SA1313 // ParameterNamesMustBeginWithLowerCaseLetter
