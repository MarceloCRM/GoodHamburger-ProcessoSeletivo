# Good Hamburger

Aplicação para cadastro e consulta de pedidos do desafio Good Hamburger, dividida em dois projetos:

- `GoodHamburger`: API ASP.NET Core com Entity Framework Core e SQL Server LocalDB.
- `GoodHamburger.Blazor`: frontend em Blazor WebAssembly consumindo a API.

## Autoria

O backend da API foi completamente desenvolvido por mim.

O frontend em Blazor foi feito com ajuda do Codex 5.4 CLI.

## Stack

- ASP.NET Core Web API
- Blazor WebAssembly
- Entity Framework Core
- SQL Server LocalDB
- Swagger / OpenAPI

## Estrutura da solução

```text
GoodHamburger.sln
|-- GoodHamburger
|   |-- Controllers
|   |-- Data
|   |-- DTOs
|   |-- Models
|   |-- Services
|   `-- Migrations
`-- GoodHamburger.Blazor
    |-- Components
    |-- Layout
    |-- Models
    |-- Pages
    |-- Services
    `-- wwwroot
```

## Requisitos

- .NET SDK 9 instalado
- SQL Server LocalDB disponível na máquina
- `dotnet-ef` instalado

Observação: a solução mistura `net8.0` no backend e `net9.0` no frontend. No ambiente em que validei o projeto, a compilação funcionou com `dotnet build` usando SDK `9.0.300`.

## Instalação das dependências

Se a ideia for apenas rodar este repositório já clonado, o comando abaixo já restaura todos os pacotes declarados nos projetos:

```powershell
dotnet restore
```

Se quiser reproduzir manualmente a instalação dos pacotes usados no código, estes são os comandos:

1. Instalar a ferramenta do Entity Framework:

```powershell
dotnet tool install --global dotnet-ef
```

2. Instalar os pacotes do backend (`GoodHamburger`):

```powershell
dotnet add GoodHamburger package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add GoodHamburger package Microsoft.EntityFrameworkCore.Design --version 8.0.11
dotnet add GoodHamburger package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
dotnet add GoodHamburger package Swashbuckle.AspNetCore --version 6.6.2
```

3. Instalar os pacotes do frontend (`GoodHamburger.Blazor`):

```powershell
dotnet add GoodHamburger.Blazor package Microsoft.AspNetCore.Components.WebAssembly --version 9.0.1
dotnet add GoodHamburger.Blazor package Microsoft.AspNetCore.Components.WebAssembly.DevServer --version 9.0.1
dotnet add GoodHamburger.Blazor package Microsoft.EntityFrameworkCore --version 8.0.11
```

Observação: os arquivos do Bootstrap já estão versionados em `GoodHamburger.Blazor/wwwroot/lib/bootstrap`, então não há etapa de `npm install` neste projeto.

## Como executar

1. Restaurar e compilar a solução:

```powershell
dotnet restore
dotnet build GoodHamburger.sln
```

2. Aplicar as migrations do backend:

```powershell
dotnet ef database update --project GoodHamburger
```

3. Subir a API:

```powershell
dotnet run --project GoodHamburger
```

API e Swagger:

- `https://localhost:7228/swagger`
- `http://localhost:5007`

4. Em outro terminal, subir o frontend Blazor:

```powershell
dotnet run --project GoodHamburger.Blazor
```

Frontend:

- `https://localhost:7048`
- `http://localhost:5008`

## Fluxo da aplicação

1. O backend expõe o cardápio em `GET /api/items`.
2. O frontend consome esse cardápio e permite selecionar no máximo um item por categoria.
3. O backend recebe `ItemIds`, valida o pedido, calcula subtotal, desconto e total, e persiste a ordem.
4. O frontend lista os pedidos criados e permite abrir detalhes e excluir registros.

## Decisões de arquitetura

### 1. Separação entre API e frontend

A solução foi separada em dois projetos independentes. Isso deixa o backend responsável pela regra de negócio e persistência, enquanto o Blazor cuida apenas da experiência de uso e do consumo da API.

### 2. Regra de preço isolada em serviço

O cálculo de desconto foi centralizado em `OrderPricingService`. Isso evita espalhar regra de combo pelos controllers e facilita a manutenção da lógica:

- sanduíche + fritas = 10%
- sanduíche + bebida = 15%
- sanduíche + fritas + bebida = 20%

### 3. Serviço de pedidos separado do controller

O `OrdersController` delega o CRUD para `OrderService`. Assim, o controller fica enxuto, e a lógica de validação, consulta e mapeamento para DTO permanece em uma camada específica de aplicação.

### 4. Cardápio seedado no banco

Os itens do desafio foram cadastrados no `AppDbContext` via `HasData`. Isso reduz setup manual e garante uma base inicial consistente para execução local.

### 5. DTOs para contrato da API

A API não expõe diretamente as entidades de domínio nas respostas de pedidos. O uso de DTOs desacopla o contrato HTTP do modelo persistido e simplifica o payload consumido pelo frontend.

### 6. Regra duplicada no frontend por usabilidade

O frontend replica a lógica de seleção por categoria e o preview de desconto para dar feedback imediato ao usuário antes do envio. Ainda assim, a validação definitiva continua no backend.

### 7. Configuração simples para ambiente local

O projeto foi preparado para rodar localmente com:

- `LocalDB` no backend
- `HttpClient` apontando para `https://localhost:7228/`
- política de CORS liberando `https://localhost:7048`

Isso simplifica o cenário de desenvolvimento, mas também cria dependência explícita dessas URLs.

## Endpoints principais da API

- `GET /api/items`: retorna o cardápio
- `GET /api/orders`: lista pedidos
- `GET /api/orders/{id}`: retorna um pedido
- `POST /api/orders`: cria um pedido
- `PUT /api/orders/{id}`: atualiza um pedido
- `DELETE /api/orders/{id}`: exclui um pedido

Payload de criação/atualização:

```json
{
  "itemIds": [1, 4, 5]
}
```

## Regras de negócio implementadas

- O pedido deve conter ao menos um item.
- O pedido pode conter apenas um item por categoria.
- Todos os itens enviados devem existir no cardápio.
- O desconto é calculado no backend antes da persistência.

## O que ficou de fora

- Autenticação e autorização.
- Testes automatizados de unidade, integração e interface.
- Edição de pedido no frontend, embora a API já tenha `PUT`.
- Paginação, filtros e busca na listagem de pedidos.
- Tratamento mais refinado de erros com tipos específicos em vez de `Exception`.
- Configuração de ambiente para produção.
- Observabilidade, logs estruturados e monitoramento.
- Configuração explícita de precisão decimal no modelo.
- Ajustes de qualidade como limpeza de warnings de nulabilidade.

## Observações importantes

- A string de conexão padrão está em `GoodHamburger/appsettings.json` e usa `(localdb)\\MSSQLLocalDB`.
- O frontend depende da URL fixa `https://localhost:7228/`. Se a porta da API mudar, será necessário ajustar `GoodHamburger.Blazor/Program.cs`.
- A política de CORS no backend também está fixa para `https://localhost:7048`. Se a porta do frontend mudar, será necessário ajustar `GoodHamburger/Program.cs`.
- Existe uma migration vazia adicional (`20260422000415_Initial.cs`) no projeto. Ela não impede a execução, mas pode ser removida ou organizada depois.

## Validação realizada

Os seguintes passos foram executados com sucesso neste repositório:

- `dotnet build GoodHamburger.sln`
- `dotnet ef database update --project GoodHamburger`
