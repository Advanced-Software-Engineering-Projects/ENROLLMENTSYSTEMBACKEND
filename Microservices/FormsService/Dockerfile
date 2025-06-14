FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Microservices/FormsService/FormsService.csproj", "Microservices/FormsService/"]
RUN dotnet restore "Microservices/FormsService/FormsService.csproj"
COPY . .
WORKDIR "/src/Microservices/FormsService"
RUN dotnet build "FormsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FormsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FormsService.dll"]