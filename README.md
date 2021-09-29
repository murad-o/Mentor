# Setup
You have to add connection string for `postgres`. You can add into `Api` project, or into environment variable if you are using `Docker` 
```bash
"ConnectionStrings:dbConnection" "<Your connection string>"
```

For sending mails set SMTP and Email configurations
```
"SmtpConfiguration:Server" "<Server>"
"SmtpConfiguration:Port" "<Port>"

"EmailConfiguration:UserName" "<Your UserName>"
"EmailConfiguration:Password" "<Your Password>"
```

To generate accessToken for login you have to set data for json web token
```
"JwtConfiguration:ValidIssuer" "<ValidIssuer>"
"JwtConfiguration:ValidAudience" "<ValidAudience>"
"JwtConfiguration:SecretKey" "<Your SecretKey>"
"JwtConfiguration:LifeTime" "<LifeTime of token>"
```

# Run
After that apply migrations and run application
