CREATE PROCEDURE ValidateDuplicateEmail
    @Email NVARCHAR(255),
    @PersonId UNIQUEIDENTIFIER
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Contacts WHERE Email = @Email AND PersonId <> @PersonId)
        THROW 50001, 'Correo electr�nico ya existe.', 1;
END
