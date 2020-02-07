using Notes.Models;
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
        public EditNotePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event for the save button
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;

            string saveData = note.Title + ':' + note.Text;

            //Check the title isn't empty
            if (note.Title == null)
            {
                await DisplayAlert("Enter a title", "Title is required", "OK");
                return;
            }

            // Update the note file
            File.WriteAllText(note.Filename, saveData);

            //Closes the note that was being edited
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Event for the delete button
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;

            string result = await DisplayPromptAsync("Validation", "To delete this note, enter the title");

            //Checking if the cancel button was pressed on the prompt
            if (result == null)
            {
                return;
            }

            //Checking if the input on the prompt was correct
            if (result != note.Title)
            {
                //Dont delete the note
                await DisplayAlert("Incorrect", "Your note has not been deleted", "OK");
                return;
            }
            else
            {
                //Delete the note
                await DisplayAlert("Deleted", "Your note has been deleted", "OK");
                //Check if the note exists
                if (File.Exists(note.Filename))
                {
                    //Delete it if it exists
                    File.Delete(note.Filename);
                }
                else
                {
                    await DisplayAlert("Error", "There was an error", "OK");
                }
                //Close the note page that you have just deleted
                await Navigation.PopAsync();
            }
        }
    }
}