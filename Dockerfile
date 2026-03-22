FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Smart_Desk_AI.csproj", "."]
COPY ["NuGet.Config", "."]
RUN dotnet nuget locals all --clear
RUN dotnet restore "Smart_Desk_AI.csproj" --configfile NuGet.Config
COPY . .
RUN dotnet publish "Smart_Desk_AI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
COPY ["Policies/", "./Policies/"]
ENTRYPOINT ["dotnet", "Smart_Desk_AI.dll"]