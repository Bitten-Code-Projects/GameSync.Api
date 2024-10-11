namespace GameSync.Domain.Shared.Commands;

/// <summary>
/// Shared command result object.
/// </summary>
public sealed class CommandResult
{
    private CommandResult()
    {
    }

    private CommandResult(string failureReason)
    {
        FailureReason = failureReason;
    }

    private CommandResult(object data)
    {
        Data = data;
    }

    /// <summary>
    /// Gets succeess result.
    /// </summary>
    public static CommandResult Success { get; } = new CommandResult();

    /// <summary>
    /// Gets or sets command result's failure reason.
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Gets a value indicating whether result is success.
    /// </summary>
    public bool IsSuccess => string.IsNullOrEmpty(FailureReason);

    /// <summary>
    /// Gets or sets command result data.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Returns successful command result with data.
    /// </summary>
    /// <param name="data">Command result data.</param>
    /// <returns>Successful command result with data.</returns>
    public static CommandResult SuccessWithData(object data)
    {
        return new CommandResult(data);
    }

    /// <summary>
    /// Returns failed command result with reason.
    /// </summary>
    /// <param name="reason">Failure reason.</param>
    /// <returns>Failed command result with reason.</returns>
    public static CommandResult Fail(string reason)
    {
        return new CommandResult(reason);
    }
}
