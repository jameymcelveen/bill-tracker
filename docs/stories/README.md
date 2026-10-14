# Stories

HUMAN LANE. One file per story. The scenarios in these files are the test plan, and the human
owns the test plan. Agents write the tests that make each scenario real.

## Format

- Start from `docs/templates/story.md`.
- Give the story a short ID of two to five capital letters, such as `AB`.
- Each scenario is a heading of the form `### AB-1: Title`. `make trace` reads these headings.
- Write scenarios in the reader's language. See `docs/guides/writing-scenarios.md`.
- Put unknowns under "Open questions" with the assumption you are building on. Never bury a
  product decision inside a scenario.
- A story still being shaped lives in `docs/stories/proposed/`. `make trace` ignores that
  folder, so a proposed story never breaks the build. Move it up when you are ready to own
  its scenarios as the test plan.
