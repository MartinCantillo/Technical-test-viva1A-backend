CREATE PROCEDURE dbo.GetPatientsCreatedAfter
    @Since DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PatientId,
        DocumentType,
        DocumentNumber,
        FirstName,
        LastName,
        BirthDate,
        PhoneNumber,
        Email,
        CreatedAt
    FROM Patients
    WHERE CreatedAt > @Since
    ORDER BY CreatedAt DESC;
END
GO


EXEC dbo.GetPatientsCreatedAfter '2003-11-09';

select * from dbo.Patients;
