using Microsoft.EntityFrameworkCore;
using Staff.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Staff.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ProgLang> ProgLangs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Gender> Genders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Experience>().HasKey(t => new { t.EmployeeId, t.ProgLangId });
            modelBuilder.Entity<Experience>()
                .HasOne(t => t.Employee).WithMany(e => e.Experiences)
                .HasForeignKey(t => t.EmployeeId);
            modelBuilder.Entity<Experience>()
                .HasOne(t => t.ProgLang).WithMany(e => e.Experiences)
                .HasForeignKey(t => t.ProgLangId);


            modelBuilder.Entity<Employee>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            modelBuilder.Entity<Experience>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser() { Id = 1, LastActionTime = DateTime.Now, UserName = "boss", Password = "pass" });

            modelBuilder.Entity<ProgLang>().HasData( new ProgLang[] {
                new ProgLang() { Id = 1, Name = "C#" },
                new ProgLang() { Id = 2, Name = "C/C++" },
                new ProgLang() { Id = 3, Name = "Fortran" },
                new ProgLang() { Id = 4, Name = "Pascal" },
                new ProgLang() { Id = 5, Name = "Python" },
            });

            modelBuilder.Entity<Department>().HasData(new Department[] {
                new Department() { Id = 1, Name = "Frontend", Floor = 1 },
                new Department() { Id = 2, Name = "Backend", Floor = 2 },
                new Department() { Id = 3, Name = "Desktop", Floor = 3 },
                new Department() { Id = 4, Name = "Embedded", Floor = 4 },
            });

            modelBuilder.Entity<Gender>().HasData(new Gender[] {
                new Gender() { Id = 1, Name = "Male"},
                new Gender() { Id = 2, Name = "Female"},
            });

        }

        public int SoftSaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public Task<int> SoftSaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }


    }
}
