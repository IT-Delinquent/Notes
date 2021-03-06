﻿using System.Collections;

namespace Notes.Settings
{
    /// <summary>
    /// Contains properties that change the apps behaviour
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Holds the value by which the notes list will be ordered by
        /// </summary>
        private static string _orderByOption = "Date - Ascending";

        /// <summary>
        /// A list of all the possible sorting options
        /// </summary>
        public static readonly string[] OrderOptions = new string[]
        {
            "Date - Ascending",
            "Date - Descending",
            "Title - Ascending",
            "Title - Descending",
            "Color"
        };

        /// <summary>
        /// Accessor and modifier for the notes list order setting
        /// </summary>
        public static string OrderByOption
        {
            get { return _orderByOption; }
            set
            {
                _orderByOption = value;
            }
        }

        /// <summary>
        /// Colors for the notes
        /// </summary>
        public static readonly Hashtable NoteColors = new Hashtable()
        {
            { "None", "#41464c" },
            { "Red", "#F44336" },
            { "Purple", "#9C27B0" },
            { "Blue", "#2196F3" },
            { "Green", "#4CAF50" },
            { "Yellow",  "#FFC107" }
        };

        /// <summary>
        /// Holds whether the user is required to pass verification before deleting a note
        /// </summary>
        private static bool _requireDeleteCheck = true;

        /// <summary>
        /// Accessor and modifier for if deletion requires verification
        /// </summary>
        public static bool RequireDeleteCheck
        {
            get { return _requireDeleteCheck; }
            set 
            { 
                _requireDeleteCheck = value; 
            }
        }

    }
}