# 🏍 MottoMap API

Sistema de Gestão Inteligente de Frota de Motos - API REST completa desenvolvida em .NET 8.0 com Machine Learning, Entity Framework Core e Oracle Database.

OBS: O projeto de testes com xUnit foi enviado separadamente em um .zip pois tivemos problemas com o Visual Studio para integrá-lo onde não conseguimos superar. Portanto foi enviada aqui para o github uma versão onde a aplicação principal da API realiza o build corretamente e o Testes.zip para que, se possível, tenha alguma validação. Desde já agradeço a compreensão :)

## 👨‍💻 Equipe de Desenvolvimento

- **RM555997** - Caio Marques
- **RM558640** - Caio Amarante
- **RM556325** - Felipe Camargo

## 📋 Visão Geral

A **MottoMap API** é uma solução completa e inteligente para gerenciamento de frotas de motocicletas, oferecendo recursos avançados de CRUD, **Machine Learning para previsão de manutenção**, paginação, filtros, HATEOAS e segurança com autenticação via API Key.

### 🔑 Funcionalidades Principais

- 🏢 **Gestão de Filiais**: Controle de unidades por cidade/estado
- 👩‍💻 **Gestão de Funcionários**: Cadastro com validação de email único
- 🏍️ **Gestão de Motos**: Controle de frota com placas antigas e Mercosul
- 🤖 **Machine Learning**: Previsão inteligente de manutenção com ML.NET
- 🔐 **Segurança**: Autenticação via API Key
- 🏥 **Health Checks**: Monitoramento da saúde da aplicação e banco de dados
- 🔗 **Relacionamentos**: Filiais ↔ Funcionários e Motos
- 🔍 **Filtros Avançados**: Busca por múltiplos critérios
- 📊 **Estatísticas**: Relatórios de ocupação por filial
- 🔄 **Versionamento**: API versionada (v1.0)

## 🏛️ Arquitetura

### Padrões Implementados
- **Repository Pattern** para acesso a dados
- **DTO Pattern** para transferência de dados
- **Mapper Pattern** para conversões
- **HATEOAS** para descoberta de recursos
- **RESTful API** com status codes apropriados
- **Middleware Pattern** para autenticação
- **Dependency Injection** para gerenciamento de dependências

### Tecnologias Utilizadas
- **.NET 8.0** - Framework principal
- **Entity Framework Core 9.0** - ORM
- **Oracle Database** - Banco de dados
- **ML.NET 3.0** - Machine Learning para previsões
- **Swagger/OpenAPI** - Documentação da API
- **ASP.NET Core Health Checks** - Monitoramento
- **C# 12** - Linguagem de programação

## 📚 Modelo de Dados

### Entidades Principais

#### 🏢 Filiais (`NET_C3_Filial`)
```csharp
- IdFilial (PK, Identity)
- Nome (required, max 100 chars)
- Endereco (required, max 200 chars)
- Cidade (required, max 80 chars)
- Estado (required, 2 chars, maiúsculo)
- CEP (optional, max 10 chars, formato: 00000-000)
```

#### 🧑‍💼 Funcionários (`NET_C3_Funcionario`)
```csharp
- IdFuncionario (PK, Identity)
- Nome (required, max 100 chars)
- Email (required, max 150 chars, unique, email format)
- IdFilial (FK to Filial)
- Funcao (required, max 80 chars)
```

#### 🏍️ Motos (`NET_C3_Motos`)
```csharp
- IdMoto (PK, Identity)
- Marca (required, max 50 chars)
- Modelo (required, max 80 chars)
- Ano (required, range 1900-2030)
- Placa (required, max 10 chars, unique, formato ABC-1234 ou ABC1D23)
- IdFilial (FK to Filial)
- Cor (optional, max 30 chars)
- Quilometragem (optional, >= 0)
```

### Relacionamentos
- **Filial** 1:N **Funcionários**
- **Filial** 1:N **Motos**

## 🔐 Segurança e Autenticação

### API Key Authentication

Todos os endpoints (exceto `/health` e `/swagger`) requerem autenticação via API Key no header:

```http
X-API-Key: MottoMap2024SecureKey
```

**Exemplo de requisição autenticada:**
```bash
curl -X GET "https://localhost:5000/api/v1/motos" \
  -H "X-API-Key: MottoMap2024SecureKey" \
  -H "accept: application/json"
```

### Códigos de Resposta de Segurança
- **401 Unauthorized**: API Key não fornecida
- **403 Forbidden**: API Key inválida

## 🤖 Machine Learning - Previsão de Manutenção

### Visão Geral
O sistema utiliza **ML.NET** com algoritmo **FastTree Binary Classification** para prever a necessidade de manutenção de motocicletas baseado em:

- 📏 **Quilometragem atual**
- 📅 **Ano de fabricação**
- ⏳ **Idade da moto** (anos desde fabricação)

### Características do Modelo

**Algoritmo**: FastTreeBinaryClassification
- Árvore de decisão rápida
- Alta precisão para dados tabulares
- Treinamento com 25+ exemplos sintéticos

**Métricas Analisadas**:
- Probabilidade de necessidade de manutenção (0-100%)
- Prioridade (ALTA, MÉDIA, BAIXA)
- Quilometragem estimada para próxima revisão
- Recomendações personalizadas

### Lógica de Decisão

| Quilometragem | Idade | Necessita Manutenção? |
|---------------|-------|----------------------|
| < 10.000 km   | 0-1 anos | ❌ Não |
| 10.000-15.000 km | 1-2 anos | ⚠️ Monitorar |
| > 15.000 km   | 2+ anos | ✅ Sim |
| > 25.000 km   | 3+ anos | 🚨 Urgente |

### Resultado da Previsão

```json
{
  "moto": {
    "id": 1,
    "marca": "Honda",
    "modelo": "CG 160 Titan",
    "placa": "ABC-1234",
    "ano": 2020,
    "quilometragem": 25000
  },
  "predicao": {
    "necessitaManutencao": true,
    "probabilidadeManutencao": 92.5,
    "prioridade": "ALTA",
    "recomendacao": "Manutenção URGENTE recomendada! Alta quilometragem e idade da moto.",
    "kmProximaRevisao": 26000
  },
  "timestamp": "2025-11-09T10:30:00Z",
  "links": {
    "self": "/api/v1/maintenanceprediction/moto/1",
    "moto": "/api/v1/motos/1",
    "all-motos": "/api/v1/motos"
  }
}
```

## 🏥 Health Checks

### Endpoints de Monitoramento

#### Health Check Básico
```http
GET /api/v1/health
```

**Resposta:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-09T10:30:00Z",
  "service": "MottoMap API",
  "version": "1.0"
}
```

#### Health Check Detalhado
```http
GET /api/v1/health/detailed
```

**Resposta:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-09T10:30:00Z",
  "service": "MottoMap API",
  "version": "1.0",
  "uptime": "02:15:30",
  "environment": "Production",
  "machineName": "SERVER-01",
  "processorCount": 8,
  "osVersion": "Microsoft Windows NT 10.0.22631.0"
}
```

#### Health Check com UI
```http
GET /health
```

Monitora automaticamente:
- ✅ Status da aplicação
- 🗄️ Conectividade com Oracle Database
- 🔍 Detalhes de saúde dos componentes

## 🌐 Endpoints da API

### Base URL
- **Development**: `https://localhost:5000/api/v1`

### 🤖 Machine Learning (`/maintenanceprediction`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/maintenanceprediction/moto/{id}` | Previsão de manutenção por ID da moto | ✅ |
| GET | `/maintenanceprediction/motos` | Previsão em lote para múltiplas motos | ✅ |
| POST | `/maintenanceprediction/preview` | Previsão simulada sem consultar BD | ✅ |
| GET | `/maintenanceprediction/filial/{idFilial}` | Previsões para todas as motos de uma filial | ✅ |

### 🏥 Health (`/health`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/health` | Health check básico | ❌ |
| GET | `/health/detailed` | Health check detalhado | ❌ |

### 🏢 Filiais (`/filiais`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/filiais` | Lista paginada de filiais | ✅ |
| GET | `/filiais/{id}` | Busca filial por ID | ✅ |
| GET | `/filiais/{id}/detalhes` | Filial com relacionamentos | ✅ |
| GET | `/filiais/{id}/estatisticas` | Estatísticas da filial | ✅ |
| GET | `/filiais/cidade/{cidade}` | Filiais por cidade | ✅ |
| GET | `/filiais/estado/{estado}` | Filiais por estado | ✅ |
| POST | `/filiais` | Criar nova filial | ✅ |
| PUT | `/filiais/{id}` | Atualizar filial | ✅ |
| DELETE | `/filiais/{id}` | Remover filial | ✅ |

### 🧑‍💼 Funcionários (`/funcionarios`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/funcionarios` | Lista paginada de funcionários | ✅ |
| GET | `/funcionarios/{id}` | Busca funcionário por ID | ✅ |
| GET | `/funcionarios/email/{email}` | Busca por email | ✅ |
| GET | `/funcionarios/filial/{idFilial}` | Funcionários por filial | ✅ |
| POST | `/funcionarios` | Criar funcionário | ✅ |
| PUT | `/funcionarios/{id}` | Atualizar funcionário | ✅ |
| DELETE | `/funcionarios/{id}` | Remover funcionário | ✅ |

### 🏍️ Motos (`/motos`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/motos` | Lista paginada com filtros avançados | ✅ |
| GET | `/motos/{id}` | Busca moto por ID | ✅ |
| GET | `/motos/placa/{placa}` | Busca por placa | ✅ |
| GET | `/motos/filial/{idFilial}` | Motos por filial | ✅ |
| GET | `/motos/marca/{marca}` | Motos por marca | ✅ |
| GET | `/motos/ano/{ano}` | Motos por ano | ✅ |
| POST | `/motos` | Criar moto | ✅ |
| PUT | `/motos/{id}` | Atualizar moto | ✅ |
| DELETE | `/motos/{id}` | Remover moto | ✅ |

## 🔧 Parâmetros de Consulta

### Paginação (Todos os endpoints GET de lista)
```
?pageNumber=1&pageSize=10&searchTerm=termo&sortBy=campo&sortDirection=asc
```

### Filtros Específicos (Motos)
```
?marca=Honda&ano=2023&quilometragemMin=1000&quilometragemMax=50000&idFilial=1
```

## 💻 Exemplos de Uso

### 🤖 Previsão de Manutenção

#### Prever manutenção para uma moto específica
```bash
GET /api/v1/maintenanceprediction/moto/1
Headers:
  X-API-Key: MottoMap2024SecureKey
```

#### Simulação de previsão (sem consultar BD)
```bash
POST /api/v1/maintenanceprediction/preview
Headers:
  X-API-Key: MottoMap2024SecureKey
  Content-Type: application/json

{
  "quilometragem": 25000,
  "ano": 2020
}
```

#### Previsão em lote para múltiplas motos
```bash
GET /api/v1/maintenanceprediction/motos?ids=1,2,3,4,5
Headers:
  X-API-Key: MottoMap2024SecureKey
```

#### Previsões para todas as motos de uma filial
```bash
GET /api/v1/maintenanceprediction/filial/1
Headers:
  X-API-Key: MottoMap2024SecureKey
```

### 🏍️ Gestão de Motos

#### Criar uma Moto
```bash
POST /api/v1/motos
Headers:
  X-API-Key: MottoMap2024SecureKey
  Content-Type: application/json

{
  "marca": "Honda",
  "modelo": "CG 160 Titan",
  "ano": 2023,
  "placa": "ABC-1234",
  "idFilial": 1,
  "cor": "Vermelha",
  "quilometragem": 5000
}
```

#### Buscar Motos com Filtros
```bash
GET /api/v1/motos?marca=Honda&ano=2023&pageSize=5&sortBy=modelo
Headers:
  X-API-Key: MottoMap2024SecureKey
```

### 🏢 Gestão de Filiais

#### Criar uma Filial
```bash
POST /api/v1/filiais
Headers:
  X-API-Key: MottoMap2024SecureKey
  Content-Type: application/json

{
  "nome": "Filial São Paulo - Centro",
  "endereco": "Rua Augusta, 1000",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01310-100"
}
```

### 👥 Gestão de Funcionários

#### Criar um Funcionário
```bash
POST /api/v1/funcionarios
Headers:
  X-API-Key: MottoMap2024SecureKey
  Content-Type: application/json

{
  "nome": "João Silva Santos",
  "email": "joao.silva@mottomap.com",
  "idFilial": 1,
  "funcao": "Gerente Operacional"
}
```

## ⚙️ Configuração e Instalação

### Pré-requisitos
- .NET 8.0 SDK
- Oracle Database (11g ou superior)
- Visual Studio 2022 ou VS Code

### Configuração do Banco de Dados

1. **Editar `appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=oracle.fiap.com.br:1521/orcl;"
  },
  "ApiKey": "MottoMap2024SecureKey"
}
```

2. **Executar Migrations**:
```bash
dotnet ef database update
```

### Executar a Aplicação

```bash
# Restaurar pacotes
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run
```

A API estará disponível em:
- **HTTPS**: https://localhost:5000
- **Swagger**: https://localhost:5000/swagger
- **Health Check**: https://localhost:5000/health

## 📄 Documentação

### Swagger/OpenAPI
A documentação interativa está disponível em `/swagger` quando a aplicação está em execução.

**Recursos do Swagger**:
- 📚 Documentação completa de todos os endpoints
- 🔐 Suporte para autenticação via API Key
- 🧪 Interface "Try It Out" para testes
- 🗂️ Schemas detalhados dos DTOs
- 📝 Exemplos de payloads
- 🏷️ Organização por tags com emojis:
  - 🤖 Machine Learning
  - 🏥 Health
  - 🏢 Filiais
  - 👥 Funcionários
  - 🏍️ Motos

### Como usar o Swagger com API Key

1. Acesse https://localhost:5000/swagger
2. Clique no botão **"Authorize"** (cadeado) no topo
3. Digite: `MottoMap2024SecureKey`
4. Clique em **"Authorize"**
5. Agora você pode testar todos os endpoints protegidos

## 🛠️ Validações e Regras de Negócio

### Validações Automáticas
- **Email único** por funcionário
- **Placa única** por moto
- **Formato de email** válido
- **Formato de placa** (ABC-1234 ou ABC1D23 Mercosul)
- **Estado** sempre em maiúsculo (2 caracteres)
- **CEP** no formato 00000-000
- **Ano** entre 1900-2030
- **Quilometragem** >= 0

### Relacionamentos
- **Funcionários** devem pertencer a uma filial existente
- **Motos** devem estar alocadas a uma filial existente
- **Filiais** não podem ser removidas se possuem funcionários ou motos

## 🔗 HATEOAS

Todas as respostas incluem links de navegação:

```json
{
  "idMoto": 1,
  "marca": "Honda",
  "modelo": "CG 160 Titan",
  "links": {
    "self": "/api/v1/motos/1",
    "update": "/api/v1/motos/1",
    "delete": "/api/v1/motos/1",
    "filial": "/api/v1/filiais/1",
    "maintenance-prediction": "/api/v1/maintenanceprediction/moto/1",
    "all": "/api/v1/motos"
  }
}
```

## 📈 Status Codes

| Código | Significado | Uso |
|--------|-------------|-----|
| 200 | OK | Busca/Atualização bem-sucedida |
| 201 | Created | Recurso criado com sucesso |
| 204 | No Content | Remoção bem-sucedida |
| 400 | Bad Request | Dados inválidos |
| 401 | Unauthorized | API Key não fornecida |
| 403 | Forbidden | API Key inválida |
| 404 | Not Found | Recurso não encontrado |
| 409 | Conflict | Conflito (email/placa duplicados) |
| 500 | Internal Server Error | Erro interno do servidor |

## 📂 Estrutura do Projeto

```
MottoMap/
    Controllers/           # Controladores da API
        FiliaisController.cs
        FuncionariosController.cs
        MotosController.cs
        MaintenancePredictionController.cs    # 🤖 ML Controller
        HealthController.cs                    # 🏥 Health Checks
    Models/                # Entidades do banco de dados
        FilialEntity.cs
        FuncionarioEntity.cs
        MotosEntity.cs
        DataPage.cs        # Modelo de paginação
    DTOs/                  # Objetos de transferência de dados
        Common/
        Filial/
        Funcionario/
        Motos/
    Mappers/               # Conversores Entity ↔ DTO
        FilialMapper.cs
        FuncionarioMapper.cs
        MotoMapper.cs
        PaginationMapper.cs
    Services/              # Serviços de negócio
        MotoMaintenancePredictionService.cs   # 🤖 Serviço ML.NET
    Data/
        AppData/           # Contexto do banco
            ApplicationContext.cs
        Repository/        # Repositórios de acesso a dados
            IRepository.cs
            BaseRepository.cs
            IFilialRepository.cs
            FilialRepository.cs
            IFuncionarioRepository.cs
            FuncionarioRepository.cs
            IMotosRepository.cs
            MotosRepository.cs
    Middleware/            # Middlewares customizados
        ApiKeyMiddleware.cs                    # 🔐 Autenticação
    Migrations/            # Migrações do Entity Framework
    wwwroot/               # Arquivos estáticos
        swagger-ui/
            custom.css     # CSS customizado do Swagger
```

## 🧪 Testando a API

### Usar Swagger UI (Recomendado)
1. Execute a aplicação
2. Acesse https://localhost:5000/swagger
3. Clique em **"Authorize"** e insira: `MottoMap2024SecureKey`
4. Use a interface "Try It Out" para testar endpoints

### Usar cURL

```bash
# Health Check (sem autenticação)
curl -X GET "https://localhost:5000/api/v1/health"

# Listar motos (com autenticação)
curl -X GET "https://localhost:5000/api/v1/motos" \
  -H "X-API-Key: MottoMap2024SecureKey"

# Previsão de manutenção
curl -X GET "https://localhost:5000/api/v1/maintenanceprediction/moto/1" \
  -H "X-API-Key: MottoMap2024SecureKey"

# Criar moto
curl -X POST "https://localhost:5000/api/v1/motos" \
  -H "X-API-Key: MottoMap2024SecureKey" \
  -H "Content-Type: application/json" \
  -d '{
    "marca": "Honda",
    "modelo": "CG 160",
    "ano": 2023,
    "placa": "ABC-1234",
    "idFilial": 1,
    "quilometragem": 5000
  }'
```

### Usar Postman

1. **Criar uma collection**
2. **Adicionar variável de ambiente**:
   - Key: `apiKey`
   - Value: `MottoMap2024SecureKey`
3. **Configurar Authorization**:
   - Type: API Key
   - Key: `X-API-Key`
   - Value: `{{apiKey}}`
4. **Importar Swagger**: File → Import → Link: `https://localhost:5000/swagger/v1/swagger.json`

## 🎯 Casos de Uso - Machine Learning

### Caso 1: Manutenção Preventiva
**Cenário**: Frota de 50 motos, identificar quais precisam manutenção urgente

```bash
GET /api/v1/maintenanceprediction/filial/1
```

**Resultado**: Lista ordenada por prioridade (ALTA → MÉDIA → BAIXA)

### Caso 2: Planejamento de Orçamento
**Cenário**: Estimar quantas motos precisarão manutenção no próximo trimestre

```bash
GET /api/v1/maintenanceprediction/motos?ids=1,2,3,4,5,6,7,8,9,10
```

**Análise**: Contar quantas têm `necessitaManutencao: true`

### Caso 3: Simulação Antes da Compra
**Cenário**: Avaliar se uma moto usada precisará manutenção imediata

```bash
POST /api/v1/maintenanceprediction/preview
{
  "quilometragem": 35000,
  "ano": 2019
}
```

**Decisão**: Se probabilidade > 70%, considerar custo de manutenção na negociação

## 📊 Métricas e Monitoramento

### Endpoints de Monitoramento

| Endpoint | Propósito | Acesso |
|----------|-----------|--------|
| `/health` | UI de Health Checks | Público |
| `/api/v1/health` | Health check básico JSON | Público |
| `/api/v1/health/detailed` | Métricas detalhadas do sistema | Público |

## 📦 Pacotes NuGet Principais

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.9" />
<PackageReference Include="Oracle.EntityFrameworkCore" Version="9.23.90" />
<PackageReference Include="Microsoft.ML" Version="3.0.1" />
<PackageReference Include="Microsoft.ML.FastTree" Version="3.0.1" />
<PackageReference Include="AspNetCore.HealthChecks.UI.Client" Version="8.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="5.1.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.5" />
```

## 🔄 Versionamento da API

A API utiliza versionamento por URL:

```
/api/v1/motos          # Versão 1.0
/api/v2/motos          # Versão 2.0 (futura)
```

**Configuração atual**: v1.0 como padrão

## 🎓 Aprendizados e Destaques

### Implementações Avançadas
1. ✅ **Machine Learning integrado** com ML.NET para previsões em tempo real
2. ✅ **Segurança robusta** com middleware de autenticação customizado
3. ✅ **Monitoramento completo** com health checks do ASP.NET Core
4. ✅ **Arquitetura escalável** com Repository Pattern e DI
5. ✅ **API RESTful completa** seguindo melhores práticas
6. ✅ **Documentação interativa** com Swagger customizado

### Diferenciais do Projeto
- 🤖 Previsão de manutenção preditiva com IA
- 🔐 Autenticação via API Key
- 🏥 Monitoramento de saúde da aplicação
- 🔄 Versionamento de API
- 📊 Métricas detalhadas do sistema
- 🎨 Interface Swagger customizada

---

<div align="center">

**🚀 MottoMap API - Gestão Inteligente de Frotas com Machine Learning**

*Desenvolvido com 💻 usando .NET 8.0, ML.NET & Oracle Database*

**Sprint 4 - 2025**

</div>
