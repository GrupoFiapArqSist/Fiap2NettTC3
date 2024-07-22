FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 83
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateway/TicketNow.Gateway/TicketNow.Gateway.Api.csproj", "Gateway/TicketNow.Gateway/"]
RUN dotnet restore "./Gateway/TicketNow.Gateway/./TicketNow.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Gateway/TicketNow.Gateway"
RUN dotnet build "./TicketNow.Gateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicketNow.Gateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:83
ENTRYPOINT ["dotnet", "TicketNow.Gateway.Api.dll"]