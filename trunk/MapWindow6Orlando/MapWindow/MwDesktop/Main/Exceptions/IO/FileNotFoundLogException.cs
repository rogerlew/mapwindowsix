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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 4:47:08 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Components;
namespace MapWindow.Main
{


    /// <summary>
    /// FileNotFound
    /// </summary>
    public class FileNotFoundLogException : System.IO.FileNotFoundException, ILogException
    {
       
        #region Constructors

        /// <summary>
        /// Creates a new instance of FileNotFoundLogException, but does not set the message or fire the log event.
        /// </summary>
        public FileNotFoundLogException()
        {

        }

        /// <summary>
        /// Creates a new instance of FileNotFoundLogException, creates a message based on the filename, and
        /// calls the Log() method.
        /// </summary>
        public FileNotFoundLogException(string filename):base(ExceptionMessages.IO_FileNotFound_S.Replace("%S", filename))
        {
            Log();
        }


        #endregion

        #region Methods
        /// <summary>
        /// Actually logs the exception.  This happens after the message has been set in the constructors of the classes of this exception.
        /// </summary>
        public virtual void Log()
        {
            LogManager.DefaultLogManager.Exception(this);
        }
        #endregion

       



    }
}
