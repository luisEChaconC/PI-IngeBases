USE PayrollSystem

-- Insert Employee Types if they don't exist
IF NOT EXISTS (SELECT 1 FROM EmployeeTypes WHERE Name = 'Full-Time')
    INSERT INTO EmployeeTypes (Name) VALUES ('Full-Time');

IF NOT EXISTS (SELECT 1 FROM EmployeeTypes WHERE Name = 'Part-Time')
    INSERT INTO EmployeeTypes (Name) VALUES ('Part-Time');

IF NOT EXISTS (SELECT 1 FROM EmployeeTypes WHERE Name = 'Professional Services')
    INSERT INTO EmployeeTypes (Name) VALUES ('Professional Services');

GO

-- Get Employee Type IDs
DECLARE @FullTimeId UNIQUEIDENTIFIER = (SELECT Id FROM EmployeeTypes WHERE Name = 'Full-Time');
DECLARE @PartTimeId UNIQUEIDENTIFIER = (SELECT Id FROM EmployeeTypes WHERE Name = 'Part-Time');
DECLARE @ProfessionalServicesId UNIQUEIDENTIFIER = (SELECT Id FROM EmployeeTypes WHERE Name = 'Professional Services');

-- Get Company IDs for reference
DECLARE @CompanyId1 UNIQUEIDENTIFIER
DECLARE @CompanyId2 UNIQUEIDENTIFIER
DECLARE @CompanyId3 UNIQUEIDENTIFIER

SELECT @CompanyId1 = Id FROM Companies WHERE Name = 'TecnoSoluciones CR'
SELECT @CompanyId2 = Id FROM Companies WHERE Name = 'Café Dorado S.A.'
SELECT @CompanyId3 = Id FROM Companies WHERE Name = 'EcoTurismo Guanacaste'

-- Get Benefit IDs from the benefits inserted in 002-script-insert-dummy-benefits.sql
-- Company 1 Benefits
DECLARE @SeguroMedicoId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Seguro Médico' AND CompanyId = @CompanyId1);
DECLARE @FondoAhorroId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Fondo de Ahorro' AND CompanyId = @CompanyId1);
DECLARE @ServicioTransporteId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Servicio de Transporte' AND CompanyId = @CompanyId1);

-- Company 2 Benefits
DECLARE @APIBonosId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'API de Bonos' AND CompanyId = @CompanyId2);
DECLARE @SubsidioAlimenticioId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Subsidio Alimenticio' AND CompanyId = @CompanyId2);
DECLARE @SeguroVidaId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Seguro de Vida' AND CompanyId = @CompanyId2);

-- Company 3 Benefits
DECLARE @APICapacitacionId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'API de Capacitación' AND CompanyId = @CompanyId3);
DECLARE @BonoAnualId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Bono Anual' AND CompanyId = @CompanyId3);
DECLARE @SubsidioEducacionId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'Subsidio para Educación' AND CompanyId = @CompanyId3);
DECLARE @APIEvaluacionId UNIQUEIDENTIFIER = (SELECT Id FROM Benefits WHERE Name = 'API de Evaluación de Desempeño' AND CompanyId = @CompanyId3);

-- Insert relationships between Employee Types and Benefits
-- Each employee type will have access to all benefits

-- Full-Time employees get all benefits
INSERT INTO EmployeeTypesXBenefits (EmployeeTypeId, BenefitId)
VALUES 
(@FullTimeId, @SeguroMedicoId),
(@FullTimeId, @FondoAhorroId),
(@FullTimeId, @ServicioTransporteId),
(@FullTimeId, @APIBonosId),
(@FullTimeId, @SubsidioAlimenticioId),
(@FullTimeId, @SeguroVidaId),
(@FullTimeId, @APICapacitacionId),
(@FullTimeId, @BonoAnualId),
(@FullTimeId, @SubsidioEducacionId),
(@FullTimeId, @APIEvaluacionId);

-- Part-Time employees get selected benefits (excluding some high-value ones)
INSERT INTO EmployeeTypesXBenefits (EmployeeTypeId, BenefitId)
VALUES 
(@PartTimeId, @SeguroMedicoId),
(@PartTimeId, @ServicioTransporteId),
(@PartTimeId, @SubsidioAlimenticioId),
(@PartTimeId, @APICapacitacionId),
(@PartTimeId, @APIEvaluacionId);

-- Professional Services employees get specific benefits
INSERT INTO EmployeeTypesXBenefits (EmployeeTypeId, BenefitId)
VALUES 
(@ProfessionalServicesId, @APIBonosId),
(@ProfessionalServicesId, @APICapacitacionId),
(@ProfessionalServicesId, @BonoAnualId),
(@ProfessionalServicesId, @APIEvaluacionId);

-- Verify the insertions
SELECT 
    et.Name as EmployeeType,
    b.Name as BenefitName,
    c.Name as CompanyName
FROM EmployeeTypesXBenefits etxb
INNER JOIN EmployeeTypes et ON etxb.EmployeeTypeId = et.Id
INNER JOIN Benefits b ON etxb.BenefitId = b.Id
INNER JOIN Companies c ON b.CompanyId = c.Id
ORDER BY et.Name, c.Name, b.Name;