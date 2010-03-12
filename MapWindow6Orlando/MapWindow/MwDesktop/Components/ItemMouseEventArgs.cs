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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2008 1:56:51 PM
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
using System.Windows.Forms;

namespace MapWindow.Components
{


    /// <summary>
    /// ItemMouseEventArgs
    /// </summary>
    public class ItemMouseEventArgs : MouseEventArgs
    {
        #region Private Variables

        LegendBox _box;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an ItemMouseEventArgs
        /// </summary>
        /// <param name="inButton">The Mouse Buttons</param>
        /// <param name="inClicks">The number of clicks</param>
        /// <param name="inX">The X coordinate</param>
        /// <param name="inY">The Y coordinate</param>
        /// <param name="inDelta">The delta of the mouse wheel</param>
        /// <param name="inItemBox">A LegendBox for comparision</param>
        public ItemMouseEventArgs(MouseButtons inButton, int inClicks, int inX, int inY, int inDelta, LegendBox inItemBox):base(inButton, inClicks, inX, inY, inDelta)
        {
            _box = inItemBox;
        }

        /// <summary>
        /// Creates a new instance of ItemMouseEventArgs from an existing MouseEventArgs.
        /// </summary>
        /// <param name="args">The existing arguments</param>
        /// <param name="inItemBox">A LegendBox for comparison</param>
        public ItemMouseEventArgs(MouseEventArgs args, LegendBox inItemBox):base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
        {

            _box = inItemBox;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the item that received the mouse down, plus the various rectangular extents encoded in the various boxes.
        /// </summary>
        public LegendBox ItemBox
        {
            get { return _box; }
            protected set { _box = value; }
        }


        #endregion



    }
}
