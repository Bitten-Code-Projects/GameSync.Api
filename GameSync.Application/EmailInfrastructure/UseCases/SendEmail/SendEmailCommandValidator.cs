namespace GameSync.Application.Examples.UseCases.CreateExample;

using FluentValidation;
using GameSync.Application.EmailInfrastructure.UseCases;

/// <summary>
/// Validates the properties of the <see cref="SendEmailCommand"/> to ensure they meet the required rules.
/// This validator is used to enforce constraints on the sender, receiver, subject, and body of an email.
/// </summary>
public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendEmailCommandValidator"/> class.
    /// Sets up validation rules for the <see cref="SendEmailCommand"/> properties.
    /// </summary>
    public SendEmailCommandValidator()
    {
        RuleFor(x => x.Sender)
            .NotEmpty()
            .WithMessage("Sender is required.");
        RuleFor(x => x.Receiver)
            .NotEmpty()
            .WithMessage("Receiver is required.");
        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required.");
        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage("Body is required.");
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty()
            .WithMessage("ReceiverEmail is required.");
    }
}