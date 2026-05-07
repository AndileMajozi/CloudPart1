CREATE DATABASE EaseEventsDb;
GO
USE EaseEventsDb;
GO

CREATE TABLE Venues (
    VenueID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Capacity INT NOT NULL CHECK (Capacity > 0),
    ImageUrl NVARCHAR(MAX) NULL
);

CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    ImageUrl NVARCHAR(MAX) NULL
);

CREATE TABLE Bookings (
    BookingID NVARCHAR(450) PRIMARY KEY,
    VenueID INT NOT NULL,
    EventID INT NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    CONSTRAINT FK_Bookings_Venues FOREIGN KEY (VenueID) REFERENCES Venues(VenueID),
    CONSTRAINT FK_Bookings_Events FOREIGN KEY (EventID) REFERENCES Events(EventID),
    CONSTRAINT CK_Bookings_EndAfterStart CHECK (EndDate > StartDate)
);
GO
