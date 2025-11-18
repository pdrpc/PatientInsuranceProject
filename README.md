# Desafio Técnico - Cadastro de Pacientes

Este projeto é uma solução Full-Stack completa para o gerenciamento e cadastro de pacientes, desenvolvida como parte de um desafio técnico. A aplicação segue os princípios de **Clean Architecture**, **SOLID** e **Boas Práticas**, utilizando **.NET 9** no backend e **Angular (Standalone)** no frontend.

## 🚀 Tecnologias Utilizadas

### Backend
* **.NET 9** (ASP.NET Core Web API)
* **Entity Framework Core 9** (Code-First, Migrations)
* **SQL Server** (Banco de Dados)
* **AutoMapper** (Mapeamento de Objetos)
* **FluentValidation** (Validação de Regras de Negócio)
* **xUnit & Moq** (Testes Unitários)
* **Swagger** (Documentação da API)

### Frontend
* **Angular 18+** (Componentes Standalone, Signals)
* **TypeScript**
* **RxJS**
* **Ngx-Toastr** (Notificações)
* **Ngx-Mask** (Máscaras de Input)
* **SCSS** (Estilização)

### Infraestrutura & DevOps
* **Docker & Docker Compose** (Orquestração de Containers)
* **Nginx** (Servidor Web para o Frontend)

---

## 🏗️ Arquitetura do Projeto

O backend foi estruturado seguindo a **Clean Architecture** para garantir a separação de responsabilidades:

* **Domain:** Núcleo do projeto. Contém as Entidades (`Patient`, `HealthInsurance`), Enums e Interfaces de Repositório (`IPatientRepository`). Sem dependências externas.
* **Application:** Regras de negócio. Contém os Serviços (`PatientService`), DTOs, Validadores (`CreatePatientDtoValidator`) e Interfaces de Serviço.
* **Infrastructure:** Implementação técnica. Contém o `DbContext`, Implementações dos Repositórios e Configurações do EF Core.
* **PatientAPI:** Camada de entrada. Contém os *Controllers*, Injeção de Dependência e configuração do Swagger.

---

## 🔧 Como Rodar o Projeto (Via Docker) - Recomendado

A maneira mais simples de rodar a aplicação é utilizando o Docker Compose, que sobe o Banco de Dados, a API e o Frontend automaticamente.

### Pré-requisitos
* [Docker](https://www.docker.com/) e Docker Compose instalados.

### Passo a Passo

1.  **Clone o repositório:**
    ```bash
    git clone <url-do-seu-repositorio>
    cd PatientInsuranceProject
    ```

2.  **Configure as Variáveis de Ambiente:**
    Crie um arquivo chamado `.env` na raiz do projeto (ao lado do `docker-compose.yml`) e defina a senha do banco de dados:
    ```ini
    # Arquivo .env
    DB_PASSWORD=SuaSenhaForte@123
    ```

3.  **Suba os containers:**
    Execute o comando abaixo na raiz do projeto:
    ```bash
    docker-compose up -d --build
    ```
    *O processo de build pode levar alguns minutos na primeira vez.*

4.  **Acesse a Aplicação:**
    * **Frontend (Angular):** [http://localhost:4200](http://localhost:4200)
    * **Swagger (API Docs):** [http://localhost:8081/swagger](http://localhost:8081/swagger)

> **Nota:** As migrações do banco de dados são aplicadas automaticamente quando a API inicia. Se os dados não aparecerem imediatamente, aguarde alguns segundos e recarregue a página, pois o SQL Server pode demorar um pouco para inicializar.

---

## 💻 Como Rodar Localmente (Desenvolvimento)

Caso prefira rodar fora do Docker para desenvolvimento/debug.

### Pré-requisitos
* .NET SDK 9.0
* Node.js (LTS)
* SQL Server (Local ou Container)

### 1. Configuração do Backend
1.  Navegue até a pasta da API:
    ```bash
    cd PatientAPI
    ```
2.  Atualize o `appsettings.Development.json` com a Connection String do seu SQL Server local.
3.  Aplique as migrações:
    ```bash
    dotnet ef database update --project ../Infrastructure --startup-project .
    ```
4.  Inicie a API (Perfil https):
    ```bash
    dotnet run --launch-profile https
    ```
    A API rodará em `https://localhost:7244`.

### 2. Configuração do Frontend
1.  Navegue até a pasta do frontend:
    ```bash
    cd frontend
    ```
2.  Instale as dependências:
    ```bash
    npm install
    ```
3.  Certifique-se que o arquivo `src/environments/environment.development.ts` aponta para a porta correta da sua API local (`https://localhost:7244/api`).
4.  Inicie o servidor de desenvolvimento:
    ```bash
    ng serve -o
    ```

---

## 🧪 Executando os Testes

O projeto inclui testes unitários para a camada de **Aplicação** (Serviços e Regras de Negócio) e **Domínio**.

Para rodar os testes, execute o seguinte comando na raiz da solução:

```bash
dotnet test
```

---

## 📋 Funcionalidades Implementadas


* ✅ CRUD Completo de Pacientes: Criar, Ler, Atualizar e Deletar.

* ✅ Listagem de Convênios: Dados mockados no banco de dados.

* ✅ Paginação e Filtros: Busca por Nome, CPF, Email e Convênio no servidor.

* ✅ Validações: CPF válido, unicidade de CPF, data de nascimento não futura, obrigatoriedade de campos.

* ✅ Exclusão Lógica: Pacientes são marcados como inativos em vez de serem apagados fisicamente.

* ✅ Docker: Ambiente totalmente containerizado.
