using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Avalonia.Controls;

namespace notatnik;

public partial class MainWindow : Window
{
    private static readonly Regex NoteHeading = new(
        @"^## (?<date>\d{4}-\d{2}-\d{2} \d{2}:\d{2}) — (?<title>.*)$",
        RegexOptions.Multiline | RegexOptions.Compiled);

    private readonly ObservableCollection<Note> _notes = new();
    private readonly string _notesPath = GetNotesFilePath();

    public MainWindow()
    {
        InitializeComponent();
        NotesList.ItemsSource = _notes;
        LoadNotes();
    }

    private void LoadNotes()
    {
        if (!File.Exists(_notesPath))
        {
            StatusText.Text = $"Notatki będą zapisywane w pliku: {_notesPath}";
            return;
        }

        try
        {
            var content = File.ReadAllText(_notesPath, Encoding.UTF8);
            var headings = NoteHeading.Matches(content);

            for (var index = 0; index < headings.Count; index++)
            {
                var heading = headings[index];
                var bodyStart = heading.Index + heading.Length;
                var bodyEnd = index + 1 < headings.Count ? headings[index + 1].Index : content.Length;
                var body = content[bodyStart..bodyEnd].Trim('\r', '\n');

                if (DateTime.TryParseExact(
                    heading.Groups["date"].Value,
                    "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out var createdAt))
                {
                    _notes.Add(new Note(createdAt, heading.Groups["title"].Value, body));
                }
            }

            StatusText.Text = $"Notatki są zapisywane w pliku: {_notesPath}";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Nie udało się odczytać notatek: {ex.Message}";
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var title = TitleInput.Text?.Trim().Replace("\r", " ").Replace("\n", " ");
        var body = BodyInput.Text?.Trim();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
        {
            StatusText.Text = "Wpisz tytuł i treść notatki.";
            return;
        }

        var createdAt = DateTime.Now;
        var note = new Note(createdAt, title, body);
        var existingContent = string.Empty;

        try
        {
            if (File.Exists(_notesPath))
            {
                existingContent = File.ReadAllText(_notesPath, Encoding.UTF8);
            }

            var newEntry = $"## {createdAt:yyyy-MM-dd HH:mm} — {title}{Environment.NewLine}{body}";
            var separator = string.IsNullOrEmpty(existingContent) ? string.Empty : $"{Environment.NewLine}{Environment.NewLine}";
            File.WriteAllText(_notesPath, newEntry + separator + existingContent, new UTF8Encoding(false));

            _notes.Insert(0, note);
            TitleInput.Text = string.Empty;
            BodyInput.Text = string.Empty;
            StatusText.Text = $"Zapisano notatkę. Plik: {_notesPath}";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Nie udało się zapisać notatki: {ex.Message}";
        }
    }

    private static string GetNotesFilePath()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "notatnik.csproj")))
            {
                return Path.Combine(directory.FullName, "notatki.md");
            }

            directory = directory.Parent;
        }

        return Path.Combine(Environment.CurrentDirectory, "notatki.md");
    }
}

public sealed record Note(DateTime CreatedAt, string Title, string Body)
{
    public string Heading => $"## {CreatedAt:yyyy-MM-dd HH:mm} — {Title}";
}
