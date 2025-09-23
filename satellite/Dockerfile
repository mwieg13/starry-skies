# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY Consumer/*.csproj ./Consumer/
RUN dotnet restore Consumer/Consumer.csproj

# Copy the rest of the code and build
COPY . .
RUN dotnet publish Consumer/Consumer.csproj -c Release -o /app

# Use runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Consumer.dll"]