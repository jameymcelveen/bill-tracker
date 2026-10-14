# Every check CI runs, runnable locally. Run `make setup` once, then `make check` before every push.
.PHONY: setup check build test web-test web-build gen-api gen-api-check ascii trace

setup:
	cd web && npm ci

check: build test web-test web-build gen-api-check ascii trace
	@echo "make check: all passed"

build:
	dotnet build --nologo

test:
	dotnet test --nologo

web-test:
	cd web && npm test

web-build:
	cd web && npm run build

gen-api:
	cd web && npm run gen:api

gen-api-check:
	cd web && npm run gen:api:check

ascii:
	python3 scripts/check_ascii.py

trace:
	python3 scripts/check_traceability.py
