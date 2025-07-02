GO
CREATE PROCEDURE sp_DeleteEmployee
    @EmployeeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    UPDATE Employees
    SET IsDeleted = 1,
        EndDate = GETDATE()
    WHERE Id = @EmployeeId;

    COMMIT TRANSACTION;
END
