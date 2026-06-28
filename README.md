# 📍 Gerenciador de Endereços — Teste Prático Desenvolvedor C#

Aplicação web desenvolvida em **ASP.NET Core MVC** que permite ao usuário realizar login e gerenciar um CRUD completo de endereços, com integração à API do ViaCEP, exportação para CSV e boas práticas de segurança e arquitetura.

---

## ✅ Requisitos Implementados

### 🔐 Autenticação
- Login com validação de credenciais
- Senhas criptografadas com **BCrypt**
- Proteção contra CSRF com `AntiForgeryToken` em todos os formulários POST
- Sessão autenticada com redirecionamento automático
- Seed automático do usuário administrador na primeira execução

### 📋 CRUD de Endereços
- Adicionar, visualizar, editar e excluir endereços
- Campos: CEP, Logradouro, Número, Complemento (opcional), Bairro, Cidade, UF
- Busca automática de endereço pelo CEP via **API ViaCEP**
- Aceita CEP com ou sem traço (`01310100` ou `01310-100`)
- CEP exibido sempre no formato `00000-000`

### 📤 Exportação CSV
- Exportação dos endereços do usuário logado
- CEP formatado como `00000-000`
- Arquivo com BOM UTF-8 para correta exibição de acentos no Excel
- Campos entre aspas duplas para evitar quebras por vírgula

### 🗄️ Banco de Dados
- Tabela `Usuarios`: `Id`, `Nome`, `Usuario`, `Senha`
- Tabela `Enderecos`: `Id`, `CEP`, `Logradouro`, `Complemento`, `Bairro`, `Cidade`, `UF`, `Numero`, `UsuarioId`
- Script de criação disponível em `Script_Banco_SQL/CriarBanco.sql`

---

## 🏗️ Arquitetura

O projeto segue separação de responsabilidades com camada de serviços:

```
TesteDevCSharp/
├── Controllers/
│   ├── AccountController.cs      ← autenticação e logout
│   └── EnderecoController.cs     ← orquestra as requisições
├── Services/
│   ├── IAccountService.cs / AccountService.cs       ← autenticação com BCrypt
│   ├── IEnderecoService.cs / EnderecoService.cs     ← CRUD de endereços
│   ├── IViaCepService.cs / ViaCepService.cs         ← integração ViaCEP
│   └── ICsvExportService.cs / CsvExportService.cs  ← exportação CSV
├── Models/
│   ├── Usuario.cs
│   └── Endereco.cs
├── Data/
│   └── AppDbContext.cs
├── Views/
│   ├── Account/Login.cshtml
│   └── Endereco/Index.cshtml, Criar.cshtml, Editar.cshtml
└── Script_Banco_SQL/
    └── CriarBanco.sql
```

---

## 🔒 Boas Práticas Aplicadas

| Prática | Descrição |
|---|---|
| BCrypt | Senhas nunca armazenadas em texto puro |
| AntiForgeryToken | Proteção CSRF em todos os POSTs |
| Async/Await | Operações com banco de dados assíncronas |
| Try/Catch | Tratamento de erros nos endpoints |
| Injeção de Dependência | Serviços registrados e injetados via DI |
| Separação de Responsabilidades | Controllers finos, lógica nos Services |
| Seed Automático | Usuário admin criado na primeira execução |

---

## 🛠️ Tecnologias Utilizadas

- **ASP.NET Core MVC** (.NET 10)
- **Entity Framework Core**
- **SQL Server**
- **BCrypt.Net** — criptografia de senhas
- **Bootstrap 5** — interface responsiva
- **HTML, CSS, JavaScript**
- **API ViaCEP** — consulta de endereços por CEP

---

## 🚀 Como Executar

### Pré-requisitos
- Visual Studio 2022
- SQL Server
- .NET 10 SDK

### Passos

1. Clone o repositório
```bash
git clone https://github.com/EwandeF/Teste-Dev-CSharp.git
```

2. Execute o script de criação do banco no **SQL Server Management Studio**
```
Script_Banco_SQL/CriarBanco.sql
```

3. Ajuste a connection string no `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=TesteDevCSharp;Trusted_Connection=True;"
}
```

4. Rode o projeto no Visual Studio (`F5`)

5. Acesse com as credenciais padrão:
   - **Usuário:** `admin`
   - **Senha:** `123456`

> O usuário administrador é criado automaticamente com senha criptografada na primeira execução.

---

## 📝 Commits por Funcionalidade

O repositório segue o padrão de **um commit por funcionalidade**, conforme exigido no teste:

- `chore: configurar ambiente inicial`
- `feat: configurar Entity Framework`
- `feat: criar estrutura do banco de dados`
- `feat: tela de login com autenticação por sessão`
- `feat: CRUD de endereços com busca por CEP via ViaCEP`
- `feat: adicionar script SQL de criação do banco de dados`
- `feat: adicionar camada de serviços e separar responsabilidades`
- `feat: encriptação de senha com BCrypt`
- `feat: adicionar ValidateAntiForgeryToken e operações assíncronas`
- `feat: seed automático do usuário admin com senha BCrypt`
- `feat: formatar CEP no formato 00000-000 na exportação CSV`