# GameSync.Api

API for GameSync

## Environment Variables

The following environment variables are required for email service configuration:

| Variable Name                  | Description                                              |
| ------------------------------ | -------------------------------------------------------- |
| BCP_GAMESYNC_EMAIL_PASSWORD    | Password for the email service authentication            |
| BCP_GAMESYNC_EMAIL_AUTH_LOGIN  | Login/username for email service authentication          |
| BCP_GAMESYNC_EMAIL_SENDER_MAIL | Email address used as the sender for all outgoing emails |

### Setting up Environment Variables

On Windows, you can set these variables using the following commands in PowerShell:

```powershell
[System.Environment]::SetEnvironmentVariable('BCP_GAMESYNC_EMAIL_PASSWORD', 'your-password', [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BCP_GAMESYNC_EMAIL_AUTH_LOGIN', 'your-login', [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BCP_GAMESYNC_EMAIL_SENDER_MAIL', 'sender@example.com', [System.EnvironmentVariableTarget]::User)
```

**Note**: After setting environment variables, you may need to restart your IDE or terminal for the changes to take effect.
