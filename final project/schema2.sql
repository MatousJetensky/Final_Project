CREATE TABLE IF NOT EXISTS film_status (
                                           id UUID PRIMARY KEY,
                                           name VARCHAR(50) NOT NULL
    );

CREATE TABLE IF NOT EXISTS film (
                                    id UUID PRIMARY KEY,
                                    title VARCHAR(200) NOT NULL,
    director VARCHAR(100),
    year INT,
    genre VARCHAR(50),
    status_id UUID NOT NULL REFERENCES film_status(id)
    );