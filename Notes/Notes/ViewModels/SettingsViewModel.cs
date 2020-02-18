using Notes.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

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



    }
}