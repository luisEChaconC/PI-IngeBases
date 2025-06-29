
USE PayrollSystem
GO

CREATE FUNCTION dbo.HasPaymentRecords
(
    @EmployeeId NVARCHAR(50)
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT;

    IF EXISTS (
        SELECT 1
        FROM PaymentDetails
        WHERE EmployeeId = @EmployeeId
    )
        SET @Result = 1;
    ELSE
        SET @Result = 0;

    RETURN @Result;
END;
