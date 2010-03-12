//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2008 4:00:47 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Components
{


    /// <summary>
    /// NavigateEventArgs
    /// </summary>
    public class NavigateEventArgs: EventArgs
    {
        #region Private Variables

        private string _path;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NavigateEventArgs
        /// </summary>
        public NavigateEventArgs(string path)
        {
            _path = path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string path that is being navigated to
        /// </summary>
        public string Path
        {
            get { return _path; }
            protected set { _path = value; }
        }

        #endregion



    }
}
