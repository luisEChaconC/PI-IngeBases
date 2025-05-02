CREATE TABLE Users (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Email VARCHAR(320) NOT NULL UNIQUE,        
    Password VARCHAR(100) NOT NULL,            
    IsAdmin BIT NOT NULL                        
);