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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 11:42:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Data;
using MapWindow.Projections;
using OSGeo.GDAL;
namespace MapWindow.Gdal32
{


    /// <summary>
    /// GdalIntRaster
    /// </summary>
    public class GdalIntRaster : Raster<int>
    {
        #region Private Variables

        readonly Dataset _dataset;
        readonly Band _band;

        

        #endregion

        #region Constructors

        /// <summary>
        /// This can be a raster with multiple bands.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromDataset"></param>
        public GdalIntRaster(string name, Dataset fromDataset):base(fromDataset.RasterYSize, fromDataset.RasterXSize)
        {
            _dataset = fromDataset;
            base.Filename = name;
            ReadHeader();
            int numBands = _dataset.RasterCount;
            for(int i = 1; i <= numBands; i++)
            {
                base.Bands.Add(new GdalIntRaster(name, fromDataset, _dataset.GetRasterBand(i)));
            }
            
        }

        /// <summary>
        /// creates a new integer raster from the specified band
        /// </summary>
        /// <param name="filename">The string path of the file if any.</param>
        /// <param name="fromDataset"></param>
        /// <param name="fromBand"></param>
        public GdalIntRaster(string filename, Dataset fromDataset, Band fromBand)
            : base(fromDataset.RasterYSize, fromDataset.RasterXSize)
        {
            _dataset = fromDataset;
            _band = fromBand;
            base.Filename = filename;
            ReadHeader();
        }

        
        #endregion

        #region Methods

        /// <summary>
        /// Reads values from the raster to the jagged array of values
        /// </summary>
        /// <param name="xOff">The horizontal offset from the left to start reading from</param>
        /// <param name="yOff">The vertical offset from the top to start reading from</param>
        /// <param name="sizeX">The number of cells to read horizontally</param>
        /// <param name="sizeY">The number of cells ot read vertically</param>
        /// <returns>A jagged array of values from the raster</returns>
        public override int[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            int[][] result = new int[sizeY][];
            int[] rawData = new int[sizeY *sizeX];

            if (_band == null)
            {
                Raster<int> ri = Bands[CurrentBand] as Raster<int>;
                if (ri != null)
                {
                    return ri.ReadRaster(xOff, yOff, sizeX, sizeY);
                }
            }
            else
            {
                _band.ReadRaster(xOff, yOff, sizeX, sizeY, rawData, sizeX, sizeY, PixelSpace, LineSpace);
                for (int row = 0; row < base.NumRows; row++)
                {
                    result[row] = new int[base.NumColumns];
                    Array.Copy(rawData, row * sizeX, result[row], 0, sizeX);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Writes values from the jagged array to the raster at the specified location
        /// </summary>
        /// <param name="buffer">A jagged array of values to write to the raster</param>
        /// <param name="xOff">The horizontal offset from the left to start reading from</param>
        /// <param name="yOff">The vertical offset from the top to start reading from</param>
        /// <param name="xSize">The number of cells to write horizontally</param>
        /// <param name="ySize">The number of cells ot write vertically</param>
        public override void WriteRaster(int[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            int[] rawValues = new int[xSize * ySize];
            for(int row = 0; row < ySize; row++)
            {
                Array.Copy(buffer[row],0,rawValues,row*xSize,xSize);
            }
            if (_band == null)
            {
                Raster<int> ri = Bands[CurrentBand] as Raster<int>;
                if (ri != null)
                {
                    ri.WriteRaster(buffer, xOff, yOff, xSize, ySize);
                }
            }
            else
            {
                _band.WriteRaster(xOff, yOff, xSize, ySize, rawValues, xSize, ySize, PixelSpace, LineSpace);
            }
            _dataset.FlushCache();
        }

        /// <summary>
        /// This is the GDAL data type
        /// </summary>
        public DataType GdalDataType
        {
            get { return _band.DataType; }
        }
      
        /// <summary>
        /// Copies the filename
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="copyValues"></param>
        public override void Copy(string filename, bool copyValues)
        {
            Driver d = _dataset.GetDriver();
            DataType myType = OSGeo.GDAL.DataType.GDT_Int32;
            if (_band != null)
            {
                myType = _band.DataType;
            }
            else
            {
                GdalIntRaster r = Bands[0] as GdalIntRaster;
                if (r != null)
                {
                    myType = r.GdalDataType;
                }
            }
            if (copyValues)
            {
                d.CreateCopy(filename, _dataset, 1, Options, GdalProgressFunc, "Copy Progress");
            }
            else
            {
                d.Create(filename, NumColumnsInFile, NumRowsInFile, NumBands, myType, Options);  
            }
        }

        /// <summary>
        /// Handles the callback progress content
        /// </summary>
        /// <param name="complete"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GdalProgressFunc(double complete, IntPtr message, IntPtr data)
        {
            ProgressHandler.Progress("Copy Progress", Convert.ToInt32(complete), "Copy Progress");
            return 0;
        }

        /// <summary>
        /// Gets the mean, standard deviation, minimum and maximum
        /// </summary>
        public override void GetStatistics()
        {
            if(_band != null)
            {
                double min, max, mean, std;
                _band.GetStatistics(0, 1, out min, out max, out mean, out std);
                Minimum = min;
                Maximum = max;
                Mean = mean;
                StdDeviation = std;
            }
            else
            {
                foreach (IRaster raster in Bands)
                {
                    raster.GetStatistics();
                }
            }
        }

       
        /// <summary>
        /// Updates the header information about the projection and the affine coefficients
        /// </summary>
        protected override void UpdateHeader()
        {
            _dataset.SetGeoTransform(Bounds.AffineCoefficients);
            _dataset.SetProjection(Projection.ToEsriString());
        }
        
     
        


        #endregion

        #region Properties

        

        #endregion

        private void ReadHeader()
        {
            DataType = typeof(int);
            base.NumColumnsInFile = _dataset.RasterXSize;
            base.NumColumns = base.NumColumnsInFile;
            base.NumRowsInFile = _dataset.RasterYSize;
            base.NumRows = base.NumRowsInFile;
            Projection = new ProjectionInfo();
            Projection.ReadProj4String(_dataset.GetProjection());
            if (_band != null)
            {
                double val;
                int hasInterval;
                _band.GetNoDataValue(out val, out hasInterval);
                base.NoDataValue = val;
            }
            double[] affine = new double[6];
            _dataset.GetGeoTransform(affine);
            base.Bounds = new RasterBounds(base.NumRows, base.NumColumns, affine);
        }

    }
}
