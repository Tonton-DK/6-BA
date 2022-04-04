# cd /
# docker build -f ContractService/Dockerfile -t service .
# docker run -p 8080:80 service

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ContractService/ContractService.csproj", "ContractService/"]
COPY ["ClassLibrary/ClassLibrary.csproj", "ClassLibrary/"]
RUN dotnet restore "ContractService/ContractService.csproj"
COPY . .
WORKDIR "/src/ContractService"
RUN dotnet build "ContractService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContractService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractService.dll"]
