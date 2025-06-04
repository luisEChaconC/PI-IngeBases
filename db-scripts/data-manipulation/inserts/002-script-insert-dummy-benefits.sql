USE PayrollSystem

DECLARE @CompanyId1 UNIQUEIDENTIFIER
DECLARE @CompanyId2 UNIQUEIDENTIFIER
DECLARE @CompanyId3 UNIQUEIDENTIFIER

SELECT @CompanyId1 = Id FROM Companies WHERE Name = 'TecnoSoluciones CR'
SELECT @CompanyId2 = Id FROM Companies WHERE Name = 'Café Dorado S.A.'
SELECT @CompanyId3 = Id FROM Companies WHERE Name = 'EcoTurismo Guanacaste'

INSERT INTO Benefits (CompanyId, Name, Description, IsActive, Type, LinkAPI, FixedPercentage, FixedAmount, RequiredMonthsWorked)
VALUES 
    (@CompanyId1, 'Seguro Médico', 'Cobertura médica completa', 1, 'FixedAmount', NULL, NULL, 5000, 6),
    (@CompanyId1, 'Fondo de Ahorro', 'Ahorro mensual para empleados', 1, 'FixedPercentage', NULL, 10, NULL, 12),
    (@CompanyId1, 'Servicio de Transporte', 'Transporte desde casa al trabajo', 1, 'FixedAmount', NULL, NULL, 2000, 3),
    (@CompanyId2, 'API de Bonos', 'Conexión a sistema de bonos externo', 1, 'API', 'https://api.ejemplo.com/bonos', NULL, NULL, 0),
    (@CompanyId2, 'Subsidio Alimenticio', 'Vale de comida mensual', 1, 'FixedAmount', NULL, NULL, 1500, 1),
    (@CompanyId2, 'Seguro de Vida', 'Seguro para casos de accidente o fallecimiento', 1, 'FixedPercentage', NULL, 5, NULL, 6),
    (@CompanyId3, 'API de Capacitación', 'Sistema externo de formación continua', 1, 'API', 'https://api.ejemplo.com/cursos', NULL, NULL, 0),
    (@CompanyId3, 'Bono Anual', 'Bono al finalizar el año fiscal', 1, 'FixedPercentage', NULL, 15, NULL, 24),
    (@CompanyId3, 'Subsidio para Educación', 'Apoyo para estudios universitarios', 1, 'FixedAmount', NULL, NULL, 10000, 18),
    (@CompanyId3, 'API de Evaluación de Desempeño', 'Evaluación automática de desempeño', 1, 'API', 'https://api.ejemplo.com/evaluacion', NULL, NULL, 0);