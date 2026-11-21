#!/usr/bin/env bash
# Applies migrations/NNN_*.sql in order, each once, each in its own transaction.
# Applied files are recorded in schema_migrations with a checksum — editing an applied file is an error:
# add a new NNN_*.sql instead.
#
# Connection: DATABASE_URL (postgres://user:pass@host:port/db).
# Without psql on the machine it runs psql inside the docker-compose "postgres" container.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
MIGRATIONS="$ROOT/migrations"
DATABASE_URL="${DATABASE_URL:-postgresql://exam:exam_dev@localhost:5434/exam}"

if command -v psql >/dev/null 2>&1; then
  psql_run() { psql "$DATABASE_URL" -X -q -v ON_ERROR_STOP=1 "$@"; }
else
  echo "psql not found — using the docker-compose postgres container"
  psql_run() { docker compose -f "$ROOT/../docker-compose.yml" exec -T postgres psql -U exam -d exam -X -q -v ON_ERROR_STOP=1 "$@"; }
fi

if command -v sha256sum >/dev/null 2>&1; then checksum() { sha256sum "$1" | cut -d' ' -f1; }
else checksum() { shasum -a 256 "$1" | cut -d' ' -f1; }; fi

psql_run -c "CREATE TABLE IF NOT EXISTS schema_migrations (
  version    text        PRIMARY KEY,
  file_name  text        NOT NULL,
  checksum   text        NOT NULL,
  applied_at timestamptz NOT NULL DEFAULT now()
)"

applied=0
for file in "$MIGRATIONS"/[0-9][0-9][0-9]_*.sql; do
  name="$(basename "$file")"
  version="${name%%_*}"
  sum="$(checksum "$file")"
  recorded="$(psql_run -tA -c "SELECT checksum FROM schema_migrations WHERE version = '$version'")"

  if [[ -n "$recorded" ]]; then
    if [[ "$recorded" != "$sum" ]]; then
      echo "✗ $name changed after it was applied (checksum mismatch). Never edit an applied migration — add a new one." >&2
      exit 1
    fi
    continue
  fi

  echo "→ $name"
  { cat "$file"; echo; echo "INSERT INTO schema_migrations (version, file_name, checksum) VALUES ('$version', '$name', '$sum');"; } \
    | psql_run --single-transaction -f -
  applied=$((applied + 1))
done

echo "✓ migrations up to date ($applied applied)"
