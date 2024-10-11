using System.Text.Json;

namespace GameSync.Api.Shared.Middleware.Models;

/// <summary>
/// Error details object.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDetails"/> class.
    /// </summary>
    public ErrorDetails()
    {
        ValidationErrors = new List<string>();
    }

    /// <summary>
    /// Gets or sets status code.
    /// </summary>
    public int StatusCode { get; set; } = 0;

    /// <summary>
    /// Gets or sets message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets validation errors.
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new List<string>();

    /// <summary>
    /// To string method.
    /// </summary>
    /// <returns>JSON serialized method.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Determines if validation errors collection should be serialized.
    /// </summary>
    /// <returns>Boolean result.</returns>
    public bool ShouldSerializeValidationErrors()
    {
        return ValidationErrors.Any();
    }
}
