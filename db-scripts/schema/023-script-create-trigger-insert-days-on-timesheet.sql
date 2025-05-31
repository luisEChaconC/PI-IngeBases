CREATE TRIGGER trg_InsertDaysOnTimesheetInsert
ON Timesheets
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TimesheetId UNIQUEIDENTIFIER
    DECLARE @StartDate DATE
    DECLARE @EndDate DATE
    DECLARE @CurrentDate DATE

    -- Cursor to handle multiple inserted timesheets
    DECLARE timesheet_cursor CURSOR FOR
    SELECT Id, StartDate, EndDate
    FROM inserted

    OPEN timesheet_cursor
    FETCH NEXT FROM timesheet_cursor INTO @TimesheetId, @StartDate, @EndDate

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @CurrentDate = @StartDate

        -- Create a day record for each date in the timesheet period
        WHILE @CurrentDate <= @EndDate
        BEGIN
            INSERT INTO Days (Date, TimesheetId, IsApproved)
            VALUES (@CurrentDate, @TimesheetId, 0)

            SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate)
        END

        FETCH NEXT FROM timesheet_cursor INTO @TimesheetId, @StartDate, @EndDate
    END

    CLOSE timesheet_cursor
    DEALLOCATE timesheet_cursor
END