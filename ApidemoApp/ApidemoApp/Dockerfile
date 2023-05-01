# docker build -f "C:\dev\gitrepos\fluent-bit-observability-demo\ApidemoApp\ApidemoApp\Dockerfile" -t apidemoapp:latest "C:\dev\gitrepos\fluent-bit-observability-demo\ApidemoApp" 
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApidemoApp/ApidemoApp.csproj", "ApidemoApp/"]
RUN dotnet restore "ApidemoApp/ApidemoApp.csproj"
COPY . .
WORKDIR "/src/ApidemoApp"
RUN dotnet build "ApidemoApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApidemoApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApidemoApp.dll"]