USE PayrollSystem

DECLARE @LifeInsuranceAPIId UNIQUEIDENTIFIER;
DECLARE @AssociationAPIId UNIQUEIDENTIFIER;
DECLARE @MedicareAPIId UNIQUEIDENTIFIER;

SELECT @LifeInsuranceAPIId = Id FROM APIs WHERE Name = 'Life Insurance';
SELECT @AssociationAPIId = Id FROM APIs WHERE Name = 'Association';
SELECT @MedicareAPIId = Id FROM APIs WHERE Name = 'Medicare';

-- Life Insurance Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', @LifeInsuranceAPIId);

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', @LifeInsuranceAPIId);


-- Association Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Association name', 'UserDefined', @AssociationAPIId);

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Salary', 'SystemDefined', @AssociationAPIId);


-- Medicare Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', @MedicareAPIId);

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', @MedicareAPIId);

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Dependents', 'UserDefined', @MedicareAPIId);



