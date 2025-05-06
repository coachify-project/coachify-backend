using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachApplication> CoachApplications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackStatus> FeedbackStatuses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonStatus> LessonStatuses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleStatus> ModuleStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestSubmission> TestSubmissions { get; set; }
        public DbSet<TestSubmissionAnswer> TestSubmissionAnswers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCoachApplicationStatus> UserCoachApplicationStatuses { get; set; }

        // Конструктор с DbContextOptions для корректной работы с DI
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Убираем конфигурацию соединения из OnConfiguring, так как это теперь выполняется через DI
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //     => optionsBuilder.UseSqlite("Data Source=coachify.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Coach>()
                .HasOne(c => c.User)
                .WithOne(u => u.CoachProfile)
                .HasForeignKey<Coach>(c => c.CoachId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Coach)
                .WithMany(co => co.Courses)
                .HasForeignKey(c => c.CoachId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId);
            
            modelBuilder.Entity<Test>()
                .HasOne(t => t.Module)
                .WithOne(m => m.Test)
                .HasForeignKey<Test>(t => t.ModuleId); 

            // Relationships between Enrollment and User, Course, Payment, Certificate
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Payment)
                .WithOne(p => p.Enrollment)
                .HasForeignKey<Payment>(p => p.EnrollId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Certificate)
                .WithOne(c => c.Enrollment)
                .HasForeignKey<Certificate>(c => c.EnrollmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationships between Feedback, Course, and Client
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Course)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CourseId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.ClientId);

            // Relationships between TestSubmission, Test, and User (Client)
            modelBuilder.Entity<TestSubmission>()
                .HasOne(ts => ts.Test)
                .WithMany(t => t.Submissions)
                .HasForeignKey(ts => ts.TestId);

            modelBuilder.Entity<TestSubmission>()
                .HasOne(ts => ts.Client)
                .WithMany(u => u.TestSubmissions)
                .HasForeignKey(ts => ts.UserId);

            // Relationships between TestSubmissionAnswer, TestSubmission, Option, and Question
            modelBuilder.Entity<TestSubmissionAnswer>()
                .HasOne(tsa => tsa.Submission)
                .WithMany(ts => ts.Answers)
                .HasForeignKey(tsa => tsa.SubmissionId);

            modelBuilder.Entity<TestSubmissionAnswer>()
                .HasOne(tsa => tsa.Option)
                .WithMany(o => o.TestSubmissionAnswers)
                .HasForeignKey(tsa => tsa.OptionId);

            modelBuilder.Entity<TestSubmissionAnswer>()
                .HasOne(tsa => tsa.Question)
                .WithMany(q => q.TestSubmissionAnswers)
                .HasForeignKey(tsa => tsa.QuestionId);

            // Cascade delete configuration for related entities
            modelBuilder.Entity<CourseStatus>()
                .HasMany(cs => cs.Courses)
                .WithOne(c => c.Status)
                .HasForeignKey(c => c.StatusId);

            modelBuilder.Entity<LessonStatus>()
                .HasMany(ls => ls.Lessons)
                .WithOne(l => l.Status)
                .HasForeignKey(l => l.StatusId);

            modelBuilder.Entity<ModuleStatus>()
                .HasMany(ms => ms.Modules)
                .WithOne(m => m.Status)
                .HasForeignKey(m => m.StatusId);

            modelBuilder.Entity<PaymentStatus>()
                .HasMany(ps => ps.Payments)
                .WithOne(p => p.Status)
                .HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<FeedbackStatus>()
                .HasMany(fs => fs.Feedbacks)
                .WithOne(f => f.Status)
                .HasForeignKey(f => f.StatusId);
        }
    }
}
