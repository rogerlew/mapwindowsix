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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/6/2008 11:17:32 AM
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
    /// DefaultTools
    /// </summary>
    public enum FunctionModes
    {
        /// <summary>
        /// Mousewheel still zooms the map, but left clicking brings items into the view.
        /// </summary>
        Info,
        /// <summary>
        /// Zooms into the map with the left mouse button and zooms out with the right mouse button. 
        /// </summary>
        Zoom,
        /// <summary>
        /// Zooms out by clicking the left mouse button.
        /// </summary>
        ZoomOut,
        /// <summary>
        /// Pans the map with the left mouse button, context with the right and zooms with the mouse wheel
        /// </summary>
        Pan,
        /// <summary>
        /// Selects shapes with the left mouse button, context with the right and zooms with the mouse wheel
        /// </summary>
        Select,
        /// <summary>
        /// Left button selects, moves or edits, right produces a context menu
        /// </summary>
        Label,
        /// <summary>
        /// Left clicking builds either a linestring or a polygon while a dialog shows distances.
        /// </summary>
        Measure,
        /// <summary>
        /// Disables all the tools
        /// </summary>
        None,
      
    }
}
