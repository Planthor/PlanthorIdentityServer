FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./src/Planthor.IdentityServerAspNetIdentity/*.csproj ./src/Planthor.IdentityServerAspNetIdentity/
RUN dotnet restore

# copy everything else and run release app
COPY ./src/Planthor.IdentityServerAspNetIdentity/. /app/src/Planthor.IdentityServerAspNetIdentity/
WORKDIR /app/src/Planthor.IdentityServerAspNetIdentity
RUN dotnet publish -c Release -o out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
LABEL authors="Planthor team"
WORKDIR /app
EXPOSE 5001
COPY --from=build /app/src/Planthor.IdentityServerAspNetIdentity/out ./
RUN echo 'dotnet Planthor.IdentityServerAspNetIdentity.dll /seed' > run_seed.sh && \
    chmod +x run_seed.sh
ENTRYPOINT ["dotnet", "Planthor.IdentityServerAspNetIdentity.dll"]
