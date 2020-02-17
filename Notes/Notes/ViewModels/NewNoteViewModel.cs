using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;

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
        }

        #endregion

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

        #endregion

        #region Private backing fields

        /// <summary>
        /// Holds the title value
        /// </summary>
        private string _title = "";

        /// <summary>
        /// Holds the text value
        /// </summary>
        private string _text;

        #endregion

        #region Public fields

        /// <summary>
        /// Accessor and modifier for the title
        /// 
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
        /// Public save command
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Public cancel command
        /// </summary>
        public ICommand CancelCommand { get; }

        #endregion

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
            await Application.Current.MainPage.Navigation.PopAsync();
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
                await Application
                    .Current
                    .MainPage
                    .DisplayAlert("Invalid Title", "Title cannot include special character ':'", "OK");
                return;
            }

            string noteData = Title + ':' + Text;
            string fileName = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");

            File.WriteAllText(fileName, noteData);

            await Application
                .Current
                .MainPage
                .Navigation
                .PopAsync();

        }

        #endregion
    }
}
