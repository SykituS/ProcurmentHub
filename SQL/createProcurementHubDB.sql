-- Create a new database called 'ProcurementHub'
-- Connect to the 'master' database to run this snippet
USE master
GO

-- Drop the database 'ProcurementHub'
-- Connect to the 'master' database to run this snippet
USE master
GO
-- Uncomment the ALTER DATABASE statement below to set the database to SINGLE_USER mode if the drop database command fails because the database is in use.
-- ALTER DATABASE ProcurementHub SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- Drop the database if it exists
IF EXISTS (
    SELECT [name]
        FROM sys.databases
        WHERE [name] = N'ProcurementHub
    '
)
DROP DATABASE ProcurementHub
GO

-- Create the new database if it does not exist already
IF NOT EXISTS (
    SELECT [name]
        FROM sys.databases
        WHERE [name] = N'ProcurementHub'
)
CREATE DATABASE ProcurementHub
GO

USE ProcurementHub;

CREATE TABLE Users (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  Name VARCHAR(255) NOT NULL,
  Email VARCHAR(255) NOT NULL,
  Password VARCHAR(255) NOT NULL,
  RegistrationDate DATETIME NOT NULL,
  VerificationStatus VARCHAR(255) DEFAULT 'pending',
  VerificationCode VARCHAR(255),
  PhoneNumber VARCHAR(255),
  Address VARCHAR(255),
  ProfilePicture VARCHAR(255),
  Preferences VARCHAR(255),
  ResetCode VARCHAR(255),
  ResetCodeExpiration DATETIME,
  IsDeleted BIT DEFAULT 0,
  DeletedAt DATETIME,
  UNIQUE (email)
);

CREATE TABLE Teams (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  TeamName VARCHAR(255) NOT NULL,
  Creator INTEGER NOT NULL,
  Description VARCHAR(255),
  Status VARCHAR(255),
  FOREIGN KEY (Creator) REFERENCES Users (Id)
);

CREATE TABLE Restaurants (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  Name VARCHAR(255) NOT NULL,
  Location VARCHAR(255) NOT NULL,
  PhoneNumber VARCHAR(255),
  Website VARCHAR(255),
  Rating FLOAT
);

CREATE TABLE Menu (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  RestaurantId INTEGER NOT NULL,
  ItemName VARCHAR(255) NOT NULL,
  ItemPrice FLOAT NOT NULL,
  Description VARCHAR(255),
  Category VARCHAR(255),
  FOREIGN KEY (RestaurantId) REFERENCES Restaurants (Id)
);

CREATE TABLE Orders (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  TeamId INTEGER NOT NULL,
  RestaurantId INTEGER NOT NULL,
  UserId INTEGER NOT NULL,
  ItemId INTEGER NOT NULL,
  Quantity INTEGER NOT NULL,
  OrderTimestamp DATETIME NOT NULL,
  Total FLOAT NOT NULL,
  FOREIGN KEY (TeamId) REFERENCES Teams (Id),
  FOREIGN KEY (RestaurantId) REFERENCES Restaurants (Id),
  FOREIGN KEY (UserId) REFERENCES Users (Id),
  FOREIGN KEY (ItemId) REFERENCES Menu (Id)
);

CREATE TABLE Payments (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  TeamId INTEGER NOT NULL,
  UserId INTEGER NOT NULL,
  OrderId INTEGER NOT NULL,
  Amount FLOAT NOT NULL,
  PaymentTimestamp DATETIME NOT NULL,
  PaymentMethod VARCHAR(255),
  FOREIGN KEY (teamId) REFERENCES Teams (Id),
  FOREIGN KEY (userId) REFERENCES Users (Id),
  FOREIGN KEY (orderId) REFERENCES Orders (Id)
);

CREATE TABLE Members (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  TeamId INTEGER NOT NULL,
  UserId INTEGER NOT NULL,
  Role VARCHAR(255),
  Contributions FLOAT,
  PaymentStatus VARCHAR(255),
  FOREIGN KEY (TeamId) REFERENCES Teams (Id),
  FOREIGN KEY (UserId) REFERENCES Users (Id)
);

CREATE TABLE UserStatistics (
  Id INTEGER PRIMARY KEY IDENTITY(1, 1),
  TeamId INTEGER NOT NULL,
  UserId INTEGER NOT NULL,
  OrderId INTEGER NOT NULL,
  PaymentId INTEGER NOT NULL,
  ItemId INTEGER NOT NULL,
  Quantity INTEGER NOT NULL,
  ItemPrice FLOAT NOT NULL,
  OrderTimestamp DATETIME NOT NULL,
  PaymentTimestamp DATETIME NOT NULL,
  PaymentMethod VARCHAR(255) NOT NULL,
  FOREIGN KEY (TeamId) REFERENCES Teams (Id),
  FOREIGN KEY (UserId) REFERENCES Users (Id),
  FOREIGN KEY (OrderId) REFERENCES Orders (Id),
  FOREIGN KEY (PaymentId) REFERENCES Payments (Id),
  FOREIGN KEY (ItemId) REFERENCES Menu (Id)
);