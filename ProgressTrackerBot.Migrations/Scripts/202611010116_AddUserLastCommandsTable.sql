CREATE TABLE IF NOT EXISTS public.user_last_commands
(
    user_id BIGINT PRIMARY KEY REFERENCES public.users(id) ON DELETE CASCADE,

    -- Имя последней команды
    command_name TEXT NOT NULL,

    -- Аргументы последней команды
    arguments TEXT[] NOT NULL DEFAULT ARRAY[]::TEXT[],

    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);