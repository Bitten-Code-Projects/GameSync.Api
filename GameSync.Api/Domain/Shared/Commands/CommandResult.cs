namespace GameSync.Api.Domain.Shared.Commands;

public class CommandResult
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

    public static CommandResult Success { get; } = new CommandResult();
    public string? FailureReason { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(FailureReason);
    public object? Data { get; set; }

    public static CommandResult SuccessWithData(object data)
    {
        return new CommandResult(data);
    }

    public static CommandResult Fail(string reason)
    {
        return new CommandResult(reason);
    }
}
