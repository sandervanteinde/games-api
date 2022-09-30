#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameApis.WebHost/GameApis.WebHost.csproj", "GameApis.WebHost/"]
COPY ["GameApis.MongoDb/GameApis.MongoDb.csproj", "GameApis.MongoDb/"]
COPY ["GameApis.Shared/GameApis.Shared.csproj", "GameApis.Shared/"]
COPY ["GameApis.TicTacToe.GameState/GameApis.TicTacToe.GameState.csproj", "GameApis.TicTacToe.GameState/"]
RUN dotnet restore "GameApis.WebHost/GameApis.WebHost.csproj"
COPY . .
WORKDIR "/src/GameApis.WebHost"
RUN dotnet build "GameApis.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameApis.WebHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameApis.WebHost.dll"]