namespace PinnyNotes.Core.Configurations;

public class DatabaseConfiguration
{
    public readonly string ConnectionString;

    public DatabaseConfiguration()
    {
        string dataPath;
        // Use exe dir for database if in Debug mode or is portable.
        if (System.Diagnostics.Debugger.IsAttached || File.Exists(Path.Combine(AppContext.BaseDirectory, "portable.txt")))
            dataPath = AppContext.BaseDirectory;
        else
            dataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Pinny Notes"
            );

        if (!Path.Exists(dataPath))
            Directory.CreateDirectory(dataPath);

        ConnectionString = $"Data Source={Path.Combine(dataPath, "pinny_notes.sqlite")}";
    }
}
