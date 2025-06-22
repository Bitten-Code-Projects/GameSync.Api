using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using GameSync.Application.Shared.Commands;
using MediatR;

namespace GameSync.Application.Account.ConfirmEmails;

/// <summary>
/// Command record used to handle user's email confirmation
/// </summary>
/// <param name="UserId">Id of a user that is being confirmed.</param>
/// <param name="Code">Email confirmation code.</param>
/// <param name="Claims">User claims from authorization token.</param>
[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:ParameterNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed")]
public sealed record ConfirmEmailCommand(
Guid UserId,
string Code,
ClaimsPrincipal Claims)
    : IRequest<CommandResult>;