
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PatientsBackend.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "PatientsBackend.dll"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PatientsBackend.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "PatientsBackend.dll"]
