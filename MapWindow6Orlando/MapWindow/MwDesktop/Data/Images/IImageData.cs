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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/9/2008 2:36:19 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Data
{


    /// <summary>
    /// IImageData
    /// </summary>
    public interface IImageData: IDataSet
    {
     

        #region Methods

        /// <summary>
        /// Gets or sets an integer indicating how many bytes exist for each pixel.
        /// Eg. 32 ARGB = 4, 24 RGB = 3, 16 bit GrayScale = 2
        /// </summary>
        int BytesPerPixel
        {
            get;
            set;
        }

        

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">The source image to copy values from.</param>
        void CopyValues(IImageData source);

        /// <summary>
        /// Release any unmanaged memory objects
        /// </summary>
        void Dispose();
       
        /// <summary>
        /// Attempts to create a bitmap for the entire image.  This may cause memory exceptions.
        /// </summary>
        /// <returns>A Bitmap of the image.</returns>
        Bitmap GetBitmap();

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="size">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        Bitmap GetBitmap(IEnvelope envelope, Size size);
       

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="window">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        Bitmap GetBitmap(IEnvelope envelope, Rectangle window);
        
         /// <summary>
        /// Creates a color structure from the byte values in the values array that correspond to the
        /// specified position.
        /// </summary>
        /// <param name="row">The integer row index for the pixel.</param>
        /// <param name="column">The integer column index for the pixel.</param>
        /// <returns>A System.Drawing.Color.</returns>
        Color GetColor(int row, int column);
        

        /// <summary>
        /// Opens the file, assuming that the filename has already been specified
        /// </summary>
        void Open();
       
        /// <summary>
        /// Opens the file with the specified filename
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        void Open(string filename);
      
         /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format
        /// </summary>
        void ReadBytes();

        /// <summary>
        /// Saves the image and associated world file to the current filename.
        /// </summary>
        void Save();
     

        /// <summary>
        /// Saves the image to a new filename.
        /// </summary>
        /// <param name="filename">The string filename to save the image to.</param>
        void SaveAs(string filename);
        
         /// <summary>
        /// Sets the color value into the byte array based on the row and column position of the pixel.
        /// </summary>
        /// <param name="row">The integer row index of the pixel to set the color of.</param>
        /// <param name="column">The integer column index of the pixel to set the color of </param>
        /// <param name="col">The color to copy values from</param>
        void SetColor(int row, int column, Color col);

         /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        void WriteBytes();

        #endregion

        #region Properties

       

        /// <summary>
        /// Gets or sets the filename.  
        /// </summary>
        string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// A shortcut to the Bounds envelope.
        /// </summary>
        IEnvelope Envelope
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the world file that stores the georeferencing information for this image.
        /// </summary>
        WorldFile WorldFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image bounds being used to define the georeferencing of the image
        /// </summary>
        RasterBounds Bounds
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the image width in pixels
        /// </summary>
        int Width
        {
            get;
            set;

        }

        /// <summary>
        /// Gets the image height in pixels
        /// </summary>
        int Height
        {
            get;
            set;
        }

         /// <summary>
        /// Gets or sets the number of bands that are in the image.  One band is a gray valued image, 3 bands for color RGB and 4 bands
        /// for ARGB.
        /// </summary>
        int NumBands
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stride in bytes.
        /// </summary>
        int Stride
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a one dimensional array of byte values
        /// </summary>
        byte[] Values
        {
            get;
            set;
        }


        #endregion

    }
}
