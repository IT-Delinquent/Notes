﻿using Notes.Helpers;
using Notes.Models;
using Notes.Settings;
using Notes.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    /// <summary>
    /// The class for the notes list page
    /// </summary>
    public class NotesPageViewModel : IPageAppearingEvent, INotifyPropertyChanged
    {
        #region Private backing fields

        /// <summary>
        /// Holds all the notes currently in the application
        /// </summary>
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();

        /// <summary>
        /// Holds the currently selected note from the list
        /// </summary>
        private Note _selectedNote;

        /// <summary>
        /// Whether the note list is shown - true by default
        /// </summary>
        private bool _noteListIsVisible = true;

        /// <summary>
        /// Whether the empty list screen is shown - false by default
        /// </summary>
        private bool _emptyListIsVisible = false;

        /// <summary>
        /// Whether the edit button is visible
        /// </summary>
        private bool _editNoteButtonIsVisible = false;

        /// <summary>
        /// Holds the current sorting option for the notes list
        /// </summary>
        private string _currentSortingOption;

        #endregion Private backing fields

        #region public methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public NotesPageViewModel()
        {
            NewNoteCommand = new Command(async () => await ShowNewNoteAsync());
            EditNoteCommand = new Command(async () => await ShowEditNoteAsync());
            SettingsCommand = new Command(async () => await ShowSettingsAsync());
        }

        /// <summary>
        /// The method for when the notespage appears
        /// Uses the code behind for the event forwarding
        /// </summary>
        public void OnAppearing()
        {
            SelectedNote = null;

            List<string> files = IOHelpers.EnumerateAllFiles();

            Notes?.Clear();

            foreach (var fileName in files)
            {
                string noteData = IOHelpers.ReadAllFileText(fileName);

                //Split the note into it's "parts"
                //The note will have three parts
                //Title
                //Text - can have multiple parts
                //Color
                string[] noteParts = noteData.Split(':');
                int notePartCount = noteParts.Count();

                //Performs some magic so that the title is the first element in the array
                //The text is from the second to the second to last element in the array (this preserves the ':')
                //The color is the final element
                string title = noteParts[0];
                string text = string.Join(":", noteParts.Skip(1).Take(notePartCount - 2));
                string color = noteParts[notePartCount - 1];

                Note _tempNote = new Note
                {
                    Filename = fileName,
                    Title = title,
                    Text = text,
                    Date = IOHelpers.GetNoteDate(fileName),
                    Color = Color.FromHex(color)
                };

                Notes.Add(_tempNote);
            }

            //sorting the notes
            _currentSortingOption = AppSettings.OrderByOption;
            SortNotes();

            ShowCorrectView();
            ShowEditButton();
        }

        #endregion public methods

        #region PropertyChanged

        /// <summary>
        /// The public event for property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The protected method for calling the property changed event handler
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged

        #region Public fields

        /// <summary>
        /// Public accessor and setter for the notes in the application
        /// </summary>
        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Public accessor and setter for the currently selected note
        /// </summary>
        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged();
                ShowEditButton();
            }
        }

        /// <summary>
        /// Public accessor and setter for displaying the notes list
        /// </summary>
        public bool NotesListIsVisible
        {
            get { return _noteListIsVisible; }
            set
            {
                _noteListIsVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Public accessor and setter for displaying the empty list view
        /// </summary>
        public bool EmptyListIsVisible
        {
            get { return _emptyListIsVisible; }
            set
            {
                _emptyListIsVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Public accessor and setter for displaying the edit note button
        /// </summary>
        public bool EditNoteButtonIsVisible
        {
            get { return _editNoteButtonIsVisible; }
            set
            {
                _editNoteButtonIsVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command for launching a new note
        /// </summary>
        public ICommand NewNoteCommand { get; }

        /// <summary>
        /// Command for launching the selected note
        /// </summary>
        public ICommand EditNoteCommand { get; }

        /// <summary>
        /// Command for launching the settings
        /// </summary>
        public ICommand SettingsCommand { get; }

        #endregion Public fields

        #region Private methods

        /// <summary>
        /// The async task for displaying a new note
        /// </summary>
        /// <returns>A task to show the new note page</returns>
        private async Task ShowNewNoteAsync()
        {
            await NavigationHelpers.PushPageAsync(new NewNotePage { });
        }

        /// <summary>
        /// The async task to display the currently selected note
        /// </summary>
        /// <returns>A task to show the edit note page, passing in the SelectedNote</returns>
        private async Task ShowEditNoteAsync()
        {
            if (SelectedNote == null)
            {
                return;
            }
            await NavigationHelpers.PushPageAsync(new EditNotePage(SelectedNote) { });
        }

        /// <summary>
        /// The async task to display the settings page
        /// </summary>
        /// <returns>A task to show the settings page</returns>
        private async Task ShowSettingsAsync()
        {
            await NavigationHelpers.PushPageAsync(new SettingsPage { });
        }

        /// <summary>
        /// Shows the correct view based on the amount of notes in the application
        /// </summary>
        private void ShowCorrectView()
        {
            if (Notes == null || Notes.Count <= 0)
            {
                //Show the empty list view
                NotesListIsVisible = false;
                EmptyListIsVisible = true;
            }
            else
            {
                //Show the notes list view
                NotesListIsVisible = true;
                EmptyListIsVisible = false;
            }
        }

        /// <summary>
        /// Decides whether to show the edit button or not
        /// </summary>
        private void ShowEditButton()
        {
            if (SelectedNote == null)
            {
                EditNoteButtonIsVisible = false;
            }
            else
            {
                EditNoteButtonIsVisible = true;
            }
        }

        /// <summary>
        /// Method to sort the Notes list based on the app settings
        /// </summary>
        private void SortNotes()
        {
            switch (_currentSortingOption)
            {
                case "Date - Ascending":
                    Notes = new ObservableCollection<Note>(Notes.OrderBy(n => n.Date));
                    break;

                case "Date - Descending":
                    Notes = new ObservableCollection<Note>(Notes.OrderByDescending(n => n.Date));
                    break;

                case "Title - Ascending":
                    Notes = new ObservableCollection<Note>(Notes.OrderBy(n => n.Title));
                    break;

                case "Title - Descending":
                    Notes = new ObservableCollection<Note>(Notes.OrderByDescending(n => n.Title));
                    break;
                case "Color":
                    Notes = new ObservableCollection<Note>(Notes.OrderBy(n => n.Color.ToString())
                        .ThenBy(x => x.Title));
                    break;
                default:
                    Notes = new ObservableCollection<Note>(Notes.OrderBy(n => n.Date));
                    break;
            }
        }

        #endregion Private methods
    }
}