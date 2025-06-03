-- ================================================
-- Script to insert dummy Timesheets using the InsertTimesheetsForPeriod procedure
-- This procedure automatically creates timesheets with proper weekly boundaries
-- and triggers the automatic creation of Days records
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

PRINT 'Starting dummy timesheet creation using InsertTimesheetsForPeriod procedure...'

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

-- Verify all required data exists
IF @CompanyId1 IS NULL OR @CompanyId2 IS NULL OR @CompanyId3 IS NULL
BEGIN
    PRINT 'ERROR: One or more companies not found!'
    RETURN
END

IF @PayrollId1 IS NULL OR @PayrollId2 IS NULL OR @PayrollId3 IS NULL
BEGIN
    PRINT 'ERROR: One or more payrolls not found!'
    RETURN
END

IF @Employee1_Company1 IS NULL OR @Employee2_Company1 IS NULL OR 
   @Employee1_Company2 IS NULL OR @Employee2_Company2 IS NULL OR
   @Employee1_Company3 IS NULL OR @Employee2_Company3 IS NULL
BEGIN
    PRINT 'ERROR: One or more employees not found!'
    RETURN
END

PRINT 'All required entities found. Creating timesheets...'

-- ================================================
-- PROCESSED PAYROLL TIMESHEETS
-- These timesheets have already been processed and paid
-- ================================================

PRINT 'Creating processed payroll timesheets...'

-- ================================================
-- Company1 - Monthly Payroll (30 days: 2024-01-01 to 2024-01-30)
-- The procedure will automatically create weekly timesheets
-- ================================================

-- Employee1 - Company1 Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-30',
    @EmployeeId = @Employee1_Company1,
    @PayrollId = @PayrollId1

PRINT 'Created timesheets for Miguel Soto (TecnoSoluciones) - Monthly period'

-- Employee2 - Company1 Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-30',
    @EmployeeId = @Employee2_Company1,
    @PayrollId = @PayrollId1

PRINT 'Created timesheets for Gabriela Rodríguez (TecnoSoluciones) - Monthly period'

-- ================================================
-- Company2 - Biweekly Payroll (15 days: 2024-01-01 to 2024-01-15)
-- The procedure will create weekly timesheets + partial week
-- ================================================

-- Employee1 - Company2 Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-15',
    @EmployeeId = @Employee1_Company2,
    @PayrollId = @PayrollId2

PRINT 'Created timesheets for Ricardo Blanco (Café Dorado) - Biweekly period'

-- Employee2 - Company2 Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-15',
    @EmployeeId = @Employee2_Company2,
    @PayrollId = @PayrollId2

PRINT 'Created timesheets for Laura Fallas (Café Dorado) - Biweekly period'

-- ================================================
-- Company3 - Weekly Payroll (7 days: 2024-01-01 to 2024-01-07)
-- The procedure will create exactly one weekly timesheet
-- ================================================

-- Employee1 - Company3 Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-07',
    @EmployeeId = @Employee1_Company3,
    @PayrollId = @PayrollId3

PRINT 'Created timesheets for Alejandro Rojas (EcoTurismo) - Weekly period'

-- Employee2 - Company3 Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-01',
    @PeriodEndDate = '2024-01-07',
    @EmployeeId = @Employee2_Company3,
    @PayrollId = @PayrollId3

PRINT 'Created timesheets for Sofía Araya (EcoTurismo) - Weekly period'

-- ================================================
-- FUTURE TIMESHEETS (PayrollId = NULL)
-- These represent work done but not yet processed in payroll
-- They demonstrate the approval workflow for pending periods
-- ================================================

PRINT 'Creating future timesheets (not yet processed)...'

-- ================================================
-- Company1 - Next Monthly Period (30 days: 2024-02-01 to 2024-03-01)
-- Monthly payroll periods must be exactly 30 days
-- ================================================

-- Employee1 - Company1 Future Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-02-01',
    @PeriodEndDate = '2024-03-01',
    @EmployeeId = @Employee1_Company1,
    @PayrollId = NULL

PRINT 'Created future timesheets for Miguel Soto (TecnoSoluciones) - February period (30 days)'

-- Employee2 - Company1 Future Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-02-01',
    @PeriodEndDate = '2024-03-01',
    @EmployeeId = @Employee2_Company1,
    @PayrollId = NULL

PRINT 'Created future timesheets for Gabriela Rodríguez (TecnoSoluciones) - February period (30 days)'

-- ================================================
-- Company2 - Next Biweekly Period (15 days: 2024-01-16 to 2024-01-30)
-- ================================================

-- Employee1 - Company2 Future Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-16',
    @PeriodEndDate = '2024-01-30',
    @EmployeeId = @Employee1_Company2,
    @PayrollId = NULL

PRINT 'Created future timesheets for Ricardo Blanco (Café Dorado) - Next biweekly period'

-- Employee2 - Company2 Future Timesheets
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-16',
    @PeriodEndDate = '2024-01-30',
    @EmployeeId = @Employee2_Company2,
    @PayrollId = NULL

PRINT 'Created future timesheets for Laura Fallas (Café Dorado) - Next biweekly period'

-- ================================================
-- Company3 - Next Weekly Period (7 days: 2024-01-08 to 2024-01-14)
-- ================================================

-- Employee1 - Company3 Future Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-08',
    @PeriodEndDate = '2024-01-14',
    @EmployeeId = @Employee1_Company3,
    @PayrollId = NULL

PRINT 'Created future timesheets for Alejandro Rojas (EcoTurismo) - Next weekly period'

-- Employee2 - Company3 Future Timesheet
EXEC dbo.InsertTimesheetsForPeriod 
    @PeriodStartDate = '2024-01-08',
    @PeriodEndDate = '2024-01-14',
    @EmployeeId = @Employee2_Company3,
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
