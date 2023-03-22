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

--CREATE TABLE TeamJoinRequest (
--	TeamID int NOT NULL,
--	PersonID int NOT NULL,

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
