USE PayrollSystem

-- ================================================
-- Script to insert dummy Timesheets using the InsertTimesheetsForPeriod procedure
-- This procedure automatically creates timesheets with proper weekly boundaries
-- and triggers the automatic creation of Days records
-- ================================================

DECLARE @CompanyId UNIQUEIDENTIFIER

DECLARE @PayrollId UNIQUEIDENTIFIER

-- Employee IDs for Company3
DECLARE @Employee1 UNIQUEIDENTIFIER
DECLARE @Employee2 UNIQUEIDENTIFIER

-- Get Company ID
SELECT @CompanyId = Id FROM Companies WHERE Name = 'EcoTurismo Guanacaste'

-- Get Employee IDs for Company3 (EcoTurismo Guanacaste)
SELECT TOP 1 @Employee1 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId
  AND np.FirstName = 'Alejandro'
  AND np.FirstSurname = 'Rojas'

SELECT TOP 1 @Employee2 = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE e.CompanyId = @CompanyId
  AND np.FirstName = 'Sofía'
  AND np.FirstSurname = 'Araya'

-- Verify all required data exists
IF @CompanyId IS NULL
BEGIN
    PRINT 'ERROR: Company not found!'
    RETURN
END

IF @Employee1 IS NULL OR @Employee2 IS NULL
BEGIN
    PRINT 'ERROR: Employees not found!'
    RETURN
END

-- ================================================
-- Company3 - Next Weekly Period (7 days: 2024-01-08 to 2024-01-14)
-- ================================================

-- Employee1 - Company3 Future Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-08',
    @PeriodEndDate = '2024-01-14',
    @EmployeeId = @Employee1,
    @PayrollId = NULL

PRINT 'Created future timesheets for Alejandro Rojas (EcoTurismo) - Next weekly period'

-- Employee2 - Company3 Future Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-08',
    @PeriodEndDate = '2024-01-14',
    @EmployeeId = @Employee2,
    @PayrollId = NULL

PRINT 'Created future timesheets for Sofía Araya (EcoTurismo) - Next weekly period'

-- ================================================
-- SUMMARY
-- ================================================

PRINT '================================================'
PRINT 'TIMESHEET CREATION COMPLETE'
PRINT '================================================'

-- Show summary of created timesheets
SELECT 
    c.Name AS Company,
    COUNT(*) AS TotalTimesheets,
    SUM(CASE WHEN t.PayrollId IS NOT NULL THEN 1 ELSE 0 END) AS ProcessedTimesheets,
    SUM(CASE WHEN t.PayrollId IS NULL THEN 1 ELSE 0 END) AS FutureTimesheets,
    MIN(t.StartDate) AS EarliestDate,
    MAX(t.EndDate) AS LatestDate
FROM Timesheets t
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
GROUP BY c.Name
ORDER BY c.Name

PRINT 'All timesheets have been created using the InsertTimesheetsForPeriod procedure.'
PRINT 'The trigger has automatically created corresponding Days records.'
PRINT 'Next step: Run the dummy Days update script to populate work hours and descriptions.'
