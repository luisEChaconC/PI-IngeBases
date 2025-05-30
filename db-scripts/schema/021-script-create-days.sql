CREATE TABLE Days (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Date DATE NOT NULL,
    HoursWorked INT,
    WorkDescription VARCHAR(300),
    IsApproved BIT NOT NULL DEFAULT 0,
    TimesheetId UNIQUEIDENTIFIER NOT NULL,
    SupervisorId UNIQUEIDENTIFIER,	
    FOREIGN KEY (TimesheetId) REFERENCES Timesheets(Id),
    FOREIGN KEY (SupervisorId) REFERENCES Supervisors(Id)
)