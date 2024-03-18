#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TalkBuddy.Presentation/TalkBuddy.Presentation.csproj", "TalkBuddy.Presentation/"]
COPY ["TalkBuddy.DAL/TalkBuddy.DAL.csproj", "TalkBuddy.DAL/"]
COPY ["TalkBuddy.Common/TalkBuddy.Common.csproj", "TalkBuddy.Common/"]
COPY ["TalkBuddy.Domain/TalkBuddy.Domain.csproj", "TalkBuddy.Domain/"]
COPY ["TalkBuddy.Service/TalkBuddy.Service.csproj", "TalkBuddy.Service/"]
RUN dotnet restore "TalkBuddy.Presentation/TalkBuddy.Presentation.csproj"
COPY . .
WORKDIR "/src/TalkBuddy.Presentation"
RUN dotnet build "TalkBuddy.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalkBuddy.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalkBuddy.Presentation.dll"]