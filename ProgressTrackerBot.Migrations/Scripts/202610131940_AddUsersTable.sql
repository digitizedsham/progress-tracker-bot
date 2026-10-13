CREATE TABLE IF NOT EXISTS public.users
(
    id bigint NOT NULL,
    first_name text NULL,
    last_name text NULL,
    username text NULL,
    premium_until timestamp with time zone NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
);