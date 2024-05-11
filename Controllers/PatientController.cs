using blogbackend.Model;
using blogbackend.Repository;
using Microsoft.AspNetCore.Mvc;
namespace blogbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientRepository _repository;


        public PatientController(PatientRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetAllProducts()
        {
            var products = _repository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetProductById(int id)
        {
            var patient = _repository.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> CreateProduct(Patient patient)
        {
            _repository.Create(patient);
            return CreatedAtAction(nameof(GetProductById), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _repository.Update(patient);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var existingPatient = _repository.GetById(id);
            if (existingPatient == null)
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }

        [HttpGet("GetPaged")]
        public ActionResult<IEnumerable<Patient>> GetPaged(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string? filterFirst,
        [FromQuery] string? filterLast,
        [FromQuery] bool? filterActive,
        [FromQuery] string? filterCity,
        [FromQuery] string sortBy = "Id")
        {
            // Apply filters
            var patients = _repository.GetPaged(pageNumber, pageSize, filterFirst, filterLast, filterActive, filterCity);

            // Apply sorting
            switch (sortBy.ToLower())
            {
                case "first":
                    patients = patients.OrderBy(p => p.FirstName, StringComparer.OrdinalIgnoreCase);
                    break;
                case "last":
                    patients = patients.OrderBy(p => p.LastName, StringComparer.OrdinalIgnoreCase);
                    break;
                case "active":
                    patients = patients.OrderBy(p => p.Status);
                    break;
                case "city":
                    patients = patients.OrderBy(p => p.City, StringComparer.OrdinalIgnoreCase);
                    break;
                default:
                    // Default sorting
                    patients = patients.OrderBy(p => p.Id);
                    break;
            }

            return Ok(patients.ToList());
        }


    }
}
