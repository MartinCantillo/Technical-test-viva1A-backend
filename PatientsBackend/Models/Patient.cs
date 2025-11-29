using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiPacientes.Models
{
public class Patient
{
[Key]
public int PatientId { get; set; }


[Required, MaxLength(10)]
public string DocumentType { get; set; } = null!;


[Required, MaxLength(20)]
public string DocumentNumber { get; set; } = null!;


[Required, MaxLength(80)]
public string FirstName { get; set; } = null!;


[Required, MaxLength(80)]
public string LastName { get; set; } = null!;


[Column(TypeName = "date")]
public DateTime BirthDate { get; set; }


[MaxLength(20)]
public string? PhoneNumber { get; set; }


[MaxLength(120), EmailAddress]
public string? Email { get; set; }


public DateTime CreatedAt { get; set; }
}
}