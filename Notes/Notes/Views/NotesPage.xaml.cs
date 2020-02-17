using Notes.Helpers;
using Notes.ViewModels;
using System;
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
            BindingContext = new NotesPageViewModel();
        }

        /// <summary>
        /// Allows the OnAppearing event to be passed to the ViewModel
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            IPageAppearingEvent onAppearingLifeCycleEvents = (IPageAppearingEvent)BindingContext;
            if (onAppearingLifeCycleEvents != null)
            {
                var lifeCycleHandler = onAppearingLifeCycleEvents;

                base.Appearing += (object sender, EventArgs e) => lifeCycleHandler.OnAppearing();
            }
        }
    }
}