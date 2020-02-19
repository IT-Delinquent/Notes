using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notes.Helpers
{
    /// <summary>
    /// A class for displaying popup dialogs
    /// </summary>
    public static class DisplayPopupHelpers
    {
        /// <summary>
        /// Display an OK popup with a title and a message
        /// </summary>
        /// <param name="title">The title of the popup</param>
        /// <param name="message">The message inside the popup</param>
        /// <returns>A task to display an OK popup</returns>
        public static async Task ShowOKDialogAsync(string title, string message)
        {
            await Application
                .Current
                .MainPage
                .DisplayAlert(title, message, "OK");
        }

        /// <summary>
        /// Dispay a prompt to accept input from the user
        /// </summary>
        /// <param name="title">The title of the popup</param>
        /// <param name="message">The message inside the popup</param>
        /// <returns>A task to display the prompt</returns>
        public static async Task<string> ShowPromptAsync(string title, string message)
        {
            return await Application
                .Current
                .MainPage
                .DisplayPromptAsync(title, message);
        }
    }
}