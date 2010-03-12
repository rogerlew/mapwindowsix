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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/13/2009 1:51:04 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.IO;
namespace MapWindow.Data
{


    /// <summary>
    /// RasterBoundsEM
    /// </summary>
    public static class RasterBoundsEM
    {


        /// <summary>
        /// Calculates the area of this envelope.  Because the word Area,
        /// like Volume, is dimension specific, this method only looks
        /// at the X and Y ordinates, and requires at least 2 ordinates.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>The 2D area as a double value.</returns>
        public static double Area(this IRasterBounds self)
        {
            if (self == null) return -1;
            return self.Width * self.Height;
        }

        /// <summary>
        /// Gets the minY, which is Y - Height.
        /// </summary>
        /// <param name="self">The <c>IRasterBounds</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Bottom(this IRasterBounds self)
        {
            return self.Y - self.Height;
        }


        /// <summary>
        /// Uses the specified distance to expand the envelope by that amount in all dimensions.
        /// </summary>
        /// <param name="self">The <c>IRasterBounds</c> that this calculation is for.</param>
        /// <param name="distance">The double distance to expand in all directions.</param>
        public static void ExpandBy(this IRasterBounds self, double distance)
        {
            self.X -= distance;
            self.Y += distance;
            self.Width += distance * 2;
            self.Height += distance * 2;
            // the maximum is now the minimum etc.
        }

        /// <summary>
        /// Gets the left value for this rectangle.  This should be the
        /// X coordinate, but is added for clarity.
        /// </summary>
        /// <param name="self">The <c>IRasterBounds</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Left(this IRasterBounds self)
        {
            return self.X;
        }

        /// <summary>
        /// Gets the right value, which is X + Width.
        /// </summary>
        /// <param name="self">The <c>IRasterBounds</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Right(this IRasterBounds self)
        {
            return self.X + self.Width;
        }

        /// <summary>
        /// Gets the maxY value, which should be Y.
        /// </summary>
        /// <param name="self">The <c>IRasterBounds</c> that this calculation is for.</param>
        /// <returns>The double value representing the Max Y value of this rectangle</returns>
        public static double Top(this IRasterBounds self)
        {
            return self.Y;
        }

 


        /// <summary>
        /// Use the Open method instead of this extension.  This only provides
        /// a default behavior that can optionally be used by implementers
        /// of the IRasterBounds interface.
        /// </summary>
        /// <param name="bounds">The bounds to open</param>
        /// <param name="filename">The *.wld or *.**w world file to open</param>
        public static void OpenWorldFile(this IRasterBounds bounds, string filename)
        {
            bounds.WorldFile = filename;
            double[] affine = new double[6];
           
            StreamReader sr = new StreamReader(filename);
        
            affine[1] = double.Parse(sr.ReadLine()); // Dx
            affine[2] = double.Parse(sr.ReadLine()); // Skew X
            affine[4] = double.Parse(sr.ReadLine()); // Skew Y
            affine[5] = double.Parse(sr.ReadLine()); // Dy
            affine[0] = double.Parse(sr.ReadLine()); // Top Left X
            affine[3] = double.Parse(sr.ReadLine()); // Top Left Y
            bounds.AffineCoefficients = affine;
            sr.Close();
        }

        /// <summary>
        /// Use the Save method instead of this extension.  This only provides
        /// a default behavior that can optionally be used by implementers
        /// of the IRasterBounds interface.
        /// </summary>
        public static void SaveWorldFile(this IRasterBounds bounds)
        {
            StreamWriter sw = new StreamWriter(bounds.WorldFile);
            double[] affine = bounds.AffineCoefficients;
            sw.WriteLine(affine[1].ToString());  // Dx
            sw.WriteLine(affine[2].ToString());  // rotation X
            sw.WriteLine(affine[4].ToString());  // rotation Y
            sw.WriteLine(affine[5].ToString());  // Dy 
            sw.WriteLine(affine[0].ToString()); // Top Left X
            sw.WriteLine(affine[3].ToString()); // Top Left Y
            sw.Close();
        }


        #region projection Handling

        /// <summary>
        /// Given any input row or column, this returns the appropriate geographic location for the 
        /// position of the center of the cell.
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the center of the specified cell</returns>
        public static Coordinate CellCenter_ToProj(this IRasterBounds bounds, int row, int column)
        {
            double[] affine = bounds.AffineCoefficients;
            double x = affine[0] + affine[1] * column + affine[2] * row;
            double y = affine[3] + affine[4] * column + affine[5] * row;
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the top left corner of the cell
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the top left corner of the specified cell</returns>
        public static Coordinate CellTopLeft_ToProj(this IRasterBounds bounds, int row, int column)
        {
            double[] affine = bounds.AffineCoefficients;
            double x = affine[0] + affine[1] * ((double)column - 0.5) + affine[2] * ((double)row - 0.5);
            double y = affine[3] + affine[4] * ((double)column - 0.5) + affine[5] * ((double)row - 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the top right corner of the cell
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the top right corner of the specified cell</returns>
        public static Coordinate CellTopRight_ToProj(this IRasterBounds bounds, int row, int column)
        {
            double[] affine = bounds.AffineCoefficients;
            double x = affine[0] + affine[1] * ((double)column + 0.5) + affine[2] * ((double)row - 0.5);
            double y = affine[3] + affine[4] * ((double)column + 0.5) + affine[5] * ((double)row - 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the bottom left corner of the cell
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the bottom left corner of the specified cell</returns>
        public static Coordinate CellBottomLeft_ToProj(this IRasterBounds bounds, int row, int column)
        {
            double[] affine = bounds.AffineCoefficients;
            double x = affine[0] + affine[1] * ((double)column - 0.5) + affine[2] * ((double)row + 0.5);
            double y = affine[3] + affine[4] * ((double)column - 0.5) + affine[5] * ((double)row + 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the bottom right corner of the cell
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the bottom right corner of the specified cell</returns>
        public static Coordinate CellBottomRight_ToProj(this IRasterBounds bounds, int row, int column)
        {
            double[] affine = bounds.AffineCoefficients;
            double x = affine[0] + affine[1] * ((double)column + 0.5) + affine[2] * ((double)row + 0.5);
            double y = affine[3] + affine[4] * ((double)column + 0.5) + affine[5] * ((double)row + 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Returns the row col index
        /// </summary>
        /// <param name="bounds">The raster bounds to perform the calculation on</param>
        /// <param name="location">Gets or sets the ICoordinate</param>
        /// <returns>An RcIndex that shows the best row or column index for the specified coordiante.</returns>
        public static RcIndex ProjToCell(this IRasterBounds bounds, Coordinate location)
        {
            double[] c = bounds.AffineCoefficients;
            int iRow, iCol;
            double rw, cl;
            if (c[2] == 0 && c[4] == 0)
            {
                // If we don't have skew terms, then the x and y terms are independant.
                // also both of the other tests will have divide by zero errors and fail.
                cl = (location.X - c[0]) / c[1];
                rw = (location.Y - c[3]) / c[5];
            }
            else if (c[2] != 0)
            {
                // this equation will have divide by zero if c[2] is zero, but works if c[4] is zero
                double div = (c[5] * c[1]) / c[2] - c[4];
                cl = (c[3] + (c[5] * location.X) / c[2] - (c[5] * c[0]) / c[2] - location.Y) / div;
                rw = (location.X - c[0] - c[1] * cl) / c[2];
            }
            else
            {
                double div = (c[4] * c[2] / c[1] - c[5]);
                rw = (c[3] + c[4] * location.X / c[1] - c[4] * c[0] / c[1] - location.Y) / div;
                cl = (location.X - c[2] * rw - c[0]) / c[1];
            }
            iRow = (int)Math.Round(rw);
            iCol = (int)Math.Round(cl);
            return new RcIndex(iRow, iCol);
        }


        #endregion

        /// <summary>
        /// Generates a new version of the affine transform that will
        /// cover the same region, but using the specified number of
        /// rows and columns instead.
        /// </summary>
        /// <param name="bounds">The raster bounds to resample</param>
        /// <param name="numRows">The new number of rows </param>
        /// <param name="numColumns">The new number of columns</param>
        /// <returns>
        /// X = [0] + [1] * Column + [2] * Row
        /// Y = [3] + [4] * Column + [5] * Row 
        /// </returns>
        public static IRasterBounds ResampleTransform(this IRasterBounds bounds, int numRows, int numColumns)
        {
            double[] affine = bounds.AffineCoefficients;
            double[] result = new double[6];
            double oldNumRows = bounds.NumRows;
            double oldNumColumns = bounds.NumColumns;
            result[0] = affine[0]; // Top Left X
            result[3] = affine[3]; // Top Left Y
            result[1] = affine[1] * oldNumColumns / numColumns; // dx
            result[5] = affine[5] * oldNumRows / numRows; // dy
            result[2] = affine[2] * oldNumRows / numRows; // skew x
            result[4] = affine[4] * oldNumColumns / numColumns; // skew y
            return new RasterBounds(numRows, numColumns, result);
        }

       
        

        /// <summary>
        /// Attemps to save the affine coefficients to the specified worldfile file name
        /// </summary>
        /// <param name="bounds">The bounds to save</param>
        /// <param name="filename">The filename to save this bounds to</param>
        public static void SaveAs(this IRasterBounds bounds, string filename)
        {
            bounds.WorldFile = filename;
            bounds.Save();
        }


        /// <summary>
        /// Converts creates a float precisions drawing matrix from the double precision affine 
        /// coordinates.  The Matrix can be manipulated and then set back.  Some precision will
        /// be lost, however, as only floats are supported.
        /// </summary>
        public static System.Drawing.Drawing2D.Matrix Get_AffineMatrix(this IRasterBounds bounds)
        {
            double[] affine = bounds.AffineCoefficients;
            return new System.Drawing.Drawing2D.Matrix(
              Convert.ToSingle(affine[0]), // XShift
              Convert.ToSingle(affine[2]), // RotationX
              Convert.ToSingle(affine[4]), // RotationY
              Convert.ToSingle(affine[3]), // YShift
              Convert.ToSingle(affine[1]), // CellWidth
              Convert.ToSingle(affine[5])); // CellHeight
            
        }
        /// <summary>
        /// Re-defines the double precision affine transform values based on the specified
        /// system.Drawing.Drawing2D.Matrix.
        /// </summary>
        /// <param name="bounds">The bounds to adjust based on the matrix</param>
        /// <param name="matrix">The matrix to use as a guide for adjustments.</param>
        public static void Set_AffineMatrix(this IRasterBounds bounds, System.Drawing.Drawing2D.Matrix matrix)
        {
            double[] affine = new double[6];
            affine[0] = Double.Parse(matrix.Elements[0].ToString()); // XShift
            affine[2] = Double.Parse(matrix.Elements[1].ToString()); // RotationX
            affine[4] = Double.Parse(matrix.Elements[2].ToString()); // RotationY
            affine[3] = Double.Parse(matrix.Elements[3].ToString()); // YShift
            affine[1] = Double.Parse(matrix.Elements[4].ToString()); // CellWidth
            affine[5] = Double.Parse(matrix.Elements[5].ToString()); // CellHeight
        }

     

        /// <summary>
        /// Images can be skewed, so this gets the point that actually defines the bottom left
        /// corner of the data in geographic coordinates
        /// </summary>
        public static Coordinate BottomLeft(this IRasterBounds bounds)
        {
            double[] affine = bounds.AffineCoefficients;
            int numRows = bounds.NumRows;
            // X' = [0] + [1] * Column + [2] * Row
            // Y' = [3] + [4] * Column + [5] * Row 
            double x = affine[0] + numRows * affine[2];
            double y = affine[3] + numRows * affine[5];
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Images can be skewed, so this gets the point that actually defines the bottom right
        /// corner of the data in geographic coordinates
        /// </summary>
        public static Coordinate BottomRight(this IRasterBounds bounds)
        {
            double[] affine = bounds.AffineCoefficients;
            double numRows = bounds.NumRows;
            double numColumns = bounds.NumColumns;
            // X' = [0] + [1] * Column + [2] * Row
            // Y' = [3] + [4] * Column + [5] * Row 
            double x = affine[0] + numColumns * affine[1] + numRows * affine[2];
            double y = affine[3] + numColumns * affine[4] + numRows * affine[5];
            return new Coordinate(x, y);
            
        }

        

        


        ///// <summary>
        ///// Gets the Left bound of the envelope completely containing this raster.
        ///// </summary>
        ///// <param name="bounds">The IRasterBounds to get the value for</param>
        //public static double Left(this IRasterBounds bounds)
        //{
           
        //    double l = bounds.BottomLeft().X;
        //    double x = bounds.BottomRight().X; if (x < l) l = x;
        //    x = bounds.TopLeft().X; if (x < l) l = x;
        //    x = bounds.TopRight().X; if (x < l) l = x;
           
        //    // left is actually half a cell width further left
        //    l = l - bounds.AffineCoefficients[1] * .5;
        //    return l;

            
        //}

      
        ///// <summary>
        ///// Gets the right bound of the envelope completely containing this raster.
        ///// </summary>
        //public static double Right(this IRasterBounds bounds)
        //{
            
        //    double r = bounds.BottomLeft().X;
        //    double x = bounds.BottomRight().X; if (x > r) r = x;
        //    x = bounds.TopLeft().X; if (x > r) r = x;
        //    x = bounds.TopRight().X; if (x > r) r = x;
        //    // right is actually half a cell width further right
        //    r = r + bounds.AffineCoefficients[1] * .5;
        //    return r;
            
        //}

        ///// <summary>
        ///// Gets the top bound of the envelope completely containing this raster.
        ///// </summary>
        //public static double Top(this IRasterBounds bounds)
        //{
        //    IRectangle r = bounds as IRectangle;
            

        //    double t = bounds.BottomLeft().Y;
        //    double y = bounds.BottomRight().Y; if (y > t) t = y;
        //    y = bounds.TopRight().Y; if (y > t) t = y;
        //    y = bounds.TopLeft().Y; if (y > t) t = y;
        //    // the top is actually half a cell width further north
        //    t += Math.Abs(bounds.AffineCoefficients[5]) * .5;
        //    return t;
            
        //}


        /// <summary>
        /// Images can be skewed, so this gets the point that actually defines the top left
        /// corner of the data in geographic coordinates
        /// </summary>
        /// <param name="bounds">The IRasterBounds to obtain the top left of</param>
        public static Coordinate TopLeft(this IRasterBounds bounds)
        {
            double[] affine = bounds.AffineCoefficients;
            // X' = [0] + [1] * Column + [2] * Row
            // Y' = [3] + [4] * Column + [5] * Row 
            double x = affine[0];
            double y = affine[3];
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Images can be skewed, so this gets the point that actually defines the top right
        /// corner of the data in geographic coordinates
        /// </summary>
        public static Coordinate TopRight(this IRasterBounds bounds)
        {
            double[] affine = bounds.AffineCoefficients;
            double numColumns = bounds.NumColumns;
            // X' = [0] + [1] * Column + [2] * Row
            // Y' = [3] + [4] * Column + [5] * Row 
            double x = affine[0] + numColumns * affine[1];
            double y = affine[3] + numColumns * affine[4];
            return new Coordinate(x, y);
            
        }

    
     

     

    }
}
