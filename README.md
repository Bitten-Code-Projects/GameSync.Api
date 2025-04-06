# GameSync.Api

API for GameSync

## Environment Variables

The following environment variables are required for email service configuration:

| Variable Name                  | Description                                              |
| ------------------------------ | -------------------------------------------------------- |
| BCP_GS_EMAIL_PASS              | Password for the email service authentication            |
| BCP_GS_EMAIL_USER              | Login/username for email service authentication          |
| BCP_GS_SENDER                  | Email address used as the sender for all outgoing emails |

### Setting up Environment Variables

On Windows, you can set these variables using the following commands in PowerShell:

```powershell
[System.Environment]::SetEnvironmentVariable('BCP_GS_EMAIL_PASS', 'your-password', [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BCP_GS_EMAIL_USER', 'your-login', [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BCP_GS_SENDER', 'sender@example.com', [System.EnvironmentVariableTarget]::User)
```

**Note**: After setting environment variables, you may need to restart your IDE or terminal for the changes to take effect.

## Configuration in `appsettings.json`

The following table describes the email settings that can be configured in the `appsettings.json` file:

| Setting          | Description                                         | Example Value          |
| ---------------- | --------------------------------------------------- | ---------------------- |
| `SmtpServer`     | Address of the SMTP server used for sending emails. | `mail.ugu.pl`          |
| `SmtpPort`       | Port of the SMTP server (typically 587 for TLS).    | `587`                  |
| `SenderEmail`    | Email address used as the sender of messages.       | `example@domain.com`   |
| `AuthLogin`      | Login for authentication on the SMTP server.        | `example-login`        |
| `Password`       | Password for authentication on the SMTP server.     | `example-password`     |

### Example `appsettings.json` Configuration

Below is an example configuration:

"EmailSettings": {
  "SmtpServer": "mail.ugu.pl",
  "SmtpPort": 587,
  "SenderEmail": "example@domain.com",
  "AuthLogin": "example-login",
  "Password": "example-password"
}