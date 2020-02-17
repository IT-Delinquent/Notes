using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Notes.Helpers
{
    /// <summary>
    /// A class for directory and IO related tasks
    /// </summary>
    public static class SaveAndLoadHelpers
    {
        /// <summary>
        /// Holds the default note file extension
        /// </summary>
        private const string _notesExtension = ".notes.txt";

        /// <summary>
        /// Used to enumerate all the note files in the app folder
        /// </summary>
        /// <returns>A string of all the note files</returns>
        public static List<string> EnumerateAllFiles()
        {
            return Directory.EnumerateFiles(App.FolderPath, '*' + _notesExtension).ToList();
        }

        /// <summary>
        /// Used to read all the text inside a note file
        /// </summary>
        /// <param name="file">The note file location</param>
        /// <returns>A string of the note file content</returns>
        public static string ReadAllFileText(string file)
        {
            return File.ReadAllText(file);
        }

        /// <summary>
        /// Gets the creation date of a note
        /// </summary>
        /// <param name="file">The note file location</param>
        /// <returns>The datetime object of a note file</returns>
        public static DateTime GetNoteDate(string file)
        {
            return File.GetCreationTime(file);
        }

        /// <summary>
        /// Created a new file location included a random name
        /// </summary>
        /// <returns>A new note file location string</returns>
        public static string GetNewFileName()
        {
            return Path.Combine(App.FolderPath, Path.GetRandomFileName() + _notesExtension);
        }

        /// <summary>
        /// Saves data to a note file
        /// </summary>
        /// <param name="file">The location of the note file</param>
        /// <param name="data">The data of the note</param>
        public static void SaveNoteData(string file, string data)
        {
            File.WriteAllText(file, data);
        }

        /// <summary>
        /// Checks if a note file can be found
        /// </summary>
        /// <param name="file">The location of a note file</param>
        /// <returns>True if the note file can be found</returns>
        public static bool NoteExists(string file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// Deletes a note file
        /// </summary>
        /// <param name="file">The location of the note file</param>
        public static void DeleteNote(string file)
        {
            File.Delete(file);
        }
    }
}