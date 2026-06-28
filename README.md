Teste Prático – Desenvolvedor C#

Objetivo

Aplicação web em C# que permite ao usuário realizar login e gerenciar um CRUD de endereços, com integração à API do ViaCEP e exportação para CSV.

Requisitos Implementados

Tela de Login


Autenticação de usuário com senha criptografada via BCrypt
Validação de credenciais
Proteção contra CSRF com AntiForgeryToken
Redirecionamento para a página de endereços após login bem-sucedido


CRUD de Endereços


Adicionar, visualizar, editar e excluir endereços
Campos: CEP, logradouro, complemento (opcional), bairro, cidade, UF, número
Busca automática de endereço pelo CEP via API ViaCEP (aceita formato 01310100 ou 01310-100)
Exportação dos endereços para arquivo CSV formatado com suporte a acentos


Banco de Dados


Tabela Usuarios: Id, Nome, Usuario, Senha
Tabela Enderecos: Id, Cep, Logradouro, Complemento, Bairro, Cidade, Uf, Numero, UsuarioId
Scripts de criação disponíveis na pasta Script_Banco_SQL/


Tecnologias Utilizadas


ASP.NET Core MVC (.NET 10)
Entity Framework Core
SQL Server
HTML, CSS, JavaScript
Bootstrap 5
API ViaCEP
BCrypt.Net para criptografia de senhas


Arquitetura

O projeto segue separação de responsabilidades com camada de serviços:

Controllers/   → orquestra as requisições
Services/      → regras de negócio
  IEnderecoService / EnderecoService
  IViaCepService / ViaCepService
  ICsvExportService / CsvExportService
  IAccountService / AccountService
Data/          → contexto do Entity Framework
Models/        → entidades do sistema

Boas Práticas Aplicadas


Senhas criptografadas com BCrypt
Proteção CSRF em todos os formulários POST
Operações assíncronas com async/await
Try/catch nos endpoints
Separação de responsabilidades (Services, Controllers, Models)


Como Executar

Pré-requisitos


Visual Studio 2022
SQL Server
.NET 10 SDK


Passos


Clone o repositório
Execute o script Script_Banco_SQL/CriarBanco.sql no SQL Server Management Studio
Ajuste a connection string no appsettings.json
Rode o projeto no Visual Studio (F5)
Acesse com usuário admin e a senha 123456