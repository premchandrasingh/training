using Microsoft.EntityFrameworkCore;
using Training.Api.Entity;

namespace Training.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }


        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>(opt =>
            {
                opt.ToTable("employee");
                opt.HasKey(e => e.EmployeeId);

                opt.Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();

                opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                opt.Property(e => e.LastName).HasMaxLength(100);
                opt.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
                opt.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                opt.Property(e => e.CreatedDate).IsRequired();

            });
        }

    }
}
