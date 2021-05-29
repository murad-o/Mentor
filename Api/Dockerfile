FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["MentorCore/MentorCore.csproj", "MentorCore/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
