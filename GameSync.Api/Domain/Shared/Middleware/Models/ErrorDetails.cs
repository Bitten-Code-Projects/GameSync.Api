using System.Text.Json;

namespace GameSync.Api.Domain.Shared.Middleware.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> ValidationErrors { get; set; }
    public ErrorDetails()
    {
        ValidationErrors = new List<string>();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public bool ShouldSerializeValidationErrors()
    {
        return ValidationErrors.Any();
    }
}
