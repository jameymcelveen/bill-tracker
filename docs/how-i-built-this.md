# How I built this

An honest account of who wrote what, and how it was checked. Kept current as the work goes.

## What I wrote by hand

- The stories and their scenarios, which are the test plan (`docs/stories/`).
- The contract (`contracts/openapi.yaml`).
- The domain (`src/BillTracker.Domain/`).
- The decisions, in my own words (`docs/decisions/`).

## What tools generated

- This project's starting layout, from my own `jig` templates.
- `web/src/api/generated/schema.d.ts`, from the contract by `openapi-typescript`. Never
  edited by hand. `make gen-api-check` fails if it drifts from the contract.

## What an AI agent wrote, and how I checked it

- (Date: what the agent wrote, and how I checked it.)

## The reins

- The lanes in `AGENTS.md`, enforced for Claude Code by deny rules in `.claude/settings.json`.
- The architecture test keeps frameworks out of the domain.
- The contract test keeps the API and the contract identical.
- The traceability check ties every scenario to a test.
- The generated-code check keeps the client in step with the contract.

## Where to look in the history

Small commits with real messages, never squashed. Read them in order to watch the thinking.
