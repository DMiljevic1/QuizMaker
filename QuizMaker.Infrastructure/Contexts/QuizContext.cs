using Microsoft.EntityFrameworkCore;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Interfaces;
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
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(qc => qc.Question)
                .WithMany()
                .HasForeignKey(qc => qc.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

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

            var baseType = clrType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(AuditBase<>))
                {
                    var keyType = baseType.GetGenericArguments()[0];
                    var method = typeof(QuizContext)
                        .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.Static | BindingFlags.NonPublic);
                    var genericMethod = method!.MakeGenericMethod(clrType, keyType);
                    genericMethod.Invoke(null, new object[] { modelBuilder });
                    break;
                }
                baseType = baseType.BaseType;
            }
        }
    }

    private static void ApplySoftDeleteFilter<TEntity, TKey>(ModelBuilder modelBuilder) where TEntity : AuditBase<TKey>
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
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

            if (entry.Entity is AuditBase<Guid> entity)
            {
                entity.IsDeleted = true;
                entity.DateDeleted = utcNow;
                entry.State = EntityState.Modified;
            }
        }

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.Entity is not AuditBase<Guid> entity)
                continue;

            if (entry.State == EntityState.Added)
                entity.DateCreated = utcNow;
            else
                entity.DateUpdated = utcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
