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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 2:08:58 PM
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

namespace MapWindow.Components
{


    /// <summary>
    /// ItemTypes
    /// </summary>
    public enum ItemTypes
    {
        /// <summary>
        /// The specified element is a folder
        /// </summary>
        Folder,

        /// <summary>
        /// The specified element is an image
        /// </summary>
        Image,
        
        /// <summary>
        /// The specified element is a vector line file format
        /// </summary>
        Line,

        /// <summary>
        /// The specified element is a vector line point format
        /// </summary>
        Point,

        /// <summary>
        /// The specified element is a vector polygon format
        /// </summary>
        Polygon,

        /// <summary>
        /// The specified element is a raster format
        /// </summary>
        Raster,

        /// <summary>
        /// The specified element is a custom format, so the custom icon is used
        /// </summary>
        Custom

    }
}
