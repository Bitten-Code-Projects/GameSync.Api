using Microsoft.AspNetCore.Identity;

namespace GameSync.Infrastructure.Context.Models;

/// <summary>
/// Represents an application user, extending the default <see cref="IdentityUser"/> class
/// by adding a property to track the last successful login IP address.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the IP address of the user's last successful login.
    /// </summary>
    public string? LastIP { get; set; }
}
