#!/bin/bash
set -e

psql \
    --username "$POSTGRES_USER" \
    --dbname "$POSTGRES_DB" \
    -v ON_ERROR_STOP=1 \
    -v db_name="$POSTGRES_DB" \
    -v app_user="$POSTGRES_APP_USER" \
    -v app_password="$POSTGRES_APP_PASSWORD" \
    -v migrator_user="$POSTGRES_MIGRATOR_USER" \
    -v migrator_password="$POSTGRES_MIGRATOR_PASSWORD" \
    <<'SQL'

-- Пользователь приложения
CREATE ROLE :"app_user"
    LOGIN
    PASSWORD :'app_password'
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOREPLICATION
    NOINHERIT;

-- Пользователь миграций
CREATE ROLE :"migrator_user"
    LOGIN
    PASSWORD :'migrator_password'
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOREPLICATION
    NOINHERIT;

-- Разрешаем подключение к БД
GRANT CONNECT ON DATABASE :"db_name"
    TO :"app_user";

GRANT CONNECT ON DATABASE :"db_name"
    TO :"migrator_user";

-- Передаём владение схемой пользователю миграций
ALTER SCHEMA public OWNER TO :"migrator_user";

-- Права приложения на существующие таблицы
GRANT USAGE ON SCHEMA public
    TO :"app_user";

GRANT SELECT, INSERT, UPDATE, DELETE
    ON ALL TABLES IN SCHEMA public
    TO :"app_user";

-- Права приложения на существующие последовательности
GRANT USAGE, SELECT
    ON ALL SEQUENCES IN SCHEMA public
    TO :"app_user";

-- Автоматические права для новых таблиц,
-- которые будут создаваться пользователем миграций
ALTER DEFAULT PRIVILEGES
    FOR ROLE :"migrator_user"
    IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE
    ON TABLES TO :"app_user";

-- Автоматические права на новые последовательности
ALTER DEFAULT PRIVILEGES
    FOR ROLE :"migrator_user"
    IN SCHEMA public
    GRANT USAGE, SELECT
    ON SEQUENCES TO :"app_user";

SQL