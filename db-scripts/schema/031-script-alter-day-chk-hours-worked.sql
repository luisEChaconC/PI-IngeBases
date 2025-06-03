ALTER TABLE Days
ADD CONSTRAINT CHK_Days_HoursWorked CHECK (HoursWorked <= 24);
