namespace PinnyNotes.Core.Migrations;

public class Schema1To2Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 1;
    public override int ResultingSchemaVersion => 2;
    public override string UpdateQuery => $@"
        -- Update Settings
        -- -- Change Notes_ShowInTaskbar to Notes_VisibilityMode
        ALTER TABLE Settings
        ADD COLUMN Notes_VisibilityMode INTEGER DEFAULT 0;

        UPDATE Settings
        SET
            Notes_VisibilityMode = CASE
                WHEN Notes_ShowInTaskbar = 1 THEN 0
                ELSE 1 
            END;

        ALTER TABLE Settings
        DROP COLUMN Notes_ShowInTaskbar;

        -- -- Add UrlToolState
        ALTER TABLE Settings
        ADD COLUMN Tool_UrlState INTEGER DEFAULT 1;

        -- Update schema version
        UPDATE SchemaInfo
        SET Version = {ResultingSchemaVersion}
        WHERE Id = 0;
    ";
}
