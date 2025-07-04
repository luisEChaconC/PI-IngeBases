USE PayrollSystem

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
    PRINT 'The trigger should automatically create Days when Timesheets is inserted.'
    RETURN
END

SELECT @DaysCount = COUNT(*) FROM Days
PRINT 'Found ' + CAST(@DaysCount AS VARCHAR(10)) + ' Days records to update'

-- ================================================
-- MANUAL WORK HOURS AND DESCRIPTIONS FOR ECOTURISMO GUANACASTE
-- Update each day individually for Alejandro Rojas and Sofía Araya
-- Using period 2024-01-08 to 2024-01-14 as an example valid week
-- ================================================

PRINT 'Manually updating daily work data for EcoTurismo Guanacaste employees...'

-- Define Employee IDs for clarity in manual updates
DECLARE @AlejandroRojasId UNIQUEIDENTIFIER
DECLARE @SofiaArayaId UNIQUEIDENTIFIER

SELECT TOP 1 @AlejandroRojasId = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Alejandro'
  AND np.FirstSurname = 'Rojas'

SELECT TOP 1 @SofiaArayaId = e.Id 
FROM Employees e
INNER JOIN NaturalPersons np ON e.Id = np.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Sofía'
  AND np.FirstSurname = 'Araya'

-- Verify Employee IDs exist
IF @AlejandroRojasId IS NULL OR @SofiaArayaId IS NULL
BEGIN
    PRINT 'ERROR: EcoTurismo Guanacaste employees not found for manual update!'
    RETURN
END

-- Alejandro Rojas (EcoTurismo) - Manual Daily Updates (2024-01-08 to 2024-01-14)
-- Weekday (Jan 8-12): 8 hours, Weekend (Jan 13-14): 10 hours
UPDATE d
SET HoursWorked = 8, WorkDescription = 'Tour planning and customer coordination'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-08' -- Monday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Nature guide services and education'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-09' -- Tuesday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Eco-tourism activities and wildlife tours'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-10' -- Wednesday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Tourist assistance and route planning'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-11' -- Thursday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Equipment maintenance and tour preparation'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-12' -- Friday

UPDATE d
SET HoursWorked = 10, WorkDescription = 'Weekend adventure tours and customer service'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-13' -- Saturday

UPDATE d
SET HoursWorked = 10, WorkDescription = 'Weekend nature tours and group activities'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @AlejandroRojasId AND d.Date = '2024-01-14' -- Sunday

PRINT 'Manually updated Days for Alejandro Rojas (EcoTurismo)'

-- Sofía Araya (EcoTurismo) - Manual Daily Updates (2024-01-08 to 2024-01-14)
-- Monday (Half day): 4 hours, Other Weekdays: 8 hours, Weekend: 10 hours
UPDATE d
SET HoursWorked = 4, WorkDescription = 'Half day: Equipment maintenance and prep'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-08' -- Monday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Nature guide services and education'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-09' -- Tuesday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Eco-tourism activities and wildlife tours'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-10' -- Wednesday

UPDATE d
SET HoursWorked = 8, WorkDescription = 'Tourist assistance and route planning'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-11' -- Thursday

UPDATE d
SET HoursWorked = 10, WorkDescription = 'Equipment maintenance and tour preparation' -- This is a weekend description but applied to a Friday for variation
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-12' -- Friday

UPDATE d
SET HoursWorked = 10, WorkDescription = 'Weekend adventure tours and customer service'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-13' -- Saturday

UPDATE d
SET HoursWorked = 10, WorkDescription = 'Weekend nature tours and group activities'
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.EmployeeId = @SofiaArayaId AND d.Date = '2024-01-14' -- Sunday

PRINT 'Manually updated Days for Sofía Araya (EcoTurismo)'

-- ================================================
-- APPROVAL LOGIC: Sequential approval rule implementation
-- This simulates a realistic approval workflow where days are approved progressively
-- ================================================

PRINT 'Applying approval logic...'

-- Step 1: All days in processed payrolls (PayrollId IS NOT NULL) are auto-approved
-- These represent completed and processed periods (None for EcoTurismo in this script)
-- This step will update 0 rows based on the modified 006 script.
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

-- Company3 Future Timesheets - Tourism weekly periods
-- Employee1 (Alejandro): Approve complete timesheet period (2024-01-08 to 2024-01-14)
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.Id = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Alejandro'
  AND t.PayrollId IS NULL
  AND d.Date BETWEEN '2024-01-08' AND '2024-01-14' -- Approve the full 7-day period
  AND d.IsApproved = 0

SET @RowsAffected = @@ROWCOUNT
PRINT 'Approved ' + CAST(@RowsAffected AS VARCHAR(10)) + ' Days for Alejandro (complete period)'

-- Employee2 (Sofía): Approve first 4 days of her timesheet period (2024-01-08 to 2024-01-11)
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.Id = e.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
WHERE c.Name = 'EcoTurismo Guanacaste'
  AND np.FirstName = 'Sofía'
  AND t.PayrollId IS NULL
  AND d.Date BETWEEN '2024-01-08' AND '2024-01-11' -- Approve first 4 days
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

-- EcoTurismo Guanacaste - Find and assign Alejandro Rojas as supervisor
UPDATE d
SET SupervisorId = s.Id
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
INNER JOIN Employees e ON t.Id = e.Id
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
-- The half-day scenario for Sofía is now included in the manual daily updates.
-- ================================================

-- Final summary
PRINT '================================================'
PRINT 'DUMMY DATA UPDATE COMPLETE'
PRINT '================================================'
PRINT 'Summary of Days updated for EcoTurismo Guanacaste:'