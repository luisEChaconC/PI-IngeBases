USE PayrollSystem

-- ================================================
-- Script to insert Payroll Managers
-- Payroll Managers:
-- - TecnoSoluciones CR (Miguel Soto)
-- - Café Dorado S.A. (Ricardo Blanco)
-- - EcoTurismo Guanacaste (Alejandro Rojas)
-- ================================================

DECLARE @PayrollManagerId UNIQUEIDENTIFIER

-- ================================================
-- Payroll Manager for TecnoSoluciones CR (Miguel Soto)
-- ================================================
SELECT TOP 1 @PayrollManagerId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'TecnoSoluciones CR'
  AND np.FirstName = 'Miguel'
  AND np.FirstSurname = 'Soto'

INSERT INTO PayrollManagers (Id)
VALUES (@PayrollManagerId)

-- ================================================
-- Payroll Manager for Café Dorado S.A. (Ricardo Blanco)
-- ================================================
SELECT TOP 1 @PayrollManagerId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'Café Dorado S.A.'
  AND np.FirstName = 'Ricardo'
  AND np.FirstSurname = 'Blanco'

INSERT INTO PayrollManagers (Id)
VALUES (@PayrollManagerId)

-- ================================================
-- Payroll Manager for EcoTurismo Guanacaste (Alejandro Rojas)
-- ================================================
SELECT TOP 1 @PayrollManagerId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Alejandro'
  AND np.FirstSurname = 'Rojas'

INSERT INTO PayrollManagers (Id)
VALUES (@PayrollManagerId)



