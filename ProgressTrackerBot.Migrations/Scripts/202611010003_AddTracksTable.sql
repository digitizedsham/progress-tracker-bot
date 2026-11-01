CREATE TABLE IF NOT EXISTS public.tracks
(
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    
    user_id BIGINT NOT NULL REFERENCES public.users(id) ON DELETE CASCADE,
    
    name VARCHAR(50) NOT NULL,
    
    -- Битовая маска дней недели
    weekdays_mask SMALLINT NOT NULL DEFAULT 0,
    
    -- Часовой интервал уведомления: 0-23
    reminder_hour SMALLINT NOT NULL DEFAULT 12,

    -- Признак отправки оповещения со звуком или без
    sound_on boolean NOT NULL DEFAULT FALSE,
    
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT ck_tracks_weekdays_mask
        CHECK (weekdays_mask BETWEEN 0 AND 127),

    CONSTRAINT ck_tracks_reminder_hour
        CHECK (reminder_hour BETWEEN 0 AND 23)
);

CREATE INDEX IF NOT EXISTS ix_tracks_user_id
    ON public.tracks(user_id);

CREATE INDEX IF NOT EXISTS ix_tracks_reminder_hour
    ON public.tracks(reminder_hour);