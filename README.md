# Setup

#### Secrets
The application uses `user-secrets` in `secrets.json` for storing secret information.

For more information visit

https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows

You have to add connection string for `postgres`. To do that you need to add secrets into `Api` project via the `dotnet` CLI tool
```bash
dotnet user-secrets set "ConnectionStrings:dbConnection" "<Your connection string>"
```

For sending mails set SMTP and Email configurations into the same project
```
dotnet user-secrets set "SmtpConfiguration:Server" "<Server>"
dotnet user-secrets set "SmtpConfiguration:Port" "<Port>"

dotnet user-secrets set "EmailConfiguration:UserName" "<Your UserName>"
dotnet user-secrets set "EmailConfiguration:Password" "<Your Password>"
```

To generate accessToken for login you have to set data for json web token
```
dotnet user-secrets set "JwtConfiguration:ValidIssuer" "<ValidIssuer>"
dotnet user-secrets set "JwtConfiguration:ValidAudience" "<ValidAudience>"
dotnet user-secrets set "JwtConfiguration:SecretKey" "<Your SecretKey>"
dotnet user-secrets set "JwtConfiguration:LifeTime" "<LifeTime of token>"
```

# Run
After that apply migrations and run application
