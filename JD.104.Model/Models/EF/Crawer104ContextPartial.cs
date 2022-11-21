using Microsoft.EntityFrameworkCore;
using Serilog;

namespace JD._104.Model.Models.EF;

public partial class Crawer104Context
{
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetUpdateDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetUpdateDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void SetUpdateDate()
    {
        var 待變更資料 = this.ChangeTracker.Entries<JobDescription>().Where(x => x.State == EntityState.Modified).ToArray();

        if (待變更資料.Length == 0)
            return;

        foreach (var item in 待變更資料)
        {
            var ignoreProps = new string[]
            {
                nameof(JobDescription.UpdatedDate),
                nameof(JobDescription.IsDeleted),
                nameof(JobDescription.AppearDate),
            };

            var properties = item.Properties.Where(x => x.IsModified && !ignoreProps.Contains(x.Metadata.Name)).ToArray();

            if (!properties.Any())
                continue;

            item.Entity.UpdatedDate = DateTime.UtcNow;


            var changedInfo = item.Property(nameof(JobDescription.Id)).CurrentValue?.ToString() + " " + string.Join(Environment.NewLine, item.Properties.Where(x => x.IsModified).Select(x => x.Metadata.Name + " : " + x.OriginalValue + " -> " + x.CurrentValue));
            changedInfo += Environment.NewLine;

            Log.Information(changedInfo);
        }
    }
}
