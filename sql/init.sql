
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'bdPatient')
BEGIN
    CREATE DATABASE bdPatient;
END
GO

USE bdPatient;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients (
        PatientId INT PRIMARY KEY IDENTITY(1,1),
        DocumentType NVARCHAR(50),
        DocumentNumber NVARCHAR(50),
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        BirthDate DATE,
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(100),
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPatientsCreatedAfter')
BEGIN
    EXEC('
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
    ');
END
GO
