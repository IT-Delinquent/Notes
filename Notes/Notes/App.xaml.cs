using System;
using System.IO;
using Xamarin.Forms;

namespace Notes
{
    public partial class App : Application
    {
        /// <summary>
        /// Holds the folder path for the application
        /// </summary>
        public static string FolderPath { get; private set; }

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public App()
        {
            //Show the application
            InitializeComponent();

            //Set the folder path to the application path
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            //Load the NotesPage  - set NotesPage as the root of the application
            MainPage = new NavigationPage(new NotesPage());
        }
    }
}