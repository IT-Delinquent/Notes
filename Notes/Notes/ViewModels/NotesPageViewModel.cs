using Notes.Helpers;
using Notes.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Notes.Views;
using Notes.Settings;

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

        #endregion Private backing fields

        #region public methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public NotesPageViewModel()
        {
            NewNoteCommand = new Command(async () => await NewNoteAsync());
            EditNoteCommand = new Command(async () => await LoadNoteAsync());
            SettingsCommand = new Command(async () => await LoadSettingsAsync());
        }

        /// <summary>
        /// The method for when the notespage appears
        /// Uses the code behind for the event forwarding
        /// </summary>
        public void OnAppearing()
        {
            SelectedNote = null;

            List<string> files = IOHelpers.EnumerateAllFiles();
            List<Note> _notes = new List<Note>();

            Notes?.Clear();

            foreach (var fileName in files)
            {
                string noteData = IOHelpers.ReadAllFileText(fileName);

                string[] noteParts = noteData.Split(':');

                string title = noteParts[0];
                string text = noteParts[1];
                string color = noteParts[2];

                Notes.Add(new Note
                {
                    Filename = fileName,
                    Title = title,
                    Text = text,
                    Date = IOHelpers.GetNoteDate(fileName),
                    Color = Color.FromHex(color)
                });
            }

            //Switch to determine how to display the list of notes
            switch (AppSettings.OrderByOption)
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
                case "Title - Discending":
                    Notes = new ObservableCollection<Note>(Notes.OrderByDescending(n => n.Title));
                    break;
                default:
                    Notes = new ObservableCollection<Note>(Notes.OrderBy(n => n.Date));
                    break;
            }
            
            //Notes = new ObservableCollection<Note>(Notes.OrderByDescending(n => n.Date));

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

        //.OrderByDescending(x => x.Date) as ObservableCollection<Note>;

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
        /// Command for lauching a new note
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
        /// <returns></returns>
        private async Task NewNoteAsync()
        {
            await NavigationHelpers.PushPageAsync(new NewNotePage { });
        }

        /// <summary>
        /// The async task to display the currently selected note
        /// </summary>
        /// <returns></returns>
        private async Task LoadNoteAsync()
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
        /// <returns></returns>
        private async Task LoadSettingsAsync()
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
        /// Shows the correct button for editing or creating a note
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

        #endregion Private methods
    }
}