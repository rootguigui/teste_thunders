# Executando o Projeto no VSCode

Este guia fornece instruções para configurar e executar o projeto Thunders Tech Test no VSCode.

## Pré-requisitos

- [Visual Studio Code](https://code.visualstudio.com/)
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [C# Extension para VSCode](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) ou [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (opcional, se usar message broker)

## Configuração

1. Clone o repositório
2. Abra o projeto no VSCode
3. Instale as extensões recomendadas quando solicitado
4. Configure o arquivo `.env` com suas configurações de banco de dados

## Executando o Projeto

### Método 1: Usando o VSCode

1. Pressione `F5` ou selecione `Run > Start Debugging`
2. Escolha `Launch AppHost` para iniciar o projeto completo
3. Ou escolha `Launch ApiService` para iniciar apenas a API

### Método 2: Usando o Terminal

1. Abra o terminal no VSCode (Ctrl+`)
2. Execute os seguintes comandos:

```bash
# Restaurar pacotes
dotnet restore

# Compilar o projeto
dotnet build

# Executar o projeto
cd Thunders.TechTest.AppHost
dotnet run
```

## Executando Testes

### Método 1: Usando o VSCode

1. Pressione `F5` ou selecione `Run > Start Debugging`
2. Escolha `Launch Tests`
3. Digite o filtro de teste quando solicitado (ex: `FullyQualifiedName~RelatorioTests`)

### Método 2: Usando o Terminal

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~RelatorioTests"
```

## Recursos Adicionais

- **Hot Reload**: Use o comando `dotnet watch run` para habilitar o hot reload
- **Debugging**: Defina breakpoints clicando na margem esquerda do editor
- **IntelliSense**: O VSCode fornece suporte completo de IntelliSense para C#

## Solução de Problemas

- **Erro de conexão com o banco de dados**: Verifique se o SQL Server está em execução e se a string de conexão está correta
- **Erro de compilação**: Execute `dotnet clean` seguido de `dotnet build`
- **Extensões não carregadas**: Reinicie o VSCode

## Estrutura do Projeto

- `Thunders.TechTest.ApiService`: API principal
- `Thunders.TechTest.AppHost`: Projeto de inicialização
- `Thunders.TechTest.OutOfBox`: Componentes prontos para uso
- `Thunders.TechTest.ServiceDefaults`: Configurações padrão
- `Thunders.TechTest.Tests`: Projeto de testes 