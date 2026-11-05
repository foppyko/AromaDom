CREATE DATABASE AromaDom;
USE AromaDom;

CREATE TABLE MaterialTypes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    loss_percent DECIMAL(5,4) NOT NULL
);

CREATE TABLE ProductTypes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    coefficient DECIMAL(5,2) NOT NULL
);


CREATE TABLE Materials (
    id INT IDENTITY(1,1) PRIMARY KEY,
    material_type_id INT NOT NULL,
    name NVARCHAR(150) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    stock_quantity DECIMAL(10,2) NOT NULL,
    min_quantity DECIMAL(10,2) NOT NULL,
    package_quantity DECIMAL(10,2) NOT NULL,
    unit NVARCHAR(20) NOT NULL,
    FOREIGN KEY (material_type_id) REFERENCES MaterialTypes(id)
);

CREATE TABLE Products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    product_type_id INT NOT NULL,
    article NVARCHAR(20) NOT NULL,
    name NVARCHAR(150) NOT NULL,
    min_partner_price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (product_type_id) REFERENCES ProductTypes(id)
);

CREATE TABLE MaterialProducts (
    id INT IDENTITY(1,1) PRIMARY KEY,
    material_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity_per_unit DECIMAL(10,4) NOT NULL,
    FOREIGN KEY (material_id) REFERENCES Materials(id),
    FOREIGN KEY (product_id) REFERENCES Products(id)
);