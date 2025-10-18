using Microsoft.EntityFrameworkCore;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Direction> Directions => Set<Direction>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Attempt> Attempts => Set<Attempt>();
    public DbSet<UserAttemptAnswer> UserAttemptAnswers => Set<UserAttemptAnswer>();
    public DbSet<UserSelectedOption> UserSelectedOptions => Set<UserSelectedOption>();
    public DbSet<UserTextAnswer> UserTextAnswers => Set<UserTextAnswer>();
    public DbSet<TestResult> TestResults => Set<TestResult>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Login).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Login).IsRequired();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.HasOne(x => x.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Phone).HasMaxLength(30).IsRequired(); ;
            e.Property(x => x.VkProfileLink).IsRequired();
        });

        modelBuilder.Entity<Direction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Group>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();

            e.HasOne(x => x.Direction)
                .WithMany(d => d.Groups)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Course)
                .WithMany(c => c.Groups)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Project)
                .WithMany(p => p.Groups)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);


        });

        modelBuilder.Entity<User>()
      .Property(u => u.Role)
      .HasConversion<string>();

        modelBuilder.Entity<Test>()
            .Property(t => t.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Question>()
            .Property(q => q.AnswerType)
            .HasConversion<string>();


             // значения по умолчанию
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Test>().Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Test>().Property(t => t.IsRepeatable).HasDefaultValue(false);
        modelBuilder.Entity<Test>().Property(t => t.IsPublic).HasDefaultValue(false);
        modelBuilder.Entity<Question>().Property(q => q.IsScoring).HasDefaultValue(true);
        modelBuilder.Entity<Attempt>().Property(a => a.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                // уникальные индексы
        modelBuilder.Entity<Question>().HasIndex(q => new { q.TestId, q.Number }).IsUnique();
        modelBuilder.Entity<UserAttemptAnswer>().HasIndex(uaa => new { uaa.AttemptId, uaa.QuestionId }).IsUnique();
        modelBuilder.Entity<TestResult>().HasIndex(tr => new { tr.TestId, tr.StudentId, tr.AttemptId }).IsUnique();


        // связи Many-to-Many 
        modelBuilder.Entity<Test>()
            .HasMany(t => t.Students)
            .WithMany(s => s.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "test_students",
                j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                j => j.HasOne<Test>().WithMany().HasForeignKey("TestId")
            );

        modelBuilder.Entity<Test>()
            .HasMany(t => t.Groups)
            .WithMany(g => g.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "test_groups",
                j => j.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                j => j.HasOne<Test>().WithMany().HasForeignKey("TestId")
            );

        modelBuilder.Entity<Test>()
            .HasMany(t => t.Courses)
            .WithMany(c => c.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "test_courses",
                j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                j => j.HasOne<Test>().WithMany().HasForeignKey("TestId")
            );

        modelBuilder.Entity<Test>()
            .HasMany(t => t.Projects)
            .WithMany(p => p.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "test_projects",
                j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                j => j.HasOne<Test>().WithMany().HasForeignKey("TestId")
            );

        modelBuilder.Entity<Test>()
            .HasMany(t => t.Directions)
            .WithMany(d => d.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "test_directions",
                j => j.HasOne<Direction>().WithMany().HasForeignKey("DirectionId"),
                j => j.HasOne<Test>().WithMany().HasForeignKey("TestId")
            );

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Groups)
            .WithMany(g => g.Students)
            .UsingEntity<Dictionary<string, object>>(
                "student_groups",
                j => j.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId")
            );
    }
}



