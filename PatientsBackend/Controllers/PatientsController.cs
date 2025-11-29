
using Microsoft.AspNetCore.Mvc;
using Data;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IPatientRepository _repo;

        public PatientsController(ApplicationDbContext db, IPatientRepository repo)
        {
            _db = db;
            _repo = repo;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _repo.ExistsDuplicateAsync(dto.DocumentType, dto.DocumentNumber);
            if (exists)
                return Conflict(new { message = "Paciente duplicado por DocumentType + DocumentNumber" });

            var patient = new Patient
            {
                DocumentType = dto.DocumentType,
                DocumentNumber = dto.DocumentNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _repo.AddAsync(patient);
            await _db.SaveChangesAsync();

            var read = new PatientReadDto(
                patient.PatientId,
                patient.DocumentType,
                patient.DocumentNumber,
                patient.FirstName,
                patient.LastName,
                patient.BirthDate,
                patient.PhoneNumber,
                patient.Email,
                patient.CreatedAt
            );

            return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, read);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] string? documentNumber = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var (items, total) = await _repo.GetPagedAsync(page, pageSize, name, documentNumber);

            var dtos = items.Select(p => new PatientReadDto(
                p.PatientId,
                p.DocumentType,
                p.DocumentNumber,
                p.FirstName,
                p.LastName,
                p.BirthDate,
                p.PhoneNumber,
                p.Email,
                p.CreatedAt
            ));

            return Ok(new
            {
                page,
                pageSize,
                total,
                items = dtos
            });
        }

 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null)
                return NotFound();

            return Ok(new PatientReadDto(
                p.PatientId,
                p.DocumentType,
                p.DocumentNumber,
                p.FirstName,
                p.LastName,
                p.BirthDate,
                p.PhoneNumber,
                p.Email,
                p.CreatedAt
            ));
        }

   
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = await _repo.GetByIdAsync(id);
            if (patient is null)
                return NotFound();

            if (dto.DocumentType is not null) patient.DocumentType = dto.DocumentType;
            if (dto.DocumentNumber is not null) patient.DocumentNumber = dto.DocumentNumber;
            if (dto.FirstName is not null) patient.FirstName = dto.FirstName;
            if (dto.LastName is not null) patient.LastName = dto.LastName;
            if (dto.BirthDate.HasValue) patient.BirthDate = dto.BirthDate.Value;
            if (dto.PhoneNumber is not null) patient.PhoneNumber = dto.PhoneNumber;
            if (dto.Email is not null) patient.Email = dto.Email;

           
            var dup = await _repo.ExistsDuplicateAsync(patient.DocumentType, patient.DocumentNumber, id);
            if (dup)
                return Conflict(new { message = "Otro paciente con el mismo DocumentType + DocumentNumber existe." });

            await _repo.UpdateAsync(patient);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _repo.GetByIdAsync(id);
            if (patient is null)
                return NotFound();

            await _repo.DeleteAsync(patient);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ---------------------------------------------
        // GET /api/patients/created-after?since=...
        // Usa Stored Procedure
        // ---------------------------------------------
        [HttpGet("created-after")]
        public async Task<IActionResult> GetCreatedAfter([FromQuery] DateTime since)
        {
            var items = await _repo.GetCreatedAfterAsync(since);

            var dtos = items.Select(p => new PatientReadDto(
                p.PatientId,
                p.DocumentType,
                p.DocumentNumber,
                p.FirstName,
                p.LastName,
                p.BirthDate,
                p.PhoneNumber,
                p.Email,
                p.CreatedAt
            ));

            return Ok(dtos);
        }
    }
}
