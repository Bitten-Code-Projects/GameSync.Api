namespace GameSync.Domain.Shared.Exceptions;

/// <summary>
/// Exception thrown when no entity found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception's message.</param>
    public NotFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception's message.</param>
    /// <param name="innerException">Exception's inner exception.</param>
    public NotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
