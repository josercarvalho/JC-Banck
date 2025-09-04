# Microsserviços BankMore

Este projeto implementa um sistema bancário baseado em microsserviços para o BankMore, focado em escalabilidade e segurança. Ele segue os princípios de Domain-Driven Design (DDD) e Command Query Responsibility Segregation (CQRS), com autenticação JWT para segurança da API e um API Gateway para centralizar o acesso aos serviços.

## Atualizações e Melhorias Recentes

O projeto passou por várias melhorias para garantir o funcionamento adequado em ambiente containerizado:

1. **Compartilhamento de Banco de Dados**: Implementação de um volume compartilhado para o banco de dados SQLite entre os contêineres, garantindo consistência dos dados.

2. **Configuração por Variáveis de Ambiente**: Substituição de strings de conexão e chaves JWT hardcoded por variáveis de ambiente configuráveis.

3. **Políticas CORS**: Adição de políticas CORS em todos os serviços para permitir comunicação entre diferentes origens.

4. **Melhorias na Inicialização do Banco de Dados**: Garantia de criação de diretórios necessários para o banco de dados SQLite.

5. **Script de Teste**: Criação de um script PowerShell para testar as funcionalidades da API após a implantação.

6. **Swagger Sempre Disponível**: Configuração do Swagger para estar sempre disponível em todos os ambientes, incluindo contêineres Docker, facilitando o teste e a documentação das APIs.

## Tecnologias Utilizadas

- **.NET 8:** O framework principal para a construção dos microsserviços.
- **C#:** A linguagem de programação primária.

- **Domain-Driven Design (DDD):** Abordagem arquitetural para modelar o software com base no domínio.
- **Command Query Responsibility Segregation (CQRS):** Padrão para separar operações de leitura e escrita.
- **MediatR:** Biblioteca para implementar o padrão Mediator, facilitando o CQRS.
- **JWT (JSON Web Tokens):** Para proteger os endpoints da API e autenticação de usuários.
- **Dapper:** Um ORM leve para acesso eficiente a dados.
- **SQLite:** Usado como banco de dados para simplicidade no desenvolvimento e testes. (Nota: Para produção, Oracle é recomendado conforme os requisitos do PDF).
- **Docker & Docker Compose:** Para conteinerização e orquestração de microsserviços.
- **xUnit:** Framework para testes unitários.
- **Moq:** Biblioteca de mocking para testes unitários.

## Estrutura do Projeto

A solução é composta pelos seguintes projetos:

- `BankMore.sln`: O arquivo de solução.
- `BankMore.Core`: Contém entidades de domínio, interfaces (repositórios), comandos, queries e handlers.
- `BankMore.Infra`: Implementa as preocupações de infraestrutura, como implementações de repositório usando Dapper e SQLite.
- `BankMore.API.ContaCorrente`: Web API ASP.NET Core para operações relacionadas à conta (ex: criar conta, login, depósito, saque, consultar saldo).
- `BankMore.API.Transferencia`: Web API ASP.NET Core para operações de transferência.

- `BankMore.Tests`: Projeto de testes unitários para a lógica central.

## Arquitetura do Sistema

### Visão Geral

O sistema BankMore segue uma arquitetura de microsserviços, onde cada serviço é responsável por um domínio específico do negócio:



### Padrões Implementados

- **CQRS (Command Query Responsibility Segregation)**: Separação entre comandos (operações de escrita) e queries (operações de leitura)
- **Mediator Pattern**: Implementado com MediatR para desacoplar os componentes do sistema
- **Repository Pattern**: Abstração do acesso a dados
- **Idempotência**: Garantia de que operações repetidas não causam efeitos colaterais indesejados

## Primeiros Passos

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Executando o Projeto com Docker Compose

1.  **Navegue até o diretório raiz do projeto:**

    ```bash
    cd D:\DevStudio2025\DESAFIO_TECNICO\CAPEGMINI\dev\BanckMore
    ```

2.  **Construa e execute os contêineres Docker:**

    ```bash
    docker-compose up --build
    ```

    Este comando irá:
    - Construir as imagens Docker para `BankMore.API.ContaCorrente` e `BankMore.API.Transferencia`.
    - Iniciar os contêineres com as configurações necessárias.
    - Configurar o volume compartilhado `db-data` para o banco de dados SQLite.
    - Definir as variáveis de ambiente para conexão com o banco de dados e chave JWT.

3.  **Acesse as APIs diretamente:**

    Após a execução do docker-compose, você pode acessar a documentação Swagger de cada serviço:
    
    - **API ContaCorrente:** `http://localhost:5074/swagger`
    - **API Transferencia:** `http://localhost:5237/swagger`
    
    O Swagger está configurado para estar sempre disponível, independentemente do ambiente de execução.

4.  **Teste as APIs com o script PowerShell:**

    Após os serviços estarem em execução, você pode executar o script de teste para verificar o funcionamento:

    ```bash
    .\test-api.ps1
    ```

    Este script irá criar contas, fazer login, realizar depósitos e transferências para validar o funcionamento do sistema.

### Executando o Projeto Localmente (sem Docker)

1.  **Navegue até o diretório raiz do projeto:**

    ```bash
    cd D:\DevStudio2025\DESAFIO_TECNICO\CAPEGMINI\dev
    ```

2.  **Execute a API ContaCorrente:**

    ```bash
    dotnet run --project BankMore.API.ContaCorrente
    ```

3.  **Execute a API Transferencia (em um terminal separado):**

    ```bash
    dotnet run --project BankMore.API.Transferencia
    ```



## Executando Testes

Para executar os testes unitários, navegue até o diretório raiz do projeto e execute:

```bash
dotnet test BankMore.Tests
```

## Detalhes da Implementação

### Compartilhamento de Banco de Dados

O projeto utiliza um volume Docker compartilhado (`db-data`) para garantir que ambos os serviços (`contacorrente.api` e `transferencia.api`) acessem o mesmo banco de dados SQLite. Isso é configurado no arquivo `docker-compose.yaml`.

### Variáveis de Ambiente

As seguintes variáveis de ambiente são configuradas no `docker-compose.yaml`:

- `ConnectionString`: Define o caminho para o banco de dados SQLite compartilhado
- `JWT_KEY`: Define a chave secreta para geração e validação de tokens JWT

### Políticas CORS

Todos os serviços (ContaCorrente e Transferencia) foram configurados com políticas CORS permissivas para facilitar o desenvolvimento e testes. Em um ambiente de produção, estas políticas devem ser restringidas para origens específicas.

### Configuração do Swagger

O Swagger foi configurado para estar sempre disponível em todos os serviços, independentemente do ambiente de execução (desenvolvimento ou produção). Isso facilita o teste e a documentação das APIs mesmo quando executadas em contêineres Docker.

### Script de Teste

O arquivo `test-api.ps1` contém um script PowerShell que demonstra o fluxo completo de uso da API:

1. Criação de contas correntes
2. Login e obtenção de token JWT
3. Realização de depósito
4. Criação de uma segunda conta
5. Transferência entre contas



## Melhorias Futuras (Baseado nos Requisitos do PDF)

- **Integração Kafka:** Implementar comunicação assíncrona entre microsserviços usando Kafka para eventos como `Transferencias Realizadas` e `Tarifas Realizadas`.
- **API de Tarifas:** Desenvolver a API de Tarifas opcional conforme descrito no PDF.
- **Testes Abrangentes:** Expandir testes unitários e de integração para cobrir todas as funcionalidades e casos de uso.
- **Tratamento de Erros:** Implementar um tratamento de erros mais robusto e padronizado em todas as APIs.
- **Melhorias de Segurança:** Implementar hash de senha adequado, gerenciamento de chave JWT mais seguro e outras melhores práticas de segurança.
- **Migração de Banco de Dados:** Implementar migrações de banco de dados para SQLite (e potencialmente Oracle para produção).
- **Orquestração de Contêineres:** Configurar ainda mais a implantação do Kubernetes para ambientes de produção.

## Autor 👦

* **José Carvalho** - *Desenvolvedor* - [GitHub](https://github.com/josercarvalho) - [Linkedin](https://www.linkedin.com/in/josercarvalho)
