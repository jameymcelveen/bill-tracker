# Writing scenarios

Three mistakes show up in almost every first draft of Given/When/Then. Each one hides a
product decision or breaks the moment the code changes.

## 1. The story comes first

A scenario hangs off a story: actor, action, purpose. Without the story, nobody can tell
whether the scenario tests the right thing.

## 2. Speak like the user, and put state in Given

Scenarios use the words the user would use. A product owner never says "component", and the
moment the component is renamed the scenario is wrong. `Given` describes the world before
anything happens. The action belongs in `When`.

Weak:

```
Given a user opens the sign-up page
When the component loads
Then the open slots are displayed
```

Strong:

```
Given two serving slots are still open this Sunday
When I open the sign-up page
Then I see both open slots with their times
```

## 3. Make the outcome testable

"The open slots are displayed" cannot fail. Which slots? Whose time zone? What if none are
open? Pinning that down is where the real product decisions hide. Each one you find becomes
either a scenario of its own or an open question in the story, with the assumption you are
building on.

## Checklist

- [ ] The story has an actor, an action and a purpose.
- [ ] Each scenario has an ID: `### AB-1: Title`.
- [ ] Given is state. When is one action. Then is one observable outcome.
- [ ] No implementation words: component, endpoint, table, cache.
- [ ] Edge cases are their own scenarios: empty, boundary dates, time zones, someone else's data.
- [ ] Unknowns are open questions, with the assumption written down.
