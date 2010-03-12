using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// An interface for sending progress messages.  Percent is an integer from 0 to 100.
    /// </summary>
    public interface IProgressHandler
    {

        /// <summary>
        /// Progress is the method that should receive a progress message.
        /// </summary>
        /// <param name="Key">The message string without any information about the status of completion.</param>
        /// <param name="Percent">An integer from 0 to 100 that indicates the condition for a status bar etc.</param>
        /// <param name="Message">A string containing both information on what the process is, as well as its completion status.</param>
        void Progress(string Key, int Percent, string Message);
    }
}
