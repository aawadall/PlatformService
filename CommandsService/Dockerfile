# 2 Stage Build 
# Using Dotnet 7 SDK Build base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy project files and restore as distinct layers
COPY *.csproj ./

# Restore Nuget Packages
RUN dotnet restore

# Copy Source Code
COPY . ./

# build
RUN dotnet publish -c Release -o out

# Using Dotnet 7 Runtime base
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy artifacts
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "CommandsService.dll"]