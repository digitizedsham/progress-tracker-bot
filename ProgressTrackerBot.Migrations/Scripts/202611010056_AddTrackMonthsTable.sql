CREATE TABLE IF NOT EXISTS public.track_months
(
    track_id BIGINT NOT NULL REFERENCES public.tracks(id) ON DELETE CASCADE,

    year SMALLINT NOT NULL,

    month SMALLINT NOT NULL,

    -- Массив значений: -1, 0, 1
    days SMALLINT[] NOT NULL,

    PRIMARY KEY (track_id, year, month),

    CONSTRAINT ck_track_months_year
        CHECK (year BETWEEN 1970 AND 32767),

    CONSTRAINT ck_track_months_month
        CHECK (month BETWEEN 1 AND 12),

    -- Количество элементов должно совпадать
    -- с количеством дней в соответствующем месяце
    CONSTRAINT ck_track_months_days_count
        CHECK (
            cardinality(days) =
            EXTRACT(
                DAY FROM (
                    make_date(year::INTEGER, month::INTEGER, 1)
                    + INTERVAL '1 month - 1 day'
                )
            )::INTEGER
        ),

    -- Допустимы только -1, 0 и 1
    CONSTRAINT ck_track_months_values
        CHECK (days <@ ARRAY[-1, 0, 1]::SMALLINT[])
);