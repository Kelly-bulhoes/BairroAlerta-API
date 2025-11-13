## 📘 Visão Geral
O módulo de Alertas da **BairroAlerta API** permite registrar, consultar e transmitir alertas em tempo real utilizando **ASP.NET Core**, **Entity Framework Core** e **SignalR**.

Este documento descreve a estrutura das classes e o funcionamento interno das camadas Model, Data, Service e Hub.

---

## 🧩 1. Model — `Alerta`
Representa uma ocorrência registrada por um usuário.

### Propriedades
- **Id** (int) — Identificador único.
- **Tipo** (string, obrigatório) — Categoria do alerta.
- **Descricao** (string, obrigatório) — Descrição da ocorrência.
- **Usuario** (string, obrigatório) — Nome de quem registrou.
- **CriadoEm** (DateTime) — Preenchido automaticamente.

---

## 🗄️ 2. Data Layer — `AlertaContext`
Gerencia o acesso ao banco usando Entity Framework Core.

### Estrutura
- **DbSet<Alerta> Alertas** — Tabela de alertas.

---

## ⚙️ 3. Services

### 3.1. Interface `IAlertasService`
Contém os métodos:
- `GetAllAsync()`
- `GetByTipoAsync(string tipo)`
- `AddAsync(Alerta alerta)`

### 3.2. Implementação `AlertasService`
Executa operações no banco:
- Buscar todos os alertas
- Filtrar por tipo
- Adicionar novos alertas

---

## 📡 4. SignalR Hub — `AlertaHub`
Envia alertas em tempo real.

### Método:
- `NovoAlerta(Alerta alerta)` → Envia a todos os clientes via `"ReceberAlerta"`.

---

## 🏗️ 5. Program.cs — Configuração da Aplicação

### Serviços configurados:
- `AddScoped<IAlertasService, AlertasService>()`
- `AddDbContext<AlertaContext>(UseInMemoryDatabase)`
- Logging com Console
- Swagger (somente Development)
- SignalR

### Middlewares:
1. HTTPS Redirection  
2. Routing  
3. Authorization  
4. Static Files  
5. Controllers  
6. Hubs (SignalR)

### Mapeamento do Hub:
- `/alertaHub`

### Logs de ciclo de vida:
- ApplicationStarted  
- ApplicationStopping  
- ApplicationStopped  

---

## 🧱 6. Arquitetura do Módulo

Fluxo:
1. A API recebe novo alerta.
2. O serviço salva no banco.
3. O Hub envia o alerta aos clientes conectados.
4. A aplicação atualiza automaticamente sem precisar recarregar.

---

## 📌 Resumo da Estrutura

| Camada | Arquivo | Função |
|-------|---------|--------|
| Model | Alerta.cs | Estrutura dos alertas |
| Data | AppDbContext.cs | Acesso ao banco |
| Service | IAlertasService.cs | Contrato |
| Service Impl. | AlertasService.cs | Regras de negócio |
| SignalR Hub | AlertsHub.cs | Notificações |
| Core | Program.cs | Configuração da aplicação |

---
