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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 2:18:46 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Main;
using MapWindow.Plugins;
namespace MapWindow.Data
{


    /// <summary>
    /// BinaryDataProvider
    /// </summary>
    public class BinaryRasterProvider : IRasterProvider
    {
        #region Private Variables

        private IProgressHandler _progressHandler;

        #endregion

        #region Constructors

     

        #endregion

        #region Methods

        /// <summary>
        /// A Non-File based open.  If no DialogReadFilter is provided, MapWindow will call
        /// this method when this plugin is selected from the Add Other Data option in the
        /// file menu.
        /// </summary>
        public virtual List<IDataSet> Open()
        {
            // This data provider uses a file format, and not the 'other data' methods.;
            return null; 
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="filename">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>An IDataSet to be added to the Map.  These can also be groups of datasets.</returns>
        public virtual IRaster Open(string filename)
        {
            
            RasterDataTypes FileDataType = GetDataType(filename);
            switch (FileDataType)
            {
                case RasterDataTypes.SINGLE:
                    BgdRaster<float> fRast = new BgdRaster<float>(filename);
                    fRast.ProgressHandler = _progressHandler;
                    fRast.Open();
                    return fRast;
                case RasterDataTypes.DOUBLE:
                    BgdRaster<double> dRast = new BgdRaster<double>(filename);
                    dRast.ProgressHandler = _progressHandler;
                    dRast.Open();
                    return dRast;
                case RasterDataTypes.SHORT:
                    BgdRaster<short> sRast = new BgdRaster<short>(filename);
                    sRast.ProgressHandler = _progressHandler;
                    sRast.Open();
                    return sRast;
                case RasterDataTypes.INTEGER:
                    BgdRaster<int> iRast = new BgdRaster<int>(filename);
                    iRast.ProgressHandler = _progressHandler;
                    iRast.Open();
                    return iRast;
                default:
                    return null;
            }
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
        /// <param name="name">The string filename for the new instance</param>
        /// <param name="dataType">The data type to use for the raster</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster</param>
        /// <param name="numBands">The number of bands to create in the raster</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>An IRaster</returns>
        public IRaster Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            
            if(dataType == typeof(short))
            {
                BgdRaster<short> r = new BgdRaster<short>(ySize, xSize);
                r.Filename = name;
                return r;
            }
            if(dataType == typeof(int))
            {
                BgdRaster<int> r = new BgdRaster<int>(ySize, xSize);
                r.Filename = name;
                return r;
            }
            if(dataType == typeof(float))
            {
                BgdRaster<float> r = new BgdRaster<float>(ySize, xSize);
                r.Filename = name;
                return r;
            }
            if(dataType == typeof(double))
            {
                BgdRaster<double> r = new BgdRaster<double>(ySize, xSize);
                r.Filename = name;
                return r;
            }
            if(dataType == typeof(byte))
            {
                BgdRaster<byte> r = new BgdRaster<byte>(ySize, xSize);
                r.Filename = name;
                return r;
            }
           
            return null;

        }

        /// <summary>
        /// Reads a binary header to determine the appropriate data type
        /// </summary>
        public static RasterDataTypes GetDataType(string filename)
        {
            System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.FileStream(filename, System.IO.FileMode.Open));
            br.ReadInt32(); // NumColumns
            br.ReadInt32(); // NumRows
            br.ReadDouble(); // CellWidth
            br.ReadDouble(); // CellHeight
            br.ReadDouble(); // xllcenter
            br.ReadDouble(); // yllcenter
            RasterDataTypes result = (RasterDataTypes)br.ReadInt32();
            br.Close();
            return result;
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
            get { return "Binary Files (*.bgd)|*.bgd"; }
        }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in MapWindow's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter
        {
            get { return "Binary Files (*.bgd)|*.bgd"; }
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
            get { return "This data provider uses a file format, and not the 'other data' methods."; }
        }

        /// <summary>
        /// Gets or sets the control or method that should report on progress
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler;  }
            set { _progressHandler = value; }
        }

        #endregion
    }
}
