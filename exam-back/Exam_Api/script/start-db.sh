#!/usr/bin/env bash
# Local dev: start Postgres 17 (docker-compose at the repo root), wait until ready, apply migrations.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
COMPOSE=(docker compose -f "$ROOT/../docker-compose.yml")

"${COMPOSE[@]}" up -d postgres
echo -n "waiting for postgres"
until "${COMPOSE[@]}" exec -T postgres pg_isready -U exam -d exam >/dev/null 2>&1; do echo -n "."; sleep 1; done
echo
"$ROOT/script/run-migrations.sh"
echo "postgres: localhost:5434 (exam / exam_dev)"
