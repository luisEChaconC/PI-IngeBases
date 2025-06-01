CREATE TRIGGER trg_DeleteDaysOnTimesheetDelete
ON Timesheets
AFTER DELETE
AS
BEGIN
    DELETE FROM Days
    WHERE Id IN (SELECT Id FROM deleted)
END;