using System.Runtime.InteropServices;

namespace PinnyNotes.WpfUi.Controls.BackgroundSpellCheck;

public class BackgroundSpellChecker : IDisposable
{
    private ISpellChecker? _spellChecker;

    private readonly Lock _lock = new();

    private bool _disposed;

    public BackgroundSpellChecker(string languageTag)
    {
        ISpellCheckerFactory factory = (ISpellCheckerFactory)new SpellCheckerFactoryCoClass();

        factory.IsSupported(languageTag, out bool isSupported);
        if (!isSupported)
            throw new InvalidOperationException($"Language '{languageTag}' is not supported by Windows Spell Checker.");

        factory.CreateSpellChecker(languageTag, out _spellChecker);
        if (_spellChecker is null)
            throw new InvalidOperationException($"Failed to create spell checker for language '{languageTag}'.");
    }

    public List<BackgroundSpellingError> CheckText(string text)
    {
        lock (_lock)
        {
            List<BackgroundSpellingError> errors = [];

            if (_spellChecker is null || string.IsNullOrEmpty(text))
                return errors;

            _spellChecker.Check(text, out IEnumSpellingError enumErrors);
            if (enumErrors is null)
                return errors;

            try
            {
                while (true)
                {
                    enumErrors.Next(out ISpellingError error);
                    if (error is null)
                        break;

                    error.get_StartIndex(out uint startIndexValue);
                    error.get_Length(out uint lengthValue);

                    int startIndex = (int)startIndexValue;
                    int length = (int)lengthValue;
                    string word = text.Substring(startIndex, Math.Min(length, text.Length - startIndex));

                    errors.Add(
                        new()
                        {
                            Word = word,
                            StartIndex = startIndex,
                            Length = length
                        }
                    );

                    Marshal.ReleaseComObject(error);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumErrors);
            }

            return errors;
        }
    }

    public List<string> GetSuggestions(string word)
    {
        List<string> suggestions = [];

        if (_spellChecker is null || string.IsNullOrEmpty(word))
            return suggestions;

        try
        {
            _spellChecker.Suggest(word, out IEnumString enumSuggestions);
            if (enumSuggestions is null)
                return suggestions;

            try
            {
                while (suggestions.Count < 10)
                {
                    IntPtr[] buffer = new IntPtr[1];
                    int hr = enumSuggestions.Next(1, buffer, out uint fetched);

                    // S_FALSE (1) means no more items
                    if (hr == 1 || fetched == 0)
                        break;

                    // S_OK (0) means success
                    if (hr == 0 && buffer[0] != IntPtr.Zero)
                    {
                        string? suggestion = Marshal.PtrToStringUni(buffer[0]);

                        // Free the COM-allocated string
                        Marshal.FreeCoTaskMem(buffer[0]);

                        if (!string.IsNullOrEmpty(suggestion))
                        {
                            suggestions.Add(suggestion);
                        }
                    }
                    else if (hr != 0)
                    {
                        // Error occurred
                        break;
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumSuggestions);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting suggestions for '{word}': {ex.Message}");
        }

        return suggestions;
    }

    public void AddToDictionary(string word)
    {
        lock (_lock)
        {
            if (_spellChecker is null || string.IsNullOrEmpty(word))
                return;

            try
            {
                _spellChecker.Add(word);
            }
            catch
            {
                // Ignore errors
            }
        }
    }

    public void IgnoreWord(string word)
    {
        lock (_lock)
        {
            if (_spellChecker is null || string.IsNullOrEmpty(word))
                return;

            try
            {
                _spellChecker.Ignore(word);
            }
            catch
            {
                // Ignore errors
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (_spellChecker != null)
        {
            Marshal.ReleaseComObject(_spellChecker);
            _spellChecker = null;
        }

        _disposed = true;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BackgroundSpellChecker()
    {
        Dispose(false);
    }
}

[ComImport]
[Guid("8E018A9D-2415-4677-BF08-794EA61F94BB")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISpellCheckerFactory
{
    void get_SupportedLanguages(out IEnumString value);
    void IsSupported([MarshalAs(UnmanagedType.LPWStr)] string languageTag, out bool value);
    void CreateSpellChecker([MarshalAs(UnmanagedType.LPWStr)] string languageTag, [MarshalAs(UnmanagedType.Interface)] out ISpellChecker value);
}

[ComImport]
[Guid("B6FD0B71-E2BC-4653-8D05-F197E412770B")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISpellChecker
{
    void get_LanguageTag([MarshalAs(UnmanagedType.BStr)] out string value);
    void Check([MarshalAs(UnmanagedType.LPWStr)] string text, [MarshalAs(UnmanagedType.Interface)] out IEnumSpellingError value);
    void Suggest([MarshalAs(UnmanagedType.LPWStr)] string word, [MarshalAs(UnmanagedType.Interface)] out IEnumString value);
    void Add([MarshalAs(UnmanagedType.LPWStr)] string word);
    void Ignore([MarshalAs(UnmanagedType.LPWStr)] string word);
    void AutoCorrect([MarshalAs(UnmanagedType.LPWStr)] string from, [MarshalAs(UnmanagedType.LPWStr)] string to);
    void GetOptionValue([MarshalAs(UnmanagedType.LPWStr)] string optionId, out byte value);
    void get_OptionIds([MarshalAs(UnmanagedType.Interface)] out IEnumString value);
    void get_Id([MarshalAs(UnmanagedType.BStr)] out string value);
    void get_LocalizedName([MarshalAs(UnmanagedType.BStr)] out string value);
    void add_SpellCheckerChanged([MarshalAs(UnmanagedType.Interface)] ISpellCheckerChangedEventHandler handler, out uint eventCookie);
    void remove_SpellCheckerChanged(uint eventCookie);
    void GetOptionDescription([MarshalAs(UnmanagedType.LPWStr)] string optionId, [MarshalAs(UnmanagedType.Interface)] out IOptionDescription value);
    void ComprehensiveCheck([MarshalAs(UnmanagedType.LPWStr)] string text, [MarshalAs(UnmanagedType.Interface)] out IEnumSpellingError value);
}

[ComImport]
[Guid("803E3BD4-2828-4410-8290-418D1D73C762")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IEnumSpellingError
{
    void Next([MarshalAs(UnmanagedType.Interface)] out ISpellingError value);
}

[ComImport]
[Guid("B7C82D61-FBE8-4B47-9B27-6C0D2E0DE0A3")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISpellingError
{
    void get_StartIndex(out uint value);
    void get_Length(out uint value);
    void get_CorrectiveAction(out int value);
    void get_Replacement([MarshalAs(UnmanagedType.BStr)] out string value);
}

[ComImport]
[Guid("00000101-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IEnumString
{
    [PreserveSig]
    int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] IntPtr[] rgelt, out uint pceltFetched);

    [PreserveSig]
    int Skip(uint celt);

    [PreserveSig]
    int Reset();

    [PreserveSig]
    int Clone(out IEnumString ppenum);
}

[ComImport]
[Guid("0B83A5B0-792F-4EAB-9799-ACF52C5ED08A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISpellCheckerChangedEventHandler
{
    void Invoke([MarshalAs(UnmanagedType.Interface)] ISpellChecker sender);
}

[ComImport]
[Guid("432E5F85-35CF-4606-A801-6F70277E1D7A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IOptionDescription
{
    void get_Id([MarshalAs(UnmanagedType.BStr)] out string value);
    void get_Heading([MarshalAs(UnmanagedType.BStr)] out string value);
    void get_Description([MarshalAs(UnmanagedType.BStr)] out string value);
    void get_Labels([MarshalAs(UnmanagedType.Interface)] out IEnumString value);
}

[ComImport]
[Guid("7AB36653-1796-484B-BDFA-E74F1DB7C1DC")]
[ClassInterface(ClassInterfaceType.None)]
internal class SpellCheckerFactoryCoClass
{
}
