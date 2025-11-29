

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class PatientRepository : IPatientRepository
{
private readonly ApplicationDbContext _db;
public PatientRepository(ApplicationDbContext db) => _db = db;


public async Task AddAsync(Patient patient)
{
await _db.Patients.AddAsync(patient);
}


public async Task DeleteAsync(Patient patient)
{
_db.Patients.Remove(patient);
await Task.CompletedTask;
}


public async Task<bool> ExistsDuplicateAsync(string documentType, string documentNumber, int? excludingId = null)
{
var q = _db.Patients.AsQueryable();
if (excludingId.HasValue) q = q.Where(p => p.PatientId != excludingId.Value);
return await q.AnyAsync(p => p.DocumentType == documentType && p.DocumentNumber == documentNumber);
}


public async Task<Patient?> GetByIdAsync(int id) => await _db.Patients.FindAsync(id);


public async Task<(IEnumerable<Patient> Items, int Total)> GetPagedAsync(int page, int pageSize, string? nameFilter, string? documentNumber)
{
var q = _db.Patients.AsQueryable();
if (!string.IsNullOrWhiteSpace(nameFilter))
{
q = q.Where(p => (p.FirstName + " " + p.LastName).Contains(nameFilter));
}
if (!string.IsNullOrWhiteSpace(documentNumber)) q = q.Where(p => p.DocumentNumber == documentNumber);


var total = await q.CountAsync();
var items = await q.OrderBy(p => p.PatientId)
.Skip((page - 1) * pageSize)
.Take(pageSize)
.ToListAsync();
return (items, total);
}


public async Task UpdateAsync(Patient patient)
{
_db.Patients.Update(patient);
await Task.CompletedTask;
}


public async Task<IEnumerable<Patient>> GetCreatedAfterAsync(DateTime since)
{
var param = new Microsoft.Data.SqlClient.SqlParameter("@Since", since);
return await _db.Patients.FromSqlRaw("EXEC dbo.GetPatientsCreatedAfter @Since", param).ToListAsync();
}
}