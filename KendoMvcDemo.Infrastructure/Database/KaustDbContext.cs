using KendoMvcDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace KendoMvcDemo.Infrastructure.Database
{
    public class KaustDbContext : DbContext
    {
        public KaustDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSchedule> CourseSchedules { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<CourseTeacher>()
                .HasKey(ct => new { ct.CourseId, ct.TeacherId });
        }
    }
}
