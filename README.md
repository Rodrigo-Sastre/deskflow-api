# 🎧 DeskFlow API — Gestão de Chamados e Helpdesk de TI

## 🎯 Sobre o Projeto

A **DeskFlow API** é uma Web API RESTful construída em **.NET Core 10** utilizando **Entity Framework Core** e **SQL Server**. O sistema automatiza o gerenciamento de chamados de suporte técnico de TI, controle do ciclo de vida dos atendimentos, histórico de interações e consultas avançadas com filtros dinâmicos.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 10 (Web API)**
- **Entity Framework Core 10**
- **SQL Server**
- **Swagger / OpenAPI**
- **Git & GitHub**

---

## 🧱 Arquitetura em Camadas

O projeto foi desenvolvido seguindo rigorosamente a separação de responsabilidades em camadas:

- **Controllers:** Recepção de requisições HTTP, validação de rotas RESTful e definição dos Status Codes (`200`, `201`, `204`, `400`, `404`).
- **Services:** Implementação das regras de negócio, validação da transição de status dos chamados e integridade das categorias.
- **Repositories:** Comunicação com o SQL Server via Entity Framework Core (`DbContext`), realizando consultas com LINQ e persistência de dados.
- **Models / Entities:** Entidades relacionais do domínio (`Chamado`, `Categoria`, `Interacao`) e DTOs de entrada.
- **Middlewares:** Tratamento global e resiliente de exceções não tratadas (`ExceptionHandlingMiddleware`), retornando respostas padronizadas em formato JSON limpo.

---

## 🧠 Ciclo de Vida do Chamado

O sistema controla a transição de status exclusivamente na camada de **Service**:

1. **Aberto:** Chamado registrado pelo solicitante com data de abertura automática.
2. **EmAndamento:** Atendimento iniciado pela equipe de suporte (`PATCH /api/chamados/{id}/iniciar`).
3. **Fechado:** Chamado encerrado exigindo texto descritivo da solução e registrando a data de conclusão (`PATCH /api/chamados/{id}/encerrar`).
   > _Nota: Não é permitido adicionar novas interações em chamados que já estejam no status **Fechado**._

---

## 🚀 Como Executar a Aplicação

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express ou Docker)

### Passo a Passo

1. **Clone este repositório:**

   ```bash
   git clone https://github.com/Rodrigo-Sastre/deskflow-api.git
   cd DeskFlow.API
   ```

   2. **Configure a Connection String no arquivo `appsettings.json`:**

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. **Criar o Banco de Dados:**
   Execute as Migrations para criar a estrutura no banco de dados (ou utilize o `script.sql` disponibilizado na raiz):
   ```bash
   dotnet ef database update
   ```

🎥 Vídeo de Apresentação
https://youtu.be/hUukmDTzXZE
