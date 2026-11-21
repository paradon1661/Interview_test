#!/usr/bin/env bash
# DEV ONLY: drop the local database volume and rebuild it from migrations/.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
docker compose -f "$ROOT/../docker-compose.yml" down -v
"$ROOT/script/start-db.sh"
