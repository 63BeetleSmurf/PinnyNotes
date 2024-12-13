using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Tools;

public partial class GibberishTool : BaseTool, ITool
{
    public ToolStates State => ToolStates.Disabled; // (ToolStates)ToolSettings.Default.GibberishToolState;

    public enum ToolActions
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

    private Random random = new();

    public GibberishTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Gibberish";
        _menuActions.Add(new("Word", GibberishWordMenuAction));
        _menuActions.Add(new("Title", GibberishTitleMenuAction));
        _menuActions.Add(new("Sentence", GibberishSentenceMenuAction));
        _menuActions.Add(new("Paragraph", GibberishParagraphMenuAction));
        _menuActions.Add(new("Article", GibberishArticleMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Name", GibberishNameMenuAction));
    }

    private void GibberishWordMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishWord());
    private void GibberishTitleMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishTitle());
    private void GibberishSentenceMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishSentence());
    private void GibberishParagraphMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishParagraph());
    private void GibberishArticleMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishArticle());
    private void GibberishNameMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GenerateGibberishTitle(2));

    private string GenerateGibberishWord(bool titleCase = false)
    {
        int length = random.Next(2, 11);
        char[] chars = new char[length];

        for (int i = 0; i < length; i++)
        {
            chars[i] = _characters[random.Next(_characters.Length)];
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
            length = random.Next(2, 6);

        string[] words = new string[length];

        for (int i = 0; i < length; i++)
            words[i] = GenerateGibberishWord(true);

        return string.Join(' ', words);
    }

    private string GenerateGibberishSentence()
    {
        int length = random.Next(15, 20);
        string[] words = new string[length];

        for (int i = 0; i < length; i++)
        {
            words[i] = GenerateGibberishWord((i == 0));
            if (i >= 2 && i < length - 2 && random.Next(1, 100) <= 10)
                words[i] = $"{words[i]},";
        }

        return $"{string.Join(' ', words)}.";
    }

    private string GenerateGibberishParagraph()
    {
        int length = random.Next(2, 6);
        string[] sentences = new string[length];

        for (int i = 0; i < length; i++)
            sentences[i] = GenerateGibberishSentence();

        return string.Join(' ', sentences);
    }

    private string GenerateGibberishArticle()
    {
        int length = random.Next(5, 10);
        string[] paragraphs = new string[length];

        for (int i = 0; i < length; i++)
        {
            if (random.Next(1, 100) <= 40)
                paragraphs[i] = $"{GenerateGibberishTitle()}{_doubleNewLine}{GenerateGibberishParagraph()}";
            else
                paragraphs[i] = GenerateGibberishParagraph();
        }

        return $"{GenerateGibberishTitle()}{_doubleNewLine}{string.Join(_doubleNewLine, paragraphs)}";
    }
}
