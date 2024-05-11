using blogbackend.DataContext;
using blogbackend.Model;

namespace blogbackend.Repository
{
    public class PatientRepository
    {
        private readonly ApplicationDataContext _context;

        public PatientRepository(ApplicationDataContext context)
        {
            _context = context;
        }

        public void Create(Patient patient)
        {
            _context.Patient.Add(patient);
            _context.SaveChanges();
        }

        // Read all products
        public IEnumerable<Patient> GetAll()
        {
            return _context.Patient.ToList();
        }

        // Read a product by Id
        public Patient GetById(int id)
        {
            var patient = _context.Patient.Find(id);
     
            if (patient != null)
            {
                return patient;
            }
            else
            {
                throw new InvalidOperationException($"Product with id {id} not found.");
            }
        }

        // Update a product
        public void Update(Patient product)
        {
            _context.Patient.Update(product);
            _context.SaveChanges();
        }

        // Delete a product
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _context.Patient.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Patient> GetPaged(int pageNumber, int pageSize, string? filterFirst = null, string? filterLast = null, bool? filterActive = null, string? filterCity = null)
        {
            var query = _context.Patient.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filterFirst))
            {
                query = query.Where(p => p.FirstName != null && p.FirstName.Contains(filterFirst));

            }

            if (!string.IsNullOrEmpty(filterLast))
            {
                query = query.Where(p => p.LastName != null && p.LastName.Contains(filterLast));

            }

            if (filterActive.HasValue) // Check if the nullable bool has a value
            {
                bool activeValue = filterActive.Value; // Get the boolean value from the nullable bool
                query = query.Where(p => p.Status == activeValue); // Compare with the boolean value
            }

            if (!string.IsNullOrEmpty(filterCity))
            {
                query = query.Where(p => p.City != null && p.City.Contains(filterCity));
            }

            // Apply pagination
            var patients = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return patients;
        }


    }
}
