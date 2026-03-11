using PinnyNotes.Core.Repositories;

namespace PinnyNotes.Core.Migrations;

public class Schema2To3Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 2;
    public override int ResultingSchemaVersion => 3;
    public override string UpdateQuery => $@"
        -- Update ApplicationData
        -- -- Change ThemeColor to ColourScheme
        ALTER TABLE ApplicationData
        ADD COLUMN ColourScheme TEXT DEFAULT NULL;

        ALTER TABLE ApplicationData
        DROP COLUMN ThemeColor;

        -- Update Settings
        -- -- Add UrlToolState
        ALTER TABLE Settings
        ADD COLUMN Tool_GuidState INTEGER DEFAULT 1;

        -- -- Fix column names
        ALTER TABLE Settings 
        RENAME COLUMN Notes_CycleColors TO Notes_CycleColours;

        ALTER TABLE Settings 
        RENAME COLUMN Notes_ColorMode TO Notes_ColourMode;

        ALTER TABLE Settings 
        RENAME COLUMN Tool_ColorState TO Tool_ColourState;

        -- Create Notes Table
        CREATE TABLE IF NOT EXISTS {NoteRepository.TableName}
            {NoteRepository.TableSchema};
    ";
}
