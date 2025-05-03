USE PayrollSystem

CREATE TABLE Benefits (
    Id UNIQUEIDENTIFIER 
       NOT NULL 
       PRIMARY KEY 
       DEFAULT NEWID(),
    Name VARCHAR(35) NOT NULL,
    Description VARCHAR(100),
    IsActive BIT NOT NULL,
    Type VARCHAR(16) 
       NOT NULL 
       CHECK (Type IN ('API', 'FixedAmount', 'FixedPercentage')),
    LinkAPI VARCHAR(100),
    FixedPercentage INT 
       CHECK (FixedPercentage BETWEEN 1 AND 100),
    FixedAmount INT 
       CHECK (FixedAmount BETWEEN 1 AND 10000000),
    RequiredMonthsWorked INT 
       CHECK (RequiredMonthsWorked BETWEEN 0 AND 480)
);
