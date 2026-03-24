# TechChallengeFIAP.Payments.Worker
Serviço de Payments Worker, responsável por processar eventos e mensageria relacionados a pagamentos em background.

## Tecnologias Utilizadas
- C# / .NET 8
- Mensageria / Filas
- Docker
- Kubernetes
- Amazon SQS

## Instruções
SUBIR IMAGEM DOCKER LOCAL
- docker build --no-cache -t payments-worker . && docker run -p 5002:80 payments-worker

SUBIR O REPOSITORIO NO Amazon Elastic Container Service
- aws ecr get-login-password --region sa-east-1 | docker login --username AWS --password-stdin 451664151831.dkr.ecr.sa-east-1.amazonaws.com
 
- docker tag payments-worker:latest 451664151831.dkr.ecr.sa-east-1.amazonaws.com/payments-worker:latest
 
- docker push 451664151831.dkr.ecr.sa-east-1.amazonaws.com/payments-worker:latest

## Fluxo de Comunicação
<img width="1000" height="1000" alt="mermaid-ai-diagram-2026-03-24-204430" src="https://github.com/user-attachments/assets/866dc3f0-70b7-4360-964d-1aade470524f" />
