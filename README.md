# GestorTarefasAPI

API RESTful para gestão de tarefas, desenvolvida em .NET com Domain-Driven Design (DDD) e princípios de SOLID, usando Entity Framework Core InMemory para persistência.

## Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Arquitetura e Estrutura de Pastas](#arquitetura-e-estrutura-de-pastas)
- [Por que DDD?](#por-que-ddd)
- [Fluxo de Dependências entre Camadas](#fluxo-de-dependências-entre-camadas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Como Rodar o Projeto](#como-rodar-o-projeto)
- [Como Rodar os Testes](#como-rodar-os-testes)
- [Documentação da API (Swagger)](#documentação-da-api-swagger)
- [Funcionalidades](#funcionalidades)

## Sobre o Projeto

Sistema de gestão de tarefas: dá pra criar, listar, editar e excluir tarefas, além de filtrar por status e data de vencimento.

## Arquitetura e Estrutura de Pastas

O projeto tem 5 camadas, cada uma no seu próprio projeto dentro da solution:

```
GestorTarefasAPI.sln
├── 0-Tests            → GestorTarefasAPI.Tests    (testes automatizados - xUnit)
├── 1-Core             → Core                       (entidades, regras de negócio, interfaces)
├── 2-Application      → Application                (casos de uso, DTOs, services)
├── 3-Api              → GestorTarefasAPI           (controllers, Swagger, ponto de entrada)
└── 4-Data             → Data                        (persistência - EF Core InMemory)
```

| Camada | O que tem lá |
|---|---|
| **Core** | As entidades (como `TaskItem`), enums e as regras de negócio que não podem ser quebradas. Também tem as interfaces de repositório, sem saber como elas são implementadas. Não depende de nenhuma outra camada. |
| **Application** | Os casos de uso, os DTOs que trafegam entre as camadas, e a orquestração das regras de negócio. Depende só do Core. |
| **Data** | A persistência de fato, usando EF Core InMemory — DbContext e a implementação do repositório. Depende só do Core. |
| **Api** | Controllers, configuração do Swagger, injeção de dependência e o `Program.cs`. Depende de Application e Data. |
| **Tests** | Testes das regras de negócio do Core e dos casos de uso do Application. |

## Por que DDD?

- **Entidade rica, não anêmica**: as regras (tipo "título é obrigatório") ficam dentro da própria entidade `TaskItem`, não espalhadas em serviços por aí. Assim ela nunca fica num estado inválido.
- **Inversão de dependência**: a interface `ITaskRepository` fica no Core, e quem implementa é o Data. O domínio não sabe (nem precisa saber) que a persistência é EF Core — ele só enxerga um contrato. Se um dia trocar de InMemory pra SQL Server, a regra de negócio nem percebe.
- **Cada camada com uma responsabilidade só**: Controller não fala com DbContext, por exemplo — sempre passa pelo Application.
- Familiaridade com o DDD, é algo que aplico desde que comecei a desenvolver e se encaixa muito bem nesse projeto.

## Fluxo de Dependências entre Camadas

```
        ┌────────────┐
        │    Api     │
        └─────┬──────┘
              │
      ┌───────┴────────┐
      ▼                ▼
┌─────────────┐  ┌──────────┐
│ Application │  │   Data   │
└──────┬──────┘  └────┬─────┘
       │              │
       └──────┬───────┘
              ▼
         ┌─────────┐
         │  Core   │
         └─────────┘
```

O Core fica no centro e não depende de mais nada — todas as outras camadas apontam pra ele, nunca o contrário.

## Tecnologias Utilizadas

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core (InMemory Provider)
- Swashbuckle (Swagger / OpenAPI)
- xUnit

## Como Rodar o Projeto

Pré-requisito: [.NET SDK](https://dotnet.microsoft.com/download) instalado.

```bash
git clone https://github.com/vbotelho-dev/GestorTarefasAPI.git
cd GestorTarefasAPI/GestorTarefasAPI
dotnet restore
dotnet run --project GestorTarefasAPI
```

A API sobe em `https://localhost:PORTA` (a porta aparece no console quando você roda).

## Como Rodar os Testes

Na raiz da solution:

```bash
dotnet test
```

## Documentação da API (Swagger)

Com o projeto rodando, acessa:

```
https://localhost:PORTA/swagger
```

Lá dá pra ver e testar todos os endpoints direto pelo navegador.

## Funcionalidades

- **Cadastro de Tarefa**: cria tarefa com título (obrigatório), descrição, data de vencimento e status, e retorna um identificador único.
- **Listagem de Tarefas**: lista todas as tarefas, com filtro por status e/ou data de vencimento.
- **Edição de Tarefa**: atualiza título, descrição, status e data de vencimento de uma tarefa já existente.
- **Exclusão de Tarefa**: remove uma tarefa pelo id.