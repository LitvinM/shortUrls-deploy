FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["test_Avtobus1/test_Avtobus1.csproj", "test_Avtobus1/"]
RUN dotnet restore "test_Avtobus1/test_Avtobus1.csproj"
COPY . .
WORKDIR "/src/test_Avtobus1"
RUN dotnet build "test_Avtobus1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test_Avtobus1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test_Avtobus1.dll"]
