FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY CloudGames.Payments.Worker/*.csproj ./CloudGames.Payments.Worker/
COPY Consumers/*.csproj ./Consumers/

# ---------------------------
# Mock: ignora restore
# RUN dotnet restore
# ---------------------------

COPY . .

WORKDIR /src/CloudGames.Payments.Worker

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["sleep", "infinity"]
