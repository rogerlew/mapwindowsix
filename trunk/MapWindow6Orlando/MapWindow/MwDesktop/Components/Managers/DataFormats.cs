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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 3:17:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Components
{


    /// <summary>
    /// DataFormats
    /// </summary>
    public enum DataFormats
    {
        /// <summary>
        /// Lines, Points and Polygons make up standard static vector formats.
        /// These are drawn dynamically based on the symbolizer.
        /// </summary>
        Vector,
        /// <summary>
        /// Rasters are grids of integers, doubles, floats, or other numeric value types.
        /// These can be symbolized and represented as images, but not drawn directly.
        /// </summary>
        Raster,
        /// <summary>
        /// Images specifically have pixels coordinates that store a color.
        /// These are drawn directly.
        /// </summary>
        Image,
        /// <summary>
        /// This represents an extended format that does not have a formal definition in MapWindow.
        /// </summary>
        Custom



    }
}
