CREATE TABLE public.notifications
(
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    track_id BIGINT NOT NULL
        REFERENCES public.tracks(id) ON DELETE CASCADE,

    user_id BIGINT NOT NULL
        REFERENCES public.users(id) ON DELETE CASCADE,

    scheduled_at TIMESTAMPTZ NOT NULL,

    status VARCHAR(20) NOT NULL DEFAULT 'pending',

    attempts SMALLINT NOT NULL DEFAULT 0,

    next_attempt_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    locked_until TIMESTAMPTZ NULL,

    sent_at TIMESTAMPTZ NULL,

    last_error TEXT NULL,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_notifications_scheduled
        UNIQUE (track_id, scheduled_at),

    CONSTRAINT ck_notifications_status
        CHECK (status IN ('pending', 'processing', 'sent', 'failed'))
);

CREATE INDEX ix_notifications_pending
    ON public.notifications(next_attempt_at, id)
    WHERE status = 'pending';