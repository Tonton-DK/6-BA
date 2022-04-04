# cd /
# docker build -f ReviewService/Dockerfile -t service .
# docker run -p 8080:80 service

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReviewService/ReviewService.csproj", "ReviewService/"]
COPY ["ClassLibrary/ClassLibrary.csproj", "ClassLibrary/"]
RUN dotnet restore "ReviewService/ReviewService.csproj"
COPY . .
WORKDIR "/src/ReviewService"
RUN dotnet build "ReviewService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReviewService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewService.dll"]
