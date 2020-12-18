#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
##WORKDIR /src
#COPY . .
#RUN dotnet restore "Frontend.csproj"
#
##WORKDIR /src
#RUN dotnet build "Frontend.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Frontend.csproj" -c Release -o /app/publish
#
#FROM base AS final
##WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Frontend.dll"]
#
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1
#ENV ASPNETCORE_URLS=http://+:80
#WORKDIR /app
#
## Copy API project folder
#COPY . .
#
## Publish our API to a new folder
#RUN dotnet publish -c Release -o ./publish
#
## Run published API
#ENTRYPOINT ["dotnet", "./publish/Frontend.dll"]


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Frontend/Frontend.csproj", "Frontend/"]
RUN dotnet restore "Frontend/Frontend.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Frontend/Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frontend/Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontend.dll"]