namespace PinnyNotes.Core.Migrations;

public class Schema3To4Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 3;
    public override int ResultingSchemaVersion => 4;
    public override string UpdateQuery => $@"
        -- Update Settings
        -- -- Add PinnedByDefault
        ALTER TABLE Settings
        ADD COLUMN Notes_PinnedByDefault INTEGER DEFAULT 0;

        -- Update schema version
        UPDATE SchemaInfo
        SET Version = {ResultingSchemaVersion}
        WHERE Id = 0;
    ";
}
