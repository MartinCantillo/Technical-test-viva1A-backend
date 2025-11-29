using System;
using System.ComponentModel.DataAnnotations;


public record PatientUpdateDto
(
string? DocumentType,
string? DocumentNumber,
string? FirstName,
string? LastName,
DateTime? BirthDate,
string? PhoneNumber,
[EmailAddress] string? Email
);