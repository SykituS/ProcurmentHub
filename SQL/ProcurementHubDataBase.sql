USE MASTER
GO

DROP DATABASE IF EXISTS ProcurementHub 
GO

CREATE DATABASE ProcurementHub
GO

USE ProcurementHub
GO

CREATE TABLE Users(
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	Password VARCHAR(150) NOT NULL,
	Role int NOT NULL,
	Status int NOT NULL,
	CreatedOn DATETIME NOT NULL,
	UpdatedOn DATETIME NOT NULL,
	VerificationCode VARCHAR(6),
	VerificationStatus int,
	VerificationCodeExpiry int,
	ResetCode VARCHAR(50),
	ResetCodeExpiry DATETIME,
	IsDeleted bit NOT NULL DEFAULT 0,
	DeletedDate DATETIME
)

CREATE TABLE Teams (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamName VARCHAR(50) NOT NULL,
	Description VARCHAR(250) NOT NULL,
	CreatedByID int NOT NULL,
	Status int NOT NULL

	CONSTRAINT FK_UserTeam FOREIGN KEY (CreatedByID) REFERENCES Users(ID) 
)

CREATE TABLE TeamsMember (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamID int NOT NULL,
	UserID int NOT NULL,
	Role int NOT NULL,

	CONSTRAINT FK_TeamTeamMemeber FOREIGN KEY (TeamID) REFERENCES Teams(ID),
	CONSTRAINT FK_UserTeamMember FOREIGN KEY (UserID) REFERENCES Users(ID)
)

CREATE TABLE Restaurants(
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Location VARCHAR(200),
	PhoneNumber int,
	Website VARCHAR(250),
)

CREATE TABLE RestaurantItemMenu(
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	RestaurantID int NOT NULL,
	ItemName int NOT NULL,
	ItemPrice MONEY NOT NULL,
	ItemDescription VARCHAR(250),
	ItemCategory VARCHAR(150),

	CONSTRAINT FK_RestaurantItemMenuRestaurant FOREIGN KEY (RestaurantID) REFERENCES Restaurants(ID)
)
