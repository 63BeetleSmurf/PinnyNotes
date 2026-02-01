using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class GibberishTool : BaseTool, ITool
{
    private enum ToolActions
    {
        GibberishWord,
        GibberishTitle,
        GibberishSentence,
        GibberishParagraph,
        GibberishArticle,
        GibberishName,
    }

    private const string _characters = "zqxjkvbbppyyggffwwmmuuccllldddrrrhhhsssnnniiiiooooaaaaattttteeeeeeeeee";
    private const string _doubleNewLine = "\r\n\r\n";

    private readonly Random _random = new();

    public ToolStates State => ToolSettings.GibberishToolState;

    public GibberishTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Gibberish",
            [
                new ToolMenuAction("Word", new RelayCommand(() => MenuAction(ToolActions.GibberishWord))),
                new ToolMenuAction("Title", new RelayCommand(() => MenuAction(ToolActions.GibberishTitle))),
                new ToolMenuAction("Sentence", new RelayCommand(() => MenuAction(ToolActions.GibberishSentence))),
                new ToolMenuAction("Paragraph", new RelayCommand(() => MenuAction(ToolActions.GibberishParagraph))),
                new ToolMenuAction("Article", new RelayCommand(() => MenuAction(ToolActions.GibberishArticle))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Name", new RelayCommand(() => MenuAction(ToolActions.GibberishName)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        switch (action)
        {
            case ToolActions.GibberishWord:
                InsertIntoNoteText(GenerateGibberishWord());
                break;
            case ToolActions.GibberishTitle:
                InsertIntoNoteText(GenerateGibberishTitle());
                break;
            case ToolActions.GibberishSentence:
                InsertIntoNoteText(GenerateGibberishSentence());
                break;
            case ToolActions.GibberishParagraph:
                InsertIntoNoteText(GenerateGibberishParagraph());
                break;
            case ToolActions.GibberishArticle:
                InsertIntoNoteText(GenerateGibberishArticle());
                break;
            case ToolActions.GibberishName:
                InsertIntoNoteText(GenerateGibberishTitle(2));
                break;
        }
    }

    private string GenerateGibberishWord(bool titleCase = false)
    {
        int length = _random.Next(2, 11);
        char[] chars = new char[length];

        for (int i = 0; i < length; i++)
        {
            chars[i] = _characters[_random.Next(_characters.Length)];
            if (i == 0 && titleCase)
                chars[i] = char.ToUpper(chars[i]);
        }

        return new(chars);
    }

    private string GenerateGibberishTitle(int? wordCount = null)
    {
        int length;
        if (wordCount != null)
            length = (int)wordCount;
        else
            length = _random.Next(2, 6);

        string[] words = new string[length];

        for (int i = 0; i < length; i++)
            words[i] = GenerateGibberishWord(true);

        return string.Join(' ', words);
    }

    private string GenerateGibberishSentence()
    {
        int length = _random.Next(15, 20);
        string[] words = new string[length];

        for (int i = 0; i < length; i++)
        {
            words[i] = GenerateGibberishWord((i == 0));
            if (i >= 2 && i < length - 2 && _random.Next(1, 100) <= 10)
                words[i] = $"{words[i]},";
        }

        return $"{string.Join(' ', words)}.";
    }

    private string GenerateGibberishParagraph()
    {
        int length = _random.Next(2, 6);
        string[] sentences = new string[length];

        for (int i = 0; i < length; i++)
            sentences[i] = GenerateGibberishSentence();

        return string.Join(' ', sentences);
    }

    private string GenerateGibberishArticle()
    {
        int length = _random.Next(5, 10);
        string[] paragraphs = new string[length];

        for (int i = 0; i < length; i++)
        {
            if (_random.Next(1, 100) <= 40)
                paragraphs[i] = $"{GenerateGibberishTitle()}{_doubleNewLine}{GenerateGibberishParagraph()}";
            else
                paragraphs[i] = GenerateGibberishParagraph();
        }

        return $"{GenerateGibberishTitle()}{_doubleNewLine}{string.Join(_doubleNewLine, paragraphs)}";
    }
}
