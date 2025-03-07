CREATE TABLE books (
    id SERIAL PRIMARY KEY,
    cover_id INT UNIQUE,
    name VARCHAR(255),
    author VARCHAR(255)
);

CREATE TABLE user_book (
    user_id INT,
    book_id INT,
    PRIMARY KEY (user_id, book_id),
    FOREIGN KEY (book_id) REFERENCES books(id)
);

