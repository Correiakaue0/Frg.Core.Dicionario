#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Frg.Core.Dicionario/Frg.Core.Dicionario.csproj", "Frg.Core.Dicionario/"]
RUN dotnet restore "Frg.Core.Dicionario/Frg.Core.Dicionario.csproj"
COPY . .
WORKDIR "/src/Frg.Core.Dicionario"
RUN dotnet build "Frg.Core.Dicionario.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frg.Core.Dicionario.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frg.Core.Dicionario.dll"]