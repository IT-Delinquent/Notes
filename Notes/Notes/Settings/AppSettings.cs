using System;
using System.Collections.Generic;
using System.Text;

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

		public static readonly string[] OrderOptions = new string[] 
		{
			"Date - Ascending",
			"Date - Descending", 
			"Title - Ascending",
			"Title - Descending"
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
		/// 
		/// Transparent, red, purple, blue, green, yellow
		/// </summary>
		public static readonly string[] NoteColors = new string[]
		{
			"#ffffff00",
			"#F44336",
			"#9C27B0",
			"#2196F3",
			"#4CAF50",
			"#FFC107"
		};


	}
}