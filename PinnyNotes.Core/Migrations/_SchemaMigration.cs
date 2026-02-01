namespace PinnyNotes.Core.Migrations;

public abstract class SchemaMigration
{
    public abstract int TargetSchemaVersion { get; }
    public abstract int ResultingSchemaVersion { get; }
    public abstract string UpdateQuery { get; }
}
