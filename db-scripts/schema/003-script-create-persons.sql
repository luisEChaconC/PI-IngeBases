CREATE TABLE Persons (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    LegalId VARCHAR(10) NOT NULL UNIQUE,
    Type VARCHAR(50) CHECK (Type IN ('Legal Entity', 'Natural Person')),
    Province VARCHAR(50),
    Canton VARCHAR(50),
    Neighborhood VARCHAR(80),
    AdditionalDirectionDetails VARCHAR(150)
);