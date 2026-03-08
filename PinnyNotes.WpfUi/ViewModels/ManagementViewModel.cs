using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using PinnyNotes.Core.DataTransferObjects;
using PinnyNotes.Core.Enums;
using PinnyNotes.Core.Repositories;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.ViewModels;

public class ManagementViewModel : BaseViewModel
{
    private readonly NoteRepository _noteRepository;
    private readonly ThemeService _themeService;

    private readonly Dictionary<int, int> _notePreviewIdIndexMap = []; // Use this since we don't have a Observable Dictionary
    private readonly Dictionary<int, NotePreviewModel> _selectedNotePreviews = [];

    public ManagementViewModel(
        NoteRepository noteRepository,
        AppMetadataService appMetadataService,
        SettingsService settingsService,
        MessengerService messengerService,
        ThemeService themeService
    ) : base(appMetadataService, settingsService, messengerService)
    {
        _noteRepository = noteRepository;
        _themeService = themeService;

        LoadNotes();

        NewNoteCommand = new RelayCommand(OnNewNoteCommand);
        OpenSettingsCommand = new RelayCommand(OnOpenSettingsCommand);

        OpenNotesCommand = new RelayCommand(OnOpenNotesCommand);
        CloseNotesCommand = new RelayCommand(OnCloseNotesCommand);
        DeleteNotesCommand = new RelayCommand(OnDeleteNotesCommand);

        MessengerService.Subscribe<NoteActionMessage>(OnNoteActionMessage);
    }

    public ICommand NewNoteCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ICommand OpenNotesCommand { get; }
    public ICommand CloseNotesCommand { get; }
    public ICommand DeleteNotesCommand { get; }

    public ObservableCollection<NotePreviewModel> NotePreviews { get; } = [];

    private async void LoadNotes()
    {
        ColourMode colourMode = SettingsService.NoteSettings.ColourMode;

        ClearNotePreviews();

        IEnumerable<NoteDto> noteDtos = await _noteRepository.GetAll();
        foreach (NoteDto noteDto in noteDtos)
            AddNotePreview(noteDto, colourMode);
    }

    private void OnNoteActionMessage(NoteActionMessage message)
    {
        switch (message.Action)
        {
            case NoteAction.Created:
                AddNotePreview(message.NoteDto);
                break;
            case NoteAction.Updated:
                UpdatedNotePreview(message.NoteDto);
                break;
            case NoteAction.Deleted:
                RemoveNotePreview(message.NoteDto.Id);
                break;
        }
    }

    private void UpdatedNotePreview(NoteDto dto)
    {
        NotePreviewModel notePreview = NotePreviews[_notePreviewIdIndexMap[dto.Id]];

        notePreview.ContentPreview = dto.Content;
        notePreview.ThemeColourScheme = dto.ThemeColourScheme;

        UpdateNotePreviewBrushes(notePreview);
    }

    private void UpdateNotePreviewBrushes(NotePreviewModel notePreview, ColourMode? colourMode = null)
    {
        Palette palette = _themeService.GetPalette(
            notePreview.ThemeColourScheme,
            colourMode ?? SettingsService.NoteSettings.ColourMode
        );

        notePreview.UpdateBrushes(palette);
    }

    private void OnNewNoteCommand()
    {
        MessengerService.Publish(new OpenNoteWindowMessage(isManagementWindowParent: true));
    }

    private void OnOpenSettingsCommand()
    {
        MessengerService.Publish(new OpenSettingsWindowMessage());
    }

    private void OnOpenNotesCommand()
    {
        List<int> selectedNoteIds = [.._selectedNotePreviews.Keys]; // Selected
        if (selectedNoteIds.Count == 0)
            selectedNoteIds = [.._notePreviewIdIndexMap.Keys]; // All

        MessengerService.Publish(new MultipleNoteWindowActionMessage(selectedNoteIds, NoteWindowAction.Open));
    }

    private void OnCloseNotesCommand()
    {
        List<int> selectedNoteIds = [.._selectedNotePreviews.Keys]; // Selected
        if (selectedNoteIds.Count == 0)
            selectedNoteIds = [.._notePreviewIdIndexMap.Keys]; // All

        MessengerService.Publish(new MultipleNoteWindowActionMessage(selectedNoteIds, NoteWindowAction.Close));
    }

    private async void OnDeleteNotesCommand()
    {
        List<int> selectedNoteIds = [.._selectedNotePreviews.Keys]; // Selected
        if (selectedNoteIds.Count == 0)
            return; // No delete all

        MessengerService.Publish(new MultipleNoteWindowActionMessage(selectedNoteIds, NoteWindowAction.Close));

        foreach (int noteId in selectedNoteIds)
        {
            if (!_notePreviewIdIndexMap.ContainsKey(noteId))
                continue; // Note may have been deleted due to being empty when closed above

            RemoveNotePreview(noteId);
            await _noteRepository.Delete(noteId);
        }
    }

    private void NotePreview_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not NotePreviewModel notePreview)
            return;

        switch (e.PropertyName)
        {
            case nameof(NotePreviewModel.IsSelected):

                if (notePreview.IsSelected && !_selectedNotePreviews.ContainsKey(notePreview.Id))
                    _selectedNotePreviews[notePreview.Id] = notePreview;
                else if (!notePreview.IsSelected && _selectedNotePreviews.ContainsKey(notePreview.Id))
                    _selectedNotePreviews.Remove(notePreview.Id);

                break;
        }
    }

    private void ClearNotePreviews()
    {
        foreach (NotePreviewModel notePreview in NotePreviews)
            notePreview.PropertyChanged -= NotePreview_PropertyChanged;

        NotePreviews.Clear();
        _notePreviewIdIndexMap.Clear();
        _selectedNotePreviews.Clear();
    }

    private void AddNotePreview(NoteDto dto, ColourMode? colourMode = null)
    {
        NotePreviewModel notePreview = new(dto);

        UpdateNotePreviewBrushes(notePreview, colourMode);

        notePreview.PropertyChanged += NotePreview_PropertyChanged;

        NotePreviews.Add(notePreview);
        _notePreviewIdIndexMap[dto.Id] = NotePreviews.Count - 1;
        if (notePreview.IsSelected)
            _selectedNotePreviews[notePreview.Id] = notePreview;
    }

    private void RemoveNotePreview(int noteId)
    {
        NotePreviewModel notePreview = NotePreviews[_notePreviewIdIndexMap[noteId]];

        notePreview.PropertyChanged -= NotePreview_PropertyChanged;

        NotePreviews.Remove(notePreview);
        _selectedNotePreviews.Remove(notePreview.Id);

        // Rebuild map not indexes will have changed
        _notePreviewIdIndexMap.Clear();
        for (int i = 0; i < NotePreviews.Count; i++)
            _notePreviewIdIndexMap[NotePreviews[i].Id] = i;
    }
}
