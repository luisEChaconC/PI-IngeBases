USE PayrollSystem

CREATE TABLE EmployeeTypesXBenefits (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Evitar ID muy complejo
    EmployeeTypeId UNIQUEIDENTIFIER NOT NULL,
    BenefitId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (EmployeeTypeId) REFERENCES EmployeeTypes(Id),
    FOREIGN KEY (BenefitId) REFERENCES Benefits(Id)
);
