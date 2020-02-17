namespace Notes.Helpers
{
    /// <summary>
    /// An interface used for when a contentpage is appearing
    /// </summary>
    public interface IPageAppearingEvent
    {
        /// <summary>
        /// The methods used for the appearing event
        /// </summary>
        void OnAppearing();
    }
}