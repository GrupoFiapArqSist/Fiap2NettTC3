#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Order/Order.Api/Order.Api.csproj", "Order/Order.Api/"]
COPY ["Common/TicketNow.Infra.CrossCutting/TicketNow.Infra.CrossCutting.csproj", "Common/TicketNow.Infra.CrossCutting/"]
COPY ["Common/TicketNow.Service/TicketNow.Service.csproj", "Common/TicketNow.Service/"]
COPY ["Common/TicketNow.Domain/TicketNow.Domain.csproj", "Common/TicketNow.Domain/"]
COPY ["Order/Order.Infra.CrossCutting/Order.Infra.CrossCutting.csproj", "Order/Order.Infra.CrossCutting/"]
COPY ["Order/Order.Infra.Data/Order.Infra.Data.csproj", "Order/Order.Infra.Data/"]
COPY ["Common/TicketNow.Infra.Data/TicketNow.Infra.Data.csproj", "Common/TicketNow.Infra.Data/"]
COPY ["Order/Order.Domain/Order.Domain.csproj", "Order/Order.Domain/"]
COPY ["Order/Order.Service/Order.Service.csproj", "Order/Order.Service/"]
RUN dotnet restore "./Order/Order.Api/./Order.Api.csproj"
COPY . .
WORKDIR "/src/Order/Order.Api"
RUN dotnet build "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Order.Api.dll"]