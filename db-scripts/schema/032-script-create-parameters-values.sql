USE PayrollSystem;

CREATE TABLE ParametersValues (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	ParameterId UNIQUEIDENTIFIER NOT NULL,
	EmployeeId UNIQUEIDENTIFIER NOT NULL,
    ValueType VARCHAR(7) 
       NOT NULL 
       CHECK (ValueType IN ('String', 'Date', 'Int')),
	StringValue VARCHAR(30),
	IntValue INT,
	DateValue DATE,
	FOREIGN KEY (ParameterId) REFERENCES APIParameters(Id),
	FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

