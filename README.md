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
- [Payloads de Exemplo para Testes](#payloads-de-exemplo-para-testes)
- [Funcionalidades](#funcionalidades)
- [Observações Importantes](#observações-importantes)
- [Próximos Passos (TO DO)](#próximos-passos-to-do)

## Sobre o Projeto

Sistema de gestão de tarefas: dá pra criar, listar, editar e excluir tarefas, além de filtrar por status e data de vencimento.

## Arquitetura e Estrutura de Pastas

O projeto tem 5 camadas, cada uma no seu próprio projeto dentro da solution:

```
GestorTarefasAPI.slnx
├── 0-Tests            → GestorTarefasAPI.Tests    (testes automatizados - xUnit e Moq)
├── 1-Core             → Core                       (entidades, regras de negócio, interfaces)
├── 2-Application      → Application                (casos de uso, DTOs, services)
├── 3-Api              → GestorTarefasAPI           (controllers, Swagger, middleware, ponto de entrada)
└── 4-Data             → Data                        (persistência - EF Core InMemory)
```

| Camada | O que tem lá |
|---|---|
| **Core** | A entidade `Tarefa`, o enum `Status` e as regras de negócio que não podem ser quebradas (título obrigatório, data de vencimento não pode estar no passado). Também tem a interface `ITarefaRepository`, sem saber como ela é implementada. Não depende de nenhuma outra camada. |
| **Application** | Os DTOs (`CriarTarefaDto`, `AtualizarTarefaDto`, `TarefaResponseDto`) e o `TarefaService`, que orquestra as regras de negócio. Depende só do Core. |
| **Data** | A persistência de fato, usando EF Core InMemory — `TarefaDbContext` e o `TarefaRepository`, que implementa `ITarefaRepository`. Depende só do Core. |
| **Api** | `TarefasController`, configuração do Swagger, o `ExceptionHandlingMiddleware`, injeção de dependência e o `Program.cs`. Depende de Application e Data. |
| **Tests** | Testes das regras de negócio do Core (`TarefaTests`) e dos casos de uso do Application (`TarefaServiceTests`, usando Moq para simular o repositório). |

## Por que DDD?

- **Entidade rica, não anêmica**: as regras (título obrigatório, data de vencimento não pode estar no passado) ficam dentro da própria entidade `Tarefa`, não espalhadas em serviços por aí. Assim ela nunca fica num estado inválido.
- **Inversão de dependência**: a interface `ITarefaRepository` fica no Core, e quem implementa é o Data (`TarefaRepository`). O domínio não sabe (nem precisa saber) que a persistência é EF Core — ele só enxerga um contrato. Se um dia trocar de InMemory pra SQL Server, a regra de negócio nem percebe.
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
- Swashbuckle (Swagger / OpenAPI), com tema escuro customizado
- xUnit
- Moq (para simular o repositório nos testes de Application)

## Como Rodar o Projeto

Pré-requisito: [.NET SDK](https://dotnet.microsoft.com/download) instalado.

1. Clone o repositório:

```bash
git clone https://github.com/vbotelho-dev/GestorTarefasAPI.git
```

2. Acesse a pasta da solution:

```bash
cd GestorTarefasAPI/GestorTarefasAPI
```

3. Restaure as dependências:

```bash
dotnet restore
```

4. Rode o projeto:

```bash
dotnet run --project GestorTarefasAPI/GestorTarefasAPI.csproj
```

A API sobe em `https://localhost:7055` (ou na porta exibida no console). O navegador abre automaticamente na página do Swagger, já com tema escuro, pronta para uso.

Alternativamente, é possível abrir o arquivo `GestorTarefasAPI.slnx` no Visual Studio e rodar com F5, selecionando o profile `https`, `http` ou `IIS Express`.

## Como Rodar os Testes

Na pasta `GestorTarefasAPI` (onde está o arquivo `.slnx`):

```bash
dotnet test
```

Isso executa os 19 testes automatizados (10 de regras de negócio do Core, 9 de casos de uso do Application).

## Documentação da API (Swagger)

Com o projeto rodando, o Swagger abre automaticamente. Caso precise acessar manualmente:

```
https://localhost:7055/swagger
```

Lá dá pra ver e testar todos os endpoints direto pelo navegador.

## Payloads de Exemplo para Testes

Alguns exemplos prontos de corpo de requisição para testar o endpoint `POST /api/Tarefas` no Swagger:

```json
{
  "titulo": "Teste do post",
  "descricao": "Testando a requisição para persistir informações",
  "dataVencimento": "2026-07-20"
}
```

```json
{
  "titulo": "Teste do post 2",
  "descricao": "Testando a requisição para persistir informações",
  "dataVencimento": "2026-07-20"
}
```

```json
{
  "titulo": "Teste do post 3",
  "descricao": "Testando a requisição para persistir informações",
  "dataVencimento": "2026-07-19"
}
```

```json
{
  "titulo": "Teste do post 5",
  "descricao": "Testando a requisição para persistir informações",
  "dataVencimento": "2026-07-19"
}
```

Após criar algumas tarefas, é possível testar os filtros de `GET /api/Tarefas` usando `status` (0 = Pendente, 1 = EmProgresso, 2 = Concluida) e/ou `dataVencimento` (ex: `2026-07-19`).

## Funcionalidades

- **Cadastro de Tarefa**: cria tarefa com título (obrigatório), descrição, data de vencimento e status, e retorna um identificador único.
- **Listagem de Tarefas**: lista todas as tarefas, com filtro por status e/ou data de vencimento.
- **Edição de Tarefa**: atualiza título, descrição, status e data de vencimento de uma tarefa já existente.
- **Exclusão de Tarefa**: remove uma tarefa pelo id.

## Observações Importantes

- **Persistência em memória**: os dados são armazenados em memória (EF Core InMemory) e são reiniciados a cada execução da aplicação, conforme especificado no desafio.
- **Tratamento de erros**: um middleware global (`ExceptionHandlingMiddleware`) converte violações de regra de negócio (`DomainException`) em respostas `400 Bad Request` com mensagem clara, e qualquer outro erro inesperado em `500` com mensagem genérica, sem expor detalhes internos do servidor.

## Próximos Passos (TO DO)

- Criar um arquivo de deploy (ex: Dockerfile e/ou pipeline de CI/CD) e subir a aplicação em um pipeline (GitHub Actions, Azure DevOps, etc.), validando automaticamente o build, os testes e, futuramente, o deploy da aplicação.
- Avaliar a criação de um endpoint `PATCH` para atualização parcial de tarefas (ex: alterar apenas o status), já que o `PUT` atual exige o envio do objeto completo por representar substituição total do recurso — ponto identificado durante o desenvolvimento ao testar a atualização de tarefas.
