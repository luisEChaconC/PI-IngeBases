ALTER TABLE Employees
ADD Position VARCHAR(150)

UPDATE Employees
SET Position = 'Ingeniero de Software'
WHERE Position IS NULL

ALTER TABLE Employees
ALTER COLUMN Position VARCHAR(150) NOT NULL