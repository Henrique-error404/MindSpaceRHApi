# 📊 MindSpace RH API (.NET)

## Gestão de Setores e Métricas de Bem-Estar

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-Oracle-F80000?logo=oracle&logoColor=white)](https://docs.microsoft.com/ef/core/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger&logoColor=black)](https://swagger.io/)
[![Pattern](https://img.shields.io/badge/Pattern-Repository-blue)](https://learn.microsoft.com/)

---

## 1. 🔭 Visão Geral

A **MindSpace RH API** é um microsserviço desenvolvido para o departamento de Recursos Humanos. Seu objetivo é gerenciar a estrutura organizacional (Setores/Departamentos) e centralizar a visualização de métricas agregadas de estresse e bem-estar dos colaboradores.

Este projeto complementa o ecossistema MindSpace, integrando-se ao mesmo banco de dados **Oracle** utilizado pelo backend Java, garantindo consistência de dados.

---

## 2. 🏗️ Decisões Arquiteturais

Para atender aos requisitos de performance e manutenibilidade, as seguintes decisões foram tomadas:

* **Minimal APIs (.NET 8):** Escolhida pela baixa sobrecarga (boilerplate), alta performance e simplicidade para criar endpoints RESTful diretos.
* **Entity Framework Core (Code-First):** Utilizado para mapeamento objeto-relacional (ORM). A abordagem *Code-First* permite versionar o esquema do banco via Migrations.
* **Repository Pattern:** Implementado (`IDepartmentRepository`) para desacoplar a lógica de acesso a dados dos endpoints, facilitando testes e manutenção.
* **Oracle Database Provider:** Utilização do driver oficial `Oracle.EntityFrameworkCore` para conexão nativa com a infraestrutura da FIAP.
* **Design-Time Factory:** Implementação de `IDesignTimeDbContextFactory` para permitir a execução de Migrations em ambiente de desenvolvimento sem conflitos de injeção de dependência.

---

## 3. ⚙️ Configuração e Variáveis de Ambiente

O projeto utiliza o arquivo `appsettings.Development.json` para configuração.

### String de Conexão (Oracle)
Certifique-se de que o arquivo `appsettings.Development.json` contenha a chave `ConnectionStrings` configurada corretamente com o **User Id** e **Password** do seu esquema Oracle.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "User Id=rm561120;Password=130305;Data Source=//oracle.fiap.com.br:1521/ORCL"
  }
}
