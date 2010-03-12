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
using MapWindow.Main;
using System.ComponentModel;
using MapWindow.Geometries;
using MapWindow.Data;
namespace MapWindow.Drawing
{
    /// <summary>
    /// Just specifies the organization of interfaces that make up a layer.
    /// It is recommended to create derived classes that inherit from an
    /// abstract layer that implements the majority of this shared functionality 
    /// </summary>
    public interface ILayer : ILegendItem, IRenderable, ISelectable
    {
        #region Events

        /// <summary>
        /// Occurs before the properties are actually shown, also allowing the event to be handled.
        /// </summary>
        event HandledEventHandler ShowProperties;

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        event EventHandler<EnvelopeArgs> ZoomToLayer;

        /// <summary>
        /// Occurs if this layer was selected
        /// </summary>
        event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs when all aspects of the layer finish loading.
        /// </summary>
        event EventHandler FinishedLoading;

        #endregion

        #region Methods


        /// <summary>
        /// Given a geographic extent, this tests the "IsVisible", "UseDynamicVisibility", "DynamicVisibilityMode" and "DynamicVisibilityWidth"
        /// In order to determine if this layer is visible. 
        /// </summary>
        /// <param name="geographicExtent">The geographic extent, where the width will be tested.</param>
        /// <returns>Boolean, true if this layer should be visible for this extent.</returns>
        bool VisibleAtExtent(IEnvelope geographicExtent);


         /// <summary>
        /// Notifies the layer that the next time an area that intersects with this region
        /// is specified, it must first re-draw content to the image buffer.
        /// </summary>
        /// <param name="region">The envelope where content has become invalidated.</param>
        void Invalidate(IEnvelope region);

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        bool IsWithinLegendSelection();
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the core dataset for this layer.
        /// </summary>
        IDataSet DataSet
        {
            get;
            set;
        }

        /// <summary>
        /// Dynamic visibility represents layers that only appear when you zoom in close enough.
        /// This value represents the geographic width where that happens.
        /// </summary>
        double DynamicVisibilityWidth
        {
            get;
            set;
        }

        
        /// <summary>
        /// This controls whether the layer is visible when zoomed in closer than the dynamic 
        /// visiblity width or only when further away from the dynamic visibility width
        /// </summary>
        DynamicVisibilityModes DynamicVisiblityMode
        {
            get; set;
        }


        /// <summary>
        /// Gets the currently invalidated region.
        /// </summary>
        IEnvelope InvalidRegion
        {
            get;
        }

        /// <summary>
        /// Gets the MapFrame that contains this layer.
        /// </summary>
        IFrame MapFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether dynamic visibility should be enabled.
        /// </summary>
        bool UseDynamicVisibility
        {
            get;
            set;
        }

        #endregion


    }
}
