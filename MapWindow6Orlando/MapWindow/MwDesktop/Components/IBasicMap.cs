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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/7/2008 10:09:46 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.ComponentModel;
namespace MapWindow.Components
{


    /// <summary>
    /// IBasicMap
    /// </summary>
    public interface IBasicMap: IMapView, ISelectable
    {
        

        #region Methods

        /// <summary>
        /// Adds a new layer to the map using an open file dialog.
        /// </summary>
        ILayer AddLayer();

        /// <summary>
        /// Converts a point from client coordinates to screen coordinates.
        /// </summary>
        /// <param name="position">The client location.</param>
        /// <returns>A System.Drawing.Point in screen coordinates</returns>
        System.Drawing.Point PointToScreen(System.Drawing.Point position);
       
        /// <summary>
        /// Converst a point from screen coordinates to client coordinates
        /// </summary>
        /// <param name="position">The System.Drawing.Point representing the screen position</param>
        /// <returns>The System.Drawing.Point</returns>
        System.Drawing.Point PointToClient(System.Drawing.Point position);

        /// <summary>
        /// Invalidates the entire Map control, forcing it to redraw itself from the back buffer stencils.
        /// This is good for drawing on top of the map, or when a layer is visible or not.  If you need
        /// to change the colorscheme as well 
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Invalidates the specified clipRectangle so that only that small region needs
        /// to redraw itself.
        /// </summary>
        /// <param name="clipRectangle"></param>
        void Invalidate(Rectangle clipRectangle);

        [Obsolete("Use AddLayer")]
        void OpenLayer();
        
       
        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle"></param>
        void RefreshMap(Rectangle clipRectangle);


        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        void ZoomToMaxExtent();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounding rectangle representing this map in screen coordinates
        /// </summary>
        Rectangle Bounds
        {
            get;
        }

        /// <summary>
        /// Gets an image that has been buffered
        /// </summary>
        Image BufferedImage
        {
            get;
        }

        

        /// <summary>
        /// Gets the client rectangle of the map control
        /// </summary>
        Rectangle ClientRectangle
        {
            get;
        }

        [Obsolete("use FunctionMode")]
        FunctionModes CursorMode
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets which tool or combination of tools are enabled on the map.")]
        FunctionModes FunctionMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        IEnvelope Envelope
        {
            get;
        }

        /// <summary>
        /// Gets the height of the control
        /// </summary>
        int Height
        {
            get;
        }


        /// <summary>
        /// Gets the screen coordinates of the 
        /// </summary>
        int Left
        {
            get;
        }

        /// <summary>
        /// Gets the legend, if any, associated with this map control.
        /// </summary>
        ILegend Legend
        {
            get;
            set;
        }

        /// <summary>
        /// A MapFrame
        /// </summary>
        IFrame MapFrame
        {
            get;
        }


        /// <summary>
        /// Gets the screen coordinates of the top of this control
        /// </summary>
        int Top
        {
            get;
        }

       

        /// <summary>
        /// Gets the width of the control
        /// </summary>
        int Width
        {
            get;
        }


        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        void ClearLayers();

        /// <summary>
        /// returns a functional list of the ILayer members.  This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns></returns>
        List<ILayer> GetLayers();
       

        

        

        
      


        #endregion



    }
}
