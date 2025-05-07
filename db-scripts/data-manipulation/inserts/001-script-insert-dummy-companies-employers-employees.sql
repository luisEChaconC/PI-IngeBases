-- ================================================
-- Script to insert 3 companies, each with:
-- - 1 Employer
-- - 3 Employees
-- Companies have 2 contacts (Email, Phone)
-- Employers and Employees have 1 contact (Phone)
-- ================================================


-- ================First Execution=================

-- Declare table variables to capture auto-generated IDs
DECLARE @CompanyPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmployerPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmpPersonIdTable TABLE (Id UNIQUEIDENTIFIER);


-- ================================================
-- Insert Company 1
-- ================================================
DECLARE @CompanyPersonId UNIQUEIDENTIFIER;

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @CompanyPersonIdTable(Id)
VALUES ('9433569757', 'Legal Entity', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Some direction info');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'Company 1', 'Company description here.', 'Monthly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'nvincent@owens-williamson.biz', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '49356016', @CompanyPersonId);


-- Employer for Company 1
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('0633382597', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employer address');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'donnavincent@clark-powers.net', '^%461VsBCy', 1);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@EmployerPersonId, 'Brian', 'Mcdaniel', 'Johnson', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '04255911', @EmployerPersonId);


-- Employee 1 for Company 1
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('0764688260', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'morrismatthew@hebert.com', '4NO121TyR&', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp1PersonId, 'Mitchell', 'Preston', 'Gonzales', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-1-1', @CompanyPersonId, 'Part-Time', 34212.71, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '98639740', @Emp1PersonId);


DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 1
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('6648702932', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'moorejason@yahoo.com', 'nL&2URZp&K', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp2PersonId, 'Gina', 'Anderson', 'Carter', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-1-2', @CompanyPersonId, 'Part-Time', 57006.88, 0);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '82459713', @Emp2PersonId);


-- ============End of First Execution==============



-- ================Second Execution================

-- Declare table variables to capture auto-generated IDs
DECLARE @CompanyPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmployerPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmpPersonIdTable TABLE (Id UNIQUEIDENTIFIER);

-- ================================================
-- Insert Company 2
-- ================================================

DECLARE @CompanyPersonId UNIQUEIDENTIFIER;

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @CompanyPersonIdTable(Id)
VALUES ('5389265343', 'Legal Entity', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Some direction info');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'Company 2', 'Company description here.', 'Monthly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'ashleewilliams@lewis-doyle.com', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '67004728', @CompanyPersonId);


-- Employer for Company 2
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('4942908531', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employer address');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'perezelizabeth@gmail.com', '$1zPJusvwK', 1);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@EmployerPersonId, 'Sherry', 'Michael', 'Strickland', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '36585571', @EmployerPersonId);


-- Employee 1 for Company 2
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('4521330394', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'kevin10@hotmail.com', '!^2$PLf1f^', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp1PersonId, 'Rebecca', 'Willis', 'Nelson', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-2-1', @CompanyPersonId, 'Part-Time', 32226.77, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '52585942', @Emp1PersonId);

DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 2
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('7886489878', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'davidflores@hotmail.com', '4^PQ3nk$)1', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp2PersonId, 'Kevin', 'Boyd', 'Wilson', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-2-2', @CompanyPersonId, 'Hourly', 34571.93, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '93731394', @Emp2PersonId);


-- ============End of Second Execution============


-- ================Third Execution================

-- Declare table variables to capture auto-generated IDs
DECLARE @CompanyPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmployerPersonIdTable TABLE (Id UNIQUEIDENTIFIER);
DECLARE @EmpPersonIdTable TABLE (Id UNIQUEIDENTIFIER);

-- ================================================
-- Insert Company 3
-- ================================================
DECLARE @CompanyPersonId UNIQUEIDENTIFIER;

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @CompanyPersonIdTable(Id)
VALUES ('7472437759', 'Legal Entity', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Some direction info');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'Company 3', 'Company description here.', 'Monthly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'raymondjohnson@murphy.info', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '87021193', @CompanyPersonId);


-- Employer for Company 3
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('3137483987', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employer address');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'fcollins@cook.com', 'B#!8C2rQUn', 1);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@EmployerPersonId, 'Jennifer', 'Mccoy', 'Nelson', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '65150913', @EmployerPersonId);


-- Employee 1 for Company 3
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('5769876198', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'lisadoyle@sweeney-todd.info', 'TB2gEUhr&M', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp1PersonId, 'Daniel', 'Jacobs', 'Allen', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-3-1', @CompanyPersonId, 'Part-Time', 39713.54, 0);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '80840936', @Emp1PersonId);

DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 3
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('6393668242', 'Natural Person', 'ProvinceX', 'CantonY', 'NeighborhoodZ', 'Employee address');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'dbonilla@mcdonald-kline.biz', '9*#0A2cgyt', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, UserId)
VALUES (@Emp2PersonId, 'Melissa', 'Webb', 'Jackson', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-3-2', @CompanyPersonId, 'Full-Time', 58975.01, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '37711478', @Emp2PersonId);

-- ============End of Third Execution============


-- ============== Fourth Execution ==============

DECLARE @CompanyId1 UNIQUEIDENTIFIER
DECLARE @CompanyId2 UNIQUEIDENTIFIER
DECLARE @CompanyId3 UNIQUEIDENTIFIER

-- Retrieve company IDs based on their names
SELECT @CompanyId1 = Id FROM Companies WHERE Name = 'Company 1'
SELECT @CompanyId2 = Id FROM Companies WHERE Name = 'Company 2'
SELECT @CompanyId3 = Id FROM Companies WHERE Name = 'Company 3'

INSERT INTO Benefits (CompanyId, Name, Description, IsActive, Type, LinkAPI, FixedPercentage, FixedAmount, RequiredMonthsWorked)
VALUES 
    (@CompanyId1, 'Seguro Médico', 'Cobertura médica completa', 1, 'FixedAmount', NULL, NULL, 5000, 6),
    (@CompanyId1, 'Fondo de Ahorro', 'Ahorro mensual para empleados', 1, 'FixedPercentage', NULL, 10, NULL, 12),
    (@CompanyId1, 'Servicio de Transporte', 'Transporte desde casa al trabajo', 1, 'FixedAmount', NULL, NULL, 2000, 3),
    (@CompanyId2, 'API de Bonos', 'Conexión a sistema de bonos externo', 1, 'API', 'https://api.ejemplo.com/bonos', NULL, NULL, 0),
    (@CompanyId2, 'Subsidio Alimenticio', 'Vale de comida mensual', 1, 'FixedAmount', NULL, NULL, 1500, 1),
    (@CompanyId2, 'Seguro de Vida', 'Seguro para casos de accidente o fallecimiento', 1, 'FixedPercentage', NULL, 5, NULL, 6),
    (@CompanyId3, 'API de Capacitación', 'Sistema externo de formación continua', 1, 'API', 'https://api.ejemplo.com/cursos', NULL, NULL, 0),
    (@CompanyId3, 'Bono Anual', 'Bono al finalizar el año fiscal', 1, 'FixedPercentage', NULL, 15, NULL, 24),
    (@CompanyId3, 'Subsidio para Educación', 'Apoyo para estudios universitarios', 1, 'FixedAmount', NULL, NULL, 10000, 18),
    (@CompanyId3, 'API de Evaluación de Desempeño', 'Evaluación automática de desempeño', 1, 'API', 'https://api.ejemplo.com/evaluacion', NULL, NULL, 0);


-- ============End of Fourth Execution============