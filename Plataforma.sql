CREATE DATABASE Platform;
USE Platform;

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role ENUM('parent', 'teacher', 'admin', 'specialist') NOT NULL, 
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Child (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    BirthDate DATE NOT NULL,
    GuardianId INT, -- Foreign key to Users (responsible)
    LearningProfile TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (GuardianId) REFERENCES Users(Id)
);

CREATE TABLE Activities (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    Type ENUM('game', 'exercise', 'quiz') NOT NULL, -- Activity type
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Progress (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ChildId INT,  -- Foreign key to Children
    ActivityId INT, -- Foreign key to Activities
    CompletedAt TIMESTAMP,
    Score DECIMAL(5, 2),
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ChildId) REFERENCES Children(Id),
    FOREIGN KEY (ActivityId) REFERENCES Activities(Id)
);

CREATE TABLE Reports (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ChildId INT, -- Foreign key to Children
    GeneratedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ReportData TEXT,
    FOREIGN KEY (ChildId) REFERENCES Children(Id)
);

CREATE TABLE Comments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ChildId INT, -- Foreign key to Children
    SpecialistId INT, -- Foreign key to Users (specialists)
    Comment TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ChildId) REFERENCES Children(Id),
    FOREIGN KEY (SpecialistId) REFERENCES Users(Id)
);

CREATE TABLE Notifications (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT, -- Foreign key to Users (responsible)
    Message TEXT,
    IsRead BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
