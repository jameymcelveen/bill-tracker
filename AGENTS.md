# Agent instructions

This file is the single source of truth for any coding agent (Claude Code, Cursor, others)
and for humans. `CLAUDE.md` imports it.

## The method: Story, Data, Architecture

1. **Story.** Actor, action, purpose. Lives in `docs/stories/`. Each story has scenarios in
   Given/When/Then, each with an ID such as `AB-1`. The scenarios are the test plan.
2. **Data.** What the actor needs and how it reaches them. Lives in `contracts/openapi.yaml`.
   The contract is the law.
3. **Architecture.** Falls out of the first two. Decisions live in `docs/decisions/`.

`docs/guides/story-data-architecture.md` explains the method and the endpoint archetypes.

## Lanes: who writes where

| Lane | Paths | Rule |
|---|---|---|
| Human | `contracts/`, `src/BillTracker.Domain/`, `content/`, `docs/stories/` | Agents never edit. Propose a change in chat instead. |
| Generator | `web/src/api/generated/` | Never edited by hand or by an agent. Run `make gen-api`. |
| Agent, reviewed | `src/BillTracker.Api/`, `tests/`, `web/src/` (except generated), `docs/guides/` | Agents write. The human reviews every change before it merges. |
| Decisions | `docs/decisions/` | Agents draft only when asked. The human rewrites in their own words and accepts. |

Claude Code enforces the human lane with deny rules in `.claude/settings.json`. Every other
agent honors this table by convention.

## First step of every task

1. Run `git remote -v` and confirm you are in the repo you think you are in.
2. Find the story and scenario IDs the task serves. No scenario, no code: ask for one.
3. Work on a branch. Never commit to `main`.

## Rules

1. Every scenario ID in `docs/stories/` has at least one test that names it:
   `[Trait("Scenario", "AB-1")]` in C#, or a test title that starts with `AB-1:` in
   TypeScript. `make trace` checks this.
2. Tests may be added. A test is never deleted, skipped or loosened to make code pass. If a
   test looks wrong, stop and say so. The only skips allowed are human-lane drills, and only
   the human removes them.
3. Every HTTP path is in the contract first. The contract test fails if the API serves a
   path the contract does not list, or the contract lists a path the API does not serve.
4. The Domain project references nothing but the base class library. The architecture test
   enforces it.
5. Endpoints live in feature folders as `IEndpointModule` classes. `Program.cs` never changes
   to add a feature.
6. Plain ASCII in code, configuration and the contract: a curly quote or a non-breaking
   space there is a bug, and AI tools produce them. Docs and art may use any character.
   `make ascii` checks it.
7. No secrets, personal data or real member data anywhere in the repo.
8. Small commits with real messages. The history is part of the work.

## Definition of done

`make check` passes: build, tests, web tests, web build, generated code current, ASCII,
traceability. The pull request says what was verified by running it and what was not.
