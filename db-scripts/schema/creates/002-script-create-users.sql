CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Email VARCHAR(320) NOT NULL UNIQUE,        
    Password VARCHAR(100) NOT NULL,            
    IsAdmin BIT NOT NULL                        
);