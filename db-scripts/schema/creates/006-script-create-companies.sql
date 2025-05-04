CREATE TABLE Companies (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(80) NOT NULL,
    Description VARCHAR(300),
    PaymentType VARCHAR(50) CHECK (PaymentType IN ('Monthly', 'Biweekly', 'Weekly')) NOT NULL,
    MaxBenefitsPerEmployee INT,
    CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreationAuthor VARCHAR(80),
    LastModificationDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastModificationAuthor VARCHAR(80),
    FOREIGN KEY (Id) REFERENCES Persons(Id) ON DELETE CASCADE
);