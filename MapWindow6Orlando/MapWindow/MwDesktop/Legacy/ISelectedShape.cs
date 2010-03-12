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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:42:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Legacy
{



    /// <summary>
    /// The <c>SelectedShape</c> interface is used to access information about a shape that is selected in the MapWindow.
    /// </summary>
    public interface ISelectedShape
    {
        /// <summary>
        /// Initializes all information in the <c>SelectedShape</c> object then highlights the shape on the map.
        /// </summary>
        /// <param name="ShapeIndex">Index of the shape in the shapefile.</param>
        /// <param name="SelectColor">Color to use when highlighting the shape.</param>
        void Add(int ShapeIndex, Color SelectColor);

        /// <summary>
        /// Returns the extents of this selected shape.
        /// </summary>
        Envelope Extents { get; }

        /// <summary>
        /// Returns the shape index of this selected shape.
        /// </summary>
        int ShapeIndex { get; }
    }

}
