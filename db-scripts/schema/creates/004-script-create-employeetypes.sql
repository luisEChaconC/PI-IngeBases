USE PayrollSystem

CREATE TABLE EmployeeTypes (
    Id UNIQUEIDENTIFIER 
       NOT NULL 
       PRIMARY KEY 
       DEFAULT NEWID(),
    Name VARCHAR(30) NOT NULL UNIQUE  -- TO DO check types
);