# cd /
# docker build -f OfferService/Dockerfile -t service .
# docker run -p 8080:80 service

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OfferService/OfferService.csproj", "OfferService/"]
COPY ["ClassLibrary/ClassLibrary.csproj", "ClassLibrary/"]
RUN dotnet restore "OfferService/OfferService.csproj"
COPY . .
WORKDIR "/src/OfferService"
RUN dotnet build "OfferService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfferService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfferService.dll"]
