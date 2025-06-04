USE PayrollSystem

-- This script creates associations between employees and benefits based on their contract types
-- It uses the EmployeeTypesXBenefits table to determine which benefits each employee is eligible for

-- Insert employee-benefit associations based on contract type eligibility
INSERT INTO EmployeeXBenefit (EmployeeId, BenefitId)
SELECT DISTINCT 
    e.Id AS EmployeeId,
    etxb.BenefitId
FROM Employees e
INNER JOIN EmployeeTypes et ON e.ContractType = et.Name
INNER JOIN EmployeeTypesXBenefits etxb ON et.Id = etxb.EmployeeTypeId
INNER JOIN Benefits b ON etxb.BenefitId = b.Id
WHERE 
    -- Only associate employees with benefits from their own company
    e.CompanyId = b.CompanyId
    -- Ensure the employee type exists
    AND et.Name IN ('Full-Time', 'Part-Time', 'Professional Services')
ORDER BY e.Id, etxb.BenefitId;

-- Verification query to show all employee-benefit associations
SELECT 
    c.Name AS CompanyName,
    CONCAT(np.FirstName, ' ', np.FirstSurname) AS EmployeeName,
    e.WorkerId,
    e.ContractType,
    b.Name AS BenefitName,
    b.Type AS BenefitType,
    CASE 
        WHEN b.Type = 'FixedAmount' THEN CONCAT('$', CAST(b.FixedAmount AS VARCHAR(10)))
        WHEN b.Type = 'FixedPercentage' THEN CONCAT(CAST(b.FixedPercentage AS VARCHAR(10)), '%')
        WHEN b.Type = 'API' THEN 'API Integration'
    END AS BenefitValue
FROM EmployeeXBenefit exb
INNER JOIN Employees e ON exb.EmployeeId = e.Id
INNER JOIN Benefits b ON exb.BenefitId = b.Id
INNER JOIN Companies c ON e.CompanyId = c.Id
INNER JOIN NaturalPersons np ON e.Id = np.Id
ORDER BY c.Name, e.WorkerId, b.Name;

-- Summary by employee type and company
SELECT 
    c.Name AS CompanyName,
    e.ContractType,
    COUNT(DISTINCT e.Id) AS EmployeesCount,
    COUNT(exb.Id) AS TotalBenefitAssociations,
    CASE 
        WHEN COUNT(DISTINCT e.Id) > 0 
        THEN CAST(COUNT(exb.Id) AS FLOAT) / COUNT(DISTINCT e.Id)
        ELSE 0 
    END AS AvgBenefitsPerEmployee
FROM Companies c
INNER JOIN Employees e ON c.Id = e.CompanyId
LEFT JOIN EmployeeXBenefit exb ON e.Id = exb.EmployeeId
GROUP BY c.Name, e.ContractType
ORDER BY c.Name, e.ContractType;
