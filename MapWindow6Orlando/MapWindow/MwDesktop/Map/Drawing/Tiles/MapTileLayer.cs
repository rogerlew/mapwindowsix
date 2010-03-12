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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/27/2010 9:39:13 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Map
{


    /// <summary>
    /// MapTileLayer
    /// </summary>
    public class MapTileLayer : Layer, IMapLayer
    {

        /// <summary>
        /// Creates a new instance of MapTileLayer
        /// </summary>
        public MapTileLayer()
        {

        }

        #region IMapLayer Members

        public void DrawRegions(MapArgs args, List<IEnvelope> regions)
        {
            // To get information on the pixel resolution you can use
            Rectangle mapRegion = args.ImageRectangle;
            // Handle tile management here based on geographic extent and pixel resolution from mapRegion

        }

        #endregion
    }
}
