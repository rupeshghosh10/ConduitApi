FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY *.sln ./
COPY Conduit.Api/Conduit.Api.csproj Conduit.Api/
COPY Conduit.Core/Conduit.Core.csproj Conduit.Core/
COPY Conduit.Data/Conduit.Data.csproj Conduit.Data/
COPY Conduit.Service/Conduit.Service.csproj Conduit.Service/
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Conduit.Api.dll"]