using clinical.APIs.Data;
using clinical.APIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinical.APIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatient()
        {
            var patients = await _context.Patients.ToListAsync();
            if (patients==null||patients.Count==0)
            {
                return NotFound();

            }
            return Ok(patients);
        }

        [HttpGet("{Patient_ID}")]
        public async Task<IActionResult> GetPatientById(int Patient_ID)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Patient_ID == Patient_ID);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
    }
}
