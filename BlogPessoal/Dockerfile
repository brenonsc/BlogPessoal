FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_URLS http://*:80
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BlogPessoal.csproj", "./"]
RUN dotnet restore "BlogPessoal.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BlogPessoal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogPessoal.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogPessoal.dll"]
