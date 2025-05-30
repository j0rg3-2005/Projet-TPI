-- Create the database if it does not exist
DROP DATABASE IF EXISTS TPI;
CREATE DATABASE TPI;
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
    FOREIGN KEY (categoryId) REFERENCES categories(id) ON DELETE CASCADE
);

-- Consumables table
CREATE TABLE IF NOT EXISTS consumables (
    id INT PRIMARY KEY AUTO_INCREMENT,
    model TEXT NOT NULL,
    stock INT NOT NULL,
    minStock INT NOT NULL,
    categoryId INT,
    FOREIGN KEY (categoryId) REFERENCES categories(id) ON DELETE CASCADE
);

-- Borrow table (equipment loan)
CREATE TABLE IF NOT EXISTS lends (
    id INT PRIMARY KEY AUTO_INCREMENT,
    status TEXT NOT NULL,
    startDate DATETIME NOT NULL,
    endDate DATETIME NOT NULL,
    requestDate DATETIME NOT NULL,
    returnDate DATETIME,
    userId INT,
    equipmentId INT,
    FOREIGN KEY (userId) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (equipmentId) REFERENCES equipment(id) ON DELETE CASCADE
);

-- Request table (consumable request)
CREATE TABLE IF NOT EXISTS request (
    id INT PRIMARY KEY AUTO_INCREMENT,
    status TEXT NOT NULL,
    requestDate DATETIME NOT NULL,
    consumableQuantity INT NOT NULL,
    userId INT,
    consumableId INT,
    FOREIGN KEY (userId) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (consumableId) REFERENCES consumables(id) ON DELETE CASCADE
);
