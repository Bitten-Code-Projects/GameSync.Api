# GameSync.Api

API for GameSync

# Startup instructions

- Use `docker compose up -d` to setup infrastructure
- Create `SEQ_API_URL` environment variable with Seq digestion url (default http://localhost:5341/ingest/otlp/v1/logs)
- Create `SEQ_API_KEY` environment variable with Seq api key (check manual https://docs.datalust.co/docs/ingestion-with-opentelemetry)
- Create `GAMESYNC_CONNECTIONSTRING` environment variable with database connection string (example server=127.0.0.1;database=gamesync;user=root;password=)