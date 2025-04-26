using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.DAL
{
    public class ApplicationDbContext : DbContext
    {
       
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<FeedbackStatus> FeedbackStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CoachApplication> CoachApplications { get; set; }
        public DbSet<CoachProfile> CoachProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonMaterial> LessonMaterials { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<TestSubmission> TestSubmissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=coachify.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройки для сущностей

            // User - уникальный Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // CoachProfile - один к одному с User
            modelBuilder.Entity<CoachProfile>()
                .HasOne(cp => cp.Applicant)
                .WithOne(u => u.CoachProfile)
                .HasForeignKey<CoachProfile>(cp => cp.UserId);

            // Course - Coach
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Coach)
                .WithMany()
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // Module - Test (один к одному)
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Test)
                .WithOne(t => t.Module)
                .HasForeignKey<Test>(t => t.ModuleId);

            // Feedback - User (Client) связь
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment - User связь
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Certificate - User связь
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Student)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TestSubmission - User связь
            modelBuilder.Entity<TestSubmission>()
                .HasOne(ts => ts.Student)
                .WithMany(u => u.TestSubmissions)
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}