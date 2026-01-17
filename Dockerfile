# ======================
# Etapa 1 — Build
# ======================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução e projetos principais
COPY *.sln ./
COPY CloudGames.Payments.Worker/*.csproj ./CloudGames.Payments.Worker/
COPY Consumers/*.csproj ./Consumers/

# Copia o projeto Contracts (mesmo que não compile)
COPY ../CloudGames.Contracts/CloudGames.Contracts/CloudGames.Contracts.csproj ./CloudGames.Contracts/CloudGames.Contracts/

# (Opcional) se houver outros projetos extras, copie aqui

# Restaura dependências
RUN dotnet restore

# Copia todo o código restante
COPY . .

# Entra no projeto principal
WORKDIR /src/CloudGames.Payments.Worker

# Publica em Release
RUN dotnet publish -c Release -o /app

# ======================
# Etapa 2 — Runtime
# ======================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app .

# Workers não expõem portas
# Inicia o worker
ENTRYPOINT ["dotnet", "CloudGames.Payments.Worker.dll"]
