CREATE FUNCTION dbo.fn_CheckPayrollExists
(
    @CompanyId UNIQUEIDENTIFIER,
    @StartDate DATE,
    @EndDate DATE
)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT = 0;

    IF EXISTS (
        SELECT 1
        FROM Payrolls
        WHERE CompanyId = @CompanyId
          AND (
                (StartDate <= @EndDate AND EndDate >= @StartDate)
              )
    )
    BEGIN
        SET @Exists = 1;
    END

    RETURN @Exists;
END