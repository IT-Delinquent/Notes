using Notes.Helpers;
using Notes.Settings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    /// <summary>
    /// The viewmodel for the settings page
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {
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
        /// Holds the sort options
        /// </summary>
        private ObservableCollection<string> _sortOptions = new ObservableCollection<string>();

        /// <summary>
        /// Holds the select item for the sort options
        /// </summary>
        private string _sortOptionsSelectedItem = AppSettings.OrderByOption;

        /// <summary>
        /// Holds whether the user is required to pass verification
        /// when deleting a note
        /// </summary>
        private bool _requiresDeleteCheck = AppSettings.RequireDeleteCheck;

        #endregion Private backing fields

        #region Public methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public SettingsViewModel()
        {
            foreach (string i in AppSettings.OrderOptions)
            {
                SortOptions.Add(i);
            }

            DeleteAllNotesCommand = new Command(async () => await DeleteAllNotes());
        }

        #endregion Public methods

        #region Public fields

        /// <summary>
        /// Accessor and modifier for the sort options
        /// </summary>
        public ObservableCollection<string> SortOptions
        {
            get { return _sortOptions; }
            set
            {
                _sortOptions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the selected sort item
        /// </summary>
        public string SortOptionsSelectedItem
        {
            get { return _sortOptionsSelectedItem; }
            set
            {
                _sortOptionsSelectedItem = value;
                OnPropertyChanged();
                AppSettings.OrderByOption = SortOptionsSelectedItem;
            }
        }

        /// <summary>
        /// Accessor and modifier for whether the user is required to pass verfication
        /// when deleting a note
        /// </summary>
        public bool RequiresDeleteCheck
        {
            get { return _requiresDeleteCheck; }
            set 
            { 
                _requiresDeleteCheck = value;
                OnPropertyChanged();
                UpdateDeleteCheck();
            }
        }

        /// <summary>
        /// Command for deleting all the notes in the application
        /// </summary>
        public ICommand DeleteAllNotesCommand { get; }

        #endregion Public fields

        #region Private methods

        /// <summary>
        /// The async task for deleting all the notes in the application
        /// Also asks for validation from the user before deleting
        /// </summary>
        /// <returns>A task to delete all the notes</returns>
        private async Task DeleteAllNotes()
        {
            await DisplayPopupHelpers
                .ShowOKDialogAsync("Are you sure?",
                "You are about to delete ALL your notes. This CANNOT be reversed!");

            string promptResult = await DisplayPopupHelpers
                .ShowPromptAsync("Are you sure?",
                "Please enter 'DELETE ALL MY NOTES' into the box below to confirm");

            if (promptResult == null)
            {
                return;
            }

            if (promptResult != "DELETE ALL MY NOTES")
            {
                await DisplayPopupHelpers
                    .ShowOKDialogAsync("Incorrect",
                    "Your notes have NOT been deleted");

                return;
            }

            await Task.Run(() => IOHelpers.DeleteAllNotes());

            await DisplayPopupHelpers
                .ShowOKDialogAsync("Deleted",
                "All your notes have now been deleted");
        }

        /// <summary>
        /// Updates the delete check bool in AppSettings
        /// </summary>
        private void UpdateDeleteCheck()
        {
            AppSettings.RequireDeleteCheck = RequiresDeleteCheck;
        }

        #endregion Private methods
    }
}