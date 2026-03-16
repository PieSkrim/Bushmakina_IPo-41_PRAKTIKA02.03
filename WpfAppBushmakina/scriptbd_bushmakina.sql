DROP DATABASE IF EXISTS "Bushmakina";

CREATE DATABASE "Bushmakina" WITH OWNER = postgres ENCODING = 'UTF8';

\c "Bushmakina"

DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'app') THEN
        CREATE ROLE app WITH LOGIN PASSWORD '123456789';
    ELSE
        ALTER ROLE app WITH LOGIN PASSWORD '123456789';
    END IF;
END
$$;

DROP SCHEMA IF EXISTS app CASCADE;
CREATE SCHEMA app AUTHORIZATION app;

CREATE TABLE app.partner_types_bushmakina (
    id SERIAL PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL
);

CREATE TABLE app.partners_bushmakina (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    type_id INTEGER NOT NULL REFERENCES app.partner_types_bushmakina(id),
    inn VARCHAR(20),
    rating INTEGER DEFAULT 0,
    address VARCHAR(500),
    director_name VARCHAR(200),
    phone VARCHAR(50),
    email VARCHAR(100),
    sales_places VARCHAR(500)
);

CREATE TABLE app.sales_records_bushmakina (
    id SERIAL PRIMARY KEY,
    partner_id INTEGER NOT NULL REFERENCES app.partners_bushmakina(id) ON DELETE CASCADE,
    product_name VARCHAR(200) NOT NULL,
    quantity INTEGER NOT NULL,
    sale_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_sales ON app.sales_records_bushmakina(partner_id);

GRANT ALL ON SCHEMA app TO app;
GRANT ALL ON ALL TABLES IN SCHEMA app TO app;
GRANT ALL ON ALL SEQUENCES IN SCHEMA app TO app;

INSERT INTO app.partner_types_bushmakina (type_name) VALUES 
('Retail Store'),
('Wholesale Store'),
('Online Shop'),
('Corporate Client');

INSERT INTO app.partners_bushmakina (name, type_id, inn, rating, address, director_name, phone, email, sales_places) VALUES
('OOO Torgovy Dom', 1, '7701234567', 10, 'Moscow, Lenina str 1', 'Ivanov I.I.', '+7 495 111 22 33', 'info@td.ru', 'Moscow'),
('IP Petrov', 2, '7709876543', 15, 'SPB, Nevsky 10', 'Petrov P.P.', '+7 812 222 33 44', 'petrov@mail.ru', 'SPB'),
('Online Shop LLC', 3, '6601234567', 8, 'Ekaterinburg', 'Sidorova A.V.', '+7 343 333 44 55', 'shop@online.ru', 'Online');

INSERT INTO app.sales_records_bushmakina (partner_id, product_name, quantity, sale_date) VALUES
(1, 'Laminate Oak', 5000, NOW()),
(1, 'Parquet Board', 3000, NOW()),
(1, 'Ceramic Tile', 4000, NOW()),
(2, 'Commercial Linoleum', 60000, NOW()),
(3, 'PVC Baseboard', 5000, NOW());

SELECT 'Partner Types: ' || COUNT(*) FROM app.partner_types_bushmakina;
SELECT 'Partners: ' || COUNT(*) FROM app.partners_bushmakina;
SELECT 'Sales Records: ' || COUNT(*) FROM app.sales_records_bushmakina;
