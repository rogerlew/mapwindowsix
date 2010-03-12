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
// The Initial Developer of this Original Code is Ted Dunsford. Created in January 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MapWindow.Components;
using MapWindow.Geometries;
using MapWindow.Main;
using System.Linq;
namespace MapWindow.Data
{
    /// <summary>
    /// Handles the native file functions for float grids, without relying on GDAL.
    /// </summary>
    public class Raster : DataSet, IRaster
    {
        #region Events

        /// <summary>
        /// Occurs when attempting to copy or save to a filename that already exists.  A developer can tap into this event
        /// in order to display an appropriate message.  A cancel property allows the developer (and ultimately the user)
        /// decide if the specified event should ultimately be cancelled.
        /// </summary>
        public event EventHandler<MessageCancel> FileExists;

        #endregion

        #region Private Variables
       
        private IList<IRaster> _bands;
        private int _currentBand; // This stores a counter for processes that need to modify progress messages to take into account multiple bands.
        private string _customFileType;
        private Type _dataType; // The data type like long, int, etc in a binary grid.
        private int _endRow;
        private int _endColumn;
        private string _filename; // String filename
        private RasterFileTypes _fileType; // The file format for the grid... this should only be Binary or ASCII
        private bool _isInRam;
        private double _mean;
        private double _minimum;
        private double _maximum;
        private double _noDataValue; // A float for no-data values
        private string _notes;
        private int _numColumns; // The count of the columns in the image
        private int _numColumnsInFile;
        private int _numRows; // The number of rows in an image
        private int _numRowsInFile;
        private long _numValueCells;
        private IValueGrid _values;
        private IProgressHandler _progressHandler;
        private List<IValueRow> _rows; 
        private int _startRow;
        private int _startColumn;
        private double _stdDev;
        private object _tag;
        private IRasterBounds _bounds;
        private int _pixelSpace;
        private int _lineSpace;
        private string _driverCode;
        private string[] _options;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Set up the default values
        /// </summary>
        public Raster()
        {
            _isInRam = true;
            _bands = new List<IRaster>();
        }

        
        #endregion

        #region Methods

        /// <summary>
        /// Creates a memberwise clone of this object.  Reference types will be copied, but
        /// they will still point to the same original object.  So a clone of this raster
        /// is pointing to the same underlying array of values etc.
        /// </summary>
        /// <returns>A Raster clone of this raster.</returns>
        public object Clone()
        {
            if (InternalDataSet != null) return InternalDataSet.Clone();
            return MemberwiseClone();
        }

        /// <summary>
        /// Creates a new IRaster that has the identical characteristics and in-ram data to the original.
        /// </summary>
        /// <returns>A Copy of the original raster.</returns>
        public virtual IRaster Copy()
        {
            if (InternalDataSet != null) return InternalDataSet.Copy();
            throw new NotImplementedException("Copy should be implemented in the internal, or sub classes.");
        }

    


        /// <summary>
        /// Creates a duplicate version of this file.  If copyValues is set to false, then a raster of NoData values is created
        /// that has the same georeferencing information as the source file of this Raster, even if this raster is just a window.
        /// </summary>
        /// <param name="filename">The string filename specifying where to create the new file.</param>
        /// <param name="copyValues">If this is false, the same size and georeferencing values are used, but they are all set to NoData.</param>
        public virtual void Copy(string filename, bool copyValues)
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.Copy(filename, copyValues);
                return;
            }
            throw new NotImplementedException("Copy should be implemented in the internal, or sub classes.");
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
        public void CreateNew(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            InternalDataSet = DataManager.DefaultDataManager.CreateRaster(name, driverCode, xSize, ySize, numBands,
                                                                          dataType, options);
        }
  


      

        /// <summary>
        /// Even if this is a window, this will cause this raster to show statistics calculated from the entire file.
        /// </summary>
        public virtual void GetStatistics()
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.GetStatistics();
                return;
            }
            throw new NotImplementedException(MessageStrings.NotImplemented);
        }

       
        

       

        /// <summary>
        /// Displays a dialog, allowing the users to open a raster.
        /// </summary>
        public virtual void Open()
        {
            InternalDataSet = DataManager.DefaultDataManager.OpenRaster();
        }

        /// <summary>
        /// Opens the specified filename.  The DefaultDataManager will determine the best type of raster to handle the specified
        /// file based on the filename or characteristics of the file.
        /// </summary>
        /// <param name="filename">The string filename of the raster to open.</param>
        public virtual void Open(string filename)
        {
            InternalDataSet = DataManager.DefaultDataManager.OpenRaster(filename, _isInRam, _progressHandler);
        }


        /// <summary>
        /// Saves the values to a the same file that was created or loaded.
        /// </summary>
        public virtual void Save()
        {
            if (InternalDataSet != null)
            {
                InternalDataSet.Save();
                return;
            }
            throw new NotImplementedException(MessageStrings.NotImplemented);
        }

        /// <summary>
        /// Saves the curretn raster to the specified file.  The current driver code and options are used.
        /// </summary>
        /// <param name="filename">The string filename to save the raster to.</param>
        public virtual void SaveAs(string filename)
        {
            SaveAs(filename, DriverCode, Options);
        }

        /// <summary>
        /// Saves the current raster to the specified file, using the specified driver, but with the 
        /// options currently specified in the Options property.
        /// </summary>
        /// <param name="filename">The string filename to save this raster as</param>
        /// <param name="driverCode">The string driver code.</param>
        public virtual void SaveAs(string filename, string driverCode)
        {
            SaveAs(filename, driverCode, Options);
        }

        /// <summary>
        /// Saves the current raster to the specified file.
        /// </summary>
        /// <param name="driverCode">The driver code to use</param>
        /// <param name="options">the string array of options that depend on the format</param>
        /// <param name="filename">The string filename to save the current raster to.</param>
        public virtual void SaveAs(string filename, string driverCode, string[] options)
        {
            // Create a new raster file
            IRaster newRaster = DataManager.DefaultDataManager.CreateRaster(filename, driverCode, NumColumns, NumRows, NumBands, DataType, options);
            
            // Copy the file based values
            // newRaster.Copy(Filename, true);
            
            // Copy the in memory values
            Raster r = newRaster as Raster;
            if(r != null)
            {
                r.SetData(this);
            }
            else
            {
                bool TO_DO_SAVE_AS_FOR_OTHER_IRASTER = true;
            }

            // Save the in-memory values.
            newRaster.Save();
        
     

            // If this had an internal raster before, it will replace it with the newly created one.
            // Since this object can't actually replace itself, it will start working with an internal
            // raster if it wasn't working with one already.
            InternalDataSet = newRaster;

        }

        
       
        /// <summary>
        /// This code is empty, but can be overridden in subtypes
        /// </summary>
        /// <param name="original"></param>
        protected virtual void SetData(IRaster original)
        {
            
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of each raster element in bytes.
        /// </summary>
        public virtual int ByteSize
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.ByteSize;
                }
                throw new NotImplementedException("This should be implemented at a level where a type parameter can be parsed.");
            }
        }


        /// <summary>
        /// Gets or sets the RasterBounds class that contains georeference information.
        /// </summary>
        [Category("General"),
        TypeConverter(typeof(Forms.GeneralTypeConverter)),
        Editor(typeof(Forms.PropertyGridEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Gets or sets the RasterBounds class that contains detailed georeference information.")]
        public virtual IRasterBounds Bounds
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Bounds;
                }
                return _bounds;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Bounds = value;
                    return;
                }
                _bounds = value;
            }
        }


        /// <summary>
        /// Gets or sets the list of bands, which are in turn rasters.  The rasters
        /// contain only one band each, instead of the list of all the bands like the
        /// parent raster.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IList<IRaster> Bands
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Bands;
                }
                return _bands;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Bands = value;
                    return;
                }
                _bands = value;

            }
        }

        /// <summary>
        /// The geographic height of a cell the projected units.  Setting this will
        /// automatically adjust the affine coefficient to a negative value.
        /// </summary>
        [Category("General"), Description("The geographic height of a cell the projected units.  Setting this will automatically adjust the affine coefficient to a negative value.")]
        public virtual double CellHeight
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.CellHeight;
                }
                if (_bounds != null) return Math.Abs(_bounds.AffineCoefficients[5]);
                return 0;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.CellHeight = value;
                    return;
                }
                // For backwards compatibility, people specifying CellHeight can be assumed
                // to want to preserve YllCenter, and not the Affine coefficient.
                _bounds.AffineCoefficients[3] = Bounds.BottomLeft().Y + value * NumRows;

                // This only allows you to change the magnitude of the cell height, not the direction.
                // For changing direction, control AffineCoefficients[5] directly.
                _bounds.AffineCoefficients[5] = Math.Sign(_bounds.AffineCoefficients[5]) * Math.Abs(value);
            }
        }


        /// <summary>
        /// The geographic width of a cell in the projected units
        /// </summary>
        [Category("General"), Description("The geographic width of a cell in the projected units.")]
        public virtual double CellWidth
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.CellWidth;
                }
                return Math.Abs(_bounds.AffineCoefficients[1]);
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.CellWidth = value;
                    return;
                }
                _bounds.AffineCoefficients[1] = Math.Sign(_bounds.AffineCoefficients[1]) * Math.Abs(value);
            }
        }

        /// <summary>
        /// This provides a zero-based integer band index that specifies which of the internal bands
        /// is currently being used for requests for data.
        /// </summary>
        [Category("Data"),Description("This provides a zero-based integer band index that specifies which of the internal bands is currently being used for requests for data.")]
        public virtual int CurrentBand
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.CurrentBand;
                }
                return _currentBand;
            }
            protected set 
            { 
                _currentBand = value;
            }
        }

        /// <summary>
        /// This does nothing unless the FileType property is set to custom.
        /// In such a case, this string allows new file types to be managed.
        /// </summary>
        [Category("Data"),Description("This does nothing unless the FileType property is set to custom.  In such a case, this string allows new file types to be managed.")]
        public virtual string CustomFileType
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.CustomFileType;
                }
                return _customFileType;
            }
            protected set { _customFileType = value; }

        }

        /// <summary>
        /// This returns the RasterDataTypes enumeration clarifying the underlying data type for this raster.
        /// </summary>
        [Category("Data"),Description("This returns the RasterDataTypes enumeration clarifying the underlying data type for this raster.")]
        public Type DataType
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.DataType;
                }
                return _dataType;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.DataType = value;
                    return;
                }
                _dataType = value;
            }
        }

        /// <summary>
        /// Gets or sets the driver code for this raster.
        /// </summary>
        public string DriverCode
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.DriverCode;
                }
                return _driverCode;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.DriverCode = value;
                    return;
                }
                _driverCode = value;
            }
        }


       

        /// <summary>
        /// The integer column index for the right column of this raster.  Most of the time this will 
        /// be NumColumns - 1.  However, if this raster is a window taken from a larger raster, then
        /// it will be the index of the endColumn from the window.
        /// </summary>
        [Category("Window"), Description("The integer column index for the right column of this raster.  Most of the time this will be NumColumns - 1.  However, if this raster is a window taken from a larger raster, then it will be the index of the endColumn from the window.")]
        public virtual int EndColumn
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.EndColumn;
                }
                return _endColumn;

            }
            protected set { _endColumn = value; }
        }

        /// <summary>
        /// The integer row index for the end row of this raster.  Most of the time this will 
        /// be numRows - 1.  However, if this raster is a window taken from a larger raster, then
        /// it will be the index of the endRow from the window.
        /// </summary>
        [Category("Window"), Description("The integer row index for the end row of this raster.  Most of the time this will  be numRows - 1.  However, if this raster is a window taken from a larger raster, then it will be the index of the endRow from the window.")]
        public virtual int EndRow
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.EndRow;
                }
                return _endRow;
            }
            protected set { _endRow = value; }
        }

        /// <summary>
        /// Gets or Sets the complete path and filename of the current file
        /// </summary>
        [Category("Data"), Description("Gets or Sets the complete path and filename of the current file")]
        public virtual string Filename
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Filename;
                }
                return _filename;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Filename = value;
                    return;
                }
                _filename = value;
            }
        }

        /// <summary>
        /// Returns the grid file type.  Only Binary or ASCII are supported natively, without GDAL.
        /// </summary>
        [Category("Data"), Description("Returns the grid file type.  Only Binary or ASCII are supported natively, without GDAL.")]
        public virtual RasterFileTypes FileType
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.FileType;
                }
                return _fileType;
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.FileType = value;
                    return;
                }
                _fileType = value;
            }
        }

        

       

        /// <summary>
        /// Gets or sets a boolean that is true if the data for this raster is in memory.
        /// </summary>
        [Category("Data"), Description("Gets or sets a boolean that is true if the data for this raster is in memory.")]
        public virtual bool IsInRam
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.IsInRam; 
                }
                return _isInRam;
            }
            protected set
            {
                _isInRam = value;
            }
        }

        /// <summary>
        /// A parameter for accessing some GDAL data.  This fequently does nothing and is usually 0.
        /// </summary>
        public int LineSpace
        {
            get { return _lineSpace; }
            set { _lineSpace = value; }
        }
      
        /// <summary>
        /// Gets the maximum data value, not counting no-data values in the grid.
        /// </summary>
        [Category("Statistics"), Description("Gets the maximum data value, not counting no-data values in the grid.")]
        public virtual double Maximum
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Maximum;
                }
                if (Value.Updated) GetStatistics();
                return _maximum;
            }
            protected set { _maximum = value; }
        }

        /// <summary>
        /// Gets the mean of the non-NoData values in this grid.  If the data is not InRam, then
        /// the GetStatistics method must be called before these values will be correct.
        /// </summary>
        [Category("Statistics"), Description("Gets the mean of the non-NoData values in this grid.  If the data is not InRam, then the GetStatistics method must be called before these values will be correct.")]
        public virtual double Mean
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Mean;
                }
                if (Value.Updated) GetStatistics();
                return _mean; 
            }
            protected set { _mean = value; }
        }
       

        /// <summary>
        /// Gets the minimum data value that is not classified as a no data value in this raster.
        /// </summary>
        [Category("Statistics"), Description("Gets the minimum data value that is not classified as a no data value in this raster.")]
        public virtual double Minimum
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Minimum;
                }
                if (Value.Updated) GetStatistics();
                return _minimum;
            }
            protected set 
            {
                //System.Diagnostics.Debug.Assert(value > -100000);
                _minimum = value;
            }
        }

       

       

        /// <summary>
        /// A double showing the no-data value for this raster.
        /// </summary>
        [Category("Data"), Description("A double showing the no-data value for this raster.")]
        public virtual double NoDataValue
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.NoDataValue;
                return _noDataValue;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.NoDataValue = value;
                    return;
                }
                _noDataValue = value;
            }
        }

        /// <summary>
        /// For binary rasters this will get cut to only 256 characters.
        /// </summary>
        [Category("General"), Description("For binary rasters this will get cut to only 256 characters.")]
        public virtual string Notes
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Notes;
                }
                return _notes;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Notes = value;
                    return;
                }
                _notes = value;
            }
        }

        

       

        /// <summary>
        /// Gets the number of bands.  In most traditional grid formats, this is 1.  For RGB images,
        /// this would be 3.  Some formats may have many bands.
        /// </summary>
        [Category("General"), Description("Gets the number of bands.  In most traditional grid formats, this is 1.  For RGB images, this would be 3.  Some formats may have many bands.")]
        public virtual int NumBands
        {
            get 
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.NumBands;
                }
                return _bands.Count;
            }
        }

        /// <summary>
        /// Gets the horizontal count of the cells in the raster.
        /// </summary>
        [Category("General"), Description("Gets the horizontal count of the cells in the raster.")]
        public virtual int NumColumns
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.NumColumns;
                return _numColumns;
            }
            set
            {
                _numColumns = value;
                if (_numColumns > 0 && _numRows > 0  && _bounds != null)
                {
                    _bounds = new RasterBounds(NumRows, NumColumns, _bounds.AffineCoefficients);
                }
            }
        }

        /// <summary>
        /// Gets the integer count of the number of columns in the source or file that this
        /// raster is a window from.  (Usually this will be the same as NumColumns)
        /// </summary>
        [Category("Window"), Description("Gets the integer count of the number of columns in the source or file that this raster is a window from.  (Usually this will be the same as NumColumns)")]
        public virtual int NumColumnsInFile
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.NumColumnsInFile;
                return _numColumnsInFile;
            }
            protected set { _numColumnsInFile = value; }
        }

        

        /// <summary>
        /// Gets the vertical count of the cells in the raster.
        /// </summary>
        [Category("General"), Description("Gets the vertical count of the cells in the raster.")]
        public virtual int NumRows
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.NumRows;
                return _numRows;
            }
            set
            {
                _numRows = value;
                if (_numColumns > 0 && _numRows > 0 && _bounds != null)
                {
                    _bounds = new RasterBounds(value, NumColumns, _bounds.AffineCoefficients);
                }
            }
        }

        /// <summary>
        /// Gets the integer count of the number of rows in the source or file that this
        /// raster is a window from.  (Usually this will be the same as NumColumns.)
        /// </summary>
        [Category("Window"), Description("Gets the integer count of the number of rows in the source or file that this raster is a window from.  (Usually this will be the same as NumColumns.)")]
        public virtual int NumRowsInFile
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.NumRowsInFile;
                return _numRowsInFile;
            }
            protected set { _numRowsInFile = value; }
        }

        /// <summary>
        /// Gets the count of the cells that are not no-data.  If the data is not InRam, then
        /// you will have to first call the GetStatistics() method to gain meaningul values.
        /// </summary>
        [Category("Statistics"), Description("Gets the count of the cells that are not no-data.  If the data is not InRam, then you will have to first call the GetStatistics() method to gain meaningul values.")]
        public virtual long NumValueCells
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.NumValueCells;
                return _numValueCells; 
            }
            protected set { _numValueCells = value; }
        }

        /// <summary>
        /// An extra string array of options that exist for support of some types of GDAL supported raster drivers
        /// </summary>
        public string[] Options
        {
            get { return _options; }
            set { _options = value; }
        }


        /// <summary>
        /// A parameter for accessing GDAL.  This frequently does nothing and is usually 0.
        /// </summary>
        public int PixelSpace
        {
            get { return _pixelSpace; }
            set { _pixelSpace = value; }
        }

       

        /// <summary>
        /// Gets the last progress handler that was set for this raster.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IProgressHandler ProgressHandler
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.ProgressHandler;
                return _progressHandler;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.ProgressHandler = value;
                    return;
                }
                _progressHandler = value;
            }
        }


        /// <summary>
        /// Gets a list of the rows in this raster that can be accessed independantly.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual List<IValueRow> Rows
        {
            get
            {
                if (InternalDataSet != null)
                {
                    return InternalDataSet.Rows;
                }
                return _rows;
            }
            protected set
            {
                _rows = value;
            }
        }

        /// <summary>
        /// The integer column index for the left column of this raster.  Most of the time this will 
        /// be 0.  However, if this raster is a window taken from a file, then
        /// it will be the row index in the file for the top row of this raster.
        /// </summary>
        [Category("Window"), Description("The integer column index for the left column of this raster.  Most of the time this will be 0.  However, if this raster is a window taken from a larger raster, then it will be the index of the startRow from the window.")]
        public virtual int StartColumn
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.StartColumn;
                return _startColumn;
            }
            protected set { _startColumn = value; }
        }

        /// <summary>
        /// The integer row index for the top row of this raster.  Most of the time this will 
        /// be 0.  However, if this raster is a window taken from a file, then
        /// it will be the row index in the file for the left row of this raster.
        /// </summary>
        [Category("Window"), Description("The integer row index for the top row of this raster.  Most of the time this will be 0.  However, if this raster is a window taken from a larger raster, then it will be the index of the startRow from the window.")]
        public virtual int StartRow
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.StartRow;
                return _startRow;
            }
            protected set { _startRow = value; }
        }


        /// <summary>
        /// Gets the standard deviation of all the Non-nodata cells.  If the data is not InRam,
        /// then you will have to first call the GetStatistics() method to get meaningful values.
        /// </summary>
        [Category("Statistics"), Description("Gets the standard deviation of all the Non-nodata cells.  If the data is not InRam,then you will have to first call the GetStatistics() method to get meaningful values.")]
        public virtual double StdDeviation
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.StdDeviation;
                if (Value.Updated) GetStatistics();
                return _stdDev;
            }
            protected set { _stdDev = value; }
        }

      
       
         /// <summary>
        /// This is provided for future developers to link this raster to other entities.
        /// It has no function internally, so it can be manipulated safely.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object Tag
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.Tag;
                return _tag; 
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Tag = value;
                    return;
                }
                _tag = value; 
            }
        }

        

        /// <summary>
        /// Gets or sets the value on the CurrentBand given a row and column undex
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IValueGrid Value
        {
            get 
            {
                if (InternalDataSet != null) return InternalDataSet.Value;
                return _values;
            }
            set 
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Value = value;
                    return;
                }
                _values = value;
            }
        }

         /// <summary>
        /// Gets or sets the X position of the lower left data cell.
        /// Setting this will adjust only the _affine[0] coefficient to ensure that the
        /// lower left corner ends up in the specified location, while keeping all the
        /// other affine coefficients the same.  This is like a horizontal Translate
        /// that locks into place the center of the lower left corner of the image.
        /// </summary>
        [Category("General"), Description("Gets or sets the X position of the lower left data cell.  " +
            "Setting this will adjust only the _affine[0] coefficient to ensure that the " +
            "lower left corner ends up in the specified location, while keeping all the " +
            "other affine coefficients the same.  This is like a horizontal Translate " +
            "that locks into place the center of the lower left corner of the image.")]
        public double Xllcenter
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Xllcenter;
                double[] affine = _bounds.AffineCoefficients;
                return affine[0] + affine[1] * .5 + affine[2] * (_numColumns - .5);
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Xllcenter = value;
                    return;
                }
                double[] affine = _bounds.AffineCoefficients;
                if(affine == null)
                {
                    affine = new double[6];
                    affine[1] = 1;
                    affine[3] = _numRows;
                    affine[5] = -1;
                }
                if(affine[1] == 0)
                {
                    affine[1] = 1;
                }
                affine[0] = value - affine[1] * .5 - affine[2] * (_numColumns - .5);
               
            }
        }

        /// <summary>
        /// Gets or sets the Y position of the lower left data cell. 
        /// Setting this will adjust only the _affine[0] coefficient to ensure that the
        /// lower left corner ends up in the specified location, while keeping all the
        /// other affine coefficients the same.  This is like a horizontal Translate
        /// that locks into place the center of the lower left corner of the image.
        /// </summary>
        [Category("General"), Description("Gets or sets the Y position of the lower left data cell.  " +
            "Setting this will adjust only the _affine[0] coefficient to ensure that the " +
            "lower left corner ends up in the specified location, while keeping all the " +
            "other affine coefficients the same.  This is like a horizontal Translate " +
            "that locks into place the center of the lower left corner of the image.")]
        public double Yllcenter
        {
            get
            {
                if (InternalDataSet != null) return InternalDataSet.Yllcenter;
                double[] affine = _bounds.AffineCoefficients;
                return affine[3] + affine[4] * .5 + affine[5] * (_numRows - .5);
            }
            set
            {
                if (InternalDataSet != null)
                {
                    InternalDataSet.Yllcenter = value;
                    return;
                }
                double[] affine = _bounds.AffineCoefficients;
                if(affine == null)
                {
                    affine = new double[6];
                    affine[1] = 1;
                    affine[3] = _numRows;
                }
                if (affine[5] == 0)
                {
                   // Cell Height can't be 0
                  affine[5] = -1;
                }
                affine[3] = value - affine[4] * .5 - affine[5] * (_numRows - .5);
            }
        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the FileExists method.  If this returns true, then the action should be cancelled.
        /// </summary>
        /// <param name="filename">The filename to write to</param>
        /// <returns>Boolean, true if the user doesn't want to overwrite</returns>
        protected bool OnFileExists(string filename)
        {
            if (FileExists != null)
            {
                MessageCancel mc = new MessageCancel(MessageStrings.FileExists_S.Replace("%S", filename));
                FileExists(this, mc);
                return mc.Cancel;
            }
            return false;
        }

        /// <summary>
        /// Gets the internal dataset as an IRaster
        /// </summary>
        protected new IRaster InternalDataSet
        {
            get { return base.InternalDataSet as IRaster; }
            set { base.InternalDataSet = value; }
        }

        #endregion


        #region Static Methods

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
        public static IRaster Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            return DataManager.DefaultDataManager.CreateRaster(name, driverCode, xSize, ySize, numBands, dataType, options);
        }

       



        /// <summary>
        /// For MapWindow style binary grids, this returns the filetype
        /// </summary>
        /// <param name="Filename">The filename with extension to test</param>
        /// <returns>A GridFileTypes enumeration listing which file type this is</returns>
        public static RasterFileTypes GetGridFileType(string Filename)
        {
            // Get the extension with period from the filename
            string Extension = System.IO.Path.GetExtension(Filename);

            // Compare the strings, ignoring case
            if (String.Compare(Extension, ".ASC", true) == 0) return RasterFileTypes.ASCII;
            if (String.Compare(Extension, ".ARC", true) == 0) return RasterFileTypes.ASCII;
            if (String.Compare(Extension, ".BGD", true) == 0) return RasterFileTypes.BINARY;
            if (String.Compare(Extension, ".FLT", true) == 0) return RasterFileTypes.FLT;
            if (String.Compare(Extension, ".ADF", true) == 0) return RasterFileTypes.ESRI;
            if (String.Compare(Extension, ".ECW", true) == 0) return RasterFileTypes.ECW;
            if (String.Compare(Extension, ".BIL", true) == 0) return RasterFileTypes.BIL;
            if (String.Compare(Extension, ".SID", true) == 0) return RasterFileTypes.MrSID;
            if (String.Compare(Extension, ".AUX", true) == 0) return RasterFileTypes.PAUX;
            if (String.Compare(Extension, ".PIX", true) == 0) return RasterFileTypes.PCIDsk;
            if (String.Compare(Extension, ".DHM", true) == 0) return RasterFileTypes.DTED;
            if (String.Compare(Extension, ".DT0", true) == 0) return RasterFileTypes.DTED;
            if (String.Compare(Extension, ".DT1", true) == 0) return RasterFileTypes.DTED;
            if (String.Compare(Extension, ".TIF", true) == 0) return RasterFileTypes.GeoTiff;
            if (String.Compare(Extension, ".IMG", true) == 0) return RasterFileTypes.BIL;
            if (String.Compare(Extension, ".DDF", true) == 0) return RasterFileTypes.SDTS;
            return RasterFileTypes.CUSTOM;
        }


        /// <summary>
        /// This instructs the default data manager to open an open file dialog with the dialog filter set to the RasterReadFilter.
        /// </summary>
        /// <returns></returns>
        public static IRaster OpenFile()
        {
            return DataManager.DefaultDataManager.OpenRaster();
        }

        /// <summary>
        /// This is significantly m
        /// </summary>
        /// <param name="filename">The string full path for the filename to open</param>
        /// <returns>A Raster object which is actually one of the type specific rasters, like FloatRaster.</returns>
        public static IRaster OpenFile(string filename)
        {

            // Ensure that the filename is valid
            if (filename == null) throw new NullLogException("filename");
            if (System.IO.File.Exists(filename) == false) throw new FileNotFoundLogException("filename");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(filename);
            return dataset as Raster;




        }

        /// <summary>
        /// Returns a native raster of the appropriate file type and data type by parsing the filename.
        /// </summary>
        /// <param name="filename">The string filename to attempt to open with a native format</param>
        /// <param name="inRam">The boolean value.</param>
        /// <returns>An IRaster which has been opened to the specified file.</returns>
        public static IRaster OpenFile(string filename, bool inRam)
        {
            // Ensure that the filename is valid
            if (filename == null) throw new NullLogException("filename");
            if (System.IO.File.Exists(filename) == false) throw new FileNotFoundLogException("filename");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(filename, inRam);
            return dataset as Raster;
        }

        /// <summary>
        /// Returns a native raster of the appropriate file type and data type by parsing the filename.
        /// </summary>
        /// <param name="filename">The string filename to attempt to open with a native format</param>
        /// <param name="inRam">The boolean value.</param>
        /// <param name="progressHandler">An overriding progress manager for this process</param>
        /// <returns>An IRaster which has been opened to the specified file.</returns>
        public static IRaster OpenFile(string filename, bool inRam, IProgressHandler progressHandler)
        {
            if (filename == null) throw new NullLogException("filename");
            if (System.IO.File.Exists(filename) == false) throw new FileNotFoundLogException("filename");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(filename, inRam, progressHandler);
            return dataset as Raster;
        }



       

        #endregion

       




    }
}
