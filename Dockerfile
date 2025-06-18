# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY *.sln .
COPY FitHub_Platform.Common/*.csproj ./FitHub_Platform.Common/
COPY FitHub_Platform.Common.Domain/*.csproj ./FitHub_Platform.Common.Domain/
COPY FitHub_Platform.Common.Repository/*.csproj ./FitHub_Platform.Common.Repository/
COPY FitHub_Platform.Common.Service/*.csproj ./FitHub_Platform.Common.Service/
COPY FitHub_Platform.Workout.API/*.csproj ./FitHub_Platform.Workout.API/
COPY FitHub_Platform.Workout.Domain/*.csproj ./FitHub_Platform.Workout.Domain/
COPY FitHub_Platform.Workout.Repository/*.csproj ./FitHub_Platform.Workout.Repository/
COPY FitHub_Platform.Workout.Service/*.csproj ./FitHub_Platform.Workout.Service/
RUN dotnet restore

# Copy the remaining files and build the app
COPY . .
RUN dotnet publish FitHub_Platform.Workout.API/FitHub_Platform.Workout.API.csproj -c Release -o out

# Use the runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FitHub_Platform.Workout.API.dll" ]