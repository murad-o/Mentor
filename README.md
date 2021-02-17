# Setup

#### Secrets
The application uses `user-secrets` in `secrets.json` for storing secret information.

For more information visit

https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows

You have to add connection string for `postgres`. To do that you need to add secrets for `WebApi` via the `dotnet` CLI tool
```bash
dotnet user-secrets set "ConnectionStrings:dbConnection" "<Your connection string>"
```

# Run
After that apply migrations and run application
