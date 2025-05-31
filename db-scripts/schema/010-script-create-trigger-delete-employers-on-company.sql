CREATE TRIGGER trg_DeleteEmployersOnCompanyDelete
ON Companies
AFTER DELETE
AS
BEGIN
    DELETE FROM Employers
    WHERE CompanyId IN (SELECT Id FROM deleted);
END;