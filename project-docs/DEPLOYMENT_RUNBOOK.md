# The League - Deployment Runbook

## Overview

This document provides comprehensive deployment procedures for The League platform. It covers development setup, staging deployment, production deployment, and operational procedures.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Development Environment](#development-environment)
3. [Build Process](#build-process)
4. [Deployment Environments](#deployment-environments)
5. [Database Migrations](#database-migrations)
6. [Production Deployment](#production-deployment)
7. [Rollback Procedures](#rollback-procedures)
8. [Monitoring & Alerting](#monitoring--alerting)
9. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Development Machine Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| OS | Windows 10 / macOS / Linux | Windows 11 / macOS Ventura |
| RAM | 8 GB | 16 GB |
| Storage | 10 GB free | SSD with 50 GB free |
| CPU | 4 cores | 8 cores |

### Required Software

| Software | Version | Installation |
|----------|---------|--------------|
| .NET SDK | 8.0+ | [Download](https://dotnet.microsoft.com/download) |
| Node.js | 20 LTS | [Download](https://nodejs.org/) |
| SQL Server | 2019+ or LocalDB | Visual Studio Installer |
| Git | 2.40+ | [Download](https://git-scm.com/) |
| Visual Studio 2022 | 17.8+ | [Download](https://visualstudio.microsoft.com/) |
| VS Code | Latest | [Download](https://code.visualstudio.com/) |
| Angular CLI | 19.x | `npm install -g @angular/cli` |

### Optional Tools

| Tool | Purpose |
|------|---------|
| Azure CLI | Cloud deployment |
| Docker Desktop | Containerized deployment |
| Postman | API testing |
| Azure Data Studio | Database management |

---

## Development Environment

### Initial Setup

#### 1. Clone Repository

```bash
git clone https://github.com/your-org/LeagueMembershipManagementPortal.git
cd LeagueMembershipManagementPortal
```

#### 2. Backend Setup

```bash
# Restore NuGet packages
dotnet restore

# Build solution
dotnet build

# Verify configuration
# Edit appsettings.json if needed for local database connection
```

**appsettings.json (Development):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TheLeagueDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Secret": "YourDevelopmentSecretKeyAtLeast32Characters!",
    "Issuer": "TheLeague",
    "Audience": "TheLeagueApp",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:7000"
      }
    }
  }
}
```

#### 3. Apply Database Migrations

```bash
cd TheLeague.Infrastructure
dotnet ef database update -s ../TheLeague.Api
```

#### 4. Run Backend

```bash
cd TheLeague.Api
dotnet run
```

API will be available at: `http://localhost:7000`
Swagger UI: `http://localhost:7000/swagger`

#### 5. Frontend Setup

```bash
cd the-league-client

# Install dependencies
npm install

# Start development server
npm start
```

Frontend will be available at: `http://localhost:4200`

### Seed Data

The application automatically seeds demo data on first run, including:
- Super Admin user: `admin@theleague.com` / `Admin123!`
- Demo clubs with sample members, sessions, events
- Demo Club Manager: `manager@willowcreek.com` / `Password123!`
- Demo Member: `member@willowcreek.com` / `Password123!`

---

## Build Process

### Backend Build

```bash
# Debug build
dotnet build --configuration Debug

# Release build
dotnet build --configuration Release

# Publish for deployment
dotnet publish TheLeague.Api -c Release -o ./publish
```

### Frontend Build

```bash
cd the-league-client

# Development build
npm run build

# Production build
npm run build -- --configuration production
```

Output directory: `the-league-client/dist/the-league-client`

### Docker Build (Optional)

**Dockerfile (Backend):**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TheLeague.Api/TheLeague.Api.csproj", "TheLeague.Api/"]
COPY ["TheLeague.Core/TheLeague.Core.csproj", "TheLeague.Core/"]
COPY ["TheLeague.Infrastructure/TheLeague.Infrastructure.csproj", "TheLeague.Infrastructure/"]
RUN dotnet restore "TheLeague.Api/TheLeague.Api.csproj"
COPY . .
WORKDIR "/src/TheLeague.Api"
RUN dotnet build "TheLeague.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheLeague.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheLeague.Api.dll"]
```

```bash
# Build Docker image
docker build -t theleague-api:latest .

# Run container
docker run -d -p 7000:80 \
  -e ConnectionStrings__DefaultConnection="your-connection-string" \
  -e Jwt__Secret="your-jwt-secret" \
  theleague-api:latest
```

---

## Deployment Environments

### Environment Configuration

| Environment | API URL | Database | Purpose |
|-------------|---------|----------|---------|
| Development | localhost:7000 | LocalDB | Local development |
| Staging | staging-api.theleague.com | Azure SQL (staging) | Pre-production testing |
| Production | api.theleague.com | Azure SQL (production) | Live system |

### Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings__DefaultConnection` | Database connection string | Yes |
| `Jwt__Secret` | JWT signing key (min 32 chars) | Yes |
| `Jwt__Issuer` | JWT issuer | Yes |
| `Jwt__Audience` | JWT audience | Yes |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | Yes |

### Azure App Service Configuration

```bash
# Set app settings via Azure CLI
az webapp config appsettings set \
  --name theleague-api \
  --resource-group theleague-rg \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="Server=..." \
    Jwt__Secret="your-production-secret"
```

---

## Database Migrations

### Creating Migrations

```bash
cd TheLeague.Infrastructure

# Create new migration
dotnet ef migrations add MigrationName -s ../TheLeague.Api
```

### Applying Migrations

**Development:**
```bash
dotnet ef database update -s ../TheLeague.Api
```

**Staging/Production:**
```bash
# Generate SQL script for review
dotnet ef migrations script --idempotent -o migration.sql -s ../TheLeague.Api

# Review script, then execute against target database
sqlcmd -S server -d database -i migration.sql
```

### Migration Checklist

- [ ] Review generated migration code
- [ ] Test migration on local database
- [ ] Generate idempotent SQL script
- [ ] Review SQL script for destructive operations
- [ ] Backup production database
- [ ] Apply migration during maintenance window
- [ ] Verify application functionality post-migration

---

## Production Deployment

### Pre-Deployment Checklist

- [ ] All tests passing (unit, integration, E2E)
- [ ] Code reviewed and approved
- [ ] Database backup completed
- [ ] Maintenance window scheduled (if needed)
- [ ] Rollback plan documented
- [ ] Monitoring alerts configured
- [ ] Stakeholders notified

### Deployment Steps

#### 1. Prepare Release

```bash
# Tag release
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0

# Build release artifacts
dotnet publish TheLeague.Api -c Release -o ./artifacts/api

cd the-league-client
npm run build -- --configuration production
```

#### 2. Deploy Database Changes

```bash
# Generate migration script
dotnet ef migrations script --idempotent -o migration.sql

# Apply to production (via secure connection)
# Review carefully before execution
```

#### 3. Deploy Backend (Azure App Service)

```bash
# Using Azure CLI
az webapp deployment source config-zip \
  --name theleague-api \
  --resource-group theleague-rg \
  --src ./artifacts/api.zip

# Or using GitHub Actions / Azure DevOps
```

#### 4. Deploy Frontend (Azure Static Web Apps / CDN)

```bash
# Using Azure CLI
az storage blob upload-batch \
  --destination '$web' \
  --source ./the-league-client/dist/the-league-client \
  --account-name theleaguecdn

# Purge CDN cache
az cdn endpoint purge \
  --name theleague-frontend \
  --profile-name theleague-cdn \
  --resource-group theleague-rg \
  --content-paths "/*"
```

#### 5. Verify Deployment

```bash
# Health check
curl https://api.theleague.com/health

# Smoke test
curl https://api.theleague.com/api/auth/me

# Check logs
az webapp log tail --name theleague-api --resource-group theleague-rg
```

### Post-Deployment Checklist

- [ ] Health endpoints responding
- [ ] Login functionality working
- [ ] Key user journeys verified
- [ ] No error spikes in logs
- [ ] Performance metrics normal
- [ ] Stakeholders notified of completion

---

## Rollback Procedures

### Immediate Rollback (< 5 minutes)

If critical issues are discovered immediately after deployment:

```bash
# Revert to previous deployment slot (if using slots)
az webapp deployment slot swap \
  --name theleague-api \
  --resource-group theleague-rg \
  --slot staging \
  --target-slot production

# Or redeploy previous version
az webapp deployment source config-zip \
  --name theleague-api \
  --resource-group theleague-rg \
  --src ./artifacts/previous-version.zip
```

### Database Rollback

**If migration can be reversed:**
```bash
dotnet ef database update PreviousMigrationName -s ../TheLeague.Api
```

**If migration cannot be reversed:**
1. Restore from backup
2. Redeploy previous application version
3. Investigate and fix issues

### Rollback Checklist

- [ ] Identify rollback trigger (error, performance, business issue)
- [ ] Notify stakeholders of rollback
- [ ] Execute rollback procedure
- [ ] Verify system functionality
- [ ] Document rollback reason
- [ ] Schedule post-mortem

---

## Monitoring & Alerting

### Key Metrics to Monitor

| Metric | Warning Threshold | Critical Threshold |
|--------|-------------------|-------------------|
| Response Time (p95) | > 500ms | > 1000ms |
| Error Rate | > 1% | > 5% |
| CPU Usage | > 70% | > 90% |
| Memory Usage | > 70% | > 90% |
| Database Connections | > 70% pool | > 90% pool |

### Azure Application Insights Configuration

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### Health Checks

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "database")
    .AddCheck("api", () => HealthCheckResult.Healthy());

app.MapHealthChecks("/health");
```

### Alert Configuration (Azure Monitor)

```json
{
  "alertRules": [
    {
      "name": "High Error Rate",
      "condition": "requests/failed > 5%",
      "severity": 1,
      "action": "email-oncall"
    },
    {
      "name": "Slow Response Time",
      "condition": "requests/duration p95 > 1000ms",
      "severity": 2,
      "action": "email-team"
    }
  ]
}
```

---

## Troubleshooting

### Common Issues

#### Issue: Application won't start

**Symptoms:** 500 error on all endpoints

**Diagnosis:**
```bash
# Check logs
az webapp log tail --name theleague-api --resource-group theleague-rg

# Common causes:
# - Missing environment variables
# - Database connection failure
# - Invalid JWT secret
```

**Resolution:**
1. Verify all required environment variables are set
2. Test database connectivity
3. Ensure JWT secret is at least 32 characters

---

#### Issue: Database migration failed

**Symptoms:** Migration script errors

**Diagnosis:**
```bash
# Check migration history
dotnet ef migrations list -s ../TheLeague.Api
```

**Resolution:**
1. Review migration SQL for errors
2. Check for schema conflicts
3. Restore from backup if needed

---

#### Issue: Authentication failures

**Symptoms:** 401 Unauthorized on all protected endpoints

**Diagnosis:**
```bash
# Check JWT configuration
# Verify issuer, audience match between API and tokens
```

**Resolution:**
1. Verify JWT Secret matches across environments
2. Check token expiration
3. Verify Issuer and Audience settings

---

#### Issue: CORS errors in browser

**Symptoms:** "Access-Control-Allow-Origin" errors

**Diagnosis:**
```bash
# Check CORS configuration in Program.cs
# Verify allowed origins include frontend URL
```

**Resolution:**
1. Add frontend URL to CORS policy
2. Ensure CORS middleware is in correct order
3. Clear browser cache

---

### Support Escalation

| Level | Contact | Response Time |
|-------|---------|---------------|
| L1 | Support Team | < 1 hour |
| L2 | Development Team | < 4 hours |
| L3 | Architecture Team | < 8 hours |

---

## Maintenance Procedures

### Scheduled Maintenance Window

**Timing:** Sundays 02:00 - 06:00 UTC

**Procedure:**
1. Enable maintenance mode in system configuration
2. Perform required updates
3. Run database maintenance (index rebuild, statistics)
4. Disable maintenance mode
5. Verify system functionality

### Database Maintenance

```sql
-- Rebuild fragmented indexes
ALTER INDEX ALL ON Members REBUILD;

-- Update statistics
UPDATE STATISTICS Members;

-- Check database integrity
DBCC CHECKDB ('TheLeagueDb');
```

### Log Rotation

Logs are automatically rotated daily with 30-day retention.

```bash
# Manual log cleanup (if needed)
az webapp log download --name theleague-api --resource-group theleague-rg
```

---

## Disaster Recovery

### Backup Strategy

| Data | Frequency | Retention | Location |
|------|-----------|-----------|----------|
| Database | Daily | 30 days | Azure Backup |
| Database | Weekly | 1 year | Azure Backup |
| File Storage | Daily | 30 days | Azure Blob |
| Configuration | On change | Indefinite | Git |

### Recovery Time Objectives

| Scenario | RTO | RPO |
|----------|-----|-----|
| Minor outage | < 1 hour | 0 |
| Database restore | < 4 hours | < 24 hours |
| Full disaster recovery | < 24 hours | < 24 hours |

### Recovery Procedures

**Database Recovery:**
```bash
# List available backups
az sql db list-deleted --server theleague-sql --resource-group theleague-rg

# Restore to point in time
az sql db restore --dest-name TheLeagueDb-restored \
  --edition Standard \
  --name TheLeagueDb \
  --resource-group theleague-rg \
  --server theleague-sql \
  --time "2024-01-15T10:00:00Z"
```

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
