-- ================================================
-- Script to update dummy Days with realistic employee data
-- This updates the Days records automatically created by the trigger
-- ================================================

-- Update Days for Company1 (TecnoSoluciones CR) - Technology Company
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

-- Update Days for Company2 (Café Dorado S.A.) - Coffee Shop
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

-- Update Days for Company3 (EcoTurismo Guanacaste) - Tourism Company
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

-- ================================================
-- APPROVAL LOGIC: Sequential approval rule implementation
-- ================================================

-- Step 1: All days in processed payrolls (PayrollId IS NOT NULL) are approved
UPDATE d
SET IsApproved = 1
FROM Days d
INNER JOIN Timesheets t ON d.TimesheetId = t.Id
WHERE t.PayrollId IS NOT NULL

-- Step 2: Future timesheets (PayrollId IS NULL) - Simulate progressive approval
-- For each timesheet, we'll approve days sequentially up to a certain point

-- Company1 Future Timesheets - Simulate different approval progress per employee
-- Employee1: Approve first 10 days of February period
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

-- Employee2: Approve first 7 days of February period
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

-- Company2 Future Timesheets - Simulate different approval progress
-- Employee1: Approve first 12 days (almost complete period)
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

-- Employee2: Approve first 5 days only
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

-- Company3 Future Timesheets - Weekly periods
-- Employee1: Approve complete week
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

-- Employee2: Approve first 4 days of the week
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

-- Step 3: Ensure non-approved days have no supervisor assigned
-- Days that are not yet approved should not have a SupervisorId
UPDATE Days
SET SupervisorId = NULL
WHERE IsApproved = 0

-- Step 4: Assign supervisors to approved days based on company
-- TecnoSoluciones CR - Miguel Soto is the supervisor
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

-- Café Dorado S.A. - Ricardo Blanco is the supervisor
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

-- EcoTurismo Guanacaste - Alejandro Rojas is the supervisor
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

-- ================================================
-- Add some realistic variations for individual employees
-- ================================================

-- Some employees take sick days (Company2 - Coffee Shop)
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

-- Some employees work partial days (Company3 - Tourism)
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
