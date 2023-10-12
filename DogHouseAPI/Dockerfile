FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DogHouseAPI/DogHouseAPI.csproj", "DogHouseAPI/"]
RUN dotnet restore "DogHouseAPI/DogHouseAPI.csproj"
COPY . .
WORKDIR "/src/DogHouseAPI"
RUN dotnet build "DogHouseAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DogHouseAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DogHouseAPI.dll"]
