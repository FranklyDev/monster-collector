using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class AuditManager
{
    public static Dictionary<string, object?> GetKeyValues(EntityEntry entry)
    {
        return entry.Properties
                    .Where(p => p.Metadata.IsPrimaryKey())
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
    }

    public static List<string?> GetOldValues(EntityEntry entry)
    {
        return entry.Properties
                    .Where(p => p.IsModified)
                    .Select(p => p.OriginalValue.ToString())
                    .ToList();
    }

    public static List<string?> GetNewValues(EntityEntry entry)
    {
        return entry.Properties
                    .Where(p => p.IsModified)
                    .Select(p => p.CurrentValue.ToString())
                    .ToList();
    }

    public static List<string> GetChangedColumns(EntityEntry entry)
    {
        return entry.Properties
                    .Where(p => p.IsModified)
                    .Select(p => p.Metadata.Name)
                    .ToList();
    }

    public static List<AuditLog> Find(string monsterId)
    {
        using (var context = new DatabaseContext())
        {
            return context.AuditLogs.Where(log => log.KeyValues.Contains(monsterId)).ToList();
        }
    }
}