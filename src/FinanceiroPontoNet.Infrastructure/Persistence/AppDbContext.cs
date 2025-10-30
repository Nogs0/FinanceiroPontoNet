using System.Linq.Expressions;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Shared;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroPontoNet.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Boleto> Boletos { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetIsDeletedOnDeletedEntities();
            SetFullAuditedFieldsOnFullAuditedEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetIsDeletedOnDeletedEntities()
        {
            var deletedEntities = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDelete);

            foreach (var entry in deletedEntities)
            {
                var entitySoftDelete = (ISoftDelete)entry.Entity;
                entitySoftDelete.DeletedAt = DateTime.UtcNow;
                entry.State = EntityState.Modified;
            }
        }

        private void SetFullAuditedFieldsOnFullAuditedEntities()
        {
            var entities = ChangeTracker.Entries().Where(e => e.Entity is FullAuditedEntity);

            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    var fullAuditedEntity = (FullAuditedEntity)entry.Entity;
                    fullAuditedEntity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    var fullAuditedEntity = (FullAuditedEntity)entry.Entity;
                    fullAuditedEntity.LastModifiedAt = DateTime.UtcNow;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");

                Expression? filterBody = null;

                //Regiao onde inserir mais Global Query Filters
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    // e.DeletedAt == null
                    var softDeleteFilter = Expression.Equal(
                        Expression.Property(
                            Expression.Convert(parameter, typeof(ISoftDelete)),
                            "DeletedAt"
                        ),
                        Expression.Constant(null)
                    );

                    if (filterBody != null)
                        filterBody = Expression.AndAlso(filterBody, softDeleteFilter);
                    else
                        filterBody = softDeleteFilter;
                }

                if (filterBody != null)
                {
                    var finalLambda = Expression.Lambda(filterBody, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(finalLambda);
                }
            }

            modelBuilder.Entity<Banco>().HasIndex(b => b.Codigo).IsUnique();
        }
    }
}
