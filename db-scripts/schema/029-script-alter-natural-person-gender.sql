USE PayrollSystem

ALTER TABLE NaturalPersons
ADD Gender CHAR(1)
    CONSTRAINT CHK_Employees_Gender CHECK (Gender IN ('F', 'M'));

GO

UPDATE NaturalPersons
SET Gender = 'M'
WHERE Gender IS NULL;

ALTER TABLE NaturalPersons
ALTER COLUMN Gender CHAR(1) NOT NULL;


