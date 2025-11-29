using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models;


public interface IPatientRepository
{
Task<Patient?> GetByIdAsync(int id);
Task<(IEnumerable<Patient> Items, int Total)> GetPagedAsync(int page, int pageSize, string? nameFilter, string? documentNumber);
Task AddAsync(Patient patient);
Task UpdateAsync(Patient patient);
Task DeleteAsync(Patient patient);
Task<bool> ExistsDuplicateAsync(string documentType, string documentNumber, int? excludingId = null);
Task<IEnumerable<Patient>> GetCreatedAfterAsync(DateTime since);
}