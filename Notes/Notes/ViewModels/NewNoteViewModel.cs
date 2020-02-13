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
    public class NewNoteViewModel : INotifyPropertyChanged
    {
        public NewNoteViewModel()
        {
            CancelCommand = new Command(async () => await CancelAsync());
            SaveCommand = new Command(
                execute: async () => await SaveAsync(),
                canExecute: () => CanExecuteCommand());
        }

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


        private string _title = "";

        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                OnPropertyChanged();
                CanExecuteCommand();
                
            }
        }

        private bool CanExecuteCommand()
        {
            return Title.Length > 0;
        }


        private string _text;

        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        private async Task CancelAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task SaveAsync()
        {
            if (Title.Contains(":"))
            {
                await Application.Current.MainPage
                    .DisplayAlert("Invalid Title", "Title cannot include special character ':'", "OK");
                return;
            }

            string noteData = Title + ':' + Text;
            string fileName = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");

            File.WriteAllText(fileName, noteData);

            await Application.Current.MainPage.Navigation.PopAsync();

        }
    }
}
