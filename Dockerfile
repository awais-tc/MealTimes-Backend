# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first and restore (better layer caching)
COPY MealTimes.Controller/MealTimes.Controller.csproj MealTimes.Controller/
COPY MealTimes.Service/MealTimes.Service.csproj       MealTimes.Service/
COPY MealTimes.Repository/MealTimes.Repository.csproj MealTimes.Repository/
COPY MealTimes.Core/MealTimes.Core.csproj             MealTimes.Core/
RUN dotnet restore MealTimes.Controller/MealTimes.Controller.csproj

# Copy the rest of the source and publish
COPY . .
RUN dotnet publish MealTimes.Controller/MealTimes.Controller.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render provides the port via the PORT env var; the app reads it in Program.cs.
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "MealTimes.Controller.dll"]
