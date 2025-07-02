CREATE PROCEDURE sp_DeleteBenefit
    @BenefitId UNIQUEIDENTIFIER,
    @ResultCode NVARCHAR(32) OUTPUT,
    @ResultMessage NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @ResultCode = '';
        SET @ResultMessage = '';

        IF EXISTS (SELECT 1 FROM DeductionDetails WHERE BenefitId = @BenefitId)
        BEGIN
            DELETE FROM EmployeeTypesXBenefits WHERE BenefitId = @BenefitId;
            UPDATE Benefits SET IsDeleted = 1 WHERE Id = @BenefitId;
            SET @ResultCode = 'MarkedAsDeleted';
            SET @ResultMessage = 'The benefit has been marked as deleted. Its visibility will remain active until the end of the month for historical purposes.';
        END
        ELSE
        BEGIN
            IF EXISTS (SELECT 1 FROM EmployeeXBenefit WHERE BenefitId = @BenefitId)
            BEGIN
                DELETE FROM EmployeeXBenefit WHERE BenefitId = @BenefitId;
                DELETE FROM EmployeeTypesXBenefits WHERE BenefitId = @BenefitId;
                DELETE FROM Benefits WHERE Id = @BenefitId;
                SET @ResultCode = 'DeletedWithAssignments';
                SET @ResultMessage = 'Benefit deleted. Active assignments have been removed.';
            END
            ELSE
            BEGIN
                DELETE FROM EmployeeTypesXBenefits WHERE BenefitId = @BenefitId;
                DELETE FROM Benefits WHERE Id = @BenefitId;
                SET @ResultCode = 'Deleted';
                SET @ResultMessage = 'Benefit deleted.';
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @ResultCode = 'Error';
        SET @ResultMessage = ERROR_MESSAGE();
    END CATCH
END