using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Repositories
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly string _systemUser = "System";

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;
            var entries = context.ChangeTracker.Entries<EntityBase>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = now;
                    entry.Entity.CreatedBy = _systemUser;
                    entry.Entity.Updated = now;
                    entry.Entity.UpdatedBy = _systemUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Updated = now;
                    entry.Entity.UpdatedBy = _systemUser;
                }
            }
        }
    }
}