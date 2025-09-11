FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["JustBeeWeb.csproj", "."]
RUN dotnet restore "./JustBeeWeb.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "JustBeeWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JustBeeWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JustBeeWeb.dll"]