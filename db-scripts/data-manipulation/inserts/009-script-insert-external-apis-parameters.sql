USE PayrollSystem

-- Life Insurance Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', '578B7C9B-6D7F-43AC-9955-006D6A0D110F')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', '578B7C9B-6D7F-43AC-9955-006D6A0D110F')


-- Association Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Association name', 'UserDefined', '131C6C5E-DB1A-4DAE-8293-84D3C0DB652B')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Salary', 'SystemDefined', '131C6C5E-DB1A-4DAE-8293-84D3C0DB652B')


-- Medicare Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', '41978094-1A94-4F6B-9CE2-43582555DE60')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', '41978094-1A94-4F6B-9CE2-43582555DE60')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Dependents', 'UserDefined', '41978094-1A94-4F6B-9CE2-43582555DE60')


