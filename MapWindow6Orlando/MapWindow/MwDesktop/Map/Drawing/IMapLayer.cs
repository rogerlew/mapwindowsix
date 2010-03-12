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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 4:44:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Geometries;
namespace MapWindow.Map
{


    /// <summary>
    /// Layer
    /// </summary>
    public interface IMapLayer : ILayer
    {
        #region Events

        ///// <summary>
        ///// Fires an event that indicates to the parent map-frame that it should first
        ///// redraw the specified clip
        ///// </summary>
        //event EventHandler<ClipArgs> BufferChanged;

        #endregion


        #region Methods

        ///// <summary>
        ///// Call StartDrawing before using this.
        ///// </summary>
        ///// <param name="rectangles">The rectangular region in pixels to clear.</param>
        ///// <param name= "color">The color to use when clearing.  Specifying transparent
        ///// will replace content with transparent pixels.</param>
        //void Clear(List<Rectangle> rectangles, Color color);

        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        void DrawRegions(MapArgs args, List<IEnvelope> regions);
        

        ///// <summary>
        ///// Indicates that the drawing process has been finalized and swaps the back buffer
        ///// to the front buffer.
        ///// </summary>
        //void FinishDrawing();

        ///// <summary>
        ///// This begins the process of drawing features in the given geographic regions
        ///// to the buffer, where the transform is specified by the GeoArgs.
        ///// </summary>
        //void Initialize(MapArgs args, List<IEnvelope> regions);

        ///// <summary>
        ///// Copies any current content to the back buffer so that drawing should occur on the
        ///// back buffer (instead of the fore-buffer).  Calling draw methods without
        ///// calling this may cause exceptions.
        ///// </summary>
        //void StartDrawing(bool preserve);

        

       

        #endregion

        

        ///// <summary>
        ///// Gets the back buffer
        ///// </summary>
        //Image BackBuffer
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Gets the buffered stencil.
        ///// </summary>
        //Image Buffer
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Gets an envelope representing the geographic extent stored by
        ///// the current layer in its drawing buffer.
        ///// </summary>
        //IEnvelope BufferEnvelope
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Gets or sets the size in pixels for the buffer.  Initialize will set this
        ///// automatically.
        ///// </summary>
        //Rectangle BufferRectangle
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Expose setter so that partial initialization can work
        ///// </summary>
        //new bool IsInitialized
        //{
        //    get;
        //    set;
        //}
       
    }




}
