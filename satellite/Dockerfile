#########################################################################################
##### Be sure to run this dockerfile from the starry_skies directory (one level up) #####
#########################################################################################

# build command:
#	docker build -t satellite -f Satellite/Dockerfile .
# run command:
#   docker run -d --name satellite001 --network starry-skies satellite

# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY Satellite/Satellite.csproj Satellite/
COPY Util/Util.csproj Util/

# Restore as distinct layers
RUN dotnet restore Satellite/Satellite.csproj

# copy everything else
COPY Satellite/ Satellite/
COPY Util/ Util/

# publish
RUN dotnet publish Satellite/Satellite.csproj -c Release -o /app

# Use runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Satellite.dll"]