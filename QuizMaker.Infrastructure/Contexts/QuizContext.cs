using Microsoft.EntityFrameworkCore;
using QuizMaker.Domain.Entities;
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

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
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

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Answer)
            .WithOne(a => a.Question)
            .HasForeignKey<Answer>(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);


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
        foreach (var entry in ChangeTracker.Entries())
        {
            var entity = entry.Entity;
            var type = entity.GetType();

            Type? baseType = type;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(AuditBase<>))
                {
                    dynamic auditEntity = entity;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntity.DateCreated = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            auditEntity.DateUpdated = DateTime.UtcNow;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            auditEntity.IsDeleted = true;
                            auditEntity.DateDeleted = DateTime.UtcNow;
                            break;
                    }
                    break;
                }
                baseType = baseType.BaseType;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
