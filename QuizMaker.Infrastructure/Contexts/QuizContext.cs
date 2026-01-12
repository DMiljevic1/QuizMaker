using Microsoft.EntityFrameworkCore;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace QuizMaker.Infrastructure.Contexts;

public class QuizContext : DbContext
{
    public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(q => q.Id);

            entity.Property(q => q.Name)
                  .HasMaxLength(100)
                  .IsRequired();
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(q => q.Id);

            entity.HasOne(qc => qc.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qc => qc.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(qc => qc.Question)
                .WithMany()
                .HasForeignKey(qc => qc.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(qc => new { qc.QuizId, qc.QuestionId})
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(q => q.Id);

            entity.Property(q => q.Text)
                  .IsRequired()
                  .HasMaxLength(1000);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Text)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.HasOne(a => a.Question)
                  .WithOne(q => q.Answer)
                  .HasForeignKey<Answer>(a => a.QuestionId);

            entity.HasIndex(a => a.QuestionId)
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(IAuditable).IsAssignableFrom(clrType))
            {
                modelBuilder.Entity(clrType)
                    .HasQueryFilter(CreateIsNotDeletedFilter(clrType));
            }
        }
    }
    private static LambdaExpression CreateIsNotDeletedFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(IAuditable.IsDeleted));
        var condition = Expression.Equal(property, Expression.Constant(false));

        return Expression.Lambda(condition, parameter);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Deleted))
        {
            if (entry.Entity is ISoftDeleteAggregate aggregate)
            {
                aggregate.SoftDelete();
                entry.State = EntityState.Modified;
                continue;
            }

            if (entry.Entity is IAuditable auditable)
            {
                auditable.IsDeleted = true;
                auditable.DateDeleted = utcNow;
                entry.State = EntityState.Modified;
            }
        }

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.Entity is not IAuditable auditable)
                continue;

            if (entry.State == EntityState.Added)
                auditable.DateCreated = utcNow;
            else
                auditable.DateUpdated = utcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
