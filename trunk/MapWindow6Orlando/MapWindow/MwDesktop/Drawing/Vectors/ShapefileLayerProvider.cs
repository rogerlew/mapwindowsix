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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 11:25:56 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Map;
using MapWindow.Plugins;
namespace MapWindow.Drawing
{


    /// <summary>
    /// ShapefileLayerProvider
    /// </summary>
    public class ShapefileLayerProvider: VectorLayerProvider
    {
        #region Private Variables

        #endregion

        #region Constructors

       

        #endregion

        #region Methods

        /// <summary>
        /// Not Implemented yet
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="featureType"></param>
        /// <param name="inRam"></param>
        /// <param name="container"></param>
        /// <param name="progressHandler"></param>
        /// <returns></returns>
        public IFeatureLayer CreateNew(string filename, FeatureTypes featureType, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            ShapefileDataProvider dp = new ShapefileDataProvider();
            IFeatureSet fs = dp.CreateNew(filename, featureType, inRam, progressHandler);
            if (progressHandler == null) progressHandler = Components.LayerManager.DefaultLayerManager.ProgressHandler;

            if (fs.FeatureType == FeatureTypes.Line)
            {
                return new MapLineLayer(fs, container);
            }
            if (fs.FeatureType == FeatureTypes.Polygon)
            {
                return new MapPolygonLayer(fs, container);
            }
            if (fs.FeatureType == FeatureTypes.Point || fs.FeatureType == FeatureTypes.MultiPoint)
            {
                return new MapPointLayer(fs, container);
            }
            return null;

           
            
        }

        /// <summary>
        /// Opens a shapefile, but returns it as a FeatureLayer
        /// </summary>
        /// <param name="filename">The string filename</param>
        /// <param name="inRam">Boolean, if this is true it will attempt to open the entire layer in memory.</param>
        /// <param name="container">A container to hold this layer.</param>
        /// <param name="progressHandler">The progress handler that should receive status messages</param>
        /// <returns>An IFeatureLayer</returns>
        public ILayer OpenLayer(string filename, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            ShapefileDataProvider dp = new ShapefileDataProvider();
            IFeatureSet fs = dp.Open(filename);

            if (fs != null)
            {
                if (fs.FeatureType == FeatureTypes.Line)
                {
                    return new MapLineLayer(fs, container);
                }
                if (fs.FeatureType == FeatureTypes.Polygon)
                {
                    return new MapPolygonLayer(fs, container);
                }
                if (fs.FeatureType == FeatureTypes.Point || fs.FeatureType == FeatureTypes.MultiPoint)
                {
                    return new MapPointLayer(fs, container);
                }
            }
            return null;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol.  Each will appear in MapWindow's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter
        {
            get { return "Shapefiles (*.shp)|*.shp"; }
        }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in MapWindow's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter
        {
            get { return "Shapefiles (*.shp)|*.shp"; }
        }

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider.  Example might be GDAL. 
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name
        {
            get { return "MapWindow"; }
        }

        /// <summary>
        /// This is a basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description
        {
            get { return "This data provider gives a simple version of .shp reading for now."; }
        }

        #endregion




       
       

       

        
    }
}
