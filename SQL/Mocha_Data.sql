USE Mocha;
GO

INSERT INTO Admin (Name, Username, Password) VALUES
('System Administrator', 'admin', 'admin_secure_hash_123');

INSERT INTO Branch (Name, Location, Phone, AdminID) VALUES
('Mocha Downtown', '101 Main St, City Center', '555-0101', 1),
('Mocha Uptown', '404 High St, North District', '555-0102', 1),
('Mocha Mall', '789 Shop Ave, West Mall', '555-0103', 1),
('Mocha Airport', 'Terminal 2, Gate B', '555-0104', 1);

INSERT INTO MenuItem (ItemName, Price, Category, AdminID) VALUES
('Espresso', 3.00, 'Beverage', 1),
('Americano', 3.50, 'Beverage', 1),
('Cappuccino', 4.50, 'Beverage', 1),
('Latte', 4.50, 'Beverage', 1),
('Mocha Special', 5.00, 'Beverage', 1),
('Caramel Macchiato', 5.25, 'Beverage', 1),
('Flat White', 4.25, 'Beverage', 1),
('Cold Brew', 4.00, 'Beverage', 1),
('Iced Tea', 3.00, 'Beverage', 1),
('Hot Chocolate', 3.75, 'Beverage', 1),
('Butter Croissant', 2.50, 'Food', 1),
('Chocolate Muffin', 3.00, 'Food', 1),
('Blueberry Scone', 3.25, 'Food', 1),
('Bagel with Cream Cheese', 3.50, 'Food', 1),
('Ham & Cheese Sandwich', 6.50, 'Food', 1),
('Chicken Panini', 7.00, 'Food', 1),
('Caesar Salad', 6.00, 'Food', 1),
('Cheesecake Slice', 5.00, 'Dessert', 1),
('Chocolate Chip Cookie', 2.00, 'Dessert', 1),
('Brownie', 3.50, 'Dessert', 1);

INSERT INTO AvailableAt (BranchID, ItemID, Quantity) VALUES
(1, 1, 100), (1, 2, 100), (1, 3, 100), (1, 11, 50), (1, 15, 30),
(2, 1, 100), (2, 4, 100), (2, 5, 100), (2, 12, 50), (2, 16, 30),
(3, 1, 100), (3, 6, 100), (3, 7, 100), (3, 13, 50), (3, 17, 30),
(4, 1, 100), (4, 8, 100), (4, 9, 100), (4, 14, 50), (4, 18, 30),
(1, 19, 200), (2, 19, 200), (3, 19, 200), (4, 19, 200);

INSERT INTO Customer (Name, Email, LoyaltyBalance, Username, Password) VALUES
('Alice Johnson', 'alice@example.com', 120, 'alicej', 'pass1'),
('Bob Smith', 'bob@example.com', 50, 'bobsmith', 'pass2'),
('Charlie Brown', 'charlie@example.com', 10, 'cbrown', 'pass3'),
('Diana Prince', 'diana@example.com', 300, 'wonderd', 'pass4'),
('Ethan Hunt', 'ethan@example.com', 0, 'ehunt', 'pass5'),
('Fiona Shrek', 'fiona@example.com', 25, 'fionaogre', 'pass6'),
('George Martin', 'george@example.com', 15, 'grmartin', 'pass7'),
('Hannah Abbott', 'hannah@example.com', 60, 'habbott', 'pass8'),
('Ian Malcolm', 'ian@example.com', 45, 'chaosian', 'pass9'),
('Julia Roberts', 'julia@example.com', 80, 'jules', 'pass10'),
('Kevin Bacon', 'kevin@example.com', 5, 'kbacon', 'pass11'),
('Laura Croft', 'laura@example.com', 200, 'tombraider', 'pass12'),
('Michael Scott', 'michael@example.com', 1000, 'bossmike', 'pass13'),
('Nina Simone', 'nina@example.com', 90, 'ninas', 'pass14'),
('Oscar Wilde', 'oscar@example.com', 30, 'owilde', 'pass15'),
('Peter Parker', 'peter@example.com', 10, 'spidey', 'pass16'),
('Quincy Jones', 'quincy@example.com', 75, 'qjones', 'pass17'),
('Rachel Green', 'rachel@example.com', 150, 'rgreen', 'pass18'),
('Steve Rogers', 'steve@example.com', 500, 'capsteve', 'pass19'),
('Tony Stark', 'tony@example.com', 9999, 'ironman', 'pass20');

INSERT INTO Employee (FName, LName, Username, Password, BranchID) VALUES
('Sarah', 'Connor', 'sconnor', 'emppass1', 1),
('John', 'Rambo', 'jrambo', 'emppass2', 1),
('Ellen', 'Ripley', 'eripley', 'emppass3', 2),
('Luke', 'Skywalker', 'lskywalker', 'emppass4', 2),
('Harry', 'Potter', 'hpotter', 'emppass5', 3),
('Hermione', 'Granger', 'hgranger', 'emppass6', 3),
('Frodo', 'Baggins', 'fbaggins', 'emppass7', 4),
('Samwise', 'Gamgee', 'sgamgee', 'emppass8', 4),
('Clark', 'Kent', 'ckent', 'emppass9', 1),
('Bruce', 'Wayne', 'bwayne', 'emppass10', 2),
('Walter', 'White', 'wwhite', 'emppass11', 1),
('Jesse', 'Pinkman', 'jpinkman', 'emppass12', 1),
('Gustavo', 'Fring', 'gfring', 'emppass13', 2),
('Saul', 'Goodman', 'sgoodman', 'emppass14', 2),
('Sherlock', 'Holmes', 'sholmes', 'emppass15', 3),
('John', 'Watson', 'jwatson', 'emppass16', 3),
('Daenerys', 'Targaryen', 'dtargaryen', 'emppass17', 4),
('Jon', 'Snow', 'jsnow', 'emppass18', 4),
('Tyrion', 'Lannister', 'tlannister', 'emppass19', 1),
('Arya', 'Stark', 'astark', 'emppass20', 2);

INSERT INTO Cashier (EmployeeID) VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10);
INSERT INTO Barista (EmployeeID) VALUES (11),(12),(13),(14),(15),(16),(17),(18),(19),(20);

INSERT INTO Orders (Status, PaymentMethod, SubTotal, Discount, TotalAmount, CustomerID, BranchID, CashierID, BaristaID) VALUES
('Completed', 'Cash', 7.50, 0.00, 7.50, 1, 1, 1, 11),
('Completed', 'Card', 9.50, 0.00, 9.50, 2, 1, 1, 12),
('Ready', 'Card', 12.00, 2.00, 10.00, 3, 2, 3, 13),
('Preparing', 'Loyalty', 4.50, 4.50, 0.00, 4, 2, 4, 14),
('Pending', 'Cash', 15.00, 0.00, 15.00, 5, 3, 5, 15),
('Completed', 'Card', 6.00, 0.00, 6.00, 6, 3, 6, 16),
('Completed', 'Cash', 3.00, 0.00, 3.00, 7, 4, 7, 17),
('Completed', 'Card', 20.00, 0.00, 20.00, 8, 4, 8, 18),
('Completed', 'Card', 8.50, 0.00, 8.50, 9, 1, 9, 19),
('Completed', 'Cash', 5.00, 0.00, 5.00, 10, 2, 10, 20),
('Completed', 'Card', 14.00, 0.00, 14.00, 11, 1, 1, 11),
('Ready', 'Card', 7.00, 0.00, 7.00, 12, 1, 2, 12),
('Preparing', 'Loyalty', 50.00, 10.00, 40.00, 13, 2, 3, 13),
('Pending', 'Cash', 3.50, 0.00, 3.50, 14, 2, 4, 14),
('Completed', 'Card', 9.00, 0.00, 9.00, 15, 3, 5, 15),
('Completed', 'Cash', 4.50, 0.00, 4.50, 16, 3, 6, 16),
('Completed', 'Card', 11.00, 0.00, 11.00, 17, 4, 7, 17),
('Completed', 'Card', 6.50, 0.00, 6.50, 18, 4, 8, 18),
('Ready', 'Loyalty', 5.25, 5.25, 0.00, 19, 1, 9, 19),
('Preparing', 'Card', 18.00, 0.00, 18.00, 20, 2, 10, 20);

INSERT INTO OrderDetails (OrderID, ItemID, Quantity, UnitPrice, LineTotal) VALUES
(1, 1, 1, 3.00, 3.00), (1, 3, 1, 4.50, 4.50),
(2, 5, 1, 5.00, 5.00), (2, 2, 1, 4.50, 4.50),
(3, 16, 1, 7.00, 7.00), (3, 18, 1, 5.00, 5.00),
(4, 4, 1, 4.50, 4.50),
(5, 15, 2, 6.50, 13.00), (5, 19, 1, 2.00, 2.00),
(6, 17, 1, 6.00, 6.00),
(7, 9, 1, 3.00, 3.00),
(8, 16, 2, 7.00, 14.00), (8, 17, 1, 6.00, 6.00),
(9, 6, 1, 5.25, 5.25), (9, 13, 1, 3.25, 3.25),
(10, 18, 1, 5.00, 5.00),
(11, 16, 2, 7.00, 14.00),
(12, 16, 1, 7.00, 7.00),
(13, 15, 5, 6.50, 32.50), (13, 20, 5, 3.50, 17.50),
(14, 2, 1, 3.50, 3.50),
(15, 3, 2, 4.50, 9.00),
(16, 4, 1, 4.50, 4.50),
(17, 7, 2, 4.25, 8.50), (17, 11, 1, 2.50, 2.50),
(18, 15, 1, 6.50, 6.50),
(19, 6, 1, 5.25, 5.25),
(20, 16, 2, 7.00, 14.00), (20, 8, 1, 4.00, 4.00);

INSERT INTO Feedback (Rating, Comment, CustomerID) VALUES
(5, 'Great coffee!', 1),
(4, 'Nice ambiance but a bit noisy.', 2),
(3, 'Service was slow today.', 3),
(5, 'Loved the Mocha Special!', 4),
(2, 'Coffee was cold.', 5),
(5, 'Best staff ever!', 6),
(4, 'Good value for money.', 7),
(1, 'My order was completely wrong.', 8),
(5, 'Excellent espresso.', 9),
(4, 'Tasty pastries.', 10),
(3, 'Okay, but overpriced.', 11),
(5, 'I come here every day.', 12),
(5, 'The Barista Walter makes art!', 13),
(4, 'Clean restrooms.', 14),
(2, 'Too crowded.', 15),
(5, 'Fast wifi, good for working.', 16),
(3, 'Music is too loud.', 17),
(4, 'Try the cheesecake!', 18),
(5, 'They gave me a free cookie!', 19),
(5, 'Stark approved.', 20);
GO