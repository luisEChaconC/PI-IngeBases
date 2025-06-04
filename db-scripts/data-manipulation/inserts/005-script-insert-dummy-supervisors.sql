-- ================================================
-- Script to insert dummy Supervisors
-- Supervisors (same as Payroll Managers):
-- - TecnoSoluciones CR (Miguel Soto)
-- - Café Dorado S.A. (Ricardo Blanco)
-- - EcoTurismo Guanacaste (Alejandro Rojas)
-- ================================================

DECLARE @SupervisorId UNIQUEIDENTIFIER

-- ================================================
-- Supervisor for TecnoSoluciones CR (Miguel Soto)
-- ================================================
SELECT TOP 1 @SupervisorId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'TecnoSoluciones CR'
  AND np.FirstName = 'Miguel'
  AND np.FirstSurname = 'Soto'

INSERT INTO Supervisors (Id)
VALUES (@SupervisorId)

-- ================================================
-- Supervisor for Café Dorado S.A. (Ricardo Blanco)
-- ================================================
SELECT TOP 1 @SupervisorId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'Café Dorado S.A.'
  AND np.FirstName = 'Ricardo'
  AND np.FirstSurname = 'Blanco'

INSERT INTO Supervisors (Id)
VALUES (@SupervisorId)

-- ================================================
-- Supervisor for EcoTurismo Guanacaste (Alejandro Rojas)
-- ================================================
SELECT TOP 1 @SupervisorId = e.Id 
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Alejandro'
  AND np.FirstSurname = 'Rojas'

INSERT INTO Supervisors (Id)
VALUES (@SupervisorId)
