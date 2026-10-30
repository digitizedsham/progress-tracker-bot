CREATE TABLE IF NOT EXISTS public.users
(
    id bigint NOT NULL,
    username text NULL,
    premium_until timestamp with time zone NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
);