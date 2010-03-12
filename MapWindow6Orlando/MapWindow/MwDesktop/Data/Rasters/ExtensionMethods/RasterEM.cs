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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/12/2009 5:30:52 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Main;

namespace MapWindow.Data
{


    /// <summary>
    /// A new model, now that we support 3.5 framework and extension methods that are essentially
    /// derived characteristics away from the IRaster interface, essentially reducing it
    /// to the simplest interface possible for future implementers, while extending the most
    /// easy-to-find functionality to the users.
    /// </summary>
    public static class RasterEM
    {
       
        #region Methods

        

        #region GeoReference

        /// <summary>
        /// This doesn't change the data, but instead performs a translation where the upper left coordinate
        /// is specified in world coordinates.
        /// </summary>
        /// <param name="raster">Moves this raster so that the upper left coordinate will match the specified position.  The skew and cellsize will remain unaltered</param>
        /// <param name="position">The location to move the upper left corner of the raster to in world coordinates.</param>
        public static void MoveTo(this IRaster raster, Coordinate position)
        {
            double[] vals = raster.Bounds.AffineCoefficients;
            vals[0] = position.X;
            vals[3] = position.Y;
        }


        /// <summary>
        /// Rotates the geospatial reference points for this image by rotating the affine coordinates.
        /// The center for this rotation will be the center of the image.
        /// </summary>
        /// <param name="raster">The raster to rotate</param>
        /// <param name="degrees">The angle in degrees to rotate the image counter clockwise.</param>
        public static void Rotate(this IRaster raster, float degrees)
        {
         
            System.Drawing.Drawing2D.Matrix m = raster.Bounds.Get_AffineMatrix();
            m.Rotate(degrees);
            raster.Bounds.Set_AffineMatrix(m);
        }
        /// <summary>
        /// Rotates the geospatial reference points for this image by rotating the affine coordinates.
        /// The center for this rotation will be the center of the image.
        /// </summary>
        /// <param name="raster">The raster to rotate about the specified coordinate</param>
        /// <param name="degrees">The angle in degrees to rotate the image counterclockwise.</param>
        /// <param name="center">The point that marks the center of the desired rotation in geographic coordiantes.</param>
        public static void RotateAt(this IRaster raster, float degrees, Coordinate center)
        {

            System.Drawing.Drawing2D.Matrix m = raster.Bounds.Get_AffineMatrix();
            m.RotateAt(degrees, new PointF(Convert.ToSingle(center.X), Convert.ToSingle(center.Y)));
            raster.Bounds.Set_AffineMatrix(m);
        }


        /// <summary>
        /// This method uses a matrix transform to adjust the scale.  The precision of using
        /// a System.Drawing.Drawing2D transform is float precision, so some accuracy may be lost.
        /// </summary>
        /// <param name="raster">The raster to apply the scale transform to</param>
        /// <param name="scaleX">The multiplier to adjust the geographic extents of the raster in the X direction</param>
        /// <param name="scaleY">The multiplier to adjust the geographic extents of the raster in the Y direction</param>
        public static void Scale(this IRaster raster, float scaleX, float scaleY)
        {

            System.Drawing.Drawing2D.Matrix m = raster.Bounds.Get_AffineMatrix();
            m.Scale(scaleX, scaleY);
            raster.Bounds.Set_AffineMatrix(m);
        }


        /// <summary>
        /// This method uses a matrix transform to adjust the shear.  The precision of using
        /// a System.Drawing.Drawing2D transform is float precision, so some accuracy may be lost.
        /// </summary>
        /// <param name="raster">The raster to apply the transform to</param>
        /// <param name="shearX">The floating point horizontal shear factor</param>
        /// <param name="shearY">The floating ponit vertical shear factor</param>
        public static void Shear(this IRaster raster, float shearX, float shearY)
        {
            System.Drawing.Drawing2D.Matrix m = raster.Bounds.Get_AffineMatrix();
            m.Shear(shearX, shearY);
            raster.Bounds.Set_AffineMatrix(m);
        }

        /// <summary>
        /// Applies a translation transform to the georeferenced coordinates on this raster.
        /// </summary>
        /// <param name="raster">The raster to apply the translation to</param>
        /// <param name="shift">An ICoordinate with shear values</param>
        public static void Translate(this IRaster raster, Coordinate shift)
        {
            double[] affine = raster.Bounds.AffineCoefficients;
            affine[0] += shift.X;
            affine[3] += shift.Y;
        }

        #endregion


        #region Nearest Values

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates.  This will
        /// return a No-Data value if the specified coordintes are outside of the grid.
        /// </summary>
        /// <param name="raster">The raster to get the value from</param>
        /// <param name="location">A valid implementation of Icoordinate specifying the geographic location.</param>
        /// <returns>The value of type T of the cell that has a center closest to the specified coordinates</returns>
        public static double GetNearestValue(this IRaster raster, Coordinate location)
        {
            RcIndex position = raster.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return raster.NoDataValue;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return raster.NoDataValue;
            return raster.Value[position.Row, position.Column];
        }

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates.  This will
        /// return a No-Data value if the specified coordintes are outside of the grid.
        /// </summary>
        /// <param name="raster">The raster to get the value from</param>
        /// <param name="x">The longitude or horizontal coordinate</param>
        /// <param name="y">The latitude or vertical coordinate</param>
        /// <returns>The double value of the cell that has a center closest to the specified coordinates</returns>
        public static double GetNearestValue(this IRaster raster, double x, double y)
        {
            RcIndex position = raster.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return raster.NoDataValue;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return raster.NoDataValue;
            return raster.Value[position.Row, position.Column];
        }

        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates.  This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="raster">The IRaster to set the value for</param>
        /// <param name="x">The longitude or horizontal coordinate</param>
        /// <param name="y">The latitude or vertical coordinate</param>
        /// <param name="value">The value to assign to the nearest cell to the specified location</param>
        public static void SetNearestValue(this IRaster raster, double x, double y, double value)
        {
       
            RcIndex position = raster.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return;
            raster.Value[position.Row, position.Column] = value;
        }

        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates.  This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="raster">The IRaster to set the value for</param>
        /// <param name="location">An Icoordinate specifying the location</param>
        /// <param name="value">The value to assign to the nearest cell to the specified location</param>
        public static void SetNearestValue(this IRaster raster, Coordinate location, double value)
        {
            RcIndex position = raster.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return;
            raster.Value[position.Row, position.Column] = value;
        }

        #endregion

        #region Projection


        /// <summary>
        /// Extends the IRaster interface to return the coordinate of the center of a row column position.
        /// </summary>
        /// <param name="raster">The raster interface to extend</param>
        /// <param name="position">The zero based integer index of the row and column of the cell to locate</param>
        /// <returns>The geographic location of the center of the specified cell</returns>
        public static Coordinate CellToProj(this IRaster raster, RcIndex position)
        {
            if (raster == null) return null;
            if (raster.Bounds == null) return null;
            return raster.Bounds.CellCenter_ToProj(position.Row, position.Column);
        }

        /// <summary>
        /// Extends the IRaster interface to return the coordinate of the center of a row column position.
        /// </summary>
        /// <param name="raster">The raster interface to extend</param>
        /// <param name="col">The zero based integer index of the column of the cell to locate</param>
        /// <param name="row">The zero based integer index of the row of the cell to locate</param>
        /// <returns>The geographic location of the center of the specified cell</returns>
        public static Coordinate CellToProj(this IRaster raster, int row, int col)
        {
            if (raster == null) return null;
            if (raster.Bounds == null) return null;
            return raster.Bounds.CellCenter_ToProj(row, col);
        }

        /// <summary>
        /// Extends the IRaster interface to return the zero based integer row and column indices
        /// </summary>
        /// <param name="raster">The raster interface to extend</param>
        /// <param name="location">The geographic coordinate describing the latitude and longitude</param>
        /// <returns>The RcIndex that describes the zero based integer row and column indices</returns>
        public static RcIndex ProjToCell(this IRaster raster, Coordinate location)
        {
            if (raster == null) return RcIndex.Empty;
            if (raster.Bounds == null) return RcIndex.Empty;
            return raster.Bounds.ProjToCell(location);
        }

        /// <summary>
        /// Extends the IRaster interface to return the zero based integer row and column indices
        /// </summary>
        /// <param name="raster">The raster interface to extend</param>
        /// <param name="x">A double precision floating point describing the longitude</param>
        /// <param name="y">A double precision floating point describing the latitude</param>
        /// <returns>The RcIndex that describes the zero based integer row and column indices</returns>
        public static RcIndex ProjToCell(this IRaster raster, double x, double y)
        {
            if (raster == null) return RcIndex.Empty;
            if (raster.Bounds == null) return RcIndex.Empty;
            return raster.Bounds.ProjToCell(new Coordinate(x, y));
        }


        #endregion

        #region Statistics



        #endregion

        #region Symbology

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.
        /// This uses the progress handler defined on this raster.
        /// </summary>
        /// <param name="raster">The raster to create hillshade information for</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        public static float[][] CreateHillShade(this IRaster raster, IShadedRelief shadedRelief)
        {
            return CreateHillShade(raster, shadedRelief, raster.ProgressHandler);
        }

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.  
        /// This should be a little faster since we are accessing the Data field directly instead of working 
        /// through a value parameter.
        /// </summary>
        /// <param name="raster">The raster to create the hillshade from.</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        /// <param name="progressHandler">An implementation of IProgressHandler for progress messages</param>
        public static float[][] CreateHillShade(this IRaster raster, IShadedRelief shadedRelief, IProgressHandler progressHandler)
        {
           
            int numCols = raster.NumColumns;
            int numRows = raster.NumRows;
            double noData = raster.NoDataValue;
            float extrusion = shadedRelief.Extrusion;
            float elevationFactor = shadedRelief.ElevationFactor;
            float lightIntensity = shadedRelief.LightIntensity;
            float ambientIntensity = shadedRelief.AmbientIntensity;
            FloatVector3 lightDirection = shadedRelief.GetLightDirection();
           


            float[] aff = new float[6]; // affine coefficients converted to float format
            for (int i = 0; i < 6; i++)
            {
                aff[i] = Convert.ToSingle(raster.Bounds.AffineCoefficients[i]);
            }
            float[][] hillshade = new float[numRows][];

            ProgressMeter pm = new ProgressMeter(progressHandler, MessageStrings.CreatingShadedRelief, numRows);
            for (int row = 0; row < numRows; row++)
            {
                hillshade[row] = new float[numCols];

                for (int col = 0; col < numCols; col++)
                {
                    // 3D position vectors of three points to create a triangle.
                    FloatVector3 v1 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v2 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v3 = new FloatVector3(0f, 0f, 0f);

                    double val = raster.Value[row, col];
                    // Cannot compute polygon ... make the best guess
                    if (col >= numCols - 1 || row <= 0)
                    {
                        if (col >= numCols - 1 && row <= 0)
                        {
                            v1.Z = (float)val;
                            v2.Z = (float)val;
                            v3.Z = (float)val;
                        }
                        else if (col >= numCols - 1)
                        {
                            v1.Z = (float)raster.Value[row, col - 1];        // 3 - 2
                            v2.Z = (float)raster.Value[row - 1, col];        // | /
                            v3.Z = (float)raster.Value[row - 1, col - 1];    // 1   *
                        }
                        else if (row <= 0)
                        {
                            v1.Z = (float)raster.Value[row + 1, col];         //  3* 2
                            v2.Z = (float)raster.Value[row, col + 1];         //  | /
                            v3.Z = (float)val;                         //  1
                        }
                    }
                    else
                    {
                        v1.Z = (float)val;                              //  3 - 2
                        v2.Z = (float)raster.Value[row - 1, col + 1];          //  | /
                        v3.Z = (float)raster.Value[row - 1, col];              //  1*
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


                    FloatVector3 cross = FloatVector3.CrossProduct(dir3, dir2); // right hand rule - cross direction should point into page... reflecting more if light direction is in the same direction

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
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to the bitmap based on the layout specified in the rasterSymbolizer</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="bitmap">This must be an Format32bbpArgb bitmap that has already been saved to a file so that it exists.</param>
        /// <returns>A System.Drawing.Bitmap if the operation was successful or null.</returns>
        /// <exception cref="MapWindow.Main.NullLogException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap)
        {
            DrawToBitmap(raster, rasterSymbolizer, bitmap, raster.ProgressHandler);
        }

        /// <summary>
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to a bitmap</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="bitmap">This must be an Format32bbpArgb bitmap that has already been saved to a file so that it exists.</param>
        /// <param name="progressHandler">The progress handler to use.</param>
        /// <returns>A System.Drawing.Bitmap if the operation was successful or null.</returns>
        /// <exception cref="MapWindow.Main.NullLogException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
        {
         
            System.Drawing.Imaging.BitmapData bmpData;
            
            if (rasterSymbolizer == null)
            {
                throw new NullLogException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            Rectangle rect = new Rectangle(0, 0, raster.NumColumns, raster.NumRows);
            try
            {
                bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            catch(Exception ex)
            {
                string originalError = ex.ToString();
                // if they have not saved the bitmap yet, it can cause an exception
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                // any further exceptions should simply throw exceptions to allow easy debugging
            }

            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);

            bool useHillShade = false;
            float[][] hillshade = rasterSymbolizer.HillShade;
            if (rasterSymbolizer.ShadedRelief.IsUsed)
            {
                hillshade = rasterSymbolizer.HillShade;
                useHillShade = true;
            }
            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, "Recreating Bitmap", raster.NumRows);

            try
            {
                for (int row = 0; row < raster.NumRows; row++)
                {
                    for (int col = 0; col < raster.NumColumns; col++)
                    {
                        // use the colorbreaks to calculate the colors
                        pixelColor = rasterSymbolizer.GetColor(raster.Value[row, col]);

                        // control transparency here
                        float alpha = rasterSymbolizer.Opacity * 255f;
                        if (alpha > 255f) alpha = 255f;
                        if (alpha < 0f) alpha = 0f;
                        byte a = Convert.ToByte(alpha);
                        byte g;
                        byte r;
                        byte b;
                        if (useHillShade && hillshade != null)
                        {
                            if (hillshade[row][col] == -1 || float.IsNaN(hillshade[row][col]))
                            {
                                pixelColor = rasterSymbolizer.NoDataColor;
                                r = pixelColor.R;
                                g = pixelColor.G;
                                b = pixelColor.B;
                            }
                            else
                            {
                                float red = pixelColor.R * hillshade[row][col];
                                float green = pixelColor.G * hillshade[row][col];
                                float blue = pixelColor.B * hillshade[row][col];
                                if (red > 255f) red = 255f;
                                if (green > 255f) green = 255f;
                                if (blue > 255f) blue = 255f;
                                if (red < 0f) red = 0f;
                                if (green < 0f) green = 0f;
                                if (blue < 0f) blue = 0f;
                                b = Convert.ToByte(blue);
                                r = Convert.ToByte(red);
                                g = Convert.ToByte(green);

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
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine(" Unable to write data to raster.");
            }
            pm.Reset();
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(bmpData, rgbData, progressHandler);
                rgbData = mySmoother.Smooth();
            }


            // Copy the values back into the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }


        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified
        /// </summary>
        /// <param name="raster">The Raster containing values that need to be drawn to the bitmap as a color scheme.</param>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once</param>
        /// <param name="progressHandler">An IProgressHandler</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use</param>
        /// <exception cref="MapWindow.Main.NullLogException">rasterSymbolizer cannot be null</exception>
        public static void PaintColorSchemeToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
        {
         
            System.Drawing.Imaging.BitmapData bmpData;
            int numRows = raster.NumRows;
            int numColumns = raster.NumColumns;
            if (rasterSymbolizer == null)
            {
                throw new NullLogException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.

            Rectangle rect = new Rectangle(0, 0, numColumns, numRows);
            try
            {
                bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            catch
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);

            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, MessageStrings.PaintingColorScheme, raster.NumRows);
            if (numRows * numColumns < 100000) pm.StepPercent = 50;
            if (numRows * numColumns < 500000) pm.StepPercent = 10;
            if (numRows * numColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // use the colorbreaks to calculate the colors
                    pixelColor = rasterSymbolizer.GetColor(raster.Value[row, col]);

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
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(bmpData, rgbData, progressHandler);
                rgbData = mySmoother.Smooth();
            }


            // Copy the values back into the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }

        #endregion

        #region Unique Values

        /// <summary>
        /// Obtains a list of unique values from the grid.
        /// </summary>
        /// <param name="raster">The IRaster to obtain unique values for</param>
        /// <returns>A list of double values, where no value is repeated.</returns>
        public static List<double> GetUniqueValues(this IRaster raster)
        {
            List<double> list = new List<double>();
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    double val = raster.Value[row, col];
                    if (list.Contains(val) == false)
                    {
                        list.Add(val);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Obtains a list of unique values.  If there are more than maxCount values, the process stops and overMaxCount is set to true.
        /// </summary>
        /// <param name="raster">the raster to obtain the unique values from.</param>
        /// <param name="maxCount">An integer specifying the maximum number of values to add to the list of unique values</param>
        /// <param name="overMaxCount">A boolean that will be true if the process was halted prematurely.</param>
        /// <returns>A list of doubles representing the independant values.</returns>
        public static List<double> GetUniqueValues(this IRaster raster, int maxCount, out bool overMaxCount)
        {
         
            overMaxCount = false;
            List<double> list = new List<double>();
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    double val = raster.Value[row, col];
                    if (list.Contains(val) == false)
                    {
                        list.Add(val);
                        if (list.Count > maxCount)
                        {
                            overMaxCount = true;
                            return list;
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// This will sample randomly from the raster, preventing duplicates.
        /// If the sampleSize is larger than this raster, this returns all of the
        /// values from the raster.
        /// </summary>
        /// <param name="raster"></param>
        /// <param name="sampleSize"></param>
        /// <returns></returns>
        public static List<double> GetRandomValues(this IRaster raster, int sampleSize)
        {
            int numRows = raster.NumRows;
            int numCols = raster.NumColumns;
            List<double> result = new List<double>();
            double noData = raster.NoDataValue;
            if(numRows * numCols < sampleSize)
            {
                for(int row = 0; row < numRows; row++)
                {
                    for(int col = 0; col < numCols; col++)
                    {
                        double val = raster.Value[row, col];
                        if(val != noData)result.Add(raster.Value[row, col]);
                    }
                }
                return result;
            }
            Random rnd = new Random(DateTime.Now.Millisecond);
            if((long)numRows * (long)numCols < (long)sampleSize * 5 && (long)numRows * (long)numCols < (long)int.MaxValue)
            {
                // When the raster is only just barely larger than the sample size, 
                // we want to prevent lost of repeat guesses that fail.  So
                // instead, we just draw from a depleating reservoir of values.
                // Random only lets us use this trick for integer sized number though.
                Dictionary<long, double> reservoir = new Dictionary<long, double>();
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        double val = raster.Value[row, col];
                        if(val != noData)reservoir.Add((long)row * numCols + col, val);
                    }
                }
                int count = numRows*numCols;
                for(int i = 0; i < sampleSize; i++)
                {
                    int indx = rnd.Next(count);
                    result.Add(reservoir[indx]);
                    reservoir.Remove(indx);
                    count--;
                }
                return result;
            }
            
            // This is the brute force method, but is actually better if the raster
            // is much larger than the sample size, since we don't need to process
            // a huge reservoir to take a small number of random samples, and the
            // actual number of repeats should be small.
            Dictionary<long, double> exclusiveResults = new Dictionary<long, double>();
            int remaining = sampleSize;
            while(remaining > 0)
            {
                int row = rnd.Next(numRows);
                int col = rnd.Next(numCols);
                double value = raster.Value[row, col];
                if(value != noData)
                {
                    long index = row * col;
                    if (exclusiveResults.ContainsKey(index)) continue;
                    exclusiveResults.Add(index, value);
                    remaining--;
                }
                
            }
            foreach (KeyValuePair<long, double> pair in exclusiveResults)
            {
                result.Add(pair.Value);
            }
            return result;
          
            
            
        }

        #endregion


        /// <summary>
        /// Gets a boolean that is true if the Window extents contain are all the information for the raster.
        /// In otherwords, StartRow = StartColumn = 0, EndRow = NumRowsInFile - 1, and EndColumn = NumColumnsInFile - 1
        /// </summary>
        public static bool IsFullyWindowed(this IRaster raster)
        {
            if (raster.StartRow != 0) return false;
            if (raster.StartColumn != 0) return false;

            if (raster.EndRow != raster.NumRowsInFile - 1) return false;
            if (raster.EndColumn != raster.NumColumnsInFile - 1) return false;
            return true;
        }
      
        #endregion

        #region Properties



        #endregion



    }
}
