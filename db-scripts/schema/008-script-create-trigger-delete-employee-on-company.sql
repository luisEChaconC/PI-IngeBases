CREATE TRIGGER trg_DeleteEmployeesOnCompanyDelete
ON Companies
AFTER DELETE
AS
BEGIN
    DELETE FROM Employees
    WHERE CompanyId IN (SELECT Id FROM deleted);
END;