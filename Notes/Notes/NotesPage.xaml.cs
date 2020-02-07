using Notes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Notes
{
    /// <summary>
    /// The class for viewing all the notes
    /// </summary>
    public partial class NotesPage : ContentPage
    {
        /// <summary>
        /// Public constructor for the class
        /// </summary>
        public NotesPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Shows the correct view (either the listview or the emptyview)
        /// </summary>
        private void ShowCorrectView()
        {
            if (Notes.Count <= 0)
            {
                //Show empty view
                listView.IsVisible = false;
                emptyview.IsVisible = true;
            }
            else
            {
                //Show notes view
                listView.IsVisible = true;
                emptyview.IsVisible = false;
            }
        }

        /// <summary>
        /// Runs everytime the page is displayed
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Look for all the note files in the application path
            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");

            //Clear the notes for overriding
            Notes.Clear();

            foreach (var filename in files)
            {
                //Collect the data from the file
                string noteData = File.ReadAllText(filename);

                //Get the location of the Title/Text seperator
                int titleLocation = noteData.IndexOf(':');

                //Add a new note to the notes list
                Notes.Add(new Note
                {
                    Filename = filename,
                    //Build the title from 0 to the title seperator location
                    Title = noteData.Substring(0, titleLocation),
                    //Build the text from the title seperator plus 1 (take out the seperator)
                    Text = noteData.Substring(noteData.IndexOf(':') + 1),
                    Date = File.GetCreationTime(filename)
                });
            }

            ShowCorrectView();

            //Only update the list if its visible
            if (listView.IsVisible)
            {
                //Assign a source for the listview - ordered by date (newest first)
                listView.ItemsSource = null;
                listView.ItemsSource = (from s in Notes orderby s.Date descending select s).ToList();
            }
        }

        /// <summary>
        /// Event for the new note button
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            //Push a new NewNotePage page
            await Navigation.PushAsync(new NewNotePage
            {
                BindingContext = new Note()
            });
        }

        /// <summary>
        /// Event for when a note is clicked from the list
        /// Opens a EditNotePage with binding to the selecteditem
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Checks that an item is actually selected
            if (e.SelectedItem != null)
            {
                //Pushes a NoteEditPage to the screen using the binding
                await Navigation.PushAsync(new EditNotePage
                {
                    BindingContext = e.SelectedItem as Note
                });
            }
        }
    }
}