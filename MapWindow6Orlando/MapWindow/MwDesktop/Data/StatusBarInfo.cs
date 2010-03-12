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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:56:48 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// StatusBarInfo
    /// </summary>
    public class StatusBarInfo
    {
        #region Private Variables

        bool _getFromProjection;
        string _alternate;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatusBarInfo
        /// </summary>
        public StatusBarInfo()
        {

        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the alternate string to show in the event that GetFromProjection is false.
        /// </summary>
        public string Alternate
        {
            get { return _alternate; }
            set { _alternate = value; }
        }


        /// <summary>
        /// Gets or sets a boolean indicating whether or not status bar coordinates should display the unites derived from the projection string.
        /// </summary>
        public bool GetFromProjection
        {
            get { return _getFromProjection; }
            set { _getFromProjection = value; }
        }

      
        #endregion



    }
}
