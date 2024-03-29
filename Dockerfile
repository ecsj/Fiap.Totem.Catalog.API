FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./

COPY . .


RUN dotnet publish ./src/API/API.csproj -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "API.dll"]