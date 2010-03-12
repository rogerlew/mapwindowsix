//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow.Drawing.PredefinedSymbols.SymbolizerTypes version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 10:20:48 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MapWindow.Drawing
{
    /// <summary>
    /// The available feature symbolizer types
    /// </summary>
    public enum SymbolizerTypes
    {
        /// <summary>
        /// The type is PointSymbolizer
        /// </summary>
        Point,
        /// <summary>
        /// The type is LineSymbolizer
        /// </summary>
        Line,
        /// <summary>
        /// The type is PolygonSymbolizer
        /// </summary>
        Polygon,
        /// <summary>
        /// The type is RasterSymbolizer
        /// </summary>
        Raster,
        /// <summary>
        /// The type of the symbolizer is unknown
        /// </summary>
        Unknown
    }
}
