using blogbackend.Model;
using Microsoft.EntityFrameworkCore;

namespace blogbackend.DataContext
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }

        // Constructor
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            : base(options)
        {
        }
    }
}
