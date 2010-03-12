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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 12:57:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Geometries;
using MapWindow.Main;

namespace MapWindow.Data
{


    /// <summary>
    /// FloatRaster
    /// </summary>
    public class ShortRaster : Raster, IRaster
    {

        #region Private Variables

        private short _shortNoDataValue;

        private ShortRasterList _bands;

        #endregion

        #region Fields

        /// <summary>
        /// Direct access to the type specific data member is provided through here.
        /// </summary>
        public short[][] Data;


        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a blank instance of the raster.  this should only be used by sub classes.
        /// </summary>
        protected ShortRaster()
        {
            base.DataType = typeof(short);

        }

        /// <summary>
        /// Since no file handling is specified, this will only work for the in-ram raster case.
        /// </summary>
        /// <param name="numRows">The integer number of rows to create</param>
        /// <param name="numColumns">The integer number of columns to create</param>
        public ShortRaster(int numRows, int numColumns)
        {
            base.DataType = typeof(short);
            if (numRows * numColumns > 64000000)
            {
                throw new ApplicationException(MessageStrings.RasterTooLarge);
            }
            base.NumRows = numRows;
            base.NumColumns = numColumns;
            base.IsInRam = true;
            Initialize();
        }

        /// <summary>
        /// Calls the basic setup for the raster
        /// </summary>
        protected void Initialize()
        {
            StartRow = 0;
            EndRow = NumRows-1;
            StartColumn = 0;
            EndColumn = NumColumns-1;
            NumColumnsInFile = NumColumns;
            NumRowsInFile = NumRows;
            
            // Just set the cell size to one
            Bounds = new RasterBounds(NumRows, NumColumns, new[] { 0.5, 1.0, 0.0, NumRows - .5, 0.0, -1.0 });
            NumValueCells = 0;
            NoDataValue = short.MinValue;// sets the no-data value to the minimum value for the specified type.
            if (IsInRam)
            {
                Data = new short[NumRows][];
                for (short row = 0; row < NumRows; row++)
                {
                    Data[row] = new short[NumColumns];
                }
            }
            Value = new ShortValueGrid(this);
            _shortNoDataValue = short.MinValue; // Sets it to the appropriate minimum for the int datatype
        }





        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep copy of this raster object so that the data values can be manipulated without
        /// interfering with the original raster.
        /// </summary>
        /// <returns></returns>
        public new ShortRaster Copy()
        {
            ShortRaster copy = MemberwiseClone() as ShortRaster;
            if (copy == null) return null;
            copy.Bounds = Bounds.Copy();
            copy.Data = new short[NumRows][];
            for (int row = 0; row < NumRows; row++)
            {
                copy.Data[row] = new short[NumColumns];
                for (int col = 0; col < NumColumns; col++)
                {
                    copy.Data[row][col] = Data[row][col];
                }
            }
            copy.Value = new ShortValueGrid(copy);
            copy.Bands = new ShortRasterList();
            foreach (ShortRaster band in _bands)
            {
                if (band != this)
                    copy.Bands.Add(band.Copy());
                else
                    copy.Bands.Add(copy);
            }
            return copy;
            
        }
        IRaster IRaster.Copy()
        {
            return Copy();
        }

        // ------------------------------------------FROM AND TO IN RAM ONLY -----------------
        /// <summary>
        /// This creates a completely new raster from the windowed domain on the original raster.  This new raster
        /// will not have a source file, and values like NumRowsInFile will correspond to the in memory version.
        /// All the values will be copied to the new source file.  InRam must be true at this level.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="startRow">The 0 based integer index of the top row to copy from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to copy from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to copy from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to copy from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="copyValues">If this is true, the valeus are saved to the file.  If this is false and the data can be loaded into Ram, no file handling is done.  Otherwise, a file of NoData values is created.</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public IRaster CopyWindow(string filename, int startRow, int endRow, int startColumn, int endColumn, bool copyValues, bool inRam)
        {
            if (inRam == false || (endColumn - startColumn + 1) * (endRow - startRow + 1) > 64000000)
            {
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }
            if (IsInRam == false)
            {
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }
            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;



            ShortRaster result = new ShortRaster(numRows, numCols);

            result.Projection = Projection;

            // The affine coefficients defining the world file are the same except that they are translated over.  Only the position of the
            // upper left corner changes.  Everything else is the same as the previous raster.

            // X = [0] + [1] * column + [2] * row;
            // Y = [3] + [4] * column + [5] * row;
            result.Bounds = new RasterBounds(result.NumRows, result.NumColumns, new double[6]);
            result.Bounds.AffineCoefficients[0] = Bounds.AffineCoefficients[0] + Bounds.AffineCoefficients[1] * startColumn + Bounds.AffineCoefficients[2] * startRow;
            result.Bounds.AffineCoefficients[1] = Bounds.AffineCoefficients[1];
            result.Bounds.AffineCoefficients[2] = Bounds.AffineCoefficients[2];
            result.Bounds.AffineCoefficients[3] = Bounds.AffineCoefficients[3] + Bounds.AffineCoefficients[4] * startColumn + Bounds.AffineCoefficients[5] * startRow;
            result.Bounds.AffineCoefficients[4] = Bounds.AffineCoefficients[4];
            result.Bounds.AffineCoefficients[5] = Bounds.AffineCoefficients[5];


            ProgressMeter pm = new ProgressMeter(ProgressHandler, MessageStrings.CopyingValues, numRows);
            // copy values directly using both data structures
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    result.Data[row][col] = Data[startRow + row][startColumn + col];
                }
                pm.CurrentValue = row;
            }
            pm.Reset();


            result.Value = new ShortValueGrid(result);
            return result;

        }


        // ------------------------------------ IN RAM ONLY ------------------------------------------------------------
        /// <summary>
        /// Gets the statistics all the values, but only if this raster is InRam and fully windowed.  Statistics from a file based
        /// raster will require casting to the appropriate file type.
        /// </summary>
        public override void GetStatistics()
        {
            if (IsInRam == false || this.IsFullyWindowed() == false)
            {
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }
            ProgressMeter pm = new ProgressMeter(ProgressHandler, MessageStrings.CalculatingStatistics, NumRows);

            short min = short.MaxValue;
            short max = short.MinValue;

            double total = 0;
            double sqrTotal = 0;
            int count = 0;

            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    short val = Data[row][col];

                    if (val.CompareTo(_shortNoDataValue) != 0)
                    {
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        double dblVal = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                }
                pm.CurrentValue = row;
            }
            Minimum = min;
            Maximum = max;
            NumValueCells = count;
            StdDeviation = (short)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
            pm.Reset();
        }


        // ----------------------------------- FROM AND TO IN RAM ONLY ---------------------------------
        /// <summary>
        /// This creates an IN MEMORY ONLY window from the in-memory window of this raster.  If, however, the requested range
        /// is outside of what is contained in the in-memory portions of this raster, an appropriate cast
        /// is required to ensure that you have the correct File handling, like a BinaryRaster etc.
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the top row to get from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to get from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to get from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to get from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded To memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public IRaster GetWindow(int startRow, int endRow, int startColumn, int endColumn, bool inRam)
        {
            if (IsInRam == false)
            {
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }
            if (startRow < StartRow || endRow > EndRow || StartColumn < startColumn || EndColumn > endColumn)
            {
                // the requested extents are outside of the extents that have been windowed into ram.  File Handling is required.
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }

            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;
            ShortRaster result = new ShortRaster(numRows, numCols);
            result.Filename = Filename;
            result.Projection = Projection;
            result.DataType = typeof(short);
            result.NumRows = numRows;
            result.NumColumns = numCols;
            result.NumRowsInFile = NumRowsInFile;
            result.NumColumnsInFile = NumColumnsInFile;
            result.NoDataValue = NoDataValue;
            result.StartColumn = startColumn;
            result.StartRow = startRow;
            result.EndColumn = endColumn;
            result.EndRow = EndRow;
            result.FileType = FileType;

            // Reposition the new "raster" so that it matches the specified window, not the whole raster
            // X = [0] + [1] * column + [2] * row;
            // Y = [3] + [4] * column + [5] * row;
            result.Bounds = new RasterBounds(result.NumRows, result.NumColumns, new double[6]);
            result.Bounds.AffineCoefficients[0] = Bounds.AffineCoefficients[0] + Bounds.AffineCoefficients[1] * startColumn + Bounds.AffineCoefficients[2] * startRow;
            result.Bounds.AffineCoefficients[1] = Bounds.AffineCoefficients[1];
            result.Bounds.AffineCoefficients[2] = Bounds.AffineCoefficients[2];
            result.Bounds.AffineCoefficients[3] = Bounds.AffineCoefficients[3] + Bounds.AffineCoefficients[4] * startColumn + Bounds.AffineCoefficients[5] * startRow;
            result.Bounds.AffineCoefficients[4] = Bounds.AffineCoefficients[4];
            result.Bounds.AffineCoefficients[5] = Bounds.AffineCoefficients[5];



            // Now we can copy any values currently in memory.


            ProgressMeter pm = new ProgressMeter(ProgressHandler, MessageStrings.CopyingValues, endRow);
            pm.StartValue = startRow;
            // copy values directly using both data structures
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    result.Data[row][col] = Data[startRow + row][startColumn + col];
                }
                pm.CurrentValue = row;
            }
            pm.Reset();

            result.Value = new ShortValueGrid(result);
            return result;
        }



        /// <summary>
        /// Obtains only the statistics for the small window specified by startRow, endRow etc.
        /// this only works if the window is also InRam.
        /// </summary>
        public void GetWindowStatistics()
        {
            if (IsInRam == false)
            {
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }


            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Calculating Statistics.", NumRows);


            double total = 0;
            double sqrTotal = 0;
            int count = 0;
            short min = short.MaxValue;
            short max = short.MinValue;

            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    short val = Data[row][col];
                    if (val != _shortNoDataValue)
                    {
                        if (val > max) max = val;
                        if (val < min) min = val;
                        double dblVal = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                }
                pm.CurrentValue = row;
            }
            Minimum = min;
            Maximum = max;
            NumValueCells = count;
            StdDeviation = (short)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
            Value.Updated = false; 
        }

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates.  This will
        /// return a No-Data value if the specified coordinates are outside of the grid.
        /// </summary>
        /// <param name="x">The longitude or horizontal coordinate</param>
        /// <param name="y">The latitude or vertical coordinate</param>
        /// <returns>The value of type short of the cell that has a center closest to the specified coordinates</returns>
        public int GetNearestIntValue(double x, double y)
        {
            RcIndex position = this.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= NumRows) return _shortNoDataValue;
            if (position.Column < 0 || position.Column >= NumColumns) return _shortNoDataValue;
            return Data[position.Row][position.Column];

        }

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates.  This will
        /// return a No-Data value if the specified coordTes are outside of the grid.
        /// </summary>
        /// <param name="location">The ICoordinate specifying the location to get a value from</param>
        /// <returns>The value of type int of the cell that has a center closest to the specified coordinates</returns>
        public int GetNearestIntValue(Coordinate location)
        {
            RcIndex position = this.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= NumRows) return _shortNoDataValue;
            if (position.Column < 0 || position.Column >= NumColumns) return _shortNoDataValue;
            return Data[position.Row][position.Column];

        }

        /// <summary>
        /// Read optimization for "not" in ram files.  This only buffers a little bit, and will drastically improve performance, without loading
        /// the whole grid at a time.
        /// </summary>
        /// <param name="row">Specifies the row in the file to read.  This uses the file row index, not any windowed value.</param>
        public virtual short[] ReadRow(int row)
        {
            throw new NotImplementedException(MessageStrings.NotImplemented);
        }


        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates.  This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="x">The longitude or horizontal coordinate</param>
        /// <param name="y">The latitude or vertical coordinate</param>
        /// <param name="value">The value of type int to assign to the nearest cell to the specified coordinates</param>
        public void SetNearestIntValue(double x, double y, short value)
        {
            RcIndex position = this.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= NumRows) return;
            if (position.Column < 0 || position.Column >= NumColumns) return;
            Data[position.Row][position.Column] = value;
        }

        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates.  This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="location">An ICoordinate specifying the location to get a data value from</param>
        /// <param name="value">The value of type T to assign to the nearest cell to the specified coordinates</param>
        public void SetNearestIntValue(Coordinate location, short value)
        {
            RcIndex position = this.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= NumRows) return;
            if (position.Column < 0 || position.Column >= NumColumns) return;
            Data[position.Row][position.Column] = value;
        }

        /// <summary>
        /// This assumes a file has been created already to house the value.
        /// </summary>
        /// <param name="row">The 0 based integer row index for the file to write to.</param>
        /// <param name="column">The 0 based column index for the file to write to.</param>
        /// <param name="value">The actual value to write.</param>
        public virtual void WriteValue(int row, int column, short value)
        {
            throw new NotImplementedException(MessageStrings.NotImplemented);
        }

        #endregion

        #region Properties

        /// <summary>
        /// This Gets or sets the list of bands in their generic format.
        /// </summary>
        public override IList<IRaster> Bands
        {
            get
            {
                return _bands;
            }
            set
            {
                ShortRasterList tmp = value as ShortRasterList;
                if (tmp == null && value != null)
                {
                    _bands = new ShortRasterList();
                    foreach (ShortRaster s in value)
                    {
                        _bands.Add(s);
                    }
                    return;
                }
                _bands = tmp;
            }
        }

        /// <summary>
        /// This gets or sets the type specific version of the bands in this raster.
        /// </summary>
        public IList<ShortRaster> IntBands
        {
            get
            {
                return _bands;
            }
            set
            {
                ShortRasterList tmp = value as ShortRasterList;
                if (tmp == null && value != null)
                {
                    _bands = new ShortRasterList();
                    foreach (ShortRaster s in value)
                    {
                        _bands.Add(s);
                    }
                    return;
                }
                _bands = tmp;
            }
        }

        /// <summary>
        /// Gets or sets the type specific version of the NoDataValue. 
        /// </summary>
        public short ShortNoDataValue
        {
            get { return _shortNoDataValue; }
            set { _shortNoDataValue = value; }
        }

        /// <summary>
        /// Gets or sets the NoDataValue.  This is technically an accessor to the NoDataDataValue, which is only visible
        /// once the raster has been cast To the raster of a specific data type.
        /// </summary>
        public override double NoDataValue
        {
            get
            {
                return _shortNoDataValue;
            }
            set
            {
                if (value > short.MaxValue)
                {
                    _shortNoDataValue = short.MaxValue;
                }
                else if (value < short.MinValue)
                {
                    _shortNoDataValue = short.MinValue;
                }
                else _shortNoDataValue = Convert.ToInt16(value);

            }
        }


        #endregion

    }
}
