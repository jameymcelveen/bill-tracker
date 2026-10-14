# BillTracker

![CI](https://img.shields.io/github/actions/workflow/status/jameymcelveen/bill-tracker/ci.yml?branch=main)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![React 19](https://img.shields.io/badge/React-19-149ECA)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1)
![Deploy: Vercel](https://img.shields.io/badge/Deploy:%20Vercel-000000)
![Deploy: Railway](https://img.shields.io/badge/Deploy:%20Railway-0B0D0E)

TODO: replace this sentence with what BillTracker does for the people who use it.

## Start here

| You are | Read this | It tells you |
|---|---|---|
| A stakeholder | The stories in [docs/stories](docs/stories/) | What people get, and why. |
| A product owner | Each story's scenarios and open questions | What "done" means, and what is still undecided. |
| A lead engineer | [Architecture](docs/architecture/overview.md), [decisions](docs/decisions/), [the contract](contracts/openapi.yaml) | How it is built and why, with the numbers. |
| Anyone wondering about AI | [How I built this](docs/how-i-built-this.md) | Who wrote what, and how it was checked. |

The method: [Story, Data, Architecture](docs/guides/story-data-architecture.md).
The words: [glossary](docs/glossary.md).

## Run it

Needs the .NET 10 SDK, Node 22 or later, Python 3, and Docker.

```
make setup     # once: installs the web dependencies
make check     # everything CI runs
```

## Run locally

```
docker compose up -d db
dotnet run --project src/BillTracker.Api
cd web && npm run dev
```

`dotnet run` is Development, so the API applies EF migrations on startup and listens on
http://localhost:5080. The web app proxies `/api` to that port. `GET /api/health` returns
`{ "status": "ok" }`.

## Deploy

**Railway (API).** `railway.json` builds `src/BillTracker.Api/Dockerfile` and checks
`/api/health`. Add the Railway PostgreSQL plugin and set `ConnectionStrings__Default` from
it. Migrations run on startup only when `ASPNETCORE_ENVIRONMENT=Development`; production
needs `dotnet ef database update` (or a release command) before traffic. Set `Cors__Origins__0`
to the Vercel origin. The API reads allowed origins from `Cors:Origins`.

**Vercel (web).** `web/vercel.json` builds the Vite app (`dist`) and rewrites every route to
`/index.html`. Set the Vercel root directory to `web`, and set `VITE_API_URL` to the Railway
API URL. That value is baked in at build time.

Variables are listed in `.env.example`. Never commit real secrets.

## How the work is organized

- `docs/stories/` the stories and their scenarios: the test plan (human lane)
- `contracts/openapi.yaml` the contract: the law (human lane)
- `src/BillTracker.Domain/` the domain: pure C#, no frameworks (human lane)
- `src/BillTracker.Api/` the API: one endpoint module per feature
- `web/` the React app; its API types are generated from the contract
- `content/` curated content (human lane)
- `docs/decisions/` decision records
- `AGENTS.md` the lanes and rules every agent follows

## First steps

1. Read `AGENTS.md` and `.claude/settings.json`: the lanes and the reins.
2. Write the first story from `docs/templates/story.md`.
3. Add its path to `contracts/openapi.yaml`, then `make gen-api`.
4. `make check`, and commit.
