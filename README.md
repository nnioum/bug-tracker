# Bug Tracker

Система трекинга багов в студенческих проектах. Команда №22.

## Стек

- Бэкенд: C# (.NET 8), ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- Фронтенд: React + Vite
- База данных: PostgreSQL в Docker Compose

## Структура репозитория

- `backend/` — серверная часть (ASP.NET Core)
- `frontend/` — веб-клиент (React)
- `docs/` — диаграммы и документация (в том числе «до и после»)
- `docker-compose.yml` — PostgreSQL
- `.env.example` — пример переменных окружения

## Как запустить

### База данных

1. Скопируйте `.env.example` в `.env` и задайте свой пароль.
2. Запустите Docker Desktop.
3. В корне репозитория выполните: `docker compose up -d`

### Бэкенд

1. Откройте папку `backend/` в Rider.
2. Запустите проект API.
3. Swagger открывается по адресу, который покажет Rider при запуске (обычно `/swagger`).

### Фронтенд

1. Перейдите в `frontend/`.
2. Выполните `npm install`, затем `npm run dev`.

## Работа с ветками

- `main` — стабильная ветка, напрямую не пушим, только через pull request.
- `feature/<название>` — новая функциональность, например `feature/tickets-crud`.
- `fix/<название>` — исправление ошибки, например `fix/login-error`.

Каждый pull request проверяет минимум один человек из команды.
