using System;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class GibberishTool
{
    private const string _characters = "zqxjkvccuummwwffgghhhhrrrrddddllllttttttaaaaaaooooooiiiiiinnnnnnsssssseeeeeeeeeeee";

    private const string _doubleNewLine = "\r\n\r\n";

    public const string Name = "Gibberish";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Word", OnWordMenuItemClick),
                new("Title", OnTitleMenuItemClick),
                new("Sentence", OnSentenceMenuItemClick),
                new("Paragraph", OnParagraphMenuItemClick),
                new("Article", OnArticleMenuItemClick),
                new("-"),
                new("Name", OnNameMenuItemClick)
            ]
        );

    private static void OnWordMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateWord());
    private static void OnTitleMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateTitle());
    private static void OnSentenceMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateSentence());
    private static void OnParagraphMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateParagraph());
    private static void OnArticleMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateArticle());
    private static void OnNameMenuItemClick(object sender, EventArgs e)
        => ToolHelper.InsertIntoNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), GenerateTitle(2));

    private static string GenerateWord(bool titleCase = false, Random? random = null)
    {
        random ??= new();

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

    private static string GenerateTitle(int? wordCount = null, Random? random = null)
    {
        random ??= new();

        int length;
        if (wordCount != null)
            length = (int)wordCount;
        else
            length = random.Next(2, 6);

        string[] words = new string[length];

        for (int i = 0; i < length; i++)
            words[i] = GenerateWord(true);

        return string.Join(' ', words);
    }

    private static string GenerateSentence(Random? random = null)
    {
        random ??= new();

        int length = random.Next(15, 20);
        string[] words = new string[length];

        for (int i = 0; i < length; i++)
        {
            words[i] = GenerateWord((i == 0));
            if (i >= 2 && i < length - 2 && random.Next(1, 100) <= 10)
                words[i] = $"{words[i]},";
        }

        return $"{string.Join(' ', words)}.";
    }

    private static string GenerateParagraph(Random? random = null)
    {
        random ??= new();

        int length = random.Next(2, 6);
        string[] sentences = new string[length];

        for (int i = 0; i < length; i++)
            sentences[i] = GenerateSentence();

        return string.Join(' ', sentences);
    }

    private static string GenerateArticle(Random? random = null)
    {
        random ??= new();

        int length = random.Next(5, 10);
        string[] paragraphs = new string[length];

        for (int i = 0; i < length; i++)
        {
            if (random.Next(1, 100) <= 40)
                paragraphs[i] = $"{GenerateTitle()}{_doubleNewLine}{GenerateParagraph()}";
            else
                paragraphs[i] = GenerateParagraph();
        }

        return $"{GenerateTitle()}{_doubleNewLine}{string.Join(_doubleNewLine, paragraphs)}";
    }
}
