# FilmoSearchPortal

Портал для поиска и обзора фильмов, построенный на связке ASP.NET Core и React.

## Описание

### 1. Бэкенд (ASP.NET Core)
- **URL:** [http://localhost:5010/](http://localhost:5010/)
- **Swagger (API Documentation):** [http://localhost:5010/swagger/index.html](http://localhost:5010/swagger/index.html)

### 2. Фронтенд (React)
- **URL:** [http://localhost:5174/](http://localhost:5174/)

##  Архитектура проекта

- **FilmoSearchPortal**: API Контроллеры и настройки.
- **BusinessLogic**: Сервисы и бизнес-логика.
- **DataAccess**: Контекст базы данных, конфигурации и миграции.
- **Domain**: Сущности и объекты передачи данных.
- **FilmoSearchPortal_Frontend**: Клиентская часть на React.
- **FilmoSearchPortal.Test**: Модульные тесты.

## Docker Support
В проекте настроен `docker-compose.yml` для одновременного запуска всех сервисов.
```bash docker-compose up --build
