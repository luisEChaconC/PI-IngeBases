-- ================================================
-- Script to insert dummy Timesheets
-- All timesheets are 7-day periods within existing payrolls
-- ================================================

-- Variables for Company and Employee IDs
DECLARE @CompanyId1 UNIQUEIDENTIFIER
DECLARE @CompanyId2 UNIQUEIDENTIFIER
DECLARE @CompanyId3 UNIQUEIDENTIFIER
DECLARE @PayrollId1 UNIQUEIDENTIFIER
DECLARE @PayrollId2 UNIQUEIDENTIFIER
DECLARE @PayrollId3 UNIQUEIDENTIFIER

-- Employee IDs for Company1
DECLARE @Employee1_Company1 UNIQUEIDENTIFIER
DECLARE @Employee2_Company1 UNIQUEIDENTIFIER

-- Employee IDs for Company2
DECLARE @Employee1_Company2 UNIQUEIDENTIFIER
DECLARE @Employee2_Company2 UNIQUEIDENTIFIER

-- Employee IDs for Company3
DECLARE @Employee1_Company3 UNIQUEIDENTIFIER
DECLARE @Employee2_Company3 UNIQUEIDENTIFIER

-- Get Company IDs
SELECT @CompanyId1 = Id FROM Companies WHERE Name = 'TecnoSoluciones CR'
SELECT @CompanyId2 = Id FROM Companies WHERE Name = 'Café Dorado S.A.'
SELECT @CompanyId3 = Id FROM Companies WHERE Name = 'EcoTurismo Guanacaste'

-- Get Payroll IDs
SELECT @PayrollId1 = Id FROM Payrolls WHERE CompanyId = @CompanyId1
SELECT @PayrollId2 = Id FROM Payrolls WHERE CompanyId = @CompanyId2
SELECT @PayrollId3 = Id FROM Payrolls WHERE CompanyId = @CompanyId3

-- Get Employee IDs for Company1 (TecnoSoluciones CR)
SELECT TOP 1 @Employee1_Company1 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId1
  AND np.FirstName = 'Miguel'
  AND np.FirstSurname = 'Soto'

SELECT TOP 1 @Employee2_Company1 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId1
  AND np.FirstName = 'Gabriela'
  AND np.FirstSurname = 'Rodríguez'

-- Get Employee IDs for Company2 (Café Dorado S.A.)
SELECT TOP 1 @Employee1_Company2 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId2
  AND np.FirstName = 'Ricardo'
  AND np.FirstSurname = 'Blanco'

SELECT TOP 1 @Employee2_Company2 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId2
  AND np.FirstName = 'Laura'
  AND np.FirstSurname = 'Fallas'

-- Get Employee IDs for Company3 (EcoTurismo Guanacaste)
SELECT TOP 1 @Employee1_Company3 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId3
  AND np.FirstName = 'Alejandro'
  AND np.FirstSurname = 'Rojas'

SELECT TOP 1 @Employee2_Company3 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId3
  AND np.FirstName = 'Sofía'
  AND np.FirstSurname = 'Araya'

-- ================================================
-- Company1 - Monthly Payroll (30 days: 2024-01-01 to 2024-01-30)
-- Breaking into 7-day timesheets + final 2-day timesheet
-- ================================================

-- Employee1 - Company1 Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee1_Company1, @PayrollId1),
    ('2024-01-08', '2024-01-14', @Employee1_Company1, @PayrollId1),
    ('2024-01-15', '2024-01-21', @Employee1_Company1, @PayrollId1),
    ('2024-01-22', '2024-01-28', @Employee1_Company1, @PayrollId1),
    ('2024-01-29', '2024-01-30', @Employee1_Company1, @PayrollId1)

-- Employee2 - Company1 Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee2_Company1, @PayrollId1),
    ('2024-01-08', '2024-01-14', @Employee2_Company1, @PayrollId1),
    ('2024-01-15', '2024-01-21', @Employee2_Company1, @PayrollId1),
    ('2024-01-22', '2024-01-28', @Employee2_Company1, @PayrollId1),
    ('2024-01-29', '2024-01-30', @Employee2_Company1, @PayrollId1)

-- ================================================
-- Company2 - Biweekly Payroll (15 days: 2024-01-01 to 2024-01-15)
-- Breaking into 7-day timesheets + final 1-day timesheet
-- ================================================

-- Employee1 - Company2 Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee1_Company2, @PayrollId2),
    ('2024-01-08', '2024-01-14', @Employee1_Company2, @PayrollId2),
    ('2024-01-15', '2024-01-15', @Employee1_Company2, @PayrollId2)

-- Employee2 - Company2 Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee2_Company2, @PayrollId2),
    ('2024-01-08', '2024-01-14', @Employee2_Company2, @PayrollId2),
    ('2024-01-15', '2024-01-15', @Employee2_Company2, @PayrollId2)

-- ================================================
-- Company3 - Weekly Payroll (7 days: 2024-01-01 to 2024-01-07)
-- Exact match with payroll period
-- ================================================

-- Employee1 - Company3 Timesheet
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee1_Company3, @PayrollId3)

-- Employee2 - Company3 Timesheet
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-01', '2024-01-07', @Employee2_Company3, @PayrollId3)

-- ================================================
-- Future Timesheets (PayrollId = NULL)
-- These represent work done but not yet processed in payroll
-- ================================================

-- ================================================
-- Company1 - Next Monthly Period (30 days: 2024-02-01 to 2024-03-01)
-- ================================================

-- Employee1 - Company1 Future Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-02-01', '2024-02-07', @Employee1_Company1, NULL),
    ('2024-02-08', '2024-02-14', @Employee1_Company1, NULL),
    ('2024-02-15', '2024-02-21', @Employee1_Company1, NULL),
    ('2024-02-22', '2024-02-28', @Employee1_Company1, NULL),
    ('2024-02-29', '2024-03-01', @Employee1_Company1, NULL)

-- Employee2 - Company1 Future Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-02-01', '2024-02-07', @Employee2_Company1, NULL),
    ('2024-02-08', '2024-02-14', @Employee2_Company1, NULL),
    ('2024-02-15', '2024-02-21', @Employee2_Company1, NULL),
    ('2024-02-22', '2024-02-28', @Employee2_Company1, NULL),
    ('2024-02-29', '2024-03-01', @Employee2_Company1, NULL)

-- ================================================
-- Company2 - Next Biweekly Period (15 days: 2024-01-16 to 2024-01-30)
-- ================================================

-- Employee1 - Company2 Future Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-16', '2024-01-22', @Employee1_Company2, NULL),
    ('2024-01-23', '2024-01-29', @Employee1_Company2, NULL),
    ('2024-01-30', '2024-01-30', @Employee1_Company2, NULL)

-- Employee2 - Company2 Future Timesheets
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-16', '2024-01-22', @Employee2_Company2, NULL),
    ('2024-01-23', '2024-01-29', @Employee2_Company2, NULL),
    ('2024-01-30', '2024-01-30', @Employee2_Company2, NULL)

-- ================================================
-- Company3 - Next Weekly Period (7 days: 2024-01-08 to 2024-01-14)
-- ================================================

-- Employee1 - Company3 Future Timesheet
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-08', '2024-01-14', @Employee1_Company3, NULL)

-- Employee2 - Company3 Future Timesheet
INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
VALUES 
    ('2024-01-08', '2024-01-14', @Employee2_Company3, NULL)
