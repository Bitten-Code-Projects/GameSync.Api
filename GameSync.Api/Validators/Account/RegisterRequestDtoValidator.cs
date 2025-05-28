using FluentValidation;
using GameSync.Application.Account.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GameSync.Api.Validators.Account;

/// <summary>
/// Validator for <see cref="RegisterRequestDto"/> that enforces validation rules
/// based on ASP.NET Core Identity password options and additional constraints for login and email.
/// </summary>
public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterRequestDtoValidator"/> class.
    /// Validation rules for password are dynamically applied based on <see cref="IdentityOptions"/>.
    /// </summary>
    /// <param name="identityOptions">Identity options to configure password requirements.</param>
    public RegisterRequestDtoValidator(IOptions<IdentityOptions> identityOptions)
    {
        var passwordOptions = identityOptions.Value.Password;

        // Password must not be empty and must meet minimum length requirement
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(passwordOptions.RequiredLength);

        // Password must contain at least one digit if required
        if (passwordOptions.RequireDigit)
        {
            RuleFor(x => x.Password)
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit.");
        }

        // Password must contain at least one uppercase letter if required
        if (passwordOptions.RequireUppercase)
        {
            RuleFor(x => x.Password)
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.");
        }

        // Password must contain at least one lowercase letter if required
        if (passwordOptions.RequireLowercase)
        {
            RuleFor(x => x.Password)
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.");
        }

        // Password must contain at least one non-alphanumeric character if required
        if (passwordOptions.RequireNonAlphanumeric)
        {
            RuleFor(x => x.Password)
                .Matches(@"[\W_]")
                .WithMessage("Password must contain at least one non-alphanumeric character.");
        }

        // Login must not be empty and length must be between 4 and 32 characters
        RuleFor(x => x.Login)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(32);

        // Email must not be empty
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");

        // Email must be a valid email address format
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format.");

        // Email length must not exceed 254 characters
        RuleFor(x => x.Email)
            .MaximumLength(254)
            .WithMessage("Email is too long.");
    }
}
