-- ============================================
-- RaceDay Event Management System
-- Database Schema and Sample Data
-- ============================================
-- Author: [Your Name/Student ID]
-- Date: 2026-09-03
-- Description: Complete database schema for the RaceDay system
-- supporting South African road running, walking, and cycling events
-- Compatible with: SQL Server 2022
-- ============================================

-- Ensure we are using the correct database context
USE master;

-- Drop the database if it exists to ensure a clean state
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'RaceDayDB')
BEGIN
    ALTER DATABASE RaceDayDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RaceDayDB;
END

-- Create the main database
CREATE DATABASE RaceDayDB;

-- Use the new database
USE RaceDayDB;

-- ============================================
-- Table Creation
-- ============================================

-- 1. Users Table
-- Stores all system users - Organisers and Participants
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    DateJoined DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    LastLoginDate DATETIME2 NULL,
    CONSTRAINT CHK_User_Role CHECK (Role IN ('Organiser', 'Participant'))
);

-- 2. Organisers Table
-- Event organisers who create and manage events
CREATE TABLE Organisers (
    OrganiserID INT PRIMARY KEY,
    OrganisationName NVARCHAR(200) NOT NULL,
    RegistrationNumber NVARCHAR(50) NULL,
    Website NVARCHAR(200) NULL,
    Bio NVARCHAR(MAX) NULL,
    YearsInOperation INT DEFAULT 0,
    Rating DECIMAL(3,2) DEFAULT 0,
    CONSTRAINT FK_Organisers_Users FOREIGN KEY (OrganiserID) REFERENCES Users(UserID),
    CONSTRAINT CHK_Organisers_Rating CHECK (Rating >= 0 AND Rating <= 5)
);

-- 3. Participants Table
-- Athletes who participate in events
CREATE TABLE Participants (
    ParticipantID INT PRIMARY KEY,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) NOT NULL,
    IDNumber NVARCHAR(13) NOT NULL UNIQUE,
    EmergencyContactName NVARCHAR(100) NOT NULL,
    EmergencyContactNumber NVARCHAR(20) NOT NULL,
    MedicalConditions NVARCHAR(500) NULL,
    ClubAffiliation NVARCHAR(100) NULL,
    ShirtSize NVARCHAR(10) NULL,
    EmergencyContactRelationship NVARCHAR(50) NULL,
    CONSTRAINT FK_Participants_Users FOREIGN KEY (ParticipantID) REFERENCES Users(UserID),
    CONSTRAINT CHK_Participant_Gender CHECK (Gender IN ('M', 'F', 'O')),
    CONSTRAINT CHK_Participant_ShirtSize CHECK (ShirtSize IN ('XS', 'S', 'M', 'L', 'XL', 'XXL'))
);

-- 4. Events Table
-- Racing events created by organisers
CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    EventDate DATE NOT NULL,
    EventTime TIME NOT NULL,
    Venue NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Province NVARCHAR(50) NOT NULL,
    EventType NVARCHAR(50) NOT NULL,
    IsActive BIT DEFAULT 1,
    MaxParticipants INT NULL,
    CurrentParticipants INT DEFAULT 0,
    RegistrationOpenDate DATETIME2 NULL,
    RegistrationCloseDate DATETIME2 NULL,
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Planning',
    Metadata NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Events_Organisers FOREIGN KEY (OrganiserID) REFERENCES Organisers(OrganiserID),
    CONSTRAINT CHK_Event_Type CHECK (EventType IN ('Running', 'Walking', 'Cycling', 'Triathlon', 'Multi-Sport')),
    CONSTRAINT CHK_Event_Status CHECK (Status IN ('Planning', 'Open', 'Closed', 'In Progress', 'Completed', 'Cancelled'))
);

-- 5. Categories Table
-- Race categories (distances, age groups, gender)
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    Distance DECIMAL(6,2) NOT NULL,
    DistanceUnit NVARCHAR(10) DEFAULT 'km',
    AgeGroupMin INT NULL,
    AgeGroupMax INT NULL,
    GenderRestriction CHAR(1) NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    LateEntryFee DECIMAL(10,2) NULL,
    MaxParticipants INT NULL,
    CurrentParticipants INT DEFAULT 0,
    StartTime TIME NULL,
    CutOffTime TIME NULL,
    CONSTRAINT FK_Categories_Events FOREIGN KEY (EventID) REFERENCES Events(EventID) ON DELETE CASCADE,
    CONSTRAINT CHK_Category_DistanceUnit CHECK (DistanceUnit IN ('km', 'miles')),
    CONSTRAINT CHK_Category_Gender CHECK (GenderRestriction IN ('M', 'F', 'O')),
    CONSTRAINT CHK_Category_EntryFee CHECK (EntryFee >= 0)
);

-- 6. Enrolments Table
-- Participant registrations for specific event categories
CREATE TABLE Enrolments (
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending',
    EntryFeePaid DECIMAL(10,2) NULL,
    PaymentReference NVARCHAR(50) NULL,
    PaymentDate DATETIME2 NULL,
    BibNumber INT NULL UNIQUE,
    WaveStartTime TIME NULL,
    EmergencyContactName NVARCHAR(100) NULL,
    EmergencyContactNumber NVARCHAR(20) NULL,
    MedicalConditions NVARCHAR(500) NULL,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Enrolments_Participants FOREIGN KEY (ParticipantID) REFERENCES Participants(ParticipantID),
    CONSTRAINT FK_Enrolments_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT CHK_Enrolment_Status CHECK (Status IN ('Pending', 'Confirmed', 'Paid', 'Cancelled', 'Completed', 'DNS', 'DNF')),
    CONSTRAINT UQ_Enrolment_ParticipantCategory UNIQUE (ParticipantID, CategoryID)
);

-- 7. Results Table
-- Race results for participants
CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL UNIQUE,
    FinishTime TIME(3) NOT NULL,
    GunTime TIME(3) NULL,
    ChipTime TIME(3) NULL,
    OverallPosition INT NOT NULL,
    GenderPosition INT NOT NULL,
    CategoryPosition INT NOT NULL,
    Pace DECIMAL(5,2) NULL,
    IsDisqualified BIT DEFAULT 0,
    DisqualificationReason NVARCHAR(255) NULL,
    ResultNotes NVARCHAR(500) NULL,
    VerifiedBy INT NULL,
    VerificationDate DATETIME2 NULL,
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentID) REFERENCES Enrolments(EnrolmentID),
    CONSTRAINT FK_Results_Users FOREIGN KEY (VerifiedBy) REFERENCES Users(UserID)
);

-- ============================================
-- Additional Constraints/Indexes
-- ============================================

CREATE INDEX IX_Enrolments_ParticipantID ON Enrolments(ParticipantID);
CREATE INDEX IX_Enrolments_CategoryID ON Enrolments(CategoryID);
CREATE INDEX IX_Enrolments_Status ON Enrolments(Status);
CREATE INDEX IX_Events_OrganiserID ON Events(OrganiserID);
CREATE INDEX IX_Events_EventDate ON Events(EventDate);
CREATE INDEX IX_Events_City ON Events(City);
CREATE INDEX IX_Categories_EventID ON Categories(EventID);
CREATE INDEX IX_Categories_Distance ON Categories(Distance);
CREATE INDEX IX_Results_EnrolmentID ON Results(EnrolmentID);
CREATE INDEX IX_Results_OverallPosition ON Results(OverallPosition);
CREATE INDEX IX_Participants_DateOfBirth ON Participants(DateOfBirth);

-- ============================================
-- Sample Data Insertion
-- ============================================

-- 1. Insert Users
INSERT INTO Users (Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, DateJoined)
VALUES
    ('thabo@comradesmarathon.co.za', 'hashed_password_1', 'Thabo', 'Mthembu', '0821234567', 'Organiser', GETDATE()),
    ('zanele@twinoceans.co.za', 'hashed_password_2', 'Zanele', 'Nkosi', '0839876543', 'Organiser', GETDATE()),
    ('sipho.dlamini@gmail.com', 'hashed_password_3', 'Sipho', 'Dlamini', '0723456789', 'Participant', GETDATE()),
    ('lindiwe.mthembu@gmail.com', 'hashed_password_4', 'Lindiwe', 'Mthembu', '0734567890', 'Participant', GETDATE()),
    ('michael.brown@gmail.com', 'hashed_password_5', 'Michael', 'Brown', '0745678901', 'Participant', GETDATE()),
    ('sarah.pieterse@gmail.com', 'hashed_password_6', 'Sarah', 'Pieterse', '0756789012', 'Participant', GETDATE());

-- 2. Insert Organisers
INSERT INTO Organisers (OrganiserID, OrganisationName, RegistrationNumber, Website, Bio, YearsInOperation, Rating)
VALUES
    (1, 'Comrades Marathon Association', 'CMA-1986-001', 'www.comradesmarathon.co.za', 'Organising South Africa''s premier ultramarathon since 1921', 40, 4.9),
    (2, 'Two Oceans Marathon Association', 'TOMA-1988-002', 'www.twooceansmarathon.co.za', 'Cape Town''s iconic ultramarathon and half marathon', 35, 4.8);

-- 3. Insert Participants
INSERT INTO Participants (ParticipantID, DateOfBirth, Gender, IDNumber, EmergencyContactName, EmergencyContactNumber, MedicalConditions, ClubAffiliation, ShirtSize, EmergencyContactRelationship)
VALUES
    (3, '1990-05-15', 'M', '9005151234088', 'Lindiwe Mthembu', '0734567890', 'Asthma - mild', 'Soweto Athletics Club', 'L', 'Spouse'),
    (4, '1988-10-20', 'F', '8810202345089', 'Thabo Dlamini', '0723456789', 'None', 'Boxer Athletics Club', 'M', 'Spouse'),
    (5, '1992-03-10', 'M', '9203103456090', 'Emily Brown', '0839876543', 'Diabetes Type 2 - controlled', 'Randburg Harriers', 'XL', 'Sister'),
    (6, '1985-12-01', 'F', '8512014567091', 'Michael Pieterse', '0745678901', 'High Blood Pressure - controlled', 'Atlantic Athletics Club', 'M', 'Spouse');

-- 4. Insert Events
INSERT INTO Events (OrganiserID, Title, Description, EventDate, EventTime, Venue, City, Province, EventType, MaxParticipants, RegistrationOpenDate, RegistrationCloseDate, Status)
VALUES
    (1, 'Comrades Marathon 2026', 'The Ultimate Human Race - 87km ultramarathon from Pietermaritzburg to Durban', '2026-06-14', '05:30:00', 'Pietermaritzburg City Hall', 'Pietermaritzburg', 'KwaZulu-Natal', 'Running', 25000, '2026-01-15', '2026-05-15', 'Open'),
    (1, 'Comrades 10km Fun Run 2026', 'A 10km community run held alongside the Comrades Marathon', '2026-06-13', '07:00:00', 'Pietermaritzburg City Hall', 'Pietermaritzburg', 'KwaZulu-Natal', 'Running', 5000, '2026-01-15', '2026-05-15', 'Open'),
    (2, 'Two Oceans Marathon 2026', 'Beautiful 56km ultramarathon around the Cape Peninsula', '2026-04-11', '06:00:00', 'Newlands Rugby Stadium', 'Cape Town', 'Western Cape', 'Running', 18000, '2026-01-10', '2026-03-15', 'Open');

-- 5. Insert Categories
-- Comrades Marathon 2026 (EventID = 1)
INSERT INTO Categories (EventID, CategoryName, Description, Distance, AgeGroupMin, AgeGroupMax, GenderRestriction, EntryFee, LateEntryFee, MaxParticipants, StartTime, CutOffTime)
VALUES
    (1, 'Comrades - Open', 'Open category for all runners', 87.0, 18, NULL, NULL, 950.00, 1200.00, 15000, '05:30:00', '12:00:00'),
    (1, 'Comrades - Masters', 'For runners 40 years and older', 87.0, 40, NULL, NULL, 950.00, 1200.00, 5000, '05:30:00', '12:00:00'),
    (1, 'Comrades - Women', 'Female only category', 87.0, 18, NULL, 'F', 950.00, 1200.00, 3000, '05:30:00', '12:00:00');

-- Comrades 10km Fun Run (EventID = 2)
INSERT INTO Categories (EventID, CategoryName, Description, Distance, AgeGroupMin, AgeGroupMax, GenderRestriction, EntryFee, LateEntryFee, MaxParticipants, StartTime, CutOffTime)
VALUES
    (2, '10km - Open', '10km fun run - open to all', 10.0, 10, NULL, NULL, 100.00, 150.00, 3000, '07:00:00', '09:00:00'),
    (2, '10km - Junior', '10km fun run for ages 10-18', 10.0, 10, 18, NULL, 50.00, 75.00, 1000, '07:00:00', '09:00:00');

-- Two Oceans Marathon 2026 (EventID = 3)
INSERT INTO Categories (EventID, CategoryName, Description, Distance, AgeGroupMin, AgeGroupMax, GenderRestriction, EntryFee, LateEntryFee, MaxParticipants, StartTime, CutOffTime)
VALUES
    (3, '56km - Open', '56km ultramarathon - open to all', 56.0, 18, NULL, NULL, 850.00, 1000.00, 12000, '06:00:00', '10:30:00'),
    (3, '56km - Grandmasters', 'For runners 50 years and older', 56.0, 50, NULL, NULL, 850.00, 1000.00, 3000, '06:00:00', '10:30:00');

-- 6. Insert Enrolments
INSERT INTO Enrolments (ParticipantID, CategoryID, EnrolmentDate, Status, EntryFeePaid, PaymentReference, PaymentDate, BibNumber, MedicalConditions)
VALUES 
    (3, 1, '2026-01-20', 'Paid', 950.00, 'PAY-CMA-001', '2026-01-20', 12567, 'Asthma - will carry inhaler'),
    (4, 3, '2026-01-22', 'Paid', 950.00, 'PAY-CMA-002', '2026-01-22', 12568, NULL),
    (5, 6, '2026-01-15', 'Paid', 850.00, 'PAY-TOM-003', '2026-01-15', 23456, 'Diabetes controlled'),
    (6, 4, '2026-01-18', 'Paid', 100.00, 'PAY-CMA-004', '2026-01-18', 12569, NULL);

-- 7. Insert Results
INSERT INTO Results (EnrolmentID, FinishTime, GunTime, ChipTime, OverallPosition, GenderPosition, CategoryPosition, Pace, IsDisqualified, ResultNotes, VerifiedBy, VerificationDate)
VALUES 
    (1, '08:15:30.000', '08:15:45.000', '08:15:30.000', 234, 189, 89, 5.70, 0, 'Strong performance, second Comrades', 1, '2026-06-15'),
    (2, '09:45:20.000', '09:45:35.000', '09:45:20.000', 512, 45, 12, 6.70, 0, 'Good debut Comrades', 1, '2026-06-15'),
    (3, '06:30:15.000', '06:30:30.000', '06:30:15.000', 156, 128, 45, 6.95, 0, 'Ran with diabetes controlled', 2, '2026-04-12');

-- ============================================
-- Update Counters
-- ============================================

UPDATE Events 
SET CurrentParticipants = (
    SELECT COUNT(DISTINCT e.ParticipantID)
    FROM Enrolments e
    JOIN Categories c ON e.CategoryID = c.CategoryID
    WHERE c.EventID = Events.EventID
    AND e.Status IN ('Paid', 'Confirmed', 'Completed')
);

UPDATE Categories 
SET CurrentParticipants = (
    SELECT COUNT(*)
    FROM Enrolments e
    WHERE e.CategoryID = Categories.CategoryID
    AND e.Status IN ('Paid', 'Confirmed', 'Completed')
);

-- ============================================
-- Verification Queries
-- ============================================

-- View all users
SELECT * FROM Users;

-- View all events with organisers
SELECT 
    e.Title,
    e.EventDate,
    e.City,
    e.Province,
    u.FirstName + ' ' + u.LastName AS OrganiserName,
    e.MaxParticipants,
    e.CurrentParticipants,
    e.Status
FROM Events e
JOIN Organisers o ON e.OrganiserID = o.OrganiserID
JOIN Users u ON o.OrganiserID = u.UserID;

-- View all enrolments with details
SELECT 
    u.FirstName + ' ' + u.LastName AS ParticipantName,
    ev.Title AS EventName,
    c.CategoryName,
    c.Distance,
    en.Status,
    en.EntryFeePaid,
    en.BibNumber
FROM Enrolments en
JOIN Participants p ON en.ParticipantID = p.ParticipantID
JOIN Users u ON p.ParticipantID = u.UserID
JOIN Categories c ON en.CategoryID = c.CategoryID
JOIN Events ev ON c.EventID = ev.EventID
ORDER BY en.EnrolmentDate DESC;

-- View results
SELECT 
    u.FirstName + ' ' + u.LastName AS ParticipantName,
    ev.Title AS EventName,
    c.CategoryName,
    r.FinishTime,
    r.OverallPosition,
    r.CategoryPosition,
    r.Pace,
    r.ResultNotes
FROM Results r
JOIN Enrolments en ON r.EnrolmentID = en.EnrolmentID
JOIN Participants p ON en.ParticipantID = p.ParticipantID
JOIN Users u ON p.ParticipantID = u.UserID
JOIN Categories c ON en.CategoryID = c.CategoryID
JOIN Events ev ON c.EventID = ev.EventID;

-- Summary statistics
SELECT 
    'Total Users' AS Statistic,
    COUNT(*) AS Value
FROM Users
UNION ALL
SELECT 
    'Total Organisers',
    COUNT(*)
FROM Organisers
UNION ALL
SELECT 
    'Total Participants',
    COUNT(*)
FROM Participants
UNION ALL
SELECT 
    'Total Events',
    COUNT(*)
FROM Events
UNION ALL
SELECT 
    'Total Categories',
    COUNT(*)
FROM Categories
UNION ALL
SELECT 
    'Total Enrolments',
    COUNT(*)
FROM Enrolments
UNION ALL
SELECT 
    'Total Results',
    COUNT(*)
FROM Results;

PRINT 'RaceDay database schema and sample data created successfully for SQL Server 2022.';