# Story, Data, Architecture

How work moves through this repo, from a sentence a stakeholder can read to code a lead
engineer will defend.

## 1. Story

**Actor, action, purpose.** "As a volunteer, I want to see the serving slots still open this
Sunday, so that I can sign up for one."

Each story gets scenarios in Given/When/Then, each with an ID (`SV-1`, `SV-2`). The scenarios
are the test plan. The human owns the test plan; the agent writes the tests that prove it.
That is behavior-driven development at the story level, without a BDD framework: the
scenario IDs carry the traceability instead.

## 2. Data

**What does the actor need to see, and how does it reach them?** Write it in
`contracts/openapi.yaml` before any code: the path, the response shape, an example, the
errors. This is the domain's language made concrete, and it is the law that both the API and
the client are held to.

Every operation declares three extensions:

- `x-story`: the story it serves.
- `x-archetype`: the shape of the data (below). The shape sets the test plan.
- `x-owned`: whether a user may only ever see their own records.

## 3. Architecture

It falls out of the first two. When a choice had a real alternative, write a decision record
that names the story that forced it, shows the numbers, and answers the obvious attacks
before anyone asks.

## Endpoint archetypes

The story decides the shape, not the row count. The same table can serve two shapes because
two stories ask different questions of it.

| Archetype | Example | Test plan highlights |
|---|---|---|
| `single` | One record by key | Found; not found is 404; malformed key is 400. |
| `reference-list` | Countries: fixed, small | Returns all; stable order; cache headers. |
| `scoped-list` | Line items of one order | Parent not found is 404; empty parent is an empty list; bounded size. |
| `paged` | Search results | Default and maximum page size; last and empty page; deterministic sort with a tiebreaker. |
| `feed` | An activity feed that grows | Cursor, not page number; no duplicates or skips while rows are added. |
| `x-owned: true` (modifier) | My notes | Another user's records are never visible or editable. |

The tiebreaker is the classic paging bug: sort by date alone, two rows share a timestamp,
and items flicker between pages. A generic plan catches it every time.

Generic plans cover the shape, never the meaning. Every endpoint also needs its
story-specific scenarios, and that is where the real failures live.

## Lanes

Who writes what is in `AGENTS.md`. In short: deterministic where possible, an agent where
judgment helps, a human where it matters.
