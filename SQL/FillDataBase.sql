-- Use the database
USE TPI;

-- Insertion des catégories
INSERT INTO categories (name, type) VALUES
('Chargeur', 'matériel'),
('Ordinateur portable', 'matériel'),
('Arduino', 'matériel'),
('Raspberry Pi', 'matériel'),
('Résistance', 'consommable'),
('Condensateur', 'consommable'),
('Diode', 'consommable'),
('Transistor', 'consommable');

-- Insertion des utilisateurs
INSERT INTO users (lastName, firstName, email, password, role) VALUES
('Dupont', 'Jean', 'jean.dupont@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'client'),
('Durand', 'Sophie', 'sophie.durand@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'client'),
('Martin', 'Paul', 'paul.martin@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Administrateur'),
('Moreau', 'Emma', 'emma.moreau@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Administrateur'),
('Bernard', 'Lucie', 'lucie.bernard@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'client'),
('Petit', 'Hugo', 'hugo.petit@example.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Administrateur');

-- Insertion de matériels disponibles
INSERT INTO equipment (model, available, serialNumber, inventoryNumber, categoryId) VALUES
('Chargeur Anker 65W', False, 'SNCHARGER01', 'INV001', 1),
('Chargeur Samsung 25W', TRUE, 'SNCHARGER02', 'INV002', 1),
('Dell Latitude 5410', FALSE, 'SNLAPTOP01', 'INV003', 2),
('MacBook Air M1', TRUE, 'SNLAPTOP02', 'INV004', 2),
('Arduino Uno', TRUE, 'SNARDUINO01', 'INV005', 3),
('Arduino Mega 2560', TRUE, 'SNARDUINO02', 'INV006', 3),
('Raspberry Pi 4B', TRUE, 'SNRPI01', 'INV007', 4),
('Raspberry Pi Zero 2', TRUE, 'SNRPI02', 'INV008', 4);

-- Insertion de consommables
INSERT INTO consumables (model, stock, minStock, categoryId) VALUES
('Résistance 220Ω', 500, 100, 5),
('Résistance 1kΩ', 500, 100, 5),
('Condensateur 10μF', 300, 50, 6),
('Condensateur 100μF', 300, 50, 6),
('Diode 1N4007', 250, 200, 7),
('Diode Zener 5V1', 250, 150, 7),
('Transistor BC547', 200, 150, 8),
('Transistor 2N2222', 200, 150, 8);

-- Création d'emprunts de matériel
INSERT INTO lends (status, startDate, endDate, requestDate, userId, equipmentId) VALUES
('en cours', '2025-04-25 09:00:00', '2025-04-30 09:00:00', '2025-04-24 08:30:00', 1, 1),
('en cours', '2025-04-25 09:30:00', '2025-04-30 09:00:00', '2025-04-24 09:00:00', 2, 3),
('retourné', '2025-04-10 10:00:00', '2025-04-17 10:00:00', '2025-04-09 09:00:00', 5, 5),
('retourné', '2025-04-05 11:00:00', '2025-04-10 15:00:00', '2025-04-04 10:30:00', 6, 7);

-- Création de demandes de consommables
INSERT INTO request (status, requestDate, consumableQuantity, userId, consumableId) VALUES
('en attente', '2025-04-26 14:00:00', 20, 1, 1),
('accepté', '2025-04-26 15:00:00', 15, 2, 2),
('en attente', '2025-04-26 16:00:00', 25, 3, 3),
('accepté', '2025-04-27 10:00:00', 10, 4, 4),
('rejeté', '2025-04-27 11:00:00', 30, 5, 5),
('en attente', '2025-04-27 12:00:00', 50, 6, 6);
