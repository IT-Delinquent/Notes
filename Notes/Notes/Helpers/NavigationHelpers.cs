using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notes.Helpers
{
    /// <summary>
    /// A class containing helpers for the navigation of the app
    /// </summary>
    public static class NavigationHelpers
    {
        /// <summary>
        /// Pops the most recent page
        /// </summary>
        /// <returns>A task to pop the most recent page</returns>
        public static async Task PopCurrentPageAsync()
        {
            await Application
                .Current
                .MainPage
                .Navigation
                .PopAsync();
        }

        /// <summary>
        /// Pushes a new page to the front of the view
        /// </summary>
        /// <param name="page">The page to push</param>
        /// <returns>A task to push the page to the front of the view</returns>
        public static async Task PushPageAsync(Page page)
        {
            await Application
                .Current
                .MainPage
                .Navigation
                .PushAsync(page);
        }
    }
}