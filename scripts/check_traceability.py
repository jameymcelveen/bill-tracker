#!/usr/bin/env python3
"""Every scenario in docs/stories/ has a test that names it, and every test names a real scenario.

Scenario headings look like:   ### AB-1: Title
C# tests name a scenario with:  [Trait("Scenario", "AB-1")]
TypeScript tests start a title: it('AB-1: ...')
"""
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
SKIP_DIRS = {".git", "node_modules", "bin", "obj", "dist"}
SCENARIO_HEADING = re.compile(r"^#{2,4}\s+([A-Z]{2,5}-\d+):", re.MULTILINE)
CS_TRAIT = re.compile(r'Trait\(\s*"Scenario"\s*,\s*"([A-Z]{2,5}-\d+)"\s*\)')
TS_TITLE = re.compile(r"""\b(?:it|test)\(\s*['"`]([A-Z]{2,5}-\d+):""")


def walk():
    for path in ROOT.rglob("*"):
        if path.is_file() and not any(part in SKIP_DIRS for part in path.relative_to(ROOT).parts):
            yield path


def main():
    scenarios = {}
    stories = ROOT / "docs" / "stories"
    for story in sorted(stories.glob("*.md")) if stories.is_dir() else []:
        for scenario in SCENARIO_HEADING.findall(story.read_text(encoding="utf-8")):
            scenarios[scenario] = story.relative_to(ROOT)

    tests = {}
    for path in walk():
        rel = path.relative_to(ROOT)
        if path.suffix == ".cs" and rel.parts[0] == "tests":
            found = CS_TRAIT.findall(path.read_text(encoding="utf-8"))
        elif path.name.endswith((".test.ts", ".test.tsx")):
            found = TS_TITLE.findall(path.read_text(encoding="utf-8"))
        else:
            continue
        for scenario in found:
            tests.setdefault(scenario, set()).add(str(rel))

    missing = sorted(set(scenarios) - set(tests))
    unknown = sorted(set(tests) - set(scenarios))

    for scenario in sorted(scenarios):
        where = ", ".join(sorted(tests.get(scenario, []))) or "NO TEST"
        print("%-8s %s" % (scenario, where))
    for scenario in missing:
        print("FAIL: %s in %s has no test" % (scenario, scenarios[scenario]))
    for scenario in unknown:
        print("FAIL: tests name %s, which no story defines: %s" % (scenario, ", ".join(sorted(tests[scenario]))))

    if missing or unknown:
        return 1
    print("Traceability check passed: %d scenario(s), each with a test" % len(scenarios))
    return 0


if __name__ == "__main__":
    sys.exit(main())
