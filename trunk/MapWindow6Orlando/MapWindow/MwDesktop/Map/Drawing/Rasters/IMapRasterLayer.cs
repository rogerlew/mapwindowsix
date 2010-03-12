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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 2:46:23 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Map
{


    /// <summary>
    /// GeoImageLayer
    /// </summary>
    public interface IMapRasterLayer : IRasterLayer, IMapLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        event EventHandler<ClipArgs> BufferChanged;

        #endregion

       
      

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
        /// will replace content with transparent pixels.</param>
        void Clear(List<Rectangle> rectangles, Color color);



        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        void FinishDrawing();




        /// <summary>
        /// Copies any current content to the back buffer so that drawing should occur on the
        /// back buffer (instead of the fore-buffer).  Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer 
        /// where drawing will be taking place.</param>
        void StartDrawing(bool preserve);
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        Image BackBuffer
        {
            get; set;
        }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        Image Buffer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        IEnvelope BufferEnvelope
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        Rectangle BufferRectangle
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets whether the image layer is initialized
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new bool IsInitialized
        {
            get; set;
        }

      

        #endregion

       
    }
}
