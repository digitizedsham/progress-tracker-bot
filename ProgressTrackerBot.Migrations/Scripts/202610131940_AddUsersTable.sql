CREATE TABLE IF NOT EXISTS public.users
(
    -- Это идентификатор пользователя, который предоставляет сам Telegram
    id BIGINT PRIMARY KEY,
    
    username text NULL,
    
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    
    premium_until timestamp with time zone NULL
);