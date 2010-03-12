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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 11:39:44 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.IO;
using MapWindow.Data;
using MapWindow.Main;
using MapWindow.Plugins;
using OSGeo.GDAL;
namespace MapWindow.Gdal32
{


    /// <summary>
    /// GdalRasterProvider
    /// </summary>
    public class GdalRasterProvider : IRasterProvider
    {
        #region Private Variables

        private Dataset _dataset;
        private IProgressHandler _prog;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GdalRasterProvider
        /// </summary>
        public GdalRasterProvider()
        {
              Gdal.AllRegister();
        }

        #endregion





        #region IRasterProvider Members

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
            if(File.Exists(name))File.Delete(name);
            Driver d = null;
            if(driverCode == null)
            {
                switch (Path.GetExtension(name).ToLower())
                {
                    case ".asc":
                    case ".adf":
                        d = Gdal.GetDriverByName("AAIGrid");
                        break;
                    case ".tiff":
                    case ".tif":
                        d = Gdal.GetDriverByName("GTiff");
                        break;
                   
                    case ".img":
                        d = Gdal.GetDriverByName("HFA");
                        break;
                    case ".gff":
                        d = Gdal.GetDriverByName("GFF");
                        break;
                    case ".dt0":
                    case ".dt1":
                    case ".dt2":
                        d = Gdal.GetDriverByName("DTED");
                        break;
                    case ".ter":
                        d = Gdal.GetDriverByName("Terragen");
                        break;
                    case ".nc":
                        d = Gdal.GetDriverByName("netCDF");
                        break;
                }
            }
            else
            {
               d = Gdal.GetDriverByName(driverCode); 
            }
            if (numBands == 0) numBands = 1;
            if(d == null) return null;
            DataType dt = DataType.GDT_Unknown;
            if(dataType == typeof(int)) dt = DataType.GDT_Int32;
            if (dataType == typeof(short)) dt = DataType.GDT_UInt16;
            if (dataType == typeof(UInt32)) dt = DataType.GDT_UInt32;
            if (dataType == typeof(UInt16)) dt = DataType.GDT_UInt16;
            if (dataType == typeof(double)) dt = DataType.GDT_Float64;
            if (dataType == typeof(float)) dt = DataType.GDT_Float32;
            if (dataType == typeof(byte)) dt = DataType.GDT_Byte;


            _dataset = d.Create(name, xSize, ySize, numBands, dt, options);
            
            if (dataType == typeof(int) || dataType == typeof(UInt16))
            {
                return new GdalIntRaster(name, _dataset);
            }
            if (dataType == typeof(short))
            {
                return new GdalShortRaster(name, _dataset);
            }
            if (dataType == typeof(float))
            {
                return new GdalFloatRaster(name, _dataset);
            }

            try
            {
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            
            return null;
        }
        
        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IRaster Open(string filename)
        {
            IRaster result = null;
            try
            {
                _dataset = Gdal.Open(filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }

            // Single band rasters are easy, just return the band as the raster.
            if(_dataset.RasterCount == 1)
            {
               result = GetBand(filename, _dataset.GetRasterBand(1));
            }

            int c = Gdal.GetDriverCount();
            string message = "";
            for(int i = 0; i < c; i++)
            {
                Driver d = Gdal.GetDriver(i);
                
                message += i + ": " + d.LongName + ":" + d.ShortName + "; " + d.HelpTopic + "\n";
            }
            System.Diagnostics.Debug.Write(message);
           

            // Otherwise, we need to make a more complicated raster structure with individual bands.
            return result;   
           
        }

        private IRaster GetBand(string filename, Band band)
        {
            IRaster result = null;
            if (band.DataType == DataType.GDT_Int32)
            {
                result = new GdalIntRaster(filename, _dataset, band);
            }
            if (band.DataType == DataType.GDT_UInt16)
            {
                result = new GdalIntRaster(filename, _dataset, band);
            }
            if (band.DataType == DataType.GDT_Float32)
            {
                result = new GdalFloatRaster(filename, _dataset, band);
            }
            if (band.DataType == DataType.GDT_Int16)
            {
                result = new GdalShortRaster(filename, _dataset, band);
            }
            if (result != null)
            {
                result.Open();
            }

            return result;
        }


        #endregion

        #region IDataProvider Members

        /// <summary>
        /// Description of the raster
        /// </summary>
        public string Description
        {
            get { return "GDAL Integer Raster"; }
        }

        /// <summary>
        /// The dialog filter to use when opening a file
        /// </summary>
        public string DialogReadFilter
        {
            get { return "GDAL Rasters|*.asc;*.adf;*.bil;*.gen;*.thf;*.blx;*.xlb;*.bt;*.dt0;*.dt1;*.dt2;*.tif;*.dem;*.ter;*.mem"; }
        }

        /// <summary>
        /// The dialog filter to use when saving to a file
        /// </summary>
        public string DialogWriteFilter
        {
            get { return "AAIGrid|*.asc;*.adf|DTED|*.dt0;*.dt1;*.dt2|GTiff|*.tif;*.tiff|TERRAGEN|*.ter|GenBin|*.bil|netCDF|*.nc|Imagine|*.img|GFF|*.gff|Terragen|*.ter"; }
        }

        /// <summary>
        /// Updated with progress information
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _prog; }
            set { _prog = value; }
        }

        /// <summary>
        /// The name of the provider
        /// </summary>
        public string Name
        {
            get { return "GDAL Raster Provider"; }
        }

        
        IDataSet IDataProvider.Open(string filename)
        {
            return Open(filename);
        }

        #endregion

       

      
    }
}
