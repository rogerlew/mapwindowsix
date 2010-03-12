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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:25:15 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Drawing;

namespace MapWindow.Legacy
{


    /// <summary>
    /// Layers
    /// </summary>
    public interface ILayers
    {
       

        #region Methods

        /// <summary>
        /// Default add layer overload.  Displays an open file dialog
        /// </summary>
        /// <returns>An array of <c>MapWindow.Interfaces.Layer</c> objects.</returns>
        ILayerOld[] Add();

        /// <summary>
        /// Adds a layer by filename.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string Filename);

        /// <summary>
        /// Adds a layer by filename.  The layer name is also set.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string Filename, string LayerName);

        /// <summary>
        /// Adds an <c>Image</c> layer to the MapWindow.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData ImageObject);

        /// <summary>
        /// Adds an <c>Image</c> layer to the MapWindow with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData ImageObject, string LayerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the MapWindow.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet ShapefileObject);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the MapWindow with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet ShapefileObject, string LayerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the MapWindow with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet ShapefileObject, string LayerName, int Color, int OutlineColor);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the MapWindow with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet ShapefileObject, string LayerName, int Color, int OutlineColor, int LineOrPointSize);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the MapWindow.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster GridObject);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the MapWindow, with the specified coloring scheme.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster GridObject, IRasterSymbolizer ColorScheme);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the MapWindow with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster GridObject, string LayerName);

        /// <summary>
        /// Adds a <c>Rster</c> object to the MapWindow with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster GridObject, IRasterSymbolizer ColorScheme, string LayerName);

        /// <summary>
        /// Adds a layer by filename.  The layer name is also set, as well as the legend visibility.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string Filename, string LayerName, bool VisibleInLegend);

        /// <summary>
        /// Adds a layer to the map, optionally placing it above the currently selected layer (otherwise at top of layer list).
        /// </summary>
        /// <param name="Visible">Whether or not the layer is visible upon adding it</param>
        /// <param name="PlaceAboveCurrentlySelected">Whether the layer should be placed above currently selected layer, or at top of layer list.</param>
        /// <param name="Filename">The name of the file to add.</param>
        /// <param name="LayerName">The name of the layer, as displayed in the legend.</param>
        ILayerOld Add(string Filename, string LayerName, bool Visible, bool PlaceAboveCurrentlySelected);

        /// <summary>
        /// Removes all layers from the map.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns true if the layer handle specified belongs to a valid layer.  Drawing layers are not 
        /// considered normal layers so this function will not return consistent results if a drawing 
        /// layer is passed in.
        /// </summary>
        /// <param name="LayerHandle">Handle of the layer to check.</param>
        /// <returns>True if the layer handle is valid.  False otherwise.</returns>
        bool IsValidHandle(int LayerHandle);


        /// <summary>
        /// Moves a layer to another position and/or group.
        /// </summary>
        /// <param name="Handle">Handle of the layer to move.</param>
        /// <param name="NewPosition">New position in the target group.</param>
        /// <param name="TargetGroup">Group to move the layer to.</param>
        void MoveLayer(int Handle, int NewPosition, int TargetGroup);

        /// <summary>
        /// Rebuilds a grid layer using the specified <c>GridColorScheme</c>
        /// </summary>
        /// <param name="LayerHandle">Handle of the grid layer.</param>
        /// <param name="GridObject">Grid object corresponding to that layer handle.</param>
        /// <param name="ColorScheme">Coloring scheme to use when rebuilding.</param>
        /// <returns></returns>
        bool RebuildGridLayer(int LayerHandle, IRaster GridObject, IRasterSymbolizer ColorScheme);

        /// <summary>
        /// Removes the layer from the MapWindow.
        /// </summary>
        /// <param name="LayerHandle"></param>
        void Remove(int LayerHandle);


        #endregion

        #region Properties

        
        /// <summary>
        /// Gets or sets the current layer handle.
        /// </summary>
        int CurrentLayer { get; set; }

        /// <summary>
        /// Finds a layer handle when given the global position of a layer.
        /// </summary>
        /// <param name="GlobalPosition">Position in the layers list, disregarding groups.</param>
        /// <returns>The handle of the layer at the specified position, or -1 if no layer is found there.</returns>
        int GetHandle(int GlobalPosition);

        /// <summary>
        /// Allows access to group properties in the legend.
        /// </summary>
        List<IGroup> Groups { get; }

        /// <summary>
        /// Returns the number of layers loaded in the MapWindow.  Drawing layers are not counted.
        /// </summary>
        int NumLayers { get; }

        /// <summary>
        /// Returns the layer object corresponding the the specified <c>LayerHandle</c>
        /// </summary>
        ILayerOld this[int LayerHandle] { get; }


        #endregion

       

       

        


    }
}
