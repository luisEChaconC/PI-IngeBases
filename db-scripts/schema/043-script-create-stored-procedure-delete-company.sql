CREATE PROCEDURE sp_DeleteCompany
    @CompanyId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    -- Verificar si la empresa tiene payrolls
    IF EXISTS (SELECT 1 FROM Payrolls WHERE CompanyId = @CompanyId)
    BEGIN
        -- Si los tiene, se hace borrado lógico
        UPDATE Companies SET IsDeleted = 1 WHERE Id = @CompanyId;
    END
    ELSE
    BEGIN
        -- Sino, borrado físico
		DELETE FROM Employees WHERE CompanyId = @CompanyId;
		DELETE FROM Employers WHERE CompanyId = @CompanyId;
        DELETE FROM Companies WHERE Id = @CompanyId;
    END

    COMMIT TRANSACTION;
END
