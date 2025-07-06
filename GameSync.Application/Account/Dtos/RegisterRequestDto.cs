namespace GameSync.Application.Account.Dtos;

/// <summary>
/// Represents the data required to register a new user.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Gets or sets the desired login name of the user.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Gets or sets the password for the new user account.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public required string Email { get; set; }
}
