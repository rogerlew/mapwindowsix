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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/25/2008 9:37:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using MapWindow.Components;
using MapWindow.Drawing;
using MapWindow.Main;
using System.Windows.Forms;
using MapWindow.Projections;

namespace MapWindow.Map
{
    /// <summary>
    /// This map draws geographic cordinates directly to pixel coordinates
    /// </summary>
    public interface IMap : IBasicMap
    {

        #region Events


       

        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Occurs after the map is refreshed
        /// </summary>
        event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs after the map is resized
        /// </summary>
        event EventHandler Resized;

        ///// <summary>
        ///// Occurs when an individual zone has finished drawing
        ///// </summary>
        //event EventHandler<ZoneEventArgs> FinishedDrawingZone;

        #endregion

    

        #region Methods

        /// <summary>
        /// Adds a new layer to the map by using an open file dialog
        /// </summary>
        /// <returns>An IMapLayer that represents the layer in the map.</returns>
        List<IMapLayer> AddLayers();

        /// <summary>
        /// Creates a new layer from the specified filename and adds it to the map.
        /// </summary>
        /// <param name="filename">The string filename to add to the map</param>
        /// <returns>The newly created IMapLayer</returns>
        IMapLayer AddLayer(string filename);




        /// <summary>
        /// Uses the file dialog to allow selection of a filename for opening the 
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        new IMapLayer AddLayer();


        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.  Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this category.</param>
        void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression,
                       ILabelSymbolizer symbolizer);

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.  Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this category.</param>
        /// <param name="width">A geographic width, so that if the map is zoomed to a geographic width smaller than this value, labels should appear.</param>
        void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression,
                       ILabelSymbolizer symbolizer, double width);



        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        IMapImageLayer[] GetImageLayers();


        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        IMapRasterLayer[] GetRasterLayers();


        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        IMapLineLayer[] GetLineLayers();

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        IMapPolygonLayer[] GetPolygonLayers();


        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        IMapPointLayer[] GetPointLayers();


        /// <summary>
        /// Gets a list of just the feature layers regardless of whether they are lines, points, or polygons
        /// </summary>
        /// <returns>An array of IMapFeatureLayers</returns>
        IMapFeatureLayer[] GetFeatureLayers();
        
        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a 
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        List<IMapRasterLayer> AddRasterLayers();


        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added</returns>
        IMapRasterLayer AddRasterLayer();



        /// <summary>
        /// Allows a mult-select open file dialog to specify several filenames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        List<IMapFeatureLayer> AddFeatureLayers();


        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns></returns>
        IMapFeatureLayer AddFeatureLayer();


        /// <summary>
        /// Allows a mult-select open file dialog to specify several filenames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        List<IMapImageLayer> AddImageLayers();


        /// <summary>
        /// Allows an open dialog without multi-select to specify a single filename
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map.</returns>
        IMapImageLayer AddImageLayer();



        /// <summary>
        /// This causes all of the datalayers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
        /// </summary>
        void Refresh();

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        void ResetBuffer();

        /// <summary>
        /// 
        /// </summary>
        void SaveLayer();

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        void ZoomIn();
        

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        void ZoomOut();
      


        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool CollectAfterDraw
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Cursor.  This will be changed as the cursor mode changes.
        /// </summary>
        Cursor Cursor
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use MapFunctions")]
        Dictionary<string, IMapFunction> MapTools
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Dictionary<string, IMapFunction> MapFunctions
        {
            get;
            set;
        }
      

        /// <summary>
        /// If this is true, then point layers in the map will only draw points that are 
        /// more than 50% revealed.  This should increase drawing speed for layers that have
        /// a large number of points.
        /// </summary>
        bool CollisionDetection
        {
            get;
            set;
        }


       
     
        /// <summary>
        /// Returns a MapFrame
        /// </summary>
        new IMapFrame MapFrame
        {
            get;
            set;
        }


        /// <summary>
        /// The layers for this map
        /// </summary>
        IMapLayerCollection Layers
        {
            get;
        }

       

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        ProjectionInfo Projection
        {
            get;
            set;
        }



        #endregion

       





    }
}

