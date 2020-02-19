using Notes.Helpers;
using Notes.Models;
using Notes.Settings;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    /// <summary>
    /// The class for the edit note view
    /// </summary>
    public class EditNoteViewModel : INotifyPropertyChanged
    {
        #region Public methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        /// <param name="note"></param>
        public EditNoteViewModel(Note note)
        {
            CurrentNote = note;

            Title = CurrentNote.Title;
            Text = CurrentNote.Text;
            Date = CurrentNote.Date;
            Color = CurrentNote.Color;

            ColorCommand = new Command<string>((x) => ColorChange(x));
        }

        #endregion Public methods

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

        #region Private backing fields

        /// <summary>
        /// The private note object that holds the currently selected note
        /// </summary>
        private Note _currentNote = new Note();

        /// <summary>
        /// Holds the title of the note
        /// </summary>
        private string _title;

        /// <summary>
        /// Holds the text of the note
        /// </summary>
        private string _text;

        /// <summary>
        /// Holds the date of the note
        /// </summary>
        private DateTime _date;

        /// <summary>
        /// Hods the color for the note
        /// </summary>
        private Color _color;

        /// <summary>
        /// Holds whether the user can save or delete the note
        /// Changes if the title is empty or note
        /// </summary>
        private bool canAction = true;

        /// <summary>
        /// Holds the save command
        /// </summary>
        private Command _saveCommand;

        /// <summary>
        /// Holds the delete command
        /// </summary>
        private Command _deleteCommand;

        #endregion Private backing fields

        #region Public fields

        /// <summary>
        /// Accessor for the current note
        /// </summary>
        public Note CurrentNote
        {
            get { return _currentNote; }
            set
            {
                _currentNote = value;
            }
        }

        /// <summary>
        /// Accessor and modifier for the title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
                //Update the save and delete buttons
                canAction = CheckAction();
                SaveCommand.ChangeCanExecute();
                DeleteCommand.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Accessor and modifier for the text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the date
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor ad modifier for the notes color
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Public save command
        /// </summary>
        public Command SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new Command(
                    execute: async () => await SaveAsync(),
                    canExecute: () => canAction));
            }
        }

        /// <summary>
        /// Public delete command
        /// </summary>
        public Command DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new Command(
                    execute: async () => await DeleteAsync(),
                    canExecute: () => canAction));
            }
        }

        /// <summary>
        /// Command for changing the notes color
        /// </summary>
        public ICommand ColorCommand { get; }

        #endregion Public fields

        #region Private methods

        /// <summary>
        /// The save async method
        /// Saves/updates the note and closes the edit note page
        /// </summary>
        /// <returns>A task to save/update the note and close the edit note page</returns>
        private async Task SaveAsync()
        {
            if (Title.Contains(":"))
            {
                await DisplayPopupHelpers
                    .ShowOKDialogAsync("Invalid Title",
                    "Title cannot include special character ':'");
                return;
            }

            string noteData = Title + ':' + Text;
            string color = Color.ToHex();

            IOHelpers.SaveNoteData(CurrentNote.Filename, noteData, color);

            await NavigationHelpers.PopCurrentPageAsync();
        }

        /// <summary>
        /// The delete async method
        /// Deletes the current note following validation and closes the note page
        /// </summary>
        /// <returns>A task to delete the current note following validation and close the note page</returns>
        private async Task DeleteAsync()
        {
            string promptResult = await DisplayPopupHelpers
                .ShowPromptAsync("Validation",
                "To delete this note, enter it's title");

            if (promptResult == null)
            {
                return;
            }

            if (promptResult != Title)
            {
                await DisplayPopupHelpers
                    .ShowOKDialogAsync("Incorrect",
                    "Your note has not been deleted");

                return;
            }

            if (IOHelpers.NoteExists(CurrentNote.Filename))
            {
                IOHelpers.DeleteNote(CurrentNote.Filename);
                await DisplayPopupHelpers
                    .ShowOKDialogAsync("Deleted",
                    "Your note has been deleted");
            }
            else
            {
                await DisplayPopupHelpers
                    .ShowOKDialogAsync("Error",
                    "The file could not be found; your note has not been deleted");
            }

            await NavigationHelpers.PopCurrentPageAsync();
        }

        /// <summary>
        /// Checks the length of the title for the note
        /// </summary>
        /// <returns>false is the title is empty</returns>
        private bool CheckAction()
        {
            if (Title.Length <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Used to update/change the notes color
        /// </summary>
        /// <param name="color">The color to change to note to</param>
        private void ColorChange(string color)
        {
            Color = Color.FromHex((string)AppSettings.NoteColors[color]);
        }

        #endregion Private methods
    }
}