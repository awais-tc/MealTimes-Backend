using System.Runtime.CompilerServices;

namespace MealTimes.Repository
{
    /// <summary>
    /// Configures Npgsql's legacy timestamp behavior. By default Npgsql maps
    /// <see cref="System.DateTime"/> to PostgreSQL 'timestamp with time zone' and requires
    /// UTC values, which would throw for DateTimes with Kind=Unspecified/Local. This app was
    /// migrated from SQL Server and uses such values in places, so we opt into the legacy
    /// behavior (DateTime -> 'timestamp without time zone', any Kind accepted).
    ///
    /// It runs as a [ModuleInitializer] so the switch is set when this assembly loads —
    /// before Npgsql builds its type mappings — and therefore applies identically at runtime,
    /// at design time, and during EF Core migration tooling.
    /// </summary>
    internal static class NpgsqlCompatibility
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
