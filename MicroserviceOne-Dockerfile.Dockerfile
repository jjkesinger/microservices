FROM mcr.microsoft.com/dotnet/core/runtime:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100-bionic AS build

WORKDIR /src
COPY Microservice.One/*.csproj ./Microservice.One/
COPY Microservice.One.Data/*.csproj ./Microservice.One.Data/
COPY Microservice.One.Test/*.csproj ./Microservice.One.Test/
COPY Microservice.Shared/*.csproj ./Microservice.Shared/

WORKDIR /src/Microservice.Shared
RUN dotnet restore "Microservice.Shared.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.One.Data
RUN dotnet restore "Microservice.One.Data.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.One
RUN dotnet restore "Microservice.One.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.One.Test
RUN dotnet restore "Microservice.One.Test.csproj" -s "https://www.nuget.org/api/v2"

WORKDIR /src
COPY . .

WORKDIR /src/Microservice.One
RUN dotnet build "Microservice.One.csproj" -c Release -o /app/build --source "https://www.nuget.org/api/v2"
RUN dotnet test -c Release -o /app/test

FROM build AS publish
RUN dotnet publish "Microservice.One.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.One.dll"]