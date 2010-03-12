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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2010 10:45:38 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Drawing;

namespace MapWindow.Data
{


    /// <summary>
    /// IImageSource
    /// </summary>
    public interface IImageSource
    {


        #region Properties

        /// <summary>
        /// Gets or sets the bounds 
        /// </summary>
        RasterBounds Bounds
        {
            get; set;
        }

        /// <summary>
        /// Gets the number of rows
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the total number of columns
        /// </summary>
        int NumColumns
        {
            get;
        }

        /// <summary>
        /// Gets the number of overviews, not counting the original image
        /// </summary>
        int NumOverviews
        { 
            get;
        }

        #endregion


        #region Methods

        /// <summary>
        ///  Returns the data from the file in the form of ARGB bytes.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y)</param>
        /// <param name="startColumn">The zero based integer index of the first column (X)</param>
        /// <param name="numRows">The number of rows to read</param>
        /// <param name="numColumns">The number of columns to read</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns>A Byte of values in ARGB order and in row-major raster-scan sequence</returns>
        byte[] ReadWindow(int startRow, int startColumn, int numRows, int numColumns, int overview);

        /// <summary>
        /// This returns the window of data as a bitmap.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y)</param>
        /// <param name="startColumn">The zero based integer index of the first column (X)</param>
        /// <param name="numRows">The number of rows to read</param>
        /// <param name="numColumns">The number of columns to read</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns></returns>
        Bitmap GetBitmap(int startRow, int startColumn, int numRows, int numColumns, int overview);



        #endregion

       


    }
}
