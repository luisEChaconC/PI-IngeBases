CREATE PROCEDURE sp_GetEmployeePayrollReport
    @CompanyId UNIQUEIDENTIFIER,
    @EmployerId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Name AS CompanyName,
        np_employer.FirstName + ' ' + np_employer.FirstSurname + ' ' + np_employer.SecondSurname AS EmployerName,
        np_employee.FirstSurname + ' ' + np_employee.SecondSurname + ' ' + np_employee.FirstName AS EmployeeName,
        p_employee.LegalId,
        e.ContractType AS EmployeeType,
        FORMAT(pay.StartDate, 'dd/MM/yyyy') + ' - ' + FORMAT(pay.EndDate, 'dd/MM/yyyy') AS PaymentPeriod,
        FORMAT(pd.IssueDate, 'dd/MM/yyyy') AS PaymentDate,
        pd.GrossSalary,
        0 AS EmployerSocialCharges,
        ISNULL(SUM(CASE WHEN dd.DeductionType = 'voluntary' THEN dd.AmountDeduced ELSE 0 END), 0) AS VoluntaryDeductions,
        (pd.GrossSalary + 0) AS EmployerCost
    FROM Companies c
    INNER JOIN Payrolls pay ON pay.CompanyId = c.Id
    LEFT JOIN PaymentDetails pd ON pd.PayrollId = pay.Id
    LEFT JOIN Employees e ON e.Id = pd.EmployeeId
    LEFT JOIN Persons p_employee ON p_employee.Id = e.Id
    LEFT JOIN NaturalPersons np_employee ON np_employee.Id = e.Id
    INNER JOIN NaturalPersons np_employer ON np_employer.Id = @EmployerId
    LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd.Id
    WHERE c.Id = @CompanyId
    GROUP BY
        c.Name,
        np_employer.FirstSurname, np_employer.SecondSurname, np_employer.FirstName,
        np_employee.FirstSurname, np_employee.SecondSurname, np_employee.FirstName,
        p_employee.LegalId,
        e.ContractType,
        pay.StartDate, pay.EndDate,
        pd.IssueDate,
        pd.GrossSalary
    ORDER BY
        pay.EndDate DESC,
        pay.StartDate DESC
END