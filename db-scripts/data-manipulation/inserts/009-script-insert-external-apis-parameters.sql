USE PayrollSystem

-- Life Insurance Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', '36ACD88B-024E-4A82-8885-5ADED125563F')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', '36ACD88B-024E-4A82-8885-5ADED125563F')


-- Association Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Association name', 'UserDefined', '6E734A4B-DCBC-4761-BE0D-A18C07307DFF')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Salary', 'SystemDefined', '6E734A4B-DCBC-4761-BE0D-A18C07307DFF')


-- Medicare Parameters
INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Date of birth', 'UserDefined', 'B88DB333-0269-4981-AB11-00D0C6EDA18D')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Gender', 'SystemDefined', 'B88DB333-0269-4981-AB11-00D0C6EDA18D')

INSERT INTO APIParameters(Name, Type, APIId)
VALUES('Dependents', 'UserDefined', 'B88DB333-0269-4981-AB11-00D0C6EDA18D')



