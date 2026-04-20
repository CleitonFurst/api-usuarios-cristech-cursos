# API Usuarios Curso

Web API desenvolvida com **.NET 8** para estudo e prática de arquitetura REST, autenticação JWT, endpoints CRUD, integração com banco de dados SQL Server, validações e boas práticas no desenvolvimento back-end.

---

## Tecnologias Utilizadas

- **.NET 8** (ASP.NET Core)
- **Entity Framework Core 8** — ORM para mapeamento objeto-relacional
- **SQL Server LocalDB** — Banco de dados relacional
- **Swagger / OpenAPI** — Documentação automática da API
- **JWT Bearer** — Autenticação baseada em tokens
- **HMACSHA512** — Hash de senhas com salt para segurança

## Dependências (NuGet Packages)

| Pacote | Versão |
|---|---|
| Microsoft.EntityFrameworkCore | 8.0.6 |
| Microsoft.EntityFrameworkCore.Design | 8.0.6 |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.6 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.6 |
| Swashbuckle.AspNetCore | 6.6.2 |
| Swashbuckle.AspNetCore.Filters | 8.0.3 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.26 |

---

## Estrutura do Projeto

```
ApiUsuariosCurso/
├── Controllers/
│   ├── LoginController.cs        # Endpoints de autenticação (login e registro)
│   └── UsuarioController.cs      # CRUD de usuários (protegido por [Authorize])
├── DTO/
│   ├── Login/
│   │   └── UsuarioLoginDTO.cs    # DTO para login (Email + Senha)
│   └── Usuario/
│       ├── UsuarioCriacaoDTO.cs  # DTO para criação de usuário
│       └── UsuarioEdicaoDTO.cs   # DTO para edição de usuário
├── Data/
│   └── AppDbContext.cs           # Contexto do Entity Framework
├── Migrations/                   # Migrations do banco de dados
├── Models/
│   ├── ResponseModel.cs          # Modelo para respostas padronizadas da API
│   └── UsuarioModel.cs           # Entidade de usuário
├── Properties/                   # Configurações de publicação
├── Services/
│   ├── Senha/
│   │   ├── ISenhaInterface.cs    # Interface de hash e token
│   │   └── SenhaService.cs       # Hash HMACSHA512 e geração de JWT
│   └── Usuario/
│       ├── IUsuarioInterface.cs  # Interface de serviços de usuário
│       └── UsuarioService.cs     # Lógica de negócio (CRUD + login)
├── ApiUsuariosCurso.csproj
├── ApiUsuariosCurso.http         # Arquivo de requisições HTTP
├── Program.cs                    # Configuração e pipeline da aplicação
├── appsettings.json              # Configurações de produção
└── appsettings.Development.json  # Configurações de desenvolvimento
```

---

## Arquitetura

O projeto segue o padrão **Service-Repository**, com separação de responsabilidades em camadas:

- **Controllers** — Recebem as requisições HTTP e delegam para os serviços. O `UsuarioController` é protegido por `[Authorize]`.
- **Services** — Contêm a regra de negócio, com interfaces (`IUsuarioInterface`, `ISenhaInterface`) para facilitar testes e injeção de dependência.
- **Data** — Camada de acesso a dados com Entity Framework Core.
- **Models** — Entidades que representam as tabelas do banco.
- **DTOs** — Objetos para transferência de dados entre camadas.

---

## Endpoints da API

### Autenticação (`/api/login`) — Sem autenticação

#### POST `/api/login/register`
Registra um novo usuário no sistema.

**Body (JSON):**
```json
{
  "usuario": "string",
  "nome": "string",
  "sobrenome": "string",
  "email": "string",
  "senha": "string",
  "confirmaSenha": "string"
}
```

**Validações:**
- Todos os campos são obrigatórios (`[Required]`)
- `confirmaSenha` deve ser igual ao campo `senha` (`[Compare("Senha")]`)

#### POST `/api/login/login`
Realiza o login de um usuário e retorna um token JWT.

**Body (JSON):**
```json
{
  "email": "string",
  "senha": "string"
}
```

**Resposta de sucesso (200 OK):**
```json
{
  "sucesso": true,
  "mensagem": "Login realizado com sucesso.",
  "dados": {
    "id": 1,
    "usuario": "string",
    "nome": "string",
    "email": "string",
    "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9...",
    "dataCriacao": "2026-04-20T00:00:00"
  }
}
```

---

### Usuários (`/api/usuario`) — Requer autenticação JWT

Todos os endpoints deste controller exigem um token JWT válido no header:
```
Authorization: Bearer {seu_token_jwt}
```

#### GET `/api/usuario`
Lista todos os usuários cadastrados.

**Resposta (200 OK):**
```json
{
  "sucesso": true,
  "mensagem": "Lista de usuários obtida com sucesso.",
  "dados": [
    { "id": 1, "usuario": "...", "nome": "...", "email": "...", ... }
  ]
}
```

#### GET `/api/usuario/{id}`
Obtém um usuário pelo ID.

**Resposta de sucesso (200 OK):**
```json
{
  "sucesso": true,
  "mensagem": "Usuário encontrado.",
  "dados": { "id": 1, "usuario": "...", ... }
}
```

**Resposta quando não encontrado (404):**
```json
{
  "sucesso": false,
  "mensagem": "Usuário não encontrado."
}
```

#### PUT `/api/usuario`
Edita os dados de um usuário.

**Body (JSON):**
```json
{
  "ID": 1,
  "usuario": "string",
  "nome": "string",
  "sobrenome": "string",
  "email": "string"
}
```

**Validações:** Todos os campos são obrigatórios (`[Required]`).

#### DELETE `/api/usuario/{id}`
Exclui um usuário pelo ID.

---

## Modelo de Dados (UsuarioModel)

| Campo | Tipo | Descrição |
|---|---|---|
| Id | `int` | Identificador único (Primary Key) |
| Usuario | `string` | Nome de usuário |
| Nome | `string` | Nome completo |
| Sobrenome | `string` | Sobrenome |
| Email | `string` | E-mail |
| Token | `string` | Token JWT de autenticação |
| DataCriacao | `DateTime` | Data de criação do registro |
| DataAlteracao | `DateTime` | Data da última alteração |
| SenhaHash | `byte[]` | Hash da senha (HMACSHA512) |
| SenhaSalt | `byte[]` | Salt usado no hash |

---

## Configuração do Banco de Dados

O projeto está configurado para usar **SQL Server LocalDB**.

**Connection String (appsettings.json):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ApiUsuariosCursoDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Para aplicar as migrations:
```bash
dotnet ef database update
```

---

## Autenticação JWT

A API utiliza **JWT Bearer** para autenticação. O token é gerado no login e deve ser enviado no header das requisições protegidas:

```
Authorization: Bearer {token_jwt}
```

### Configuração no appsettings.json

```json
"AppSettings": {
  "Token": "MinhaChaveSecreta12345..."
}
```

> **Atenção:** Em ambiente de produção, substitua a chave secreta por uma string forte e mantenha-a em segredo (use variáveis de ambiente ou Azure Key Vault).

---

## Como Executar

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/CleitonFurst/api-usuarios-cristech-cursos.git
   cd api-usuarios-cristech-cursos
   ```

2. **Restaurar os pacotes:**
   ```bash
   dotnet restore
   ```

3. **Aplicar as migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Rodar a aplicação:**
   ```bash
   dotnet run
   ```

5. **Acessar o Swagger:**
   - Desenvolvimento: `https://localhost:7xxx/swagger`

6. **Autenticar no Swagger:**
   - Clique no botão **Authorize** no Swagger UI
   - Insira o token no formato: `Bearer {seu_token_jwt}`

---

## Funcionalidades Implementadas

- [x] Cadastro de usuários com validação de dados
- [x] Login com autenticação JWT
- [x] Hash de senhas com **HMACSHA512** e salt
- [x] Geração de token JWT com claims (Email, Nome)
- [x] CRUD completo de usuários (Listar, Buscar por ID, Editar, Excluir)
- [x] Endpoints protegidos por `[Authorize]`
- [x] Respostas padronizadas da API (ResponseModel)
- [x] Documentação automática com Swagger/OpenAPI
- [x] Injeção de dependência com interfaces
- [x] Entity Framework Core com Code-First
- [x] Migrations para versionamento do banco

---

## Segurança

- Senhas nunca são armazenadas em texto puro — são hashadas com **HMACSHA512** e salt único por usuário.
- Validação de confirmação de senha no DTO de entrada.
- Uso de `[ApiController]` para validação automática de modelos.
- Autenticação baseada em **JWT Bearer** para endpoints protegidos.
- O token JWT expira em 1 dia e contém claims de Email e Nome do usuário.
- A chave secreta do JWT deve ser alterada em produção.

---

## Licença

Este projeto é de uso pessoal para fins educacionais.

---

## Autor

**Cleiton Fürst**
- [GitHub](https://github.com/CleitonFurst)
- [LinkedIn](https://www.linkedin.com/in/cleitonfurst/)
