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
        ISNULL(SUM(pd.GrossSalary), 0) AS TotalGrossSalary,
        ISNULL(SUM(dd.AmountDeduced), 0) AS TotalAmountDeducted
    FROM Payrolls p
    INNER JOIN PayrollManagers pm ON p.PayrollManagerId = pm.Id
    INNER JOIN Employees e_pm ON pm.Id = e_pm.Id
    INNER JOIN NaturalPersons np ON e_pm.Id = np.Id
    LEFT JOIN PaymentDetails pd ON pd.PayrollId = p.Id
    LEFT JOIN DeductionDetails dd ON dd.PaymentDetailsId = pd.Id
    WHERE p.CompanyId = @CompanyId
    GROUP BY
        p.Id, p.StartDate, p.EndDate,
        np.FirstName, np.FirstSurname, np.SecondSurname
    ORDER BY p.EndDate DESC
END
