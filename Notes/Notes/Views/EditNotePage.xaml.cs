using Notes.Models;
using Notes.ViewModels;
using System;
using System.IO;
using Xamarin.Forms;

namespace Notes
{
    /// <summary>
    /// The class for editing a note
    /// </summary>
    public partial class EditNotePage : ContentPage
    {
        /// <summary>
        /// The constructor for the class
        /// </summary>
        public EditNotePage(Note note)
        {
            InitializeComponent();
            BindingContext = new EditNoteViewModel(note);
        }
    }
}