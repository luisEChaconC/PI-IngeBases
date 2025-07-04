-- ================================================
-- Script to insert 3 companies, each with:
-- - 1 Employer
-- - 3 Employees
-- Companies have 2 contacts (Email, Phone)
-- Employers and Employees have 1 contact (Phone)
-- ================================================
USE PayrollSystem

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
VALUES ('9433569757', 'Legal Entity', 'San José', 'Escazú', 'San Rafael', '200m oeste del Centro Comercial Multiplaza');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'TecnoSoluciones CR', 'Empresa líder en servicios tecnológicos y desarrollo de software.', 'Monthly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'contacto@empresa1.co.cr', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '22893016', @CompanyPersonId);


-- Employer for Company 1
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('0633382597', 'Natural Person', 'San José', 'Santa Ana', 'Pozos', 'Condominio Valle del Sol, casa #45');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'carlos.mendoza@empresa1.co.cr', '^%461VsBCy', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@EmployerPersonId, 'Carlos', 'Mendoza', 'Jiménez', 'M', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '88255911', @EmployerPersonId);


-- Employee 1 for Company 1
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('0764688260', 'Natural Person', 'Heredia', 'Belén', 'La Asunción', 'Residencial Los Arcos, casa #23');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'miguel.soto@empresa1.co.cr', '4NO121TyR&', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp1PersonId, 'Miguel', 'Soto', 'Vargas', 'M', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-1-1', @CompanyPersonId, '2021-03-15', 'Part-Time', 34212.71, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '70639740', @Emp1PersonId);


DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 1
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('6648702932', 'Natural Person', 'San José', 'Montes de Oca', 'San Pedro', 'Condominio Lomas del Este, apartamento #12B');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'gabriela.rodriguez@empresa1.co.cr', 'nL&2URZp&K', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp2PersonId, 'Gabriela', 'Rodríguez', 'Castro', 'F', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-1-2', @CompanyPersonId, '2021-07-22', 'Part-Time', 57006.88, 0);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '62459713', @Emp2PersonId);

GO
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
VALUES ('5389265343', 'Legal Entity', 'Alajuela', 'Alajuela', 'El Coyol', 'Zona Industrial, 300m norte de AutoMercado');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'Café Dorado S.A.', 'Productora y exportadora de café de alta calidad desde Costa Rica.', 'Biweekly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'info@empresa2.co.cr', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '24304728', @CompanyPersonId);


-- Employer for Company 2
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('4942908531', 'Natural Person', 'Alajuela', 'Atenas', 'Centro', 'Barrio Los Jardines, 200m sur de la Iglesia');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'valeria.morales@empresa2.co.cr', '$1zPJusvwK', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@EmployerPersonId, 'Valeria', 'Morales', 'Quesada', 'F', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '86585571', @EmployerPersonId);


-- Employee 1 for Company 2
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('4521330394', 'Natural Person', 'Heredia', 'San Pablo', 'Rincón de Ricardo', '100m este del parque municipal');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'ricardo.blanco@empresa2.co.cr', '!^2$PLf1f^', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp1PersonId, 'Ricardo', 'Blanco', 'Navarro', 'M', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-2-1', @CompanyPersonId, '2021-11-08', 'Part-Time', 32226.77, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '72585942', @Emp1PersonId);

DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 2
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('7886489878', 'Natural Person', 'Cartago', 'La Unión', 'Tres Ríos', 'Condominio Monte Verde, casa #78');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'laura.fallas@empresa2.co.cr', '4^PQ3nk$)1', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp2PersonId, 'Laura', 'Fallas', 'Méndez', 'F', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-2-2', @CompanyPersonId, '2022-01-17', 'Professional Services', 34571.93, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '83731394', @Emp2PersonId);

GO
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
VALUES ('7472437759', 'Legal Entity', 'Guanacaste', 'Liberia', 'Centro', 'Diagonal al Banco Nacional');

SELECT TOP 1 @CompanyPersonId = Id FROM @CompanyPersonIdTable;

INSERT INTO Companies (Id, Name, Description, PaymentType, MaxBenefitsPerEmployee, CreationAuthor, LastModificationAuthor)
VALUES (@CompanyPersonId, 'EcoTurismo Guanacaste', 'Servicios de turismo ecológico y sostenible en la región de Guanacaste.', 'Weekly', 3, 'System', 'System');

-- Company Contacts
INSERT INTO Contacts (Type, Email, PersonId)
VALUES ('Email', 'contacto@empresa3.co.cr', @CompanyPersonId);
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '26271193', @CompanyPersonId);


-- Employer for Company 3
DECLARE @EmployerPersonId UNIQUEIDENTIFIER;
DECLARE @EmployerUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmployerPersonIdTable(Id)
VALUES ('3137483987', 'Natural Person', 'Guanacaste', 'Nicoya', 'Santa Ana', 'Residencial Las Palmeras, casa #15');

SELECT TOP 1 @EmployerPersonId = Id FROM @EmployerPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@EmployerUserId, 'mariana.villalobos@empresa3.co.cr', 'B#!8C2rQUn', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@EmployerPersonId, 'Mariana', 'Villalobos', 'Solano', 'F', @EmployerUserId);

INSERT INTO Employers (Id, CompanyId)
VALUES (@EmployerPersonId, @CompanyPersonId);

-- Employer Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '85150913', @EmployerPersonId);


-- Employee 1 for Company 3
DECLARE @Emp1PersonId UNIQUEIDENTIFIER;
DECLARE @Emp1UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('5769876198', 'Natural Person', 'Puntarenas', 'Esparza', 'Espíritu Santo', '150m oeste del Centro Educativo');

SELECT TOP 1 @Emp1PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp1UserId, 'alejandro.rojas@empresa3.co.cr', 'TB2gEUhr&M', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp1PersonId, 'Alejandro', 'Rojas', 'Varela', 'M', @Emp1UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp1PersonId, 'WID-3-1', @CompanyPersonId, '2022-05-03', 'Part-Time', 39713.54, 0);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '60840936', @Emp1PersonId);

DELETE FROM @CompanyPersonIdTable
DELETE FROM @EmployerPersonIdTable
DELETE FROM @EmpPersonIdTable

-- Employee 2 for Company 3
DECLARE @Emp2PersonId UNIQUEIDENTIFIER;
DECLARE @Emp2UserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Persons (LegalId, Type, Province, Canton, Neighborhood, AdditionalDirectionDetails)
OUTPUT INSERTED.Id INTO @EmpPersonIdTable(Id)
VALUES ('6393668242', 'Natural Person', 'Limón', 'Pococí', 'Guápiles', 'Barrio Los Colegios, 200m sur del Más x Menos');

SELECT TOP 1 @Emp2PersonId = Id FROM @EmpPersonIdTable;

INSERT INTO Users (Id, Email, Password, IsAdmin)
VALUES (@Emp2UserId, 'sofia.araya@empresa3.co.cr', '9*#0A2cgyt', 0);

INSERT INTO NaturalPersons (Id, FirstName, FirstSurname, SecondSurname, Gender, UserId)
VALUES (@Emp2PersonId, 'Sofía', 'Araya', 'Barrantes', 'F', @Emp2UserId);

INSERT INTO Employees (Id, WorkerId, CompanyId, EmployeeStartDate, ContractType, GrossSalary, HasToReportHours)
VALUES (@Emp2PersonId, 'WID-3-2', @CompanyPersonId, '2022-09-12', 'Full-Time', 58975.01, 1);

-- Employee Contact (Phone)
INSERT INTO Contacts (Type, PhoneNumber, PersonId)
VALUES ('Phone Number', '77711478', @Emp2PersonId);

GO
-- ============End of Third Execution============
