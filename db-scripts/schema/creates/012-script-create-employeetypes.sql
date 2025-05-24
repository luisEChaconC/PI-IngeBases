USE PayrollSystem

CREATE TABLE EmployeeTypes (
    Id UNIQUEIDENTIFIER 
       NOT NULL 
       PRIMARY KEY 
       DEFAULT NEWID(),
    Name VARCHAR(30) CHECK (Name IN ('Full-Time', 'Part-Time', 'Professional Services', 'Hourly')) NOT NULL, 
);