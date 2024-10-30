using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer
{
    public class StudentDbContext : DbContext
    {
         public DbSet<Student> Students { get; set; }
         public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }

    }
}
