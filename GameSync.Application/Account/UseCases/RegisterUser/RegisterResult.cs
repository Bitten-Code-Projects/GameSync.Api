namespace GameSync.Application.Account.UseCases.RegisterUser
{
    /// <summary>
    /// Represents the result of a user registration attempt.
    /// </summary>
    public class RegisterResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registration was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets an array of error messages describing why the registration failed, if applicable.
        /// </summary>
        public string[] Errors { get; set; } = [];
    }
}
