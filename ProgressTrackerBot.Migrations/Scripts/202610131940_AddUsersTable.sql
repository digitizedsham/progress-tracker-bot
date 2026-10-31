CREATE TABLE IF NOT EXISTS public.users
(
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    telegram_id BIGINT NOT NULL UNIQUE,
    username text NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    premium_until timestamp with time zone NULL
);