using System;

namespace Notes.Models
{
    /// <summary>
    /// A class for holding information about a note
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The filename used to store the note
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The title of the note
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The text inside the note
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The creation time of the note
        /// </summary>
        public DateTime Date { get; set; }
    }
}