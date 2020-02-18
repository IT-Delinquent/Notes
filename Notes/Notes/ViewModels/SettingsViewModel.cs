using Notes.Helpers;
using Notes.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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

        public SettingsViewModel()
        {
            foreach (string i in AppSettings.OrderOptions)
            {
                SortOptions.Add(i);
            }

            DeleteAllNotesCommand = new Command(async () => await DeleteAllNotes());
        }

        private ObservableCollection<string> _sortOptions = new ObservableCollection<string>();

        public ObservableCollection<string> SortOptions
        {
            get { return _sortOptions; }
            set 
            { 
                _sortOptions = value;
                OnPropertyChanged();
            }
        }

        private string _sortOptionsSelectedItem = AppSettings.OrderByOption;

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

            await Task.Run( () => IOHelpers.DeleteAllNotes());

            await DisplayPopupHelpers
                .ShowOKDialogAsync("Deleted",
                "All your notes have now been deleted");
        }

        public ICommand DeleteAllNotesCommand { get; }

    }
}