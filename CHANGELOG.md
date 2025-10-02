# Change Log


## v1.15.0 - Pending

### Added
- Setting to change note visibility, Show in taskbar, hide in taskbar or hide in taskbar and task switcher (Alt+Tab and Win+Tab).
- Database migration system, seamlessly upgrade the database after upgrading the application.
- Text Lock, toggle Locked from the text box context menu to prevent note text being edited.
- URL tool, encode and decode text for use in URls.
- 

### Changed
- Tray icon implimentation, removes dependancy on WinForms.
- 

### Fixed
- Notes sometimes not going behind other windows when unpinned.
- 


## v1.14.0 - 30/09/2025

### Added
- Color tool.
- Backend database.
- Copy (Ctrl+C), Alternative Copy (Ctrl+Shift+C) and Alternative Copy Fallback (Ctrl+Shift+C with no selection) action options.
- Paste (Ctrl+V) and Alternative Copy (Ctrl+Shift+C) action options.
- Trim option for all copy and paste actions.

### Changed
- Code refactoring.
- Rearranged Show In Task Bar and Auto Indent settings.

### Fixed
- Additional copy, cut and paste actions being skipped in some situations, e.g. triggered by mapped mouse button.
- Clipboard being cleared when note if activated and AutoCopy is enabled.
- Child settings not enabling/disabling when parent value changed.
- Undo (Ctrl+Z) not working after applying tool actions.
- Drag drop not working for loading files.


## v1.13.0 - 01/07/2025

### Added
- Settings to change transparency values.
- Settings to change default note size.
- Settings to change fonts.
- Setting to toggle text wrapping.
- Setting to choose copy behaviour when no text selected.

### Changed
- Updated to .NET 9.
- General code refactoring.
- Simplified transparency settings.
- Simplified tool settings.
- Rearranged settings window.

### Fixed
- Occasional crash when pasting from clipboard.


## v1.12.1 - 21/11/2024

### Fixed
- Crash when pressing Shift + Tab at the start of a note.
- Shift + Tab not removing the correct number of spaces.


## v1.12.0 - 20/11/2024

### Added
- Option to check for new version on start up (disabled by default).
- Settings to enable and favourite tools.
- Debug flag allowing release and debug instances to run together.

### Changed
- Moved sorting functions from Lists tool to their own Sort tool.
- Organised settings window.
- Icons updated.

### Fixed
- Notes always turning transparent when activated if Opaque When Focused was off.
- Line count no longer counts trailing empty line when NewLineAtEnd is set and no text is selected.
- Shortcuts not showing icon after install.


## v1.11.3 - 12/11/2024

### Fixed
- Bug where theme was not applied when colour was set to yellow and cycle colours was off.


## v1.11.2 - 03/11/2024

### Changed
- Trigger current running instance to open a new note when a new instance is opened, rather than no action.


## v1.11.1 - 23/10/2024

### Added
- Setting to toggle showing notes in Taskbar.

### Fixed
- Settings window opening off screen when note near or over the edge of the screen.
- Revert mouse button swapped check. Handled already by windows so reported bug must have been a mistake.


## v1.11.0 - 22/10/2024

### Added
- Tray icon - Click to show open notes, double click to create a new note and right click for menu.
- Bracket tool.
- Backtick action to Quote tool.
- Trim action to Quote tool.
- Ability to use Shift + Tab to remove whitespace.
- Setting to hide the title bar on note.
- Setting to use spaces instead of tabs when Tab is pressed.
- Setting to use a mono spaced font.
- Title bar menu item to reset note back to default size.

### Changed
- Minimum note size.

### Fixed
- Title bar only being draggable with left mouse button even when buttons switched in Windows.
- Save prompt did now show if a note was closed by right clicking in Windows Taskbar.
- Notes would always open on primary monitor, even if opened from parent on anothe monitor.


## v1.10.0 - 22/07/2024

### Added
- Minimize settings - Specify if notes should prevent themselves being minimized, e.g. with Show Desktop.
- Tab indent - Selecting text and pressing tab will now indent the selected text.
- Remove tool - Remove various whitespace, slashes or occurrences of current selection.
- Settings window - Settings move out of menu and into their own window.
- Start positions - More start positions have been added.
- Transparency - Note can now be transparent depending on given settings.
- Dark mode - Notes can appear dark with the selected color used as accent colors.

### Fixed
- Notes order - Unpinned notes now come above pinned notes when focused for editing.
- Dragging issue - Fixed issue where dragging a word would overwrite note text.


## v1.9.0 - 11/07/2024

### Added
- Gibberish Tool - Generate gibberish/sample text from words to full articles.
- DateTime Tool - Initial implementation, more date functions to be added.

### Changed
- Colour menu now shows a preview of the colour.
- Performance improvements.
- Various code refactoring and optimisations in preparation to move to a MVVM architecture.

### Fixed
- Fixed spelling suggestions not showing in context menu.


## v1.8.0 - 09/07/2024

### Added
- Slash tool: Toggle, replace or remove forward and back slashes.

### Changed
- Various code refactoring to ease future work towards new major version.


## v1.7.0 - 05/05/2023

### Added
- Counts menu to show line, word and char counts.
- Quote tools to wrap text in single or double quotes.
- Trim tool to remove blank lines.
- List tools for dashed list (markdown) and to remove list markers.
- Keep new line at end visible.
- Middle click to paste.
- Auto indent.
- Hold Ctrl to copy when selecting text by mouse clicks.
- Toggle maximize when title bar double clicked.


## v1.6.0 - 15/04/2023

### Added
- Added split tool.
- Added join tool.

### Changed
- Sort tools menu alphabetically.
- Move caret to end of text after change.

### Fixed
- Remember save state of note.
- Fix tools to work with full note text or selected text.
- Text box accepts tab.


## v1.5.0 - 12/04/2023

### Added
- All tools now work on selected note text or all text if nothing selected.
- Text case options.
- Options to trim text copied from or pasted into a note.
- Triple click to select full line, quadruple click to ignore wrapping and select entire line.

### Changed
- Moved tools into text box context menu.
- Checking for updates is now opt-in.


## v1.4.0 - 16/03/2023

### Added
- HTML entity encode and decode options.

### Changed
- Rearrange menu, colours now easier to access.
- Adjust positioning so it is easier to get notes behind others.

### Fixed
- Only update gravity after drag stopped.
- Do not add new line when note is empty.
- Fix screen bounds check.


## v1.3.0 - 11/03/2023

### Added
- Note colours with cycle colours option.
- Option to always ensure text ends with new line.
- Start-up position option.

### Changed
- Centre dialogs on parent note.
- Don't enable Auto Copy by default.
- Leave extra room for title bar with upward gravity as well as downward.


## v1.2.1 - 09/03/2023

### Fixed
- Settings were not loading when creating new notes.


## v1.2.0 - 08/03/2023

### Added
- Weekly check for new releases.
- Border to main window.
- Drag and drop handling for text and files.
- Hashing options.

### Changed
- Move save option into context menu.
- Moved settings to sub menu.

### Fixed
- JSON error handling, leave text unchanged if invalid.


## v1.1.0 - 07/03/2023

### Added
- JSON prettify option.
- Spell checker option.
- Option to clear note.
- List enumerate option and sorting options.
- Indent options.
- Base64 Encoding and Decoding.
- Trim option to menu.
- Auto copy option.

### Changed
- Swap icons for pinned and unpinned to make more sense.

### Fixed
- Focus on text box when loaded.
