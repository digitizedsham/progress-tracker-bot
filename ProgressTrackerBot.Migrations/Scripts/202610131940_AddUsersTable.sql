CREATE TABLE IF NOT EXISTS public.users
(
    -- Это идентификатор пользователя, который предоставляет сам Telegram
    id BIGINT PRIMARY KEY,
    
    username TEXT NULL,
    
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    
    premium_until TIMESTAMPTZ NULL
);