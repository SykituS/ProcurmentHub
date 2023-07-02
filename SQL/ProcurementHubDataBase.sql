USE MASTER
GO

DROP DATABASE IF EXISTS ProcurementHub 
GO

CREATE DATABASE ProcurementHub
GO

USE ProcurementHub
GO

CREATE TABLE Persons(
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL,
)

CREATE TABLE Users(
	ID UNIQUEIDENTIFIER NOT NULL Primary key,
	UserName nvarchar(max),
	PasswordHash nvarchar(max),
	SecurityStamp nvarchar(max),
	PersonID int,
	PasswordChangeToken nvarchar(128),
	PasswordChangeTokenValidDateTime datetime,
	PrivacyAgreed bit,
	PrivacyAgreedOn datetime,
	Disabled bit not null,
	VerificationCode nvarchar(6),
	VerificationCodeValidDateTime datetime,
	CreatedOn datetime not null,
	UpdatedOn datetime not null,

	CONSTRAINT FK_User_Person FOREIGN KEY (PersonID) REFERENCES Persons(ID) 
)

CREATE TABLE Teams (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamName VARCHAR(50) NOT NULL,
	Description VARCHAR(250) NOT NULL,
	Status int NOT NULL,
	TeamJoinCode varchar(6) NOT NULL,
	TeamJoinPassword nvarchar(max),
	CreatedByID int NOT NULL,
	CreatedOn datetime not null,
	UpdatedByID int not null,
	UpdatedOn datetime not null,

	CONSTRAINT FK_Team_PersonCreated FOREIGN KEY (CreatedByID) REFERENCES Persons(ID),
	CONSTRAINT FK_Team_PersonUpdated FOREIGN KEY (UpdatedByID) REFERENCES Persons(ID) 
)

CREATE TABLE TeamMembers (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamID int NOT NULL,
	PersonID int NOT NULL,
	Role int NOT NULL,	

	CONSTRAINT FK_TeamMember_Team FOREIGN KEY (TeamID) REFERENCES Teams(ID),
	CONSTRAINT FK_TeamMember_Person FOREIGN KEY (PersonID) REFERENCES Persons(ID)
)

CREATE TABLE TeamRestaurants (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamID int NOT NULL,
	Name varchar(255) NOT NULL,
	Address varchar(255) NOT NULL,
	Description varchar(255),
	CreatedByID int NOT NULL,
	CreatedOn datetime not null,
	UpdatedByID int not null,
	UpdatedOn datetime not null,

	CONSTRAINT FK_TeamRestaurants_Team FOREIGN KEY (TeamID) REFERENCES Teams(ID),
	CONSTRAINT FK_TeamRestaurants_PersonCreated FOREIGN KEY (CreatedByID) REFERENCES Persons(ID),
	CONSTRAINT FK_TeamRestaurants_PersonUpdated FOREIGN KEY (UpdatedByID) REFERENCES Persons(ID) 
)

CREATE TABLE TeamRestaurantsItems (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamRestaurnatsID int NOT NULL,
	Name varchar(255) NOT NULL,
	Description varchar(255),
	Price money NOT NULL,
	CreatedByID int NOT NULL,
	CreatedOn datetime not null,
	UpdatedByID int not null,
	UpdatedOn datetime not null,

	CONSTRAINT FK_TeamRestaurantsItems_TeamRestaurants FOREIGN KEY (TeamRestaurnatsID) REFERENCES TeamRestaurants(ID),
	CONSTRAINT FK_TeamRestaurantsItems_PersonCreated FOREIGN KEY (CreatedByID) REFERENCES Persons(ID),
	CONSTRAINT FK_TeamRestaurantsItems_PersonUpdated FOREIGN KEY (UpdatedByID) REFERENCES Persons(ID) 
)

CREATE TABLE TeamOrders (
	ID UNIQUEIDENTIFIER NOT NULL Primary key,
	TeamID int NOT NULL,
	TeamRestaurantsID int NOT NULL,
	Status int NOT NULL,
	OrderStartedByID int NOT NULL,
	OrderStartedOn datetime NOT NULL,
	TotalPriceOfOrder money,
	OrderPayedByID int NOT NULL,
	OrderFinishedOn datetime NOT NULL,

	CONSTRAINT FK_TeamOrders_Teams FOREIGN KEY (TeamID) REFERENCES Teams(ID),
	CONSTRAINT FK_TeamOrders_TeamRestaurants FOREIGN KEY (TeamID) REFERENCES TeamRestaurants(ID),
	CONSTRAINT FK_TeamOrders_PersonOrderStarted FOREIGN KEY (OrderStartedByID) REFERENCES Persons(ID),
	CONSTRAINT FK_TeamOrders_PersonOrderPayed FOREIGN KEY (OrderPayedByID) REFERENCES Persons(ID) 
)

CREATE TABLE TeamOrdersItems (
	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TeamOrdersID UNIQUEIDENTIFIER,
	TeamRestaurantsItemsID int NOT NULL,
	Quantity int NOT NULL,
	TotalPriceOfItem money NOT NULL,
	DivideToken UNIQUEIDENTIFIER,
	DivideOnNumberOfPersons int,
	DividedPrice money,

	CONSTRAINT FK_TeamOrdersItems_TeamOrders FOREIGN KEY (TeamOrdersID) REFERENCES TeamOrders(ID),
	CONSTRAINT FK_TeamOrdersItems_TeamRestaurantsItems FOREIGN KEY (TeamRestaurantsItemsID) REFERENCES TeamRestaurantsItems(ID) 
)
--CREATE TABLE TeamJoinRequest (
--	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
--	TeamID int NOT NULL,
--	PersonID int NOT NULL,
--	JoinStatus int NOT NULL,
--)

--CREATE TABLE Restaurants(
--	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
--	Name VARCHAR(100) NOT NULL,
--	Location VARCHAR(200),
--	PhoneNumber int,
--	Website VARCHAR(250),
--)

--CREATE TABLE RestaurantItemMenu(
--	ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
--	RestaurantID int NOT NULL,
--	ItemName int NOT NULL,
--	ItemPrice MONEY NOT NULL,
--	ItemDescription VARCHAR(250),
--	ItemCategory VARCHAR(150),

--	CONSTRAINT FK_RestaurantItemMenuRestaurant FOREIGN KEY (RestaurantID) REFERENCES Restaurants(ID)
--)
