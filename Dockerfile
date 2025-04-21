FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY src/ .

WORKDIR backend/Forum.API

RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app 

COPY --from=build-env /app/out .


CMD ASPNETCORE_URLS="http://*:$PORT" dotnet Forum.API.dll