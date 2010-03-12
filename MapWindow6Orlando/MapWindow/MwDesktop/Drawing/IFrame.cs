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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using MapWindow.Geometries;
using MapWindow.Projections;

namespace MapWindow.Drawing
{
    /// <summary>
    /// This interface stores a single extent window describing a view, and also contains
    /// the list of all the layers associated with that view.  The layers are ordered.
    /// </summary>
    public interface IFrame: IGroup
    {
        #region Events

        /// <summary>
        /// Occurs when some items should no longer render, and the map needs a refresh.
        /// </summary>
        event EventHandler UpdateMap;

        /// <summary>
        /// Occurs after zooming to a specific location on the map and causes a camera recent.
        /// </summary>
        event EventHandler<EnvelopeArgs> ExtentsChanged;

      

        #endregion


        #region Methods


        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets a boolean that controls whether or not a newly added layer
        /// will also force a zoom to that layer.  If this is true, then nothing
        /// will happen.  Otherwise, adding layers to this frame or a group in this
        /// frame will set the extent.
        /// </summary>
        bool ExtentsInitialized
        {
            get; set;
        }

        /// <summary>
        /// Drawing layers are tracked separately, and do not appear in the legend.
        /// </summary>
        List<ILayer> DrawingLayers
        {
            get;
            set;

        }

        /// <summary>
        /// Gets or sets the currently active layer.  
        /// </summary>
        ILayer SelectedLayer
        {
            get;
        }

        /// <summary>
        /// Controls the smoothing mode.  Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        SmoothingMode SmoothingMode
        {
            get;
            set;
        }

        /// <summary>
        /// This describes the current viewing extents.  Changing this will change the data
        /// that appears when the data frame renders itself.
        /// </summary>
        IEnvelope Extents
        {
            get;
            set;
        }

        /// <summary>
        /// The view extents describes the central, viewable portion of the frame, which is 1/3 the width and height of the whole frame
        /// </summary>
        IEnvelope ViewExtents
        {
            get;
        }

        /// <summary>
        /// Gets or sets the projection that is being used by this map-frame.
        /// This is set by the first layer loaded.
        /// </summary>
        ProjectionInfo Projection
        {
            get; set;
        }


       

        #endregion

        
    }
}
