FROM mcr.microsoft.com/dotnet/core/runtime:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100-bionic AS build

WORKDIR /src
COPY Microservice.Two/*.csproj ./Microservice.Two/
COPY Microservice.Two.Data/*.csproj ./Microservice.Two.Data/
COPY Microservice.Two.Test/*.csproj ./Microservice.Two.Test/
COPY Microservice.Shared/*.csproj ./Microservice.Shared/

WORKDIR /src/Microservice.Shared
RUN dotnet restore "Microservice.Shared.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.Two.Data
RUN dotnet restore "Microservice.Two.Data.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.Two
RUN dotnet restore "Microservice.Two.csproj" -s "https://www.nuget.org/api/v2"
WORKDIR /src/Microservice.Two.Test
RUN dotnet restore "Microservice.Two.Test.csproj" -s "https://www.nuget.org/api/v2"

WORKDIR /src
COPY . .

WORKDIR /src/Microservice.Two
RUN dotnet build "Microservice.Two.csproj" -c Release -o /app/build --source "https://www.nuget.org/api/v2"
RUN dotnet test -c Release -o /app/test

FROM build AS publish
RUN dotnet publish "Microservice.Two.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.Two.dll"]