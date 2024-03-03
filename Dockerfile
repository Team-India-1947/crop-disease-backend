# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY crop_disease_app/*.csproj ./crop_disease_app/
RUN dotnet restore

COPY crop_disease_app/. ./crop_disease_app/
WORKDIR /source/crop_disease_app
RUN dotnet publish -c release -o /app

# Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
COPY certificate.pfx .

ENTRYPOINT ["dotnet", "crop_disease_app.dll"]