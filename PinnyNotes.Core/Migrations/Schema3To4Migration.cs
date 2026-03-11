namespace PinnyNotes.Core.Migrations;

public class Schema3To4Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 3;
    public override int ResultingSchemaVersion => 4;
    public override string UpdateQuery => $@"
        -- Update Settings
        -- -- Add Application_StartupBehaviour
        ALTER TABLE Settings
        ADD COLUMN Application_StartupBehaviour INTEGER DEFAULT 0;

        -- -- Add Application_NewInstanceBehaviour
        ALTER TABLE Settings
        ADD COLUMN Application_NewInstanceBehaviour INTEGER DEFAULT 0;

        -- -- Add PinnedByDefault
        ALTER TABLE Settings
        ADD COLUMN Notes_PinnedByDefault INTEGER DEFAULT 0;
    ";
}
