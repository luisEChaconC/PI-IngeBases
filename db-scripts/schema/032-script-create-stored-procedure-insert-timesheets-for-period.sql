CREATE PROCEDURE dbo.InsertTimesheetsForPeriod
(@PeriodStartDate DATE,
@PeriodEndDate DATE,
@EmployeeId UNIQUEIDENTIFIER,
@PayrollId UNIQUEIDENTIFIER)
AS
BEGIN
	DECLARE @TimesheetStartDate DATE
	DECLARE @TimesheetEndDate DATE
	DECLARE @DaysLeftForSunday INT
	DECLARE @NextSunday DATE
	DECLARE @CurrentDate DATE

	SET @CurrentDate = @PeriodStartDate

	WHILE @CurrentDate <= @PeriodEndDate
	BEGIN
		SET @TimesheetStartDate = @CurrentDate
		SET @DaysLeftForSunday = 6 - dbo.DayNumber(@TimesheetStartDate)
		SET @NextSunday = DATEADD(Day, @DaysLeftForSunday, @TimesheetStartDate)

		IF @NextSunday > @PeriodEndDate
			SET @TimesheetEndDate = @PeriodEndDate
		ELSE
			SET @TimesheetEndDate = @NextSunday

		INSERT INTO Timesheets (StartDate, EndDate, EmployeeId, PayrollId)
		VALUES (@TimesheetStartDate, @TimesheetEndDate, @EmployeeId, @PayrollId)

		SET @CurrentDate = dbo.NextDay(@TimesheetEndDate)
	END
END