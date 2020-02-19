using Notes.Helpers;
using Notes.Settings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    /// <summary>
    /// The class for the new note view
    /// </summary>
    public class NewNoteViewModel : INotifyPropertyChanged
    {
        #region Public methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public NewNoteViewModel()
        {
            CancelCommand = new Command(async () => await CancelAsync());
            SaveCommand = new Command(async () => await SaveAsync(), CanSaveNote);
            ColorCommand = new Command<string>((x) => ColorChange(x));
            //Start the note off with the default color
            ColorCommand.Execute("None");
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
        /// Holds the title value
        /// </summary>
        private string _title = "";

        /// <summary>
        /// Holds the text value
        /// </summary>
        private string _text;

        /// <summary>
        /// Hods the color for the note
        /// </summary>
        private Color _color;

        #endregion Private backing fields

        #region Public fields

        /// <summary>
        /// Accessor and modifier for the title
        /// Also runs ChangeCanExecute for the save command
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
                ((Command)SaveCommand).ChangeCanExecute();
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
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Public cancel command
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Public color command
        /// </summary>
        public ICommand ColorCommand { get; }

        #endregion Public fields

        #region Private methods

        /// <summary>
        /// Whether the save command can be executed or not
        /// </summary>
        /// <returns>A boolean</returns>
        private bool CanSaveNote()
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
        /// The cancel async method
        ///
        /// Closes the new note page
        /// </summary>
        /// <returns>A task to close the new note page</returns>
        private async Task CancelAsync()
        {
            await NavigationHelpers.PopCurrentPageAsync();
        }

        /// <summary>
        /// The save async method
        ///
        /// Saves the note and closes the new note window
        /// </summary>
        /// <returns>A task to save the note and close the new note page</returns>
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
            string fileName = IOHelpers.GetNewFileName();
            string color = Color.ToHex();

            IOHelpers.SaveNoteData(fileName, noteData, color);

            await NavigationHelpers.PopCurrentPageAsync();
        }

        /// <summary>
        /// Used to set the color of the note
        /// </summary>
        /// <param name="color">The color to upda the note to</param>
        private void ColorChange(string color)
        {
            Color = Color.FromHex((string)AppSettings.NoteColors[color]);
        }

        #endregion Private methods
    }
}