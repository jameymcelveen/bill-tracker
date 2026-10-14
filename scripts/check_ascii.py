#!/usr/bin/env python3
"""Fail if code, configuration or the contract contains a non-ASCII character.

A curly quote or a non-breaking space in a C# string, a YAML key or a JSON file is a bug,
and AI tools produce them. Docs and art (Markdown, banners) may use any character, and so
may content/: it is text people read, which keeps its own punctuation and alphabets.

Usage: python3 scripts/check_ascii.py [path ...]
With no paths, checks every file git knows about, or every file under the repo when it is
not a git repository yet. Dependencies and build output are skipped.
"""
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
SKIP_DIRS = {".git", "node_modules", "bin", "obj", "dist", "TestResults", ".vs", ".idea"}
SKIP_FILES = {"package-lock.json"}
CHECK_EXT = {
    ".cs", ".csproj", ".props", ".targets", ".sln", ".slnx",
    ".json", ".yaml", ".yml", ".csv", ".http",
    ".ts", ".tsx", ".js", ".mjs", ".css", ".html",
    ".py", ".sh",
}
CHECK_NAMES = {"Makefile", "Dockerfile", ".editorconfig", ".gitignore"}


def candidates():
    try:
        out = subprocess.run(["git", "ls-files", "-co", "--exclude-standard"], cwd=ROOT,
                             capture_output=True, text=True, check=True).stdout
        return [ROOT / p for p in out.splitlines() if p]
    except (subprocess.CalledProcessError, FileNotFoundError):
        return [p for p in ROOT.rglob("*") if p.is_file()]


def checked(path):
    try:
        rel = path.relative_to(ROOT)
    except ValueError:
        rel = path
    if any(part in SKIP_DIRS for part in rel.parts) or path.name in SKIP_FILES:
        return False
    if rel.parts and rel.parts[0] == "content":
        return False
    return path.suffix.lower() in CHECK_EXT or path.name in CHECK_NAMES


def main():
    paths = [Path(p).resolve() for p in sys.argv[1:]] or candidates()
    bad = 0
    for path in paths:
        if not path.is_file() or not checked(path):
            continue
        for lineno, line in enumerate(path.read_bytes().split(b"\n"), 1):
            for col, byte in enumerate(line, 1):
                if byte > 126 or (byte < 32 and byte not in (9, 13)):
                    print("%s:%d:%d non-ASCII byte 0x%02x" % (path, lineno, col, byte))
                    bad += 1
                    break
    if bad:
        print("%d line(s) failed the ASCII check" % bad)
        return 1
    print("ASCII check passed (code, configuration and the contract)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
