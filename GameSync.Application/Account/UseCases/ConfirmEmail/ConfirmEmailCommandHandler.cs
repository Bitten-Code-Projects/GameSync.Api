using GameSync.Application.Account.Interfaces;
using GameSync.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Application.Account.UseCases.ConfirmEmail;

/// <summary>
/// Handles the user email confirmation command by delegating the operation to the identity service.
/// </summary>
public sealed class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, CommandResult>
{
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandHandler"/> class.
    /// </summary>
    /// <param name="identityService">
    /// The <see cref="IIdentityService"/> used to perform user email confirmation logic.
    /// </param>
    public ConfirmEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the <see cref="ConfirmEmailCommand"/> and triggers the email confirmation process.
    /// </summary>
    /// <param name="request">The <see cref="ConfirmEmailCommand"/> containing the user ID and confirmation code.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{CommandResult}"/> representing the asynchronous operation.
    /// The result contains the outcome of the email confirmation process.</returns>
    public async Task<CommandResult> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.ConfirmEmailAsync(request.UserId, request.Code);
    }
}