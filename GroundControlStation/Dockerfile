# run command:
# docker build -t gcs -f GroundControlStation/Dockerfile .


# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#########################################################################################
##### Be sure to run this dockerfile from the starry_skies directory (one level up) #####
#########################################################################################

# Copy everything
COPY GroundControlStation/GroundControlStation.csproj GroundControlStation/
COPY Util/Util.csproj Util/

# Restore as distinct layers
RUN dotnet restore GroundControlStation/GroundControlStation.csproj

# copy everything else
COPY GroundControlStation/ GroundControlStation/
COPY Util/ Util/

# publish
RUN dotnet publish GroundControlStation/GroundControlStation.csproj -c Release -o /app

# Build and publish a release
#RUN dotnet publish -o out

# expose port for Kafka
EXPOSE 9092

# Use runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GroundControlStation.dll"]