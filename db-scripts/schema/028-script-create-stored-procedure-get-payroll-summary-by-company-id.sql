CREATE PROCEDURE sp_GetPayrollSummaryByCompanyId
    @CompanyId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id AS PayrollId,
        p.StartDate,
        p.EndDate,
        CONCAT(np.FirstName, ' ', np.FirstSurname, ' ', np.SecondSurname) AS PayrollManagerFullName,
        ISNULL(pd.TotalGrossSalary, 0) AS TotalGrossSalary,
        ISNULL(SUM(dd.AmountDeduced), 0) AS TotalAmountDeducted
    FROM Payrolls p
    INNER JOIN PayrollManagers pm ON p.PayrollManagerId = pm.Id
    INNER JOIN Employees e_pm ON pm.Id = e_pm.Id
    INNER JOIN NaturalPersons np ON e_pm.Id = np.Id
    LEFT JOIN (
        SELECT PayrollId, SUM(GrossSalary) AS TotalGrossSalary
        FROM PaymentDetails
        GROUP BY PayrollId
    ) pd ON pd.PayrollId = p.Id
    LEFT JOIN PaymentDetails pd2 ON pd2.PayrollId = p.Id
    LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd2.Id
    WHERE p.CompanyId = @CompanyId
    GROUP BY
        p.Id, p.StartDate, p.EndDate,
        np.FirstName, np.FirstSurname, np.SecondSurname,
        pd.TotalGrossSalary
    ORDER BY p.EndDate DESC
END