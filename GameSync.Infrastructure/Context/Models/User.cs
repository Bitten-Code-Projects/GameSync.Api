namespace GameSync.Infrastructure.Context.Models;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// IdentityUser model extended by LastIP property.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the IP address from the last successful login.
    /// </summary>
    public virtual string? LastIP { get; set; }
}
