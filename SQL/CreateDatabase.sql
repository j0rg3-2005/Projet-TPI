-- Create the database if it does not exist
CREATE DATABASE IF NOT EXISTS TPI;
USE TPI;

-- Users table
CREATE TABLE IF NOT EXISTS users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    lastName TEXT NOT NULL,
    firstName TEXT NOT NULL,
    email TEXT NOT NULL,
    password TEXT NOT NULL,
    role TEXT NOT NULL
);

-- Categories table
CREATE TABLE IF NOT EXISTS categories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name TEXT NOT NULL,
    type TEXT NOT NULL
);

-- Equipment table
CREATE TABLE IF NOT EXISTS equipment (
    id INT PRIMARY KEY AUTO_INCREMENT,
    model TEXT NOT NULL,
    inventoryNumber TEXT,
    available BOOLEAN NOT NULL DEFAULT TRUE,
    serialNumber TEXT,
    categoryId INT,
    FOREIGN KEY (categoryId) REFERENCES categories(id)
);

-- Consumables table
CREATE TABLE IF NOT EXISTS consumables (
    id INT PRIMARY KEY AUTO_INCREMENT,
    model TEXT NOT NULL,
    stock INT NOT NULL,
    minStock INT NOT NULL,
    categoryId INT,
    FOREIGN KEY (categoryId) REFERENCES categories(id)
);

-- Borrow table (equipment loan)
CREATE TABLE IF NOT EXISTS borrow (
    id INT PRIMARY KEY AUTO_INCREMENT,
    status TEXT NOT NULL,
    startDate DATETIME,
    endDate DATETIME,
    requestDate DATETIME,
    userId INT,
    equipmentId INT,
    FOREIGN KEY (userId) REFERENCES users(id),
    FOREIGN KEY (equipmentId) REFERENCES equipment(id)
);

-- Request table (consumable request)
CREATE TABLE IF NOT EXISTS request (
    id INT PRIMARY KEY AUTO_INCREMENT,
    status TEXT NOT NULL,
    requestDate DATETIME,
    consumableQuantity INT,
    userId INT,
    consumableId INT,
    FOREIGN KEY (userId) REFERENCES users(id),
    FOREIGN KEY (consumableId) REFERENCES consumables(id)
);
