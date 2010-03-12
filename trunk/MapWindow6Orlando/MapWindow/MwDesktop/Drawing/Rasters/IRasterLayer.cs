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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2008 3:42:21 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Data;
using MapWindow.Main;
using MapWindow.Projections;
namespace MapWindow.Drawing
{


    /// <summary>
    /// A raster layer describes using a single raster, and the primary application will be using this as a texture.
    /// </summary>
    public interface IRasterLayer : ILayer
    {
       

        #region Methods


        /// <summary>
        /// Creates a bmp texture and saves it to the same filename as the raster, but with a bmp ending.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        void WriteBitmap();
       

        /// <summary>
        /// Creates a bmp texture and saves it to the same filename as the raster but with a bmp ending.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="progressHandler">An implementation of IProgressHandler to recieve status messages</param>
        void WriteBitmap(IProgressHandler progressHandler);
       

        /// <summary>
        ///  Creates a bmp texture and saves it to the specified filename.  The filename should end in bmp.
        ///  This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="filename">The string filename to write to</param>
        void WriteBitmap(string filename);
        

        /// <summary>
        /// Creates a bmp texture and saves it to the specified filename.  The filename should end in bmp.
        /// This also generates a bpw world file for the texture.
        /// </summary>
        /// <param name="filename">The string filename to write to</param>
        /// <param name="progressHandler">The progress handler for creating a new bitmap.</param>
        /// <exception cref="MapWindow.Main.FileCantBeDeletedException">The file could not be deleted because it is in use by another application.</exception>
        void WriteBitmap(string filename, IProgressHandler progressHandler);
       
       



        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the boundaries of the raster.
        /// </summary>
        IRasterBounds Bounds
        {
            get;
            set;
        }
       

        /// <summary>
        /// Gets the geographic width of the cells for this raster (East-West)
        /// </summary>
        double CellWidth
        {
            get;
            
        }

        /// <summary>
        /// Gets the geographic height of the cells for this raster (North-South)
        /// </summary>
        double CellHeight
        {
            get;
        }

        /// <summary>
        /// Gets or sets the underlying dataset raster for this object
        /// </summary>
        new IRaster DataSet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data type of the values in this raster.
        /// </summary>
        Type DataType
        {
            get;
        }

        /// <summary>
        /// Gets the eastern boundary of this raster.
        /// </summary>
        double East
        {
            get;

        }

        /// <summary>
        /// This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.
        /// </summary>
        float ElevationFactor
        {
            get;
            set;

        }

        /// <summary>
        /// Gets the exageration beyond normal elevation values.  A value of 1 is normal elevation, a vlaue of 0 would be flat,
        /// while a value of 2 would be twice the normal elevation.  This applies to the three-dimensional rendering and is
        /// not related to the shaded relief pattern created by the texture.
        /// </summary>
        float Extrusion
        {
            get;
            set;
        }

       

        /// <summary>
        /// Gets the filename where this raster is saved.
        /// </summary>
        string Filename
        {
            get;
        }

        /// <summary>
        /// Gets the maximum value of this raster.  If this is an elevation raster, this is also the top.
        /// </summary>
        double Maximum
        {
            get;
        }

        /// <summary>
        /// Gets the minimum value of this raster.  If this is an elevation raster, this is also the bottom.
        /// </summary>
        double Minimum
        {
            get;
        }

        /// <summary>
        /// Gets the value that is used when no actual data exists for the specified location.
        /// </summary>
        double NoDataValue
        {
            get;
        }

        /// <summary>
        /// Gets the northern boundary of this raster.
        /// </summary>
        double North
        {
            get;

        }

       

        /// <summary>
        /// Gets the geographic projection that this raster is using.
        /// </summary>
        ProjectionInfo Projection
        {
            get;
        }
       

        /// <summary>
        /// Gets the number of bands in this raster.
        /// </summary>
        int NumBands
        {
            get;
        }
        /// <summary>
        /// Gets the number of columns in this raster.
        /// </summary>
        int NumColumns
        {
            get;
        }
        /// <summary>
        /// Gets the number of rows in this raster.
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the southern boundary of this raster.
        /// </summary>
        double South
        {
            get;
        }

        /// <summary>
        /// Gets or sets the collection of symbolzier properties to use for this raster.
        /// </summary>
        IRasterSymbolizer Symbolizer
        {
            get;
            set;
           
        }

        /// <summary>
        /// Gets the western boundary of this raster.
        /// </summary>
        double West
        {
            get;

        }

       
   

       

        #endregion

       
      

    }
}
