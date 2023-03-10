FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Webhooks.Sample/Webhooks.Sample.csproj", "src/Webhooks.Sample/"]
COPY ["src/Webhooks/Webhooks.csproj", "src/Webhooks/"]
RUN dotnet restore "src/Webhooks.Sample/Webhooks.Sample.csproj"
COPY . .
WORKDIR "/src/src/Webhooks.Sample"
RUN dotnet build "Webhooks.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webhooks.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webhooks.Sample.dll"]
