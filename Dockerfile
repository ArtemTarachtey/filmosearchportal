FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["FilmoSearchPortal/FilmoSearchPortal.csproj", "FilmoSearchPortal/"]
COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"] 

RUN dotnet restore "FilmoSearchPortal/FilmoSearchPortal.csproj"

COPY . .
WORKDIR "/src/FilmoSearchPortal"
RUN dotnet publish "FilmoSearchPortal.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FilmoSearchPortal.dll"]