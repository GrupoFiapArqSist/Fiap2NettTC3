#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Event/Event.Api/Event.Api.csproj", "Event/Event.Api/"]
COPY ["Common/TicketNow.Infra.CrossCutting/TicketNow.Infra.CrossCutting.csproj", "Common/TicketNow.Infra.CrossCutting/"]
COPY ["Event/Event.Infra.CrossCutting/Event.Infra.CrossCutting.csproj", "Event/Event.Infra.CrossCutting/"]
COPY ["Event/Event.Infra.Data/Event.Infra.Data.csproj", "Event/Event.Infra.Data/"]
COPY ["Common/TicketNow.Infra.Data/TicketNow.Infra.Data.csproj", "Common/TicketNow.Infra.Data/"]
COPY ["Common/TicketNow.Domain/TicketNow.Domain.csproj", "Common/TicketNow.Domain/"]
COPY ["Event/Event.Domain/Event.Domain.csproj", "Event/Event.Domain/"]
COPY ["Event/Event.Service/Event.Service.csproj", "Event/Event.Service/"]
COPY ["Common/TicketNow.Service/TicketNow.Service.csproj", "Common/TicketNow.Service/"]
RUN dotnet restore "./Event/Event.Api/./Event.Api.csproj"
COPY . .
WORKDIR "/src/Event/Event.Api"
RUN dotnet build "./Event.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Event.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:81
ENTRYPOINT ["dotnet", "Event.Api.dll"]