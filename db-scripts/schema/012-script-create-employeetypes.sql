USE PayrollSystem

CREATE TABLE EmployeeTypes (
    Id UNIQUEIDENTIFIER 
       NOT NULL 
       PRIMARY KEY 
       DEFAULT NEWID(),
    Name VARCHAR(30) CHECK (Name IN ('Full-Time', 'Part-Time', 'Professional Services')) NOT NULL
);

INSERT INTO EmployeeTypes (Name) VALUES ('Full-Time')
INSERT INTO EmployeeTypes (Name) VALUES ('Part-Time')
INSERT INTO EmployeeTypes (Name) VALUES ('Professional Services')