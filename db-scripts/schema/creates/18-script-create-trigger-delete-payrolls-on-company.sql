CREATE TRIGGER trg_DeletePayrollsOnCompanyDelete
ON Companies
AFTER DELETE
AS
BEGIN
    DELETE FROM Payrolls
    WHERE CompanyId IN (SELECT Id FROM deleted);
END;