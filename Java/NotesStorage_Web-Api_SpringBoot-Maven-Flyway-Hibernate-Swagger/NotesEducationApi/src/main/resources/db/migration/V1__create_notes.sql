CREATE TABLE notes (
    id BIGSERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    text TEXT,
    created_at TIMESTAMP NOT NULL,
    done BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE INDEX idx_notes_title ON notes(title);