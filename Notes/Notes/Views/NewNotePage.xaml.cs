using Notes.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Notes.ViewModels;

namespace Notes
{
    /// <summary>
    /// The class for creating a new note
    /// </summary>
    public partial class NewNotePage : ContentPage
    {
        /// <summary>
        /// The constructor for the class
        /// </summary>
        public NewNotePage()
        {
            InitializeComponent();
            BindingContext = new NewNoteViewModel();
        }
    }
}