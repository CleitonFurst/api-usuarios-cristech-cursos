# API Usuarios Curso

Web API desenvolvida com **.NET 8** para estudo e prática de arquitetura REST, endpoints de autenticação, integração com banco de dados SQL Server, validações e boas práticas no desenvolvimento back-end.

---

## Tecnologias Utilizadas

- **.NET 8** (ASP.NET Core)
- **Entity Framework Core 8** — ORM para mapeamento objeto-relacional
- **SQL Server LocalDB** — Banco de dados relacional
- **Swagger / OpenAPI** — Documentação automática da API
- **HMACSHA512** — Hash de senhas com salt para segurança

## Dependências (NuGet Packages)

| Pacote | Versão |
|---|---|
| Microsoft.EntityFrameworkCore | 8.0.6 |
| Microsoft.EntityFrameworkCore.Design | 8.0.6 |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.6 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.6 |
| Swashbuckle.AspNetCore | 6.6.2 |

---

## Estrutura do Projeto

```
ApiUsuariosCurso/
├── Controllers/
│   └── LoginController.cs       # Endpoints de autenticação e registro
├── DTO/Usuario/
│   └── UsuarioCriacaoDTO.cs     # Data Transfer Object para criação de usuário
├── Data/
│   └── AppDbContext.cs          # Contexto do Entity Framework
├── Migrations/                  # Migrations do banco de dados
├── Models/
│   ├── ResponseModel.cs         # Modelo para respostas padronizadas da API
│   └── UsuarioModel.cs          # Entidade de usuário
├── Properties/                  # Configurações de publicação
├── Services/
│   ├── Senha/
│   │   ├── ISenhaInterface.cs
│   │   └── SenhaService.cs      # Serviço de hash de senhas (HMACSHA512)
│   └── Usuario/
│       ├── IUsuarioInterface.cs
│       └── UsuarioService.cs    # Lógica de negócio do usuário
├── ApiUsuariosCurso.csproj
├── ApiUsuariosCurso.http        # Arquivo de requisições HTTP
├── Program.cs                   # Configuração e pipeline da aplicação
├── appsettings.json             # Configurações de produção
└── appsettings.Development.json # Configurações de desenvolvimento
```

---

## Arquitetura

O projeto segue o padrão **Service-Repository**, com separação de responsabilidades em camadas:

- **Controllers** — Recebem as requisições HTTP e delegam para os serviços.
- **Services** — Contêm a regra de negócio, com interfaces (`IUsuarioInterface`, `ISenhaInterface`) para facilitar testes e injeção de dependência.
- **Data** — Camada de acesso a dados com Entity Framework Core.
- **Models** — Entidades que representam as tabelas do banco.
- **DTOs** — Objetos para transferência de dados entre camadas.

---

## Endpoints da API

### POST `/api/login/register`
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

**Resposta de sucesso (200 OK):**
```json
{
  "sucesso": true,
  "mensagem": "string",
  "dados": {
    "id": 1,
    "usuario": "string",
    "nome": "string",
    "sobrenome": "string",
    "email": "string",
    "token": "string",
    "dataCriacao": "2026-04-20T00:00:00",
    "dataAlteracao": "2026-04-20T00:00:00"
  }
}
```

---

## Modelo de Dados (UsuarioModel)

| Campo | Tipo | Descrição |
|---|---|---|
| Id | `int` | Identificador único (Primary Key) |
| Usuario | `string` | Nome de usuário |
| Nome | `string` | Nome completo |
| Sobrenome | `string` | Sobrenome |
| Email | `string` | E-mail |
| Token | `string` | Token de autenticação |
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

---

## Funcionalidades Implementadas

- [x] Cadastro de usuários com validação de dados
- [x] Hash de senhas com **HMACSHA512** e salt
- [x] Geração de token de autenticação
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

---

## Licença

Este projeto é de uso pessoal para fins educacionais.

---

## Autor

**Cleiton Fürst**
- [GitHub](https://github.com/CleitonFurst)
- [LinkedIn](https://www.linkedin.com/in/cleitonfurst/)
