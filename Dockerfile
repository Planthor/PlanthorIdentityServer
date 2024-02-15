FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./src/Planthor.IdentityServerAspNetIdentity/*.csproj ./src/Planthor.IdentityServerAspNetIdentity/
RUN dotnet restore

# copy everything else and build app
COPY ./src/Planthor.IdentityServerAspNetIdentity/. /app/Planthor.IdentityServerAspNetIdentity/
WORKDIR /app/Planthor.IdentityServerAspNetIdentity
RUN dotnet publish -c Release -o out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
LABEL authors="Planthor team "
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/Planthor.IdentityServerAspNetIdentity/out ./
ENTRYPOINT ["dotnet", "Planthor.IdentityServerAspNetIdentity.dll"]