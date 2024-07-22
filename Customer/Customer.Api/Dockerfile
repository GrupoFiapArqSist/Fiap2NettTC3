FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 82
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Customer/Customer.Api/Customer.Api.csproj", "Customer/Customer.Api/"]
COPY ["Common/TicketNow.Infra.CrossCutting/TicketNow.Infra.CrossCutting.csproj", "Common/TicketNow.Infra.CrossCutting/"]
COPY ["Customer/Customer.Infra.CrossCutting/Customer.Infra.CrossCutting.csproj", "Customer/Customer.Infra.CrossCutting/"]
COPY ["Customer/Customer.Infra.Data/Customer.Infra.Data.csproj", "Customer/Customer.Infra.Data/"]
COPY ["Common/TicketNow.Domain/TicketNow.Domain.csproj", "Common/TicketNow.Domain/"]
COPY ["Common/TicketNow.Infra.Data/TicketNow.Infra.Data.csproj", "Common/TicketNow.Infra.Data/"]
COPY ["Customer/Customer.Domain/Customer.Domain.csproj", "Customer/Customer.Domain/"]
COPY ["Customer/Customer.Service/Customer.Service.csproj", "Customer/Customer.Service/"]
COPY ["Common/TicketNow.Service/TicketNow.Service.csproj", "Common/TicketNow.Service/"]
RUN dotnet restore "./Customer/Customer.Api/./Customer.Api.csproj"
COPY . .
WORKDIR "/src/Customer/Customer.Api"
RUN dotnet build "./Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:82
ENTRYPOINT ["dotnet", "Customer.Api.dll"]