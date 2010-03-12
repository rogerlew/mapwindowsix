//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Components
{
    /// <summary>
    /// Controls the behavior associated with the mouse
    /// </summary>
    public enum MouseModes
    {
        /// <summary>
        /// No special functionality occurs when clicking or moving the mouse
        /// </summary>
        None = 0,
        /// <summary>
        /// Left click zooms in, Right click zooms out
        /// </summary>
        Zoom = 1,        
        /// <summary>
        /// In Pan mode, you can drag and drop the map to move the extents
        /// </summary>
        Pan = 2,
        /// <summary>
        /// Establishes a selection from the currently selected layer
        /// based on the position of a click or a drag-rectangle
        /// </summary>
        Select = 3,
        /// <summary>
        /// Uses the wheel to zoom, both buttons pressed will pan, the
        /// left button alone will select.  If a context menu is set using
        /// the ContextMenu property, the right button will raise that
        /// context menu.
        /// </summary>
        Normal = 4
    }

    /// <summary>
    /// Controls how the map behaves during a resize
    /// </summary>
    public enum ResizeModes
    {
        /// <summary>
        /// In stretch mode, the image displayed in the map is resized to fit the new dimensions as you resize, 
        /// but the aspect ratio is not preserved.  Not recommended for mapping applications.
        /// </summary>
        Stretch = 2,

        /// <summary>
        /// In reveal mode, new regions are hidden (or revealed) during a resize, rather than stretching the image
        /// </summary>
        Reveal = 1, 

        /// <summary>
        /// In normal mode)
        /// if the control is wider than it is tall:
        ///    The map is stretched to fit the same extents vertically, and reveals or hides information horizontally
        /// otherwise:
        ///    The map is stretched to fit the same extents horizontally, and reveals or hides information vertically
        /// </summary>
        Normal = 0

    }

    /// <summary>
    /// Controls how the map behaves while zooming
    /// </summary>
    public enum ZoomModes
    {
        /// <summary>
        /// Draws a stand-in of the stretched back buffer until the image can be redrawn.
        /// This has the fastest update, but doesn't look as good during zooming.
        /// </summary>
        Stretch = 2,

        /// <summary>
        /// Recalculates the drawing image on the fly, but with noticeable lag
        /// </summary>
        Redraw = 1,

        /// <summary>
        /// Populates a ZoomBuffer with stored images until enough images can be stored
        /// that rapid, clean zooming is possible.  This method consumes more memory,
        /// and may not be capable of drawing buffers fast enough.  If a buffer doesn't 
        /// exist yet, the Redraw method will be used.
        /// </summary>
        Buffer = 0
    }
}