using Notes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Notes
{
    public class NotesPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Public event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to fire the changed event handler
        /// </summary>
        /// <param name="name"></param>
        void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private ObservableCollection<Note> notes;

        public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public void NotesPageAppearing()
        {

        }

    }
}
