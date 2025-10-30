FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS="http://0.0.0.0:8080"
ENV ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=financeiroPontoNet;Username=postgres;Password=master"
ENV IS_RUNNING_IN_CONTAINER="true"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

COPY ["src/FinanceiroPontoNet.Web/FinanceiroPontoNet.Web.csproj", "FinanceiroPontoNet.Web/"]
COPY ["src/FinanceiroPontoNet.Application/FinanceiroPontoNet.Application.csproj", "FinanceiroPontoNet.Application/"]
COPY ["src/FinanceiroPontoNet.Infrastructure/FinanceiroPontoNet.Infrastructure.csproj", "FinanceiroPontoNet.Infrastructure/"]
COPY ["src/FinanceiroPontoNet.Domain/FinanceiroPontoNet.Domain.csproj", "FinanceiroPontoNet.Domain/"]

RUN dotnet restore "/src/FinanceiroPontoNet.Web/FinanceiroPontoNet.Web.csproj"

COPY "/src" .

WORKDIR "/src/FinanceiroPontoNet.Web"
RUN dotnet build "FinanceiroPontoNet.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./FinanceiroPontoNet.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceiroPontoNet.Web.dll"]