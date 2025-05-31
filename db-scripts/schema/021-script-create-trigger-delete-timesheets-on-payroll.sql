CREATE TRIGGER trg_DeleteTimesheetsOnPayrollDelete
ON Payrolls
AFTER DELETE
AS
BEGIN
    DELETE FROM Timesheets
    WHERE PayrollId IN (SELECT Id FROM deleted);
END;