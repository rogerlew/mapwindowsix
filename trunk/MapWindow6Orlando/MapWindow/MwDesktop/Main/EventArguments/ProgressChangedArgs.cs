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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 11:55:46 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Main
{


    /// <summary>
    /// ProgressChangedArgs
    /// </summary>
    public class ProgressChangedArgs : ProgressChangedEventArgs
    {
        #region Private Variables
        
        private string _key;
        private string _message;

       

        #endregion

        #region Constructors


        /// <summary>
        /// Creates a new instance of ProgressChangedArgs
        /// </summary>
        /// <param name="progressPercentage">The integer percentage of completion from 0 to 100</param>
        /// <param name="userState">An object identifying the process</param>
        public ProgressChangedArgs(int progressPercentage, object userState)
            : base(progressPercentage, userState)
        {
           
        }

        /// <summary>
        /// Creates a new instance of ProgressChangedArgs
        /// </summary>
        /// <param name="progressPercentage">The integer percentage of completion from 0 to 100</param>
        /// <param name="userState">An object identifying the process</param>
        /// <param name="message">A string describing the full message that should appear.</param>
        public ProgressChangedArgs(int progressPercentage, object userState, string message):base(progressPercentage, userState)
        {
            _message = message;
        }

        /// <summary>
        /// Creates a new instance of ProgressChangedArgs
        /// </summary>
        /// <param name="key">A string key that indicates how this progress event should be handled</param>
        /// <param name="progressPercentage">The integer percentage of completion from 0 to 100</param>
        /// <param name="userState">An object identifying the process</param>
        /// <param name="message">The message to display.</param>
        public ProgressChangedArgs(int progressPercentage, object userState, string message, string key) : base(progressPercentage, userState)
        {
            _message = message;
            _key = key;
        }

        #endregion

        #region Methods
        

        #endregion

        #region Properties

        /// <summary>
        /// A string key that indicates how this progress event should be handled
        /// </summary>
        public string Key
        {
            get { return _key; }
            protected set { _key = value; }
        }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string Message
        {
            get { return _message; }
            protected set { _message = value; }
        }

        #endregion



    }
}
