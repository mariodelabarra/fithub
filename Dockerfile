# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY *.sln .
COPY src/ ./src/
COPY test/ ./test/
RUN dotnet restore FitHub.Platform.sln

# Copy the remaining files and build the app
COPY src/ .
RUN dotnet publish src/FitHub.Platform.Workout.API/FitHub.Platform.Workout.API.csproj -c Release -o out

# Use the runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FitHub.Platform.Workout.API.dll" ]