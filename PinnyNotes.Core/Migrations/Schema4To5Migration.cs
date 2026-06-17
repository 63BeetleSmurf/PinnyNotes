namespace PinnyNotes.Core.Migrations;

public class Schema4To5Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 4;
    public override int ResultingSchemaVersion => 5;
    public override string UpdateQuery => $@"
        -- Update Settings
        -- -- Add Application_StartWithWindows
        ALTER TABLE Settings
        ADD COLUMN Application_StartWithWindows INTEGER DEFAULT 0;

        -- -- Add Notes_TitleBarItems
        ALTER TABLE Notes
        ADD COLUMN TitleBarItems TEXT DEFAULT '[1,0,2,0,3]';
    ";
}
