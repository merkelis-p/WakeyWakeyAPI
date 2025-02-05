﻿FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WakeyWakeyAPI.csproj", "./"]
RUN dotnet restore "WakeyWakeyAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "WakeyWakeyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WakeyWakeyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WakeyWakeyAPI.dll"]
