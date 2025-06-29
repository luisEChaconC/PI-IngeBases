CREATE TRIGGER trg_AutoSubmitDays
ON Days
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE d
    SET 
        IsSubmitted = 1,
        LastSubmitTimestamp = GETUTCDATE()
    FROM Days d
    INNER JOIN inserted i ON d.Id = i.Id
    INNER JOIN deleted del ON d.Id = del.Id
    WHERE 
        d.IsApproved = 0
        AND (
            (ISNULL(i.HoursWorked, -1) != ISNULL(del.HoursWorked, -1))
            OR
            (ISNULL(i.WorkDescription, '') != ISNULL(del.WorkDescription, ''))
        )
        AND (
            i.HoursWorked IS NOT NULL 
            AND i.WorkDescription IS NOT NULL
        );
END; 