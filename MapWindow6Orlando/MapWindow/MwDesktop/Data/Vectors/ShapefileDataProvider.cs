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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 2:52:59 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using MapWindow.Main;
using MapWindow.Geometries;
using MapWindow.Plugins;

namespace MapWindow.Data
{


    /// <summary>
    /// ShapefileDataProvider
    /// </summary>
    public class ShapefileDataProvider : VectorProvider
    {

        private IProgressHandler _progressHandler;
      
        #region Constructors

        /// <summary>
        /// Creates a new instance of ShapefileDataProvider
        /// </summary>
        public ShapefileDataProvider()
        {

        }

        #endregion



        #region Methods

        /// <summary>
        /// This tests the specified file in order to determine what type of vector the file contains.
        /// This returns unspecified if the file format is not supported by this provider.
        /// </summary>
        /// <param name="filename">The string filename to test</param>
        /// <returns>A FeatureType clarifying what sort of features are stored on the data type.</returns>
        public virtual FeatureTypes GetFeatureType(string filename)
        {
            ShapefileHeader sh = new ShapefileHeader(filename);
            if (sh.ShapeType == ShapeTypes.Polygon || sh.ShapeType == ShapeTypes.PolygonM || sh.ShapeType == ShapeTypes.PolygonZ)
            {
                return FeatureTypes.Polygon;
            }
            if (sh.ShapeType == ShapeTypes.PolyLine || sh.ShapeType == ShapeTypes.PolyLineM || sh.ShapeType == ShapeTypes.PolyLineZ)
            {
                return FeatureTypes.Line;
            }
            if (sh.ShapeType == ShapeTypes.Point || sh.ShapeType == ShapeTypes.PointM || sh.ShapeType == ShapeTypes.PointZ)
            {
                return FeatureTypes.Point;
            }
            if(sh.ShapeType == ShapeTypes.MultiPoint || sh.ShapeType == ShapeTypes.MultiPointM || sh.ShapeType == ShapeTypes.MultiPointZ)
            {
                return FeatureTypes.MultiPoint;
            }
            return FeatureTypes.Unspecified;
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="filename">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        public virtual IFeatureSet Open(string filename)
        {
            return Shapefile.OpenFile(filename);
        }
        IDataSet IDataProvider.Open(string filename)
        {
            return Open(filename);
        }

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the filename
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="filename">The string filename for the new instance</param>
        /// <param name="featureType">Point, Line, Polygon etc.  Sometimes this will be specified, sometimes it will be "Unspecified"</param>
        /// <param name="inRam">Boolean, true if the dataset should attempt to store data entirely in ram</param>
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <returns>An IRaster</returns>
        public virtual IFeatureSet CreateNew(string filename, FeatureTypes featureType, bool inRam, IProgressHandler progressHandler)
        {
            if (featureType == FeatureTypes.Point)
            {
                PointShapefile ps = new PointShapefile();
                ps.Filename = filename;
                return ps;
            }
            else if (featureType == FeatureTypes.Line)
            {
                LineShapefile ls = new LineShapefile();
                ls.Filename = filename;
                return ls;
            }
            else if (featureType == FeatureTypes.Polygon)
            {
                PolygonShapefile ps = new PolygonShapefile();
                ps.Filename = filename;
                return ps;
            }
            else if (featureType == FeatureTypes.MultiPoint)
            {
                MultiPointShapefile mps = new MultiPointShapefile();
                mps.Filename = filename;
                return mps;
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

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        #endregion


    }
}
