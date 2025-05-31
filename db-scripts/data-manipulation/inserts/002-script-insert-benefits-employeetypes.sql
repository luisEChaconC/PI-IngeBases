USE PayrollSystem

-- Tipos de empleados
INSERT INTO EmployeeTypes (Id, Name) VALUES
(NEWID(), 'FullTime'),
(NEWID(), 'PartTime'),
(NEWID(), 'ByContract');

-- Beneficios
INSERT INTO Benefits (Id, Name, Description, IsActive, Type, LinkAPI, FixedPercentage, FixedAmount, RequiredMonthsWorked)
VALUES
(NEWID(), 'Health Insurance', 'Full medical coverage', 1, 'FixedAmount', NULL, NULL, 25000, 6),
(NEWID(), 'Gym Membership', 'Monthly gym access', 1, 'FixedPercentage', NULL, 50, NULL, 3),
(NEWID(), 'Learning Platform', 'Access to online courses', 1, 'API', 'https://learning.api', NULL, NULL, 0);


-- Supongamos que estos son los IDs correctos
DECLARE @FullTimeId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM EmployeeTypes WHERE Name = 'FullTime');
DECLARE @PartTimeId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM EmployeeTypes WHERE Name = 'PartTime');
DECLARE @ByContractId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM EmployeeTypes WHERE Name = 'ByContract');

DECLARE @HealthBenefitId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Benefits WHERE Name = 'Health Insurance');
DECLARE @GymBenefitId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Benefits WHERE Name = 'Gym Membership');
DECLARE @LearningBenefitId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Benefits WHERE Name = 'Learning Platform');

-- Inserts en tabla de relaci�n
INSERT INTO EmployeeTypesXBenefits (EmployeeTypeId, BenefitId)
VALUES 
(@FullTimeId, @HealthBenefitId),
(@PartTimeId, @GymBenefitId),
(@FullTimeId, @GymBenefitId),
(@ByContractId, @LearningBenefitId),
(@FullTimeId, @LearningBenefitId),
(@PartTimeId, @LearningBenefitId);
