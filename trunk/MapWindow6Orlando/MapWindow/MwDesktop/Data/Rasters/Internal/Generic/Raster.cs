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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 4:01:55 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MapWindow.Components;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Main;
namespace MapWindow.Data
{


    /// <summary>
    /// Raster
    /// </summary>
    public class Raster<T> : Raster where T : IEquatable<T>, IComparable<T>, IConvertible
    {
        #region Private Variables

        /// <summary>
        /// The actual data values, stored as a jagged array of values of type T
        /// </summary>
        public T[][] Data;




        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Raster
        /// </summary>
        public Raster()
        {

        }

        

        /// <summary>
        /// Creates an raster of data type T
        /// </summary>
        /// <param name="numRows">The number of rows in the raster</param>
        /// <param name="numColumns">The number of columns in the raster</param>
        public Raster(int numRows, int numColumns)
        {
            base.NumRows = numRows;
            base.NumColumns = numColumns;
            base.IsInRam = true;

            if (numRows * numColumns > 64000000)
            {
                base.NumRowsInFile = numRows;
                base.NumColumnsInFile = numColumns;
                base.IsInRam = false;
                base.Bounds = new RasterBounds(numRows, numColumns, new[] { 0.5, 1.0, 0.0, numRows - .5, 0.0, -1.0 });
                base.NoDataValue = Global.MinimumValue<T>().ToDouble(null); // sets the no-data value to the minimum value for the specified type.
                base.Value = new ValueGrid<T>(this);
                return;
            }
            

            Initialize();
        }

        /// <summary>
        /// Used especially by the "save as" situation, this simply creates a new reference pointer for the actual data values.
        /// </summary>
        /// <param name="original"></param>
        protected override void SetData(IRaster original)
        {
            Raster<T> temp = original as Raster<T>;
            if (temp == null) return;
            Data = temp.Data;
            Value = temp.Value;
        }

        /// <summary>
        /// Calls the basic setup for the raster
        /// </summary>
        protected void Initialize()
        {
            StartRow = 0;
            EndRow = NumRows - 1;
            StartColumn = 0;
            EndColumn = NumColumns - 1;
            NumColumnsInFile = NumColumns;
            NumRowsInFile = NumRows;

            // Just set the cell size to one
            Bounds = new RasterBounds(NumRows, NumColumns, new[] { 0.5, 1.0, 0.0, NumRows - .5, 0.0, -1.0 });
            NumValueCells = 0;
            if (IsInRam)
            {
                Data = new T[NumRows][];
                for (int row = 0; row < NumRows; row++)
                {
                    Data[row] = new T[NumColumns];
                }
            }
            Value = new ValueGrid<T>(this);
            NoDataValue = Global.MinimumValue<T>().ToDouble(null); // Sets it to the appropriate minimum for the int datatype
            DataType = Global.GetNumericType(default(T));
        }





        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep copy of this raster object so that the data values can be manipulated without
        /// interfering with the original raster.
        /// </summary>
        /// <returns></returns>
        public new Raster<T> Copy()
        {
            Raster<T> copy = MemberwiseClone() as Raster<T>;
            if (copy == null) return null;
            copy.Bounds = Bounds.Copy();
            copy.Data = new T[NumRows][];
            for (int row = 0; row < NumRows; row++)
            {
                copy.Data[row] = new T[NumColumns];
                for (int col = 0; col < NumColumns; col++)
                {
                    copy.Data[row][col] = Data[row][col];
                }
            }
            copy.Value = new ValueGrid<T>(copy);
            copy.Bands = new List<IRaster>();
            foreach (IRaster band in base.Bands)
            {
                if (band != this)
                    copy.Bands.Add(band.Copy());
                else
                    copy.Bands.Add(copy);
            }
            return copy;
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
        public IRaster CopyWindow(string filename, int startRow, int endRow, int startColumn, int endColumn,
                                           bool copyValues, bool inRam)
        {
            if (inRam == false || (endColumn - startColumn + 1) * (endRow - startRow + 1) > 64000000)
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            if (IsInRam == false)
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;


            Raster<T> result = new Raster<T>(numRows, numCols);

            result.Projection = Projection;

            // The affine coefficients defining the world file are the same except that they are translated over.  Only the position of the
            // upper left corner changes.  Everything else is the same as the previous raster.

            // X = [0] + [1] * column + [2] * row;
            // Y = [3] + [4] * column + [5] * row;
            result.Bounds.AffineCoefficients = new double[6];
            result.Bounds.AffineCoefficients[0] = Bounds.AffineCoefficients[0] +
                                                  Bounds.AffineCoefficients[1] * startColumn +
                                                  Bounds.AffineCoefficients[2] * startRow;
            result.Bounds.AffineCoefficients[1] = Bounds.AffineCoefficients[1];
            result.Bounds.AffineCoefficients[2] = Bounds.AffineCoefficients[2];
            result.Bounds.AffineCoefficients[3] = Bounds.AffineCoefficients[3] +
                                                  Bounds.AffineCoefficients[4] * startColumn +
                                                  Bounds.AffineCoefficients[5] * startRow;
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
            result.Value = new ValueGrid<T>(result);
            return result;
        }

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.  
        /// This should be a little faster since we are accessing the Data field directly instead of working 
        /// through a value parameter.
        /// </summary>
        public virtual float[][] CreateHillShade(IShadedRelief shadedRelief, IProgressHandler progressHandler)
        {
            if (Data == null) return null;

            int numCols = NumColumns;
            int numRows = NumRows;
            float noData = Convert.ToSingle(NoDataValue);
            float extrusion = shadedRelief.Extrusion;
            float elevationFactor = shadedRelief.ElevationFactor;
            float lightIntensity = shadedRelief.LightIntensity;
            float ambientIntensity = shadedRelief.AmbientIntensity;
            FloatVector3 lightDirection = shadedRelief.GetLightDirection();
           

            float[] aff = new float[6]; // affine coefficients converted to float format
            for (int i = 0; i < 6; i++)
            {
                aff[i] = Convert.ToSingle(Bounds.AffineCoefficients[i]);
            }
            float[][] hillshade = new float[numRows][];

            ProgressMeter pm = new ProgressMeter(progressHandler, MessageStrings.CreatingShadedRelief, numRows);
            if (NumRows * NumColumns < 100000) pm.StepPercent = 50;
            if (NumRows * NumColumns < 500000) pm.StepPercent = 10;
            if (NumRows * NumColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < numRows; row++)
            {
                hillshade[row] = new float[numCols];

                for (int col = 0; col < numCols; col++)
                {
                    // 3D position vectors of three points to create a triangle.
                    FloatVector3 v1 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v2 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v3 = new FloatVector3(0f, 0f, 0f);

                    T val = Data[row][col];
                    // Cannot compute polygon ... make the best guess
                    if (col >= numCols - 1 || row <= 0)
                    {
                        if (col >= numCols - 1 && row <= 0)
                        {
                            v1.Z = val.ToSingle(null);
                            v2.Z = val.ToSingle(null);
                            v3.Z = val.ToSingle(null);
                        }
                        else if (col >= numCols - 1)
                        {
                            v1.Z = Data[row][col - 1].ToSingle(null); // 3 - 2
                            v2.Z = Data[row - 1][col].ToSingle(null); // | /
                            v3.Z = Data[row - 1][col - 1].ToSingle(null); // 1   *
                        }
                        else if (row <= 0)
                        {
                            v1.Z = Data[row + 1][col].ToSingle(null); //  3* 2
                            v2.Z = Data[row][col + 1].ToSingle(null); //  | /
                            v3.Z = val.ToSingle(null); //  1
                        }
                    }
                    else
                    {
                        v1.Z = val.ToSingle(null); //  3 - 2
                        v2.Z = Data[row - 1][col + 1].ToSingle(null); //  | /
                        v3.Z = Data[row - 1][col].ToSingle(null); //  1*
                    }

                    // Test for no-data values and don't calculate hillshade in that case
                    if (v1.Z == noData || v2.Z == noData || v3.Z == noData)
                    {
                        hillshade[row][col] = -1; // should never be negative otherwise.
                        continue;
                    }
                    // Apply the Conversion Factor to put elevation into the same range as lat/lon
                    v1.Z = v1.Z * elevationFactor * extrusion;
                    v2.Z = v2.Z * elevationFactor * extrusion;
                    v3.Z = v3.Z * elevationFactor * extrusion;

                    // Complete the vectors using the latitude/longitude coordinates
                    v1.X = aff[0] + aff[1] * col + aff[2] * row;
                    v1.Y = aff[3] + aff[4] * col + aff[5] * row;

                    v2.X = aff[0] + aff[1] * (col + 1) + aff[2] * (row + 1);
                    v2.Y = aff[3] + aff[4] * (col + 1) + aff[5] * (row + 1);

                    v3.X = aff[0] + aff[1] * col + aff[2] * (row + 1);
                    v3.Y = aff[3] + aff[4] * col + aff[5] * (row + 1);

                    // We need two direction vectors in order to obtain a cross product
                    FloatVector3 dir2 = FloatVector3.Subtract(v2, v1); // points from 1 to 2
                    FloatVector3 dir3 = FloatVector3.Subtract(v3, v1); // points from 1 to 3

                    FloatVector3 cross = FloatVector3.CrossProduct(dir3, dir2);
                    // right hand rule - cross direction should point into page... reflecting more if light direction is in the same direction

                    // Normalizing this vector ensures that this vector is a pure direction and won't affect the intensity
                    cross.Normalize();

                    // Hillshade now has an "intensity" modifier that should be applied to the R, G and B values of the color found at each pixel.
                    hillshade[row][col] = FloatVector3.Dot(cross, lightDirection) * lightIntensity + ambientIntensity;
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            // Setting this indicates that a hillshade has been created more recently than characteristics have been changed.
            shadedRelief.HasChanged = false;
            return hillshade;
        }

        /// <summary>
        /// Creates a bitmap based on the specified RasterSymbolizer
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MapWindow.Main.NullLogException">rasterSymbolizer cannot be null</exception>
        public virtual void DrawToBitmap(IRasterSymbolizer rasterSymbolizer, Bitmap bitmap,
                                         IProgressHandler progressHandler)
        {
            BitmapData bmpData;
            if (Data == null) return;

            if (rasterSymbolizer == null)
                throw new NullLogException("rasterSymbolizer");

            if (rasterSymbolizer.Scheme == null || rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.

            Rectangle rect = new Rectangle(0, 0, NumColumns, NumRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "format")
                    throw new BitmapFormatException();
                LogManager.DefaultLogManager.Exception(ex);
                throw;
            }
            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);

            bool useHillShade = false;
            float[][] hillshade = rasterSymbolizer.HillShade;
            if (rasterSymbolizer.ShadedRelief.IsUsed)
            {
                hillshade = rasterSymbolizer.HillShade;
                useHillShade = true;
            }
            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, MessageStrings.CreatingTexture, NumRows);
            if (NumRows * NumColumns < 100000) pm.StepPercent = 50;
            if (NumRows * NumColumns < 500000) pm.StepPercent = 10;
            if (NumRows * NumColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    // use the colorbreaks to calculate the colors
                    pixelColor = rasterSymbolizer.GetColor(Data[row][col].ToDouble(null));

                    // control transparency here
                    int alpha = Convert.ToInt32(rasterSymbolizer.Opacity * 255);
                    if (alpha > 255) alpha = 255;
                    if (alpha < 0) alpha = 0;
                    byte a = (byte)alpha;


                    byte r, g, b;
                    if (useHillShade && hillshade != null)
                    {
                        if (hillshade[row][col] == -1)
                        {
                            pixelColor = rasterSymbolizer.NoDataColor;
                            r = pixelColor.R;
                            g = pixelColor.G;
                            b = pixelColor.B;
                        }
                        else
                        {
                            int red = Convert.ToInt32(pixelColor.R * hillshade[row][col]);
                            int green = Convert.ToInt32(pixelColor.G * hillshade[row][col]);
                            int blue = Convert.ToInt32(pixelColor.B * hillshade[row][col]);
                            if (red > 255) red = 255;
                            if (green > 255) green = 255;
                            if (blue > 255) blue = 255;
                            if (red < 0) red = 0;
                            if (green < 0) green = 0;
                            if (blue < 0) blue = 0;
                            b = (byte)blue;
                            r = (byte)red;
                            g = (byte)green;
                        }
                    }
                    else
                    {
                        r = pixelColor.R;
                        g = pixelColor.G;
                        b = pixelColor.B;
                    }

                    int offset = row * bmpData.Stride + col * 4;
                    rgbData[offset] = b;
                    rgbData[offset + 1] = g;
                    rgbData[offset + 2] = r;
                    rgbData[offset + 3] = a;
                }
                pm.CurrentValue = row;
            }
            pm.Reset();

            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother sm = new Smoother(bmpData, rgbData, progressHandler);
                rgbData = sm.Smooth();
            }
            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);

            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }

        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified
        /// </summary>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once</param>
        /// <param name="progressHandler">An IProgressHandler</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use</param>
        /// <exception cref="MapWindow.Main.NullLogException">rasterSymbolizer cannot be null</exception>
        public virtual void PaintColorSchemeToBitmap(IRasterSymbolizer rasterSymbolizer, Bitmap bitmap,
                                                     IProgressHandler progressHandler)
        {
            Debug.WriteLine("IntRaster - PaintColorSchemeToBitamp");
            BitmapData bmpData;
            if (Data == null) return;

            if (rasterSymbolizer == null)
                throw new NullLogException("rasterSymbolizer");

            if (rasterSymbolizer.Scheme == null || rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.

            Rectangle rect = new Rectangle(0, 0, NumColumns, NumRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "format")
                    throw new BitmapFormatException();
                LogManager.DefaultLogManager.Exception(ex);
                throw;
            }
            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);


            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, MessageStrings.PaintingColorScheme, NumRows);
            if (NumRows * NumColumns < 100000) pm.StepPercent = 50;
            if (NumRows * NumColumns < 500000) pm.StepPercent = 10;
            if (NumRows * NumColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    // use the colorbreaks to calculate the colors
                    pixelColor = rasterSymbolizer.GetColor(Data[row][col].ToDouble(null));

                    // control transparency here
                    int alpha = Convert.ToInt32(rasterSymbolizer.Opacity * 255);
                    if (alpha > 255) alpha = 255;
                    if (alpha < 0) alpha = 0;
                    byte a = (byte)alpha;


                    byte r = pixelColor.R;
                    byte g = pixelColor.G;
                    byte b = pixelColor.B;

                    int offset = row * bmpData.Stride + col * 4;
                    rgbData[offset] = b;
                    rgbData[offset + 1] = g;
                    rgbData[offset + 2] = r;
                    rgbData[offset + 3] = a;
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }


        /// <summary>
        /// Gets the statistics all the values.  If the entire content is not currently in-ram,
        /// ReadRow will be used to read individual lines and performing the calculations.
        /// </summary>
        public override void GetStatistics()
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, MessageStrings.CalculatingStatistics, NumRows);

            T min = Global.MaximumValue<T>();
            T max = Global.MinimumValue<T>();

            double total = 0;
            double sqrTotal = 0;
            int count = 0;

            if (IsInRam == false || this.IsFullyWindowed() == false)
            {
                for (int row = 0; row < NumRowsInFile; row++)
                {
                    T[] values = ReadRow(row);
                    for (int col = 0; col < NumColumnsInFile; col++)
                    {
                        T val = values[col];
                        double dblVal = val.ToDouble(null);
                        if (dblVal == NoDataValue) continue;
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                    pm.CurrentValue = row;
                }
            }
            else
            {
                for (int row = 0; row < NumRows; row++)
                {
                    for (int col = 0; col < NumColumns; col++)
                    {
                        T val = Data[row][col];
                        double dblVal = val.ToDouble(null);
                        if (dblVal == NoDataValue) continue;
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                    pm.CurrentValue = row;
                }
            }

            
            double test = min.ToDouble(null);
            base.Minimum = test;
            base.Maximum = max.ToDouble(null);
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
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
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public IRaster GetWindow(int startRow, int endRow, int startColumn, int endColumn, bool inRam)
        {
            if (IsInRam == false)
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            if (startRow < StartRow || endRow > EndRow || StartColumn < startColumn || EndColumn > endColumn)
            {
                // the requested extents are outside of the extents that have been windowed into ram.  File Handling is required.
                throw new ArgumentException(MessageStrings.RasterRequiresCast);
            }

            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;
            Raster<T> result = new Raster<T>(numRows, numCols);
            result.Filename = Filename;
            result.Projection = Projection;
            result.DataType = typeof(int);
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
            result.Bounds.AffineCoefficients = new double[6];
            result.Bounds.AffineCoefficients[0] = Bounds.AffineCoefficients[0] +
                                                  Bounds.AffineCoefficients[1] * startColumn +
                                                  Bounds.AffineCoefficients[2] * startRow;
            result.Bounds.AffineCoefficients[1] = Bounds.AffineCoefficients[1];
            result.Bounds.AffineCoefficients[2] = Bounds.AffineCoefficients[2];
            result.Bounds.AffineCoefficients[3] = Bounds.AffineCoefficients[3] +
                                                  Bounds.AffineCoefficients[4] * startColumn +
                                                  Bounds.AffineCoefficients[5] * startRow;
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

            result.Value = new ValueGrid<T>(result);
            return result;
        }

        /// <summary>
        /// Obtains only the statistics for the small window specified by startRow, endRow etc.
        /// this only works if the window is also InRam.
        /// </summary>
        public void GetWindowStatistics()
        {
            if (IsInRam == false)
                throw new ArgumentException(MessageStrings.RasterRequiresCast);


            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Calculating Statistics.", NumRows);


            double total = 0;
            double sqrTotal = 0;
            int count = 0;
            T min = Global.MaximumValue<T>();
            T max = Global.MinimumValue<T>();

            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    T val = Data[row][col];
                    double dblVal = val.ToDouble(null);
                    if (dblVal != NoDataValue)
                    {
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                }
                pm.CurrentValue = row;
            }
            Minimum = min.ToDouble(null);
            Maximum = max.ToDouble(null);
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
            Value.Updated = false;
        }

        /// <summary>
        /// Prevent nested opening.
        /// </summary>
        public override void Open()
        {
            Open(Filename);
        }

        /// <summary>
        /// Prevent the base raster "factory" style open function from working in subclasses.
        /// </summary>
        /// <param name="filename"></param>
        public override void  Open(string filename)
        {
            if(IsInRam)
            {
                Data = ReadRaster();
                GetStatistics();
            }
            
        }

        /// <summary>
        /// This saves content from memory stored in the Data field to the file using whatever
        /// file format the file already exists as.
        /// </summary>
        public override void Save()
        {
            UpdateHeader();
            WriteRaster(Data);
        }

      
        /// <summary>
        /// Reads a specific 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public T[] ReadRow(int row)
        {
            return ReadRaster(0, row, NumColumns, 1)[0];
        }
        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This assumes 0 offsets, the size of the entire image, and 0 for the pixel or line space.
        /// </summary>
        /// <returns>An array of values of type T, in row major order</returns>
        public T[][] ReadRaster()
        {
            return ReadRaster(0, 0, NumColumns, NumRows);
        }

     
        /// <summary>
        /// This Method should be overrridden by classes, and provides the primary ability
        /// </summary>
        /// <param name="xOff">The horizontal offset of the area to read values from</param>
        /// <param name="sizeX">The number of values to read into the buffer</param>
        /// <param name="yOff">The vertical offset of the window to read values from</param>
        /// <param name="sizeY">The vertical size of the window to read into the buffer</param>
        /// <returns></returns>
        public virtual T[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

        /// <summary>
        /// Reads a specific 
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <param name="row">The integer row to write to the raster</param>
        /// <returns></returns>
        public void WriteRow(T[] buffer, int row)
        {
            T[][] bufferJagged = new T[1][];
            bufferJagged[0] = buffer;
            WriteRaster(bufferJagged, 0, row, NumColumns, 1);
        }
        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This assumes 0 offsets, the size of the entire image, and 0 for the pixel or line space.
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <returns>An array of values of type T, in row major order</returns>
        public void WriteRaster(T[][] buffer)
        {
            WriteRaster(buffer, 0, 0, NumColumns, NumRows);
        }

        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This specifies a window where the xSize and ySize specified and 0 is used for the pixel and line space.
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <param name="xOff">The horizontal offset of the area to read values from</param>
        /// <param name="xSize">The number of values to read into the buffer</param>
        /// <param name="yOff">The vertical offset of the window to read values from</param>
        /// <param name="ySize">The vertical size of the window to read into the buffer</param>
        /// <returns>An array of values of type T, in row major order</returns>
        public virtual void WriteRaster(T[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

       
        /// <summary>
        /// During a save opperation, this instructs the program to perform any writing that is not handled by
        /// the write raster content.
        /// </summary>
        protected virtual void UpdateHeader()
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }


        #endregion

        #region Properties

        /// <summary>
        /// This only works for a few numeric types, and will return 0 if it is not identifiable as one
        /// of these basic types: byte, short, int, long, float, double, decimal, UInt16, UInt32, UInt64, 
        /// </summary>
        public override int ByteSize
        {
            get
            {
                return GetByteSize(default(T));
            }
        }


        private static int GetByteSize(object value)
        {
            if (value is byte) return 1;
            if (value is short) return 2;
            if (value is int) return 4;
            if (value is long) return 8;
            if (value is float) return 4;
            if (value is double) return 8;
            if (value is decimal) return 16;
            if (value is UInt16) return 2;
            if (value is UInt32) return 4;
            if (value is UInt64) return 8;
            
            return 0;
        }


        #endregion



    }
}
