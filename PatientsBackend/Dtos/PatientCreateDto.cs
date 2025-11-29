using System;
using System.ComponentModel.DataAnnotations;


public record PatientCreateDto
(
[Required, MaxLength(10)] string DocumentType,
[Required, MaxLength(20)] string DocumentNumber,
[Required, MaxLength(80)] string FirstName,
[Required, MaxLength(80)] string LastName,
DateTime BirthDate,
string? PhoneNumber,
[EmailAddress] string? Email
);