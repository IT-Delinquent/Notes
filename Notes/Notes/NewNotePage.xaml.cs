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

        /// <summary>
        /// Event for the save button
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="e">The arguments sent with the event</param>
        //private async void OnSaveButtonClicked(object sender, EventArgs e)
        //{
        //    Note note = (Note)BindingContext;

        //    string saveData = note.Title + ':' + note.Text;

        //    //Making sure that a title has been entered
        //    if (note.Title == null)
        //    {
        //        await DisplayAlert("Enter a title", "Title is required", "OK");
        //        return;
        //    }


        //    if (note.Title.Contains(":"))
        //    {
        //        await DisplayAlert("Invalid Title", "The title cannot contain ':'", "OK");
        //        return;
        //    }

        //    // Create a new file path and save the data
        //    var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
        //    File.WriteAllText(filename, saveData);

        //    //Close the new note after saving (might change this to leave the note visible and just save...)
        //    await Navigation.PopAsync();
        //}
    }
}