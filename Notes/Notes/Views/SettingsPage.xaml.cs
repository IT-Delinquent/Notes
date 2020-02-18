using Notes.ViewModels;
using Xamarin.Forms;

namespace Notes.Views
{
    /// <summary>
    /// Code behind for the settings page
    /// </summary>
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// The constructor for the class
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}