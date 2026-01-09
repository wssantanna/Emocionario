# Emocionario API

API RESTful desenvolvida com ASP.NET Core 9.0 utilizando Minimal APIs para gerenciamento de usuários do sistema Emocionario.

## Arquitetura

O projeto segue os princípios de Clean Architecture e Domain-Driven Design (DDD), com separação clara de responsabilidades entre as camadas:

- **Domain**: Entidades e Value Objects com regras de negócio
- **Application**: Serviços e DTOs para lógica de aplicação
- **Infrastructure**: Repositórios e acesso a dados
- **API**: Endpoints HTTP e configurações

## Tecnologias Utilizadas

- .NET 9.0
- ASP.NET Core Minimal APIs
- Entity Framework Core (InMemory Database)
- FluentValidation
- Scalar (Documentação OpenAPI)
- C# 13

## Estrutura do Projeto

```
Emocionario.Api/
├── Endpoints/
│   └── UsuariosEndpoints.cs
├── Middleware/
│   └── ExceptionHandlerMiddleware.cs
├── Validators/
│   ├── CriarUsuarioDtoValidator.cs
│   └── AtualizarUsuarioDtoValidator.cs
└── Program.cs
```

## Execução

### Pré-requisitos

- .NET 9.0 SDK

### Comandos

```bash
cd src/Emocionario.Api
dotnet run
```

A aplicação estará disponível em:
- HTTP: `http://localhost:5089`
- Documentação (Scalar): `http://localhost:5089/scalar/v1`
- OpenAPI Spec: `http://localhost:5089/openapi/v1.json`

## Endpoints

### Health Check

```http
GET /health
```

Verifica o status de saúde da API.

**Resposta 200:**
```json
{
  "status": "Healthy",
  "timestamp": "2026-01-09T19:20:34.392Z",
  "application": "Emocionario API",
  "version": "1.0.0"
}
```

---

### Usuários

Base URL: `/api/usuarios`

#### Criar Usuário

```http
POST /api/usuarios
Content-Type: application/json

{
  "nome": "Joao",
  "sobrenome": "Silva",
  "email": "joao.silva@exemplo.com",
  "dataNascimento": "1990-05-15"
}
```

**Validações:**
- Nome: 3-50 caracteres, apenas letras
- Sobrenome: 3-50 caracteres, apenas letras
- Email: formato válido, máximo 255 caracteres, único
- Data de Nascimento: não pode ser futura (opcional)

**Resposta 201:**
```json
{
  "id": "8dbed6fc-d98f-421f-bdac-df7c2a77f30a",
  "nome": "Joao",
  "sobrenome": "Silva",
  "email": "joao.silva@exemplo.com",
  "dataNascimento": "1990-05-15",
  "criadoEm": "2026-01-09T19:22:50.408Z",
  "atualizadoEm": null
}
```

**Resposta 400 (Validação):**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Nome": ["O nome deve ter entre 3 e 50 caracteres."],
    "Email": ["O email deve ser um endereço válido."]
  }
}
```

**Resposta 409 (Email duplicado):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Operação inválida",
  "status": 409,
  "detail": "Já existe um usuário com este email."
}
```

---

#### Obter Usuário por ID

```http
GET /api/usuarios/{id}
```

**Parâmetros:**
- `id` (path): GUID do usuário

**Resposta 200:**
```json
{
  "id": "8dbed6fc-d98f-421f-bdac-df7c2a77f30a",
  "nome": "Joao",
  "sobrenome": "Silva",
  "email": "joao.silva@exemplo.com",
  "dataNascimento": "1990-05-15",
  "criadoEm": "2026-01-09T19:22:50.408Z",
  "atualizadoEm": null
}
```

**Resposta 404:**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Usuário não encontrado",
  "status": 404,
  "detail": "Nenhum usuário encontrado com o ID 8dbed6fc-d98f-421f-bdac-df7c2a77f30a."
}
```

---

#### Obter Usuário por Email

```http
GET /api/usuarios/email/{email}
```

**Exemplo:**
```http
GET /api/usuarios/email/joao.silva@exemplo.com
```

**Resposta 200:**
```json
{
  "id": "8dbed6fc-d98f-421f-bdac-df7c2a77f30a",
  "nome": "Joao",
  "sobrenome": "Silva",
  "email": "joao.silva@exemplo.com",
  "dataNascimento": "1990-05-15",
  "criadoEm": "2026-01-09T19:22:50.408Z",
  "atualizadoEm": null
}
```

**Resposta 404:**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Usuário não encontrado",
  "status": 404,
  "detail": "Nenhum usuário encontrado com o email 'joao.silva@exemplo.com'."
}
```

---

#### Atualizar Usuário

```http
PUT /api/usuarios/{id}
Content-Type: application/json

{
  "id": "8dbed6fc-d98f-421f-bdac-df7c2a77f30a",
  "nome": "Joao Pedro",
  "sobrenome": "Silva Santos",
  "dataNascimento": "1990-05-15"
}
```

**Observações:**
- O `id` no corpo deve corresponder ao `id` na URL
- Todos os campos são opcionais (atualização parcial)
- O email não pode ser alterado
- O campo `atualizadoEm` é atualizado automaticamente

**Validações:**
- Nome: 3-50 caracteres, apenas letras (se fornecido)
- Sobrenome: 3-50 caracteres, apenas letras (se fornecido)
- Data de Nascimento: não pode ser futura (se fornecido)

**Resposta 204:** Sem corpo

**Resposta 400 (ID incompatível):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "ID incompatível",
  "status": 400,
  "detail": "O ID fornecido na rota não corresponde ao ID no corpo da requisição."
}
```

**Resposta 404:**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Usuário não encontrado",
  "status": 404,
  "detail": "Nenhum usuário encontrado com o ID 8dbed6fc-d98f-421f-bdac-df7c2a77f30a."
}
```

---

#### Excluir Usuário

```http
DELETE /api/usuarios/{id}
```

**Parâmetros:**
- `id` (path): GUID do usuário

**Resposta 204:** Sem corpo

**Resposta 404:**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Usuário não encontrado",
  "status": 404,
  "detail": "Nenhum usuário encontrado com o ID 8dbed6fc-d98f-421f-bdac-df7c2a77f30a."
}
```

---

## Tratamento de Erros

A API implementa middleware global de tratamento de exceções, retornando respostas padronizadas no formato Problem Details (RFC 7807/9110).

### Códigos de Status

| Status | Tipo | Descrição |
|--------|------|-----------|
| 400 | Bad Request | Dados de entrada inválidos |
| 404 | Not Found | Recurso não encontrado |
| 409 | Conflict | Conflito de dados |
| 500 | Internal Server Error | Erro inesperado |

### Formato de Resposta de Erro

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Título do erro",
  "status": 400,
  "detail": "Descrição detalhada do erro",
  "timestamp": "2026-01-09T19:20:34.392Z"
}
```

Para erros de validação (400), o campo `errors` contém os detalhes:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Nome": ["O nome deve ter entre 3 e 50 caracteres."],
    "Email": ["O email deve ser um endereço válido."]
  }
}
```

---

## Validação de Dados

A API utiliza FluentValidation para validar dados de entrada de forma declarativa.

### CriarUsuarioDto
- **Nome**: Obrigatório, 3-50 caracteres, apenas letras
- **Sobrenome**: Obrigatório, 3-50 caracteres, apenas letras
- **Email**: Obrigatório, formato válido, máximo 255 caracteres
- **DataNascimento**: Opcional, não pode ser futura

### AtualizarUsuarioDto
- **Id**: Obrigatório (GUID)
- **Nome**: Opcional, 3-50 caracteres se fornecido, apenas letras
- **Sobrenome**: Opcional, 3-50 caracteres se fornecido, apenas letras
- **DataNascimento**: Opcional, não pode ser futura se fornecido

---

## CORS

A API está configurada para aceitar requisições de qualquer origem em ambiente de desenvolvimento.

Para produção, configure CORS com origens específicas em [Program.cs](Program.cs).

---

## Banco de Dados

A aplicação utiliza Entity Framework Core InMemory Database para desenvolvimento e testes.

### Características:
- Não requer instalação de banco de dados
- Dados são perdidos ao reiniciar a aplicação
- Não recomendado para produção

### Migração para Produção

Para utilizar banco de dados persistente, modifique [DependencyInjection.cs](../Emocionario.Infrastructure/DependencyInjection.cs):

```csharp
// InMemory (atual):
services.AddDbContext<EmocionarioDbContext>(options =>
    options.UseInMemoryDatabase("EmocionarioDB"));

// SQL Server (exemplo):
services.AddDbContext<EmocionarioDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
```

---

## Exemplos com cURL

### Criar Usuário
```bash
curl -X POST "http://localhost:5089/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria",
    "sobrenome": "Santos",
    "email": "maria.santos@exemplo.com",
    "dataNascimento": "1995-08-20"
  }'
```

### Buscar Usuário por ID
```bash
curl -X GET "http://localhost:5089/api/usuarios/{id}" \
  -H "Accept: application/json"
```

### Buscar Usuário por Email
```bash
curl -X GET "http://localhost:5089/api/usuarios/email/maria.santos@exemplo.com" \
  -H "Accept: application/json"
```

### Atualizar Usuário
```bash
curl -X PUT "http://localhost:5089/api/usuarios/{id}" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "{id}",
    "nome": "Maria Clara",
    "sobrenome": "Santos Silva"
  }'
```

### Excluir Usuário
```bash
curl -X DELETE "http://localhost:5089/api/usuarios/{id}"
```

---

## Padrões Implementados

- Clean Architecture
- Domain-Driven Design (DDD)
- Minimal APIs
- Dependency Injection
- Repository Pattern
- FluentValidation
- Global Exception Handling
- Problem Details (RFC 7807)
- OpenAPI/Swagger
- Async/Await
- Value Objects imutáveis
- Factory Methods
