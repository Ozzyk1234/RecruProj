# Stage 1 - build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Skopiuj pliki .csproj i przywróć zależności
COPY ["RecruProj/RecruProj.csproj", "RecruProj/"]
COPY ["RecrtuProj.Tests/RecrtuProj.Tests.csproj", "RecrtuProj.Tests/"]
RUN dotnet restore "RecruProj/RecruProj.csproj"

# Skopiuj resztę kodu i zbuduj
COPY . .
RUN dotnet build "RecruProj/RecruProj.csproj" -c Release

# Uruchom testy
RUN dotnet test "RecrtuProj.Tests/RecrtuProj.Tests.csproj"

# Publikuj
RUN dotnet publish "RecruProj/RecruProj.csproj" -c Release -o /app/publish

# Stage 2 - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RecruProj.dll"]