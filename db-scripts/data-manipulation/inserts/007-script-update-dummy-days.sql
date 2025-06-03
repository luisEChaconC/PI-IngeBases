-- ================================================
-- Script to update dummy Days with realistic employee data
-- This updates the Days records automatically created by the trigger
-- when Timesheets are inserted by the InsertTimesheetsForPeriod procedure
-- 
-- The trigger creates Days with:
-- - Date, TimesheetId, IsApproved = 0
-- This script adds:
-- - HoursWorked, WorkDescription, and manages approval workflow
-- ================================================

-- First, ensure we have Days records to work with
-- (They should exist if timesheets were created and trigger fired)
DECLARE @DaysCount INT
DECLARE @RowsAffected INT

PRINT 'Updating dummy data for Days automatically created by trigger...'

-- Verify Days exist before proceeding
IF NOT EXISTS (SELECT 1 FROM Days)
BEGIN
    PRINT 'WARNING: No Days records found. Please ensure Timesheets have been created first.'
    PRINT 'The trigger should automatically create Days when Timesheets are inserted.'
    RETURN
END

SELECT @DaysCount = COUNT(*) FROM Days
PRINT 'Found ' + CAST(@DaysCount AS VARCHAR(10)) + ' Days records to update'

-- ================================================
-- BASIC WORK HOURS AND DESCRIPTIONS BY COMPANY
-- ================================================

-- Update Days for Company1 (TecnoSoluciones CR) - Technology Company
-- Standard 8-hour workdays Monday-Friday, weekends off
UPDATE d
SET 
    HoursWorked = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) IN (1, 7) THEN 0  -- Sunday, Saturday
        ELSE 8  -- Monday to Friday
    END,
    WorkDescription = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) IN (1, 7) THEN NULL  -- Weekend
        WHEN DATEPART(WEEKDAY, d.Date) = 2 THEN 'Weekly planning and code review'  -- Monday (Day 1)
        WHEN DATEPART(WEEKDAY, d.Date) = 3 THEN 'Software development and testing'  -- Tuesday (Day 2)
        WHEN DATEPART(WEEKDAY, d.Date) = 4 THEN 'Client meetings and bug fixes'  -- Wednesday (Day 3)
        WHEN DATEPART(WEEKDAY, d.Date) = 5 THEN 'Development and documentation'  -- Thursday (Day 4)
        WHEN DATEPART(WEEKDAY, d.Date) = 6 THEN 'Code deployment and maintenance'  -- Friday (Day 5)
    END
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
WHERE c.Name = 'TecnoSoluciones CR'

SET @RowsAffected = @@ROWCOUNT
PRINT 'Updated ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for TecnoSoluciones CR'

-- Update Days for Company2 (Café Dorado S.A.) - Coffee Shop
-- Works 7 days a week: 8 hours weekdays, 6 hours weekends
UPDATE d
SET 
    HoursWorked = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) IN (1, 7) THEN 6  -- Weekend shifts (6 hours)
        ELSE 8  -- Weekday shifts (8 hours)
    END,
    WorkDescription = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) = 1 THEN 'Weekend service and inventory check'  -- Sunday (Day 7)
        WHEN DATEPART(WEEKDAY, d.Date) = 2 THEN 'Coffee preparation and customer service'  -- Monday (Day 1)
        WHEN DATEPART(WEEKDAY, d.Date) = 3 THEN 'Barista duties and equipment maintenance'  -- Tuesday (Day 2)
        WHEN DATEPART(WEEKDAY, d.Date) = 4 THEN 'Customer service and sales support'  -- Wednesday (Day 3)
        WHEN DATEPART(WEEKDAY, d.Date) = 5 THEN 'Coffee brewing and quality control'  -- Thursday (Day 4)
        WHEN DATEPART(WEEKDAY, d.Date) = 6 THEN 'Customer service and store cleaning'  -- Friday (Day 5)
        WHEN DATEPART(WEEKDAY, d.Date) = 7 THEN 'Weekend service and cash reconciliation'  -- Saturday (Day 6)
    END
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
WHERE c.Name = 'Café Dorado S.A.'

SET @RowsAffected = @@ROWCOUNT
PRINT 'Updated ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Café Dorado S.A.'

-- Update Days for Company3 (EcoTurismo Guanacaste) - Tourism Company  
-- Busy tourism schedule: 8 hours weekdays, 10 hours weekends
UPDATE d
SET 
    HoursWorked = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) IN (1, 7) THEN 10  -- Busy weekend tours (10 hours)
        ELSE 8  -- Regular weekday tours (8 hours)
    END,
    WorkDescription = CASE 
        WHEN DATEPART(WEEKDAY, d.Date) = 1 THEN 'Weekend nature tours and group activities'  -- Sunday (Day 7)
        WHEN DATEPART(WEEKDAY, d.Date) = 2 THEN 'Tour planning and customer coordination'  -- Monday (Day 1)
        WHEN DATEPART(WEEKDAY, d.Date) = 3 THEN 'Nature guide services and education'  -- Tuesday (Day 2)
        WHEN DATEPART(WEEKDAY, d.Date) = 4 THEN 'Eco-tourism activities and wildlife tours'  -- Wednesday (Day 3)
        WHEN DATEPART(WEEKDAY, d.Date) = 5 THEN 'Tourist assistance and route planning'  -- Thursday (Day 4)
        WHEN DATEPART(WEEKDAY, d.Date) = 6 THEN 'Equipment maintenance and tour preparation'  -- Friday (Day 5)
        WHEN DATEPART(WEEKDAY, d.Date) = 7 THEN 'Weekend adventure tours and customer service'  -- Saturday (Day 6)
    END
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
WHERE c.Name = 'EcoTurismo Guanacaste'

SET @RowsAffected = @@ROWCOUNT
PRINT 'Updated ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for EcoTurismo Guanacaste'

-- ================================================
-- APPROVAL LOGIC: Sequential approval rule implementation
-- This simulates a realistic approval workflow where days are approved progressively
-- ================================================

PRINT 'Applying approval logic...'

-- Step 1: All days in processed payrolls (PayrollId IS NOT NULL) are auto-approved
-- These represent completed and processed periods
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.PayrollId IS NOT NULL
  AND d.IsApproved = 0  -- Only update if not already approved

SET @RowsAffected = @@ROWCOUNT
PRINT 'Auto-approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for processed payrolls'

-- Step 2: Future timesheets (PayrollId IS NULL) - Simulate progressive approval
-- This simulates real-world scenarios where supervisors approve days incrementally

-- Company1 Future Timesheets - Simulate different approval progress per employee
-- Employee1 (Miguel): Approve first 10 days of February period
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'TecnoSoluciones CR'
  AND np.FirstName = 'Miguel'
  AND t.PayrollId IS NULL
  AND d.Date <= '2024-02-10'
  AND d.IsApproved = 0
  AND d.HoursWorked > 0  -- Only approve days with actual work

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Miguel (partial approval)'

-- Employee2 (Gabriela): Approve first 7 days of February period
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'TecnoSoluciones CR'
  AND np.FirstName = 'Gabriela'
  AND t.PayrollId IS NULL
  AND d.Date <= '2024-02-07'
  AND d.IsApproved = 0
  AND d.HoursWorked > 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Gabriela (partial approval)'

-- Company2 Future Timesheets - Coffee shop approval patterns
-- Employee1 (Ricardo): Approve almost complete period (12 days)
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'Café Dorado S.A.'
  AND np.FirstName = 'Ricardo'
  AND t.PayrollId IS NULL
  AND d.Date <= '2024-01-28'
  AND d.IsApproved = 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Ricardo (nearly complete)'

-- Employee2 (Laura): Approve only first 5 days
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'Café Dorado S.A.'
  AND np.FirstName = 'Laura'
  AND t.PayrollId IS NULL
  AND d.Date <= '2024-01-20'
  AND d.IsApproved = 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Laura (partial approval)'

-- Company3 Future Timesheets - Tourism weekly periods
-- Employee1 (Alejandro): Approve complete timesheet period
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Alejandro'
  AND t.PayrollId IS NULL
  AND d.IsApproved = 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Alejandro (complete period)'

-- Employee2 (Sofía): Approve first 4 days of her timesheet period
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Sofía'
  AND t.PayrollId IS NULL
  AND d.Date <= '2024-01-11'
  AND d.IsApproved = 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Sofía (partial approval)'

-- Step 3: Security rule - Ensure non-approved days have no supervisor assigned
-- This maintains data integrity in the approval workflow
UPDATE Days
SET SupervisorId = NULL
WHERE IsApproved = 0
  AND SupervisorId IS NOT NULL

SET @RowsAffected = @@ROWCOUNT
PRINT 'Cleared SupervisorId for ' + CAST(@RowsAffected AS VARCHAR(10)) + ' non-approved Days'

-- Step 4: Assign supervisors to approved days based on company
-- Only approved days should have supervisor assignments

-- TecnoSoluciones CR - Find and assign Miguel Soto as supervisor
UPDATE d
SET SupervisorId = s.Id
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN Supervisors s ON s.Id IN (
    SELECT e2.Id 
    FROM Employees e2 
    INNER JOIN NaturalPersons np ON e2.Id = np.Id 
    WHERE e2.CompanyId = c.Id 
      AND np.FirstName = 'Miguel' 
      AND np.FirstSurname = 'Soto'
)
WHERE c.Name = 'TecnoSoluciones CR'
  AND d.IsApproved = 1
  AND d.SupervisorId IS NULL

SET @RowsAffected = @@ROWCOUNT
PRINT 'Assigned Miguel Soto as supervisor for ' + CAST(@RowsAffected AS VARCHAR(10)) + ' approved Days (TecnoSoluciones)'

-- Café Dorado S.A. - Find and assign Ricardo Blanco as supervisor
UPDATE d
SET SupervisorId = s.Id
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN Supervisors s ON s.Id IN (
    SELECT e2.Id 
    FROM Employees e2 
    INNER JOIN NaturalPersons np ON e2.Id = np.Id 
    WHERE e2.CompanyId = c.Id 
      AND np.FirstName = 'Ricardo' 
      AND np.FirstSurname = 'Blanco'
)
WHERE c.Name = 'Café Dorado S.A.'
  AND d.IsApproved = 1
  AND d.SupervisorId IS NULL

SET @RowsAffected = @@ROWCOUNT
PRINT 'Assigned Ricardo Blanco as supervisor for ' + CAST(@RowsAffected AS VARCHAR(10)) + ' approved Days (Café Dorado)'

-- EcoTurismo Guanacaste - Find and assign Alejandro Rojas as supervisor
UPDATE d
SET SupervisorId = s.Id
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN Supervisors s ON s.Id IN (
    SELECT e2.Id 
    FROM Employees e2 
    INNER JOIN NaturalPersons np ON e2.Id = np.Id 
    WHERE e2.CompanyId = c.Id 
      AND np.FirstName = 'Alejandro' 
      AND np.FirstSurname = 'Rojas'
)
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND d.IsApproved = 1
  AND d.SupervisorId IS NULL

SET @RowsAffected = @@ROWCOUNT
PRINT 'Assigned Alejandro Rojas as supervisor for ' + CAST(@RowsAffected AS VARCHAR(10)) + ' approved Days (EcoTurismo)'

-- ================================================
-- REALISTIC VARIATIONS: Individual employee scenarios
-- These updates simulate real-world variations in work patterns
-- ================================================

PRINT 'Applying realistic work variations...'

-- Scenario 1: Sick leave (Company2 - Coffee Shop)
-- Laura takes a sick day
UPDATE d
SET HoursWorked = 0, WorkDescription = 'Sick leave'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'Café Dorado S.A.'
  AND np.FirstName = 'Laura'
  AND d.Date = '2024-01-10'  -- One specific sick day

PRINT 'Applied sick leave for Laura on 2024-01-10'

-- Scenario 2: Part-time/Half days (Company3 - Tourism)
-- Sofía works half days on Mondays for equipment maintenance
UPDATE d
SET HoursWorked = 4, WorkDescription = 'Half day: Equipment maintenance and prep'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Sofía'
  AND DATEPART(WEEKDAY, d.Date) = 2  -- Monday (Day 1) half days
  AND d.Date >= '2024-01-08'

PRINT 'Applied half days for Sofía on Mondays'

-- Final summary
PRINT '================================================'
PRINT 'DUMMY DATA UPDATE COMPLETE'
PRINT '================================================'
PRINT 'Summary of Days updated:'

SELECT 
    c.Name AS Company,
    COUNT(*) AS TotalDays,
    SUM(CASE WHEN d.IsApproved = 1 THEN 1 ELSE 0 END) AS ApprovedDays,
    SUM(CASE WHEN d.SupervisorId IS NOT NULL THEN 1 ELSE 0 END) AS DaysWithSupervisor,
    SUM(d.HoursWorked) AS TotalHours
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.EmployeeId = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
GROUP BY c.Name
ORDER BY c.Name

PRINT 'All dummy data has been successfully applied to automatically generated Days records.'
