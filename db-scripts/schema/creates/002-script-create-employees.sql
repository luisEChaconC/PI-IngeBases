CREATE TABLE Employees (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    WorkerId VARCHAR(80) NOT NULL UNIQUE,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    EmployeeStartDate DATE NOT NULL DEFAULT GETDATE(),
    ContractType VARCHAR(50) CHECK (ContractType IN ('Full-Time', 'Part-Time', 'Professional Services', 'Hourly')) NOT NULL,
    GrossSalary DECIMAL(10, 2) NOT NULL,
    HasToReportHours BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE NO ACTION,
    FOREIGN KEY (Id) REFERENCES NaturalPersons(Id) ON DELETE CASCADE
);