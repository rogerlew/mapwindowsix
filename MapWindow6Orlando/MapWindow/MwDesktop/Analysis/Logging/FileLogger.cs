//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MapWindow.Plugins;
namespace MapWindow.Analysis.Logging
{
    class FileLogger : Object, ILogger
    {
        
        private string _debugFile;
        private int _key;

        /// <summary>
        /// If this is something other than null, then the debug messages will be appended to this file.
        /// </summary>
        public virtual string DebugFile
        {
            get{ return _debugFile; }
            set { _debugFile = value; }
        }

        #region ILogHandler Members

        /// <summary>
        /// An exception was thrown, so this will post the stack trace and message to debug
        /// </summary>
        /// <param name="ex"></param>
        public virtual void Exception(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// This handles the situation where a public method has been entered
        /// </summary>
        /// <param name="MethodName">The method name</param>
        /// <param name="Parameters">The list of parameters</param>
        public virtual void PublicMethodEntered(string MethodName, List<string> Parameters)
        {
            string message = "Entering: " + MethodName + "(\n";
            
          
            for (int I = 0; I < Parameters.Count; I++)
            {
                message += "     " + Parameters[I] + "\n";
            }
            message += ") at " + DateTime.Now.ToShortDateString() + ": " + DateTime.Now.ToShortTimeString();
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }

        /// <summary>
        /// This handles the situation where a public method has been left
        /// </summary>
        /// <param name="MethodName">The method name of the function being left</param>
        public virtual void PublicMethodLeft(string MethodName)
        {
            string message = "Leaving: " + MethodName;
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }

        /// <summary>
        /// Handles the situation where a status message has been posted
        /// </summary>
        /// <param name="Message">The status message text</param>
        public virtual void Status(string Message)
        {
            string message = "Status: " + Message;
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }

        /// <summary>
        /// This isn't really used because this saves data to a file
        /// </summary>
        /// <param name="baseMessage"></param>
        /// <param name="percent"></param>
        /// <param name="message"></param>
        public virtual void Progress(string baseMessage, int percent, string message)
        {
            // We don't actually want to save progress messages to the file
        }


        /// <summary>
        /// Handles the situation where a simple messagebox where only a message was specified
        /// was shown to the user.  It also shows the result that the user pressed.
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="result">The boolean result</param>
        public virtual void MessageBoxShown(string MessageText, System.Windows.Forms.DialogResult result)
        {
            
            // This can be fleshed out later
            string message = "Messagebox Shown: " + MessageText + " and the user chose: " + result;
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }
        /// <summary>
        /// This method allows the logger to recieve information about input boxes that were shown
        /// as well as the values enterred into them and the result.
        /// </summary>
        /// <param name="MessageText">The string message that appeared on the InputBox</param>
        /// <param name="result">The ystem.Windows.Forms.DialogResult describing if the value was cancelled </param>
        /// <param name="value">The string containing the value entered.</param>
        public virtual void InputBoxShown(string MessageText, System.Windows.Forms.DialogResult result, string value)
        {
            // This can be fleshed out later
            string message = "InputBox Shown: " + MessageText + " and the user entered: " + value + " and the result was " + result;
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }

        }

        #endregion

        // I'm not currently using this code, but it is high teck so I left it here for possible future thoughts.
        private static string MethodCallingLogger()
        {
            try
            {
                StackTrace myStackTrace = new StackTrace(true);
                System.Reflection.Module myModule = myStackTrace.GetFrame(0).GetMethod().Module;
                int iFrame = 2;
                StackFrame lFrame = myStackTrace.GetFrame(iFrame);
                try
                {
                    while(iFrame < myStackTrace.FrameCount && lFrame.GetMethod().Module.Equals(myModule))
                    {
                        iFrame += 1;
                        lFrame = myStackTrace.GetFrame(iFrame);
                    }
                }
                catch 
                {
                    // could not go farther up the stack
                }
                string lFrameFilename = lFrame.GetFileName();
                if(lFrameFilename != null)
                {
                    return System.IO.Path.GetFileName(lFrameFilename) + ": " + lFrame.GetMethod().Name;
                }
                return lFrame.GetMethod().Name;
            }
            catch
            {
                return null;
            }
        }

       


       
        /// <summary>
        /// Provides a description of this Logger
        /// </summary>
        public string Description
        {
            get { return "This is a default file logger in MapWindow illustrating how to implement the interface for logging."; }
        }

        /// <summary>
        /// This allows us to retrieve this logger from the Manager when we wish to remove it.
        /// </summary>
        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

      
    }
}
