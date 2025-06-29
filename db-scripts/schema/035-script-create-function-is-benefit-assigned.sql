CREATE FUNCTION dbo.IsBenefitAssigned
(
    @BenefitId UNIQUEIDENTIFIER
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsAssigned BIT;

    IF EXISTS (
        SELECT 1
        FROM EmployeeXBenefit
        WHERE BenefitId = @BenefitId
    )
        SET @IsAssigned = 1;
    ELSE
        SET @IsAssigned = 0;

    RETURN @IsAssigned;
END;
