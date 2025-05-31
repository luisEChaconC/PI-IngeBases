-- ================================================
-- Script to insert dummy Payrolls
-- Payment periods:
-- - Company1: Monthly (30 days)
-- - Company2: Biweekly (15 days) 
-- - Company3: Weekly (7 days)
-- ================================================

-- Variables for Company and PayrollManager IDs
DECLARE @CompanyId1 UNIQUEIDENTIFIER
DECLARE @CompanyId2 UNIQUEIDENTIFIER
DECLARE @CompanyId3 UNIQUEIDENTIFIER
DECLARE @PayrollManagerId1 UNIQUEIDENTIFIER
DECLARE @PayrollManagerId2 UNIQUEIDENTIFIER
DECLARE @PayrollManagerId3 UNIQUEIDENTIFIER

-- Get Company IDs
SELECT @CompanyId1 = Id FROM Companies WHERE Name = 'TecnoSoluciones CR'
SELECT @CompanyId2 = Id FROM Companies WHERE Name = 'Café Dorado S.A.'
SELECT @CompanyId3 = Id FROM Companies WHERE Name = 'EcoTurismo Guanacaste'

-- Get PayrollManager IDs (using the employees we made payroll managers)
SELECT TOP 1 @PayrollManagerId1 = pm.Id 
FROM PayrollManagers pm
INNER JOIN Employees e ON pm.Id = e.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId1

SELECT TOP 1 @PayrollManagerId2 = pm.Id 
FROM PayrollManagers pm
INNER JOIN Employees e ON pm.Id = e.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId2

SELECT TOP 1 @PayrollManagerId3 = pm.Id 
FROM PayrollManagers pm
INNER JOIN Employees e ON pm.Id = e.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId3

-- ================================================
-- Company1 - Monthly Payroll (30 days)
-- ================================================
INSERT INTO Payrolls (StartDate, EndDate, CompanyId, PayrollManagerId)
VALUES 
    ('2024-01-01', '2024-01-30', @CompanyId1, @PayrollManagerId1)

-- ================================================
-- Company2 - Biweekly Payroll (15 days)
-- ================================================
INSERT INTO Payrolls (StartDate, EndDate, CompanyId, PayrollManagerId)
VALUES 
    ('2024-01-01', '2024-01-15', @CompanyId2, @PayrollManagerId2)

-- ================================================
-- Company3 - Weekly Payroll (7 days)
-- ================================================
INSERT INTO Payrolls (StartDate, EndDate, CompanyId, PayrollManagerId)
VALUES 
    ('2024-01-01', '2024-01-07', @CompanyId3, @PayrollManagerId3)
