using PinnyNotes.Core.Repositories;

namespace PinnyNotes.Core.Migrations;

public class Schema2To3Migration : SchemaMigration
{
    public override int TargetSchemaVersion => 2;
    public override int ResultingSchemaVersion => 3;
    public override string UpdateQuery => $@"
        -- Update ApplicationData
        -- -- Change ThemeColor to ColorScheme
        ALTER TABLE ApplicationData
        ADD COLUMN ColorScheme TEXT DEFAULT NULL;

        ALTER TABLE ApplicationData
        DROP COLUMN ThemeColor;

        -- Update Settings
        -- -- Add UrlToolState
        ALTER TABLE Settings
        ADD COLUMN Tool_GuidState INTEGER DEFAULT 1;

        -- Create Notes Table
        CREATE TABLE IF NOT EXISTS {NoteRepository.TableName}
            {NoteRepository.TableSchema};

        -- Update schema version
        UPDATE SchemaInfo
        SET Version = {ResultingSchemaVersion}
        WHERE Id = 0;
    ";
}
