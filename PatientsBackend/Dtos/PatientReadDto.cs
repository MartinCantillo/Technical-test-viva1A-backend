using System;

public record PatientReadDto
(
int PatientId,
string DocumentType,
string DocumentNumber,
string FirstName,
string LastName,
DateTime BirthDate,
string? PhoneNumber,
string? Email,
DateTime CreatedAt
);