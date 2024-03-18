# Ticket Now
Plataforma de venda de ingressos para eventos, desenvolvida em .NET 7 com arquitetura de microsserviços.

### 📋 Pré-requisitos

* .NET 7 ([Download .NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
* Docker ([Download Docker Desktop](https://www.docker.com/products/docker-desktop/))

## Integrantes

- [Lucas Hanke](https://github.com/lucasbagrt)
- [João Gasparini](https://github.com/joaogasparini)
- [Victoria Pacheco](https://github.com/vickypacheco)
- [Rafael Araujo](https://github.com/RafAraujo)
- [Cristian Kulessa](https://github.com/Kulessa)

## Build 

Para rodar este projeto, siga estes passos:

* Inicie os microsserviços conforme abaixo:
  * Crie o banco de dados no Docker utilizando o comando abaixo.
  * Execute as migrações para criar o banco de dados de cada microsserviço.
  * Execute o serviço correspondente a cada microsserviço.
  * Utilize o Gateway para acessar os endpoints dos microsserviços.

### Microsserviços e Banco de Dados

Os microsserviços foram desenvolvidos utilizando a tecnologia .NET 7 e integram-se a um banco de dados SQL Server. Abaixo estão os passos para configurar o banco de dados utilizando Docker:

```docker
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=1q2w3e4r@#$' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### Gateway

O Gateway foi implementado utilizando Ocelot para permitir o acesso aos endpoints dos microsserviços.

## Comunicação entre Microsserviços

##Queue

Implementamos uma fila com RabbitMQ para permitir a comunicação assíncrona entre os microsserviços. (detalhar mais)

Além da fila de mensagens, os serviços também se comunicam através dos endpoints e do gateway para acessar informações de outros microsserviços. Isso permite realizar validações e obter dados necessários para aplicar a lógica de negócio de forma distribuída, ele pode fazer uma requisição HTTP para o endpoint correspondente no microsserviço alvo, utilizando o gateway como intermediário.

## Detalhes dos Microserviços

##Costumer 

Microsserviço responsável pelo gerenciamento dos clientes.

Endpoints

![image](https://github.com/GrupoFiapArqSist/Fiap2NettTC3/assets/143532676/9bd5f6bb-309b-4516-8a61-c41c36385ffc)
![image](https://github.com/GrupoFiapArqSist/Fiap2NettTC3/assets/143532676/87189840-26fe-4a66-ac0e-f2875e505905)

##Event

Microsserviço responsável pelo gerenciamento dos eventos.

Endpoints

![image](https://github.com/GrupoFiapArqSist/Fiap2NettTC3/assets/143532676/5b1be442-deb0-4ead-b985-71c55e9a68f2)

##Order

Microsserviço responsável pelo gerenciamento dos pedidos de ingressos.

Endpoints

![image](https://github.com/GrupoFiapArqSist/Fiap2NettTC3/assets/143532676/d8beacd2-6d44-48b1-9700-b75ab73e995a)

##Payment

(inserir payment)

