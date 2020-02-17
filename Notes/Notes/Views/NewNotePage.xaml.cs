using Notes.ViewModels;
using Xamarin.Forms;

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