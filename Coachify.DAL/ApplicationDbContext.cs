using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.DAL;

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
    public DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }
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
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestSubmission> TestSubmissions { get; set; }
    public DbSet<TestSubmissionAnswer> TestSubmissionAnswers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCoachApplicationStatus> UserCoachApplicationStatuses { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=coachify.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique Constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Seed initial roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin" },
            new Role { RoleId = 2, RoleName = "Client" },
            new Role { RoleId = 3, RoleName = "Coach" }
        );

        modelBuilder.Entity<Coach>(b =>
        {
            b.HasKey(c => c.CoachId);

            b.HasOne(c => c.User)
                .WithOne(u => u.CoachProfile)
                .HasForeignKey<Coach>(c => c.CoachId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Property(c => c.CoachId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Coach)
            .WithMany(co => co.Courses)
            .HasForeignKey(c => c.CoachId);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId);
        
        modelBuilder.Entity<Module>()
            .HasMany(m => m.Skills)
            .WithMany(s => s.Modules)
            .UsingEntity(j => j.ToTable("ModuleSkill"));


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
        modelBuilder.Entity<CourseStatus>().HasData(
            new CourseStatus { StatusId = 1, Name = "Draft" },
            new CourseStatus { StatusId = 2, Name = "Pending" },
            new CourseStatus { StatusId = 3, Name = "Published" },
            new CourseStatus { StatusId = 4, Name = "Rejected" }
            );
        
        modelBuilder.Entity<EnrollmentStatus>().HasData(
            new EnrollmentStatus { StatusId = 1, Name = "Not Started" },
            new EnrollmentStatus { StatusId = 2, Name = "In Progress" },
            new EnrollmentStatus { StatusId = 3, Name = "Completed" }
        );


        modelBuilder.Entity<LessonStatus>()
            .HasMany(ls => ls.Lessons)
            .WithOne(l => l.Status)
            .HasForeignKey(l => l.StatusId);

        modelBuilder.Entity<ModuleStatus>().HasData(
            new ModuleStatus { StatusId = 1, StatusName = "Draft" },
            new ModuleStatus { StatusId = 2, StatusName = "Not Started" },
            new ModuleStatus { StatusId = 3, StatusName = "In progress" },
            new ModuleStatus { StatusId = 4, StatusName = "Completed" }
            );
        

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