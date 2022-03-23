
# cd /
# docker build -f MicroService/Dockerfile -t service .
# docker run -p 8080:80 service

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroService/MicroService.csproj", "MicroService/"]
COPY ["ClassLibrary/ClassLibrary.csproj", "ClassLibrary/"]
RUN dotnet restore "MicroService/MicroService.csproj"
COPY . .
WORKDIR "/src/MicroService"
RUN dotnet build "MicroService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.dll"]
