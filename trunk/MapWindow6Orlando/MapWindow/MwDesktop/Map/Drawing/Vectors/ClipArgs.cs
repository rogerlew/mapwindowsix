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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/21/2009 11:04:37 AM
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

namespace MapWindow.Map
{


    /// <summary>
    /// ClipArgs
    /// </summary>
    public class ClipArgs : EventArgs
    {
        #region Private Variables

        private List<Rectangle> _clipRegions;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ClipArgs
        /// </summary>
        public ClipArgs(List<Rectangle> clipRectangles)
        {
            _clipRegions = clipRectangles;
        }
        /// <summary>
        /// Creates a ClipArg from a single rectangle instead of a list of rectangles
        /// </summary>
        /// <param name="clipRectangle">The clip rectangle</param>
        public ClipArgs(Rectangle clipRectangle)
        {
            _clipRegions = new List<Rectangle>() { clipRectangle };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ClipRectangle for this event.
        /// </summary>
        public List<Rectangle> ClipRectangles
        {
            get { return _clipRegions; }
            protected set { _clipRegions = value; }
        }

        #endregion



    }
}
