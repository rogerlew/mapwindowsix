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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/5/2008 2:38:20 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Windows.Forms;
using MapWindow.Geometries;
namespace MapWindow.Map
{


    /// <summary>
    /// MouseArgs
    /// </summary>
    public class GeoMouseArgs: MouseEventArgs
    {
        #region Private Variables

        Coordinate _geographicLocation;
        IMap _map;
        private bool _handled;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MouseArgs
        /// </summary>
        /// <param name="e"></param>
        /// <param name="inMap"></param>
        public GeoMouseArgs(MouseEventArgs e, IMap inMap)
            :base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            if (inMap != null)
            {
                _geographicLocation = inMap.PixelToProj(e.Location);
                _map = inMap;
            }
        }

        #endregion

        #region Methods

        

        #endregion

        #region Properties

        /// <summary>
        /// Gets the position of the Mouse Event in geographic coordinates
        /// </summary>
        public Coordinate GeographicLocation
        {
            get { return _geographicLocation; }
            protected set { _geographicLocation = value; }
        }

        /// <summary>
        /// Gets a simple interface for the map where these events were generated
        /// </summary>
        public IMap Map
        {
            get { return _map; }
            protected set { _map = value; }
        }

        /// <summary>
        /// Gets or sets a handled.  If this is set to true, then the mouse event is considered to
        /// be handled and will not be passed to any other functions in the stack.
        /// </summary>
        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }

        #endregion



    }
}
