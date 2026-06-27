# Teste Prático – Desenvolvedor C#

## Objetivo
Aplicação web em C# que permite ao usuário realizar login e gerenciar um CRUD de endereços, com integração à API do ViaCEP e exportação para CSV.

## Requisitos Implementados

### Tela de Login
- Autenticação de usuário
- Validação de credenciais
- Redirecionamento para a página de endereços após login bem-sucedido

### CRUD de Endereços
- Adicionar, visualizar, editar e excluir endereços
- Campos: CEP, logradouro, complemento (opcional), bairro, cidade, UF, número
- Busca automática de endereço pelo CEP via API ViaCEP
- Exportação dos endereços para arquivo CSV

### Banco de Dados
- Tabela `Usuarios`: Id, Nome, Usuario, Senha
- Tabela `Enderecos`: Id, Cep, Logradouro, Complemento, Bairro, Cidade, Uf, Numero, UsuarioId
- Scripts de criação disponíveis na pasta `Scripts/`

## Tecnologias Utilizadas
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core
- SQL Server
- HTML, CSS, JavaScript
- Bootstrap 5
- API ViaCEP

## Como Executar

### Pré-requisitos
- Visual Studio 2022
- SQL Server
- .NET 10 SDK

### Passos
1. Clone o repositório
2. Execute o script `Scripts/CriarBanco.sql` no SQL Server Management Studio
3. Ajuste a connection string no `appsettings.json`
4. Rode o projeto no Visual Studio (`F5`)
5. Acesse com usuário `admin` e a senha `123456`