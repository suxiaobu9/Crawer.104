using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer._104.Models.EF;

public partial class Crawer104Context
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdateDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetUpdateDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        SetUpdateDate();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetUpdateDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void SetUpdateDate()
    {
        var 待變更資料 = this.ChangeTracker.Entries<職缺>().Where(x => x.State == EntityState.Modified).ToArray();

        if (待變更資料.Length == 0)
            return;

        foreach (var item in 待變更資料)
        {
            if (!item.Properties.Any(x => x.Metadata.Name != nameof(職缺.更新時間) && x.Metadata.Name != nameof(職缺.被刪除) && x.IsModified))
                continue;

            item.Entity.更新時間 = DateTime.UtcNow;

            Log.Information(string.Join(Environment.NewLine, item.Properties.Where(x => x.IsModified).Select(x => x.Metadata.Name + " : " + x.OriginalValue + " -> " + x.CurrentValue)));
        }
    }
}
