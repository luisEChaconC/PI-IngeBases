GO
CREATE PROCEDURE sp_DeleteEmployee
    @EmployeeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM PaymentDetails WHERE EmployeeId = @EmployeeId)
    BEGIN
        UPDATE Employees
        SET IsDeleted = 1,
            EndDate = GETDATE()
        WHERE Id = @EmployeeId;
    END
    ELSE
    BEGIN
        DELETE FROM Employees WHERE Id = @EmployeeId;
    END

    COMMIT TRANSACTION;
END
